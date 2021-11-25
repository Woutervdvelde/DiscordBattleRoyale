using BattleRoyale.Services;
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
        public ThreadHandler ThreadHandler { get; set; }

        [Command("thread")]
        [Alias("t")]
        [Summary("Creates a thread")]
        public async Task ThreadAsync()
        {
            SocketTextChannel ch = Context.Channel as SocketTextChannel;
            SocketThreadChannel thread = await ThreadHandler.CreateNewThread(ch, Context.User);

            //ch.CreateThreadAsync("test"); //this works fine.
            //await ch.CreateThreadAsync("test"); //this works but 


            //SocketThreadChannel thread = await ch.CreateThreadAsync("test");
            
            //
            //Console.WriteLine(thread);
            //await thread.JoinAsync();
            //await thread.SendMessageAsync("Welcome");
            //await ReplyAsync("meow");
        }
    }
}
