using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyale.Modules
{
    public class PingModule : ModuleBase<SocketCommandContext>
    {
        // !ping -> pong
        [Command("ping")]
        [Summary("Pings the bot.")]
        public Task PingAsync() => ReplyAsync("pong");
    }
}
