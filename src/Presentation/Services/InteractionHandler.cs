using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Lilia.Presentation.Services
{
    public class InteractionHandler : IHostedService
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _handler;
        private readonly IServiceProvider _services;
        private readonly ILogger<InteractionHandler> _logger;

        public InteractionHandler(DiscordSocketClient client, InteractionService handler, IServiceProvider services, ILogger<InteractionHandler> logger)
        {
            _client = client;
            _handler = handler;
            _services = services;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _client.Ready += ReadyAsync;
            _handler.Log += LogAsync;
            _client.InteractionCreated += HandleInteraction;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _client.Ready -= ReadyAsync;
            _handler.Log -= LogAsync;
            _client.InteractionCreated -= HandleInteraction;
            return Task.CompletedTask;
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                var context = new SocketInteractionContext(_client, interaction);
                var result = await _handler.ExecuteCommandAsync(context, _services);

                if(!result.IsSuccess)
                {
                    _logger.LogError("Error handling interactions: {ErrorReason}", result.ErrorReason);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while handling interaction.");

                if(interaction.Type == InteractionType.ApplicationCommand)
                {
                    await interaction.DeleteOriginalResponseAsync();
                }
            }
        }

        private async Task ReadyAsync()
        {
            await _handler.RegisterCommandsGloballyAsync();
            _logger.LogInformation("Commands registered globally.");
        }

        private Task LogAsync(LogMessage message)
        {
            LogLevel severity = message.Severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Warning =>  LogLevel.Warning,
                LogSeverity.Info => LogLevel.Information,
                LogSeverity.Verbose => LogLevel.Trace,
                LogSeverity.Debug => LogLevel.Debug,
                _ => LogLevel.Information
            };

            _logger.Log(severity, message.Exception, "{Message}", message.Message ?? message.Exception?.Message);
            return Task.CompletedTask;
        }
    }
}