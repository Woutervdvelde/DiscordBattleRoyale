using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;

namespace BattleRoyale.Services
{
    public class InteractionHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public InteractionHandler(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _client.ButtonExecuted += OnButtonExecuted;
        }

        public async Task InitializeAsync()
        {
            //TODO same as ThreadHandler improve Initialize method
        }

        private async Task OnButtonExecuted(SocketMessageComponent component)
        {
            await component.DeferAsync();
        }
    }
}
