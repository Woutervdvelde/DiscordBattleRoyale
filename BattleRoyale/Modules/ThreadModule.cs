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
            SocketTextChannel ch = Context.Channel as SocketTextChannel;
            if (ch == null) return;

            //WTF???
            ch.CreateThreadAsync("test"); //this works
            await ch.CreateThreadAsync("test"); //this doesn't


            //SocketThreadChannel thread = await ch.CreateThreadAsync("test");
            
            //
            //Console.WriteLine(thread);
            //await thread.JoinAsync();
            //await thread.SendMessageAsync("Welcome");
            //await ReplyAsync("meow");
        }
    }
}
