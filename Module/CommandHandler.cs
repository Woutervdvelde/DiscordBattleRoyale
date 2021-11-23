using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyale
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly char _prefix;

        public CommandHandler(DiscordSocketClient client, CommandService commands, char prefix)
        {
            _prefix = prefix;
            _commands = commands;
            _client = client;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argIndex = 0;
            if (!(message.HasCharPrefix(_prefix, ref argIndex) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argIndex)) ||
                message.Author.IsBot)
                return;
        }
    }
}
