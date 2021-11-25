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

        public async Task<SocketThreadChannel> CreateNewThread(SocketTextChannel channel, SocketUser user)
        {
            string threadName = GenerateThreadName(user);
            _ = channel.CreateThreadAsync(threadName);
            await Task.Delay(1000);
            SocketThreadChannel thread = channel.Threads.Where(t => t.Name == threadName).First();

            return thread;
        }

        private string GenerateThreadName(SocketUser user)
        {
            return $"{user.Username}s Game ({DateTime.Now.ToString("yyyyMMddHHmmss")})";
        }

        public async Task OnThreadCreated(SocketThreadChannel thread)
        {

        }
    }
}