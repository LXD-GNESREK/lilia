using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public InteractionHandler(DiscordSocketClient client, InteractionService handler, IServiceProvider services, ILogger<InteractionHandler> logger, IConfiguration configuration)
        {
            _client = client;
            _handler = handler;
            _services = services;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _handler.AddModulesAsync(typeof(Modules.UnitModule).Assembly, _services);

            _client.Ready += ReadyAsync;
            _handler.Log += LogAsync;
            _client.InteractionCreated += HandleInteraction;
            _handler.InteractionExecuted += HandleInteractionExecuted;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _client.Ready -= ReadyAsync;
            _handler.Log -= LogAsync;
            _client.InteractionCreated -= HandleInteraction;
            _handler.InteractionExecuted -= HandleInteractionExecuted;
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

                    if(interaction.HasResponded)
                    {
                        await interaction.FollowupAsync($"**Error:** {result.ErrorReason}", ephemeral: true);
                    }
                    else
                    {
                        await interaction.RespondAsync($"**Error:** {result.ErrorReason}", ephemeral: true);
                    }
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
            ulong? devGuildID = _configuration.GetValue<ulong?>("Discord:DevGuildId");

            if(devGuildID.HasValue && devGuildID.Value != 0)
            {
                Console.WriteLine($"[DEBUG] Discovered {_handler.Modules.Count} modules and {_handler.SlashCommands.Count} slash commands.");
                await _handler.RegisterCommandsToGuildAsync(devGuildID.Value);
                _logger.LogInformation("Commands registered to Server: {GuildId}", devGuildID.Value);
            }
            else
            {
                await _handler.RegisterCommandsGloballyAsync();
                _logger.LogInformation("Commands registered globally.");
            }
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

        private async Task HandleInteractionExecuted(ICommandInfo command, IInteractionContext context, IResult result)
        {
            if(!result.IsSuccess)
            {
                _logger.LogError("Command {CommandName} failed: {ErrorReason}", command?.Name, result.ErrorReason);

                if (result.Error == InteractionCommandError.UnknownCommand) return;

                if (context.Interaction.HasResponded)
                {
                    await context.Interaction.FollowupAsync($"**Error:** {result.ErrorReason}", ephemeral: true);
                }
                else
                {
                    await context.Interaction.RespondAsync($"**Error:** {result.ErrorReason}", ephemeral: true);
                }
            }
        }
    }
}