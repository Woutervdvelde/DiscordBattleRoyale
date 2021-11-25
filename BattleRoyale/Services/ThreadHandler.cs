using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace BattleRoyale.Services
{
    public class ThreadHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public ThreadHandler(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _client.ThreadCreated += OnThreadCreated;
        }

        public async Task InitializeAsync()
        {
            //TODO better initialization?
            //just initializing it to recognize the constructor at this point...
        }

        public async Task CreateNewThread(SocketTextChannel channel)
        {
            channel.CreateThreadAsync("test"); //creates thread, doesn't throw error
            await channel.CreateThreadAsync("test"); //creates thread, does throw error
        }

        public async Task OnThreadCreated(SocketThreadChannel thread)
        {

        }
    }
}