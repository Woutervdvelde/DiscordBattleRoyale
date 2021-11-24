using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyale.Modules
{
    public class ThreadModule : ModuleBase<SocketCommandContext>
    {
        [Command("thread")]
        [Alias("t")]
        [Summary("Creates a thread")]
        public async Task ThreadAsync()
        {

        }
    }
}
