using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Module
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        // !ping -> pong
        [Command("ping")]
        [Summary("Pings the bot.")]
        public Task PingAsync() => ReplyAsync("pong");
    }
}
