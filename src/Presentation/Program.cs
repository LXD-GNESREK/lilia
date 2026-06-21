using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

using Lilia.Infrastructure.Data;
using Lilia.Presentation.Services;

namespace Lilia.Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    string connectionString = context.Configuration.GetConnectionString("DefaultConnection")
                        ?? throw new InvalidOperationException("Database connection string not found.");
                    services.AddDbContext<LiliaDBContext>(options => 
                        options.UseSqlite(connectionString));
                    
                    var discordConfig = new DiscordSocketConfig
                    {
                        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
                        AlwaysDownloadUsers = true
                    };

                    services.AddSingleton(new DiscordSocketClient(discordConfig));
                    services.AddHostedService<DiscordBotService>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}