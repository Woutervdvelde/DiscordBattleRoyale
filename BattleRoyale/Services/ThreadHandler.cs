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

        public void Initialize()
        {
            //TODO better initialization?
            //just initializing it to recognize the constructor at this point...
        }

        /// <summary>
        /// Creates a new <see cref="SocketThreadChannel"/> in a desired <see cref="SocketTextChannel"/>
        /// </summary>
        /// <param name="channel">channel the thread will be created in</param>
        /// <param name="name">name of the thread</param>
        /// <returns></returns>
        public async Task<SocketThreadChannel> CreateNewThread(SocketTextChannel channel, string name)
        {
            _ = channel.CreateThreadAsync(name);
            SocketThreadChannel thread = await GetThread(channel, name, 10);
            return thread;
        }

        /// <summary>
        /// This will try to retrieve a <see cref="SocketThreadChannel"/> by name. <br/>
        /// It will wait 250ms before each attempt since it takes time to create a thread.
        /// </summary>
        /// <param name="channel">channel that needs to be searched for threads</param>
        /// <param name="name">Name of the desired thread</param>
        /// <param name="retryCount">Amount of retries</param>
        /// <returns></returns>
        private async Task<SocketThreadChannel> GetThread(SocketTextChannel channel, string name, int retryCount)
        {
            SocketThreadChannel thread = null;
            int retries = 0;

            while (thread == null && retries < retryCount)
            {
                await Task.Delay(250);
                var result = channel.Threads.Where(t => t.Name == name);
                if (result.Any())
                    thread = result.First();

                retries++;
            }

            return thread;
        }

        public async Task OnThreadCreated(SocketThreadChannel thread)
        {

        }
    }
}