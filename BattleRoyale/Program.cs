using BattleRoyale.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BattleRoyale
{
	class Program
	{
		private DiscordSocketClient _client;

		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			using (var services = ConfigureServices())
            {
				var client = services.GetRequiredService<DiscordSocketClient>();
				client.Log += Log;
				services.GetRequiredService<CommandService>().Log += Log;

				await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));
				await client.StartAsync();

				await services.GetRequiredService<CommandHandler>().InitializeAsync();
				await services.GetRequiredService<ThreadHandler>().InitializeAsync();

				await Task.Delay(-1);

			}
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}

		private ServiceProvider ConfigureServices()
        {
			return new ServiceCollection()
				.AddSingleton<DiscordSocketClient>()
				.AddSingleton<CommandService>()
				.AddSingleton<CommandHandler>()
				.AddSingleton<ThreadHandler>()
				.BuildServiceProvider();
        }
	}
}
