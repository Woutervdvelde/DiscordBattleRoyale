using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Module;

namespace BattleRoyale
{
	class Program
	{
		private DiscordSocketClient _client;
		private char _prefix;

		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			_prefix = Environment.GetEnvironmentVariable("prefix")[0];
			_client = new DiscordSocketClient();
			_client.Log += Log;

			await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));
			await _client.StartAsync();

			new CommandHandler(_client, new Discord.Commands.CommandService(), _prefix);

			await Task.Delay(-1);
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
