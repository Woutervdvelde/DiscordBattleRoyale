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

        //This is a test module for creating threads
        [Command("thread")]
        [Alias("t")]
        [Summary("Creates a thread")]
        public async Task ThreadAsync()
        {
            SocketTextChannel ch = Context.Channel as SocketTextChannel;
            string threadName = GenerateThreadName(Context.User);
            SocketThreadChannel thread = await ThreadHandler.CreateNewThread(ch, threadName);
            await thread.JoinAsync();
            await thread.SendMessageAsync("Welcome everyone!");
            //SocketThreadChannel thread = await ch.CreateThreadAsync("test");
            
            //
            //Console.WriteLine(thread);
            //await thread.JoinAsync();
            //await thread.SendMessageAsync("Welcome");
            //await ReplyAsync("meow");
        }

        private string GenerateThreadName(SocketUser user)
        {
            return $"{user.Username.Normalize()}s Game ({DateTime.Now.ToString("yyyyMMddHHmmss")})";
        }
    }
}
