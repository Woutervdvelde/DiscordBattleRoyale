﻿using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Discord;

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
            await GameController.ParseInteraction(component);
        }

        public List<SelectMenuOptionBuilder> ConvertEnumToSelectMenuOptions<K>(Dictionary<K, string> descriptions)
        {
            List<SelectMenuOptionBuilder> options = new List<SelectMenuOptionBuilder>();
            foreach (string value in Enum.GetNames(typeof(K))) {
                descriptions.TryGetValue((K)Enum.Parse(typeof(K), value), out string description);
                options.Add(new SelectMenuOptionBuilder() { 
                    Label = value,
                    Value = value,
                    Description = description
                });
            } ;

            return options;
        }
    }
}
