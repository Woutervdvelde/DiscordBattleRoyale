using BattleRoyale.Services;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyale.Modules
{
    [RequireUserPermission(Discord.GuildPermission.Administrator)]
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
            string threadName = "test";
            SocketThreadChannel thread = await ThreadHandler.CreateNewThread(ch, threadName);

            await thread.JoinAsync();
            await thread.SendMessageAsync("Welcome everyone!");
        }

        [Command("rm_t")]
        [Alias("rt")]
        [Summary("Deletes all threads in SocketTextChannel")]
        public async Task RemoveThreads() => await ThreadHandler.RemoveAllThreadsInChannel((SocketTextChannel)Context.Channel);
    }
}
