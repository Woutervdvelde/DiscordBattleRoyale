using Discord;
using Discord.WebSocket;
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
			_client = new DiscordSocketClient();
			_client.Log += Log;

			await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));
			await _client.StartAsync();

			await Task.Delay(-1);
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
