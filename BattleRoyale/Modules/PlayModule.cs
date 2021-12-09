﻿using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BattleRoyale.Services;
using Controller;
using BattleRoyaleGame = Controller.Game;
using Model;

namespace BattleRoyale.Modules
{
    public class PlayModule : ModuleBase<SocketCommandContext>
    {
        public ThreadHandler ThreadHandler { get; set; }
        public InteractionHandler InteractionHandler { get; set; }

        [Command("play")]
        [Summary("Creates a new battle royale")]
        public async Task CreateGame()
        {
            SocketTextChannel channel = Context.Channel as SocketTextChannel;
            if (channel == null) return;

            BattleRoyaleGame game = await TryCreateGame(Context);
            if (game == null) return;

            SocketThreadChannel thread = await TryCreateThread(channel, game);
            if (thread == null) return;

            RestUserMessage settingsMessage = await GameController.SendAdminMessage(channel, game);
            RestUserMessage inviteMessage = await GameController.SendInviteMessage(channel, game);
            
            GameController.CreateGame(game, thread, settingsMessage, inviteMessage);
        }

        private async Task<BattleRoyaleGame> TryCreateGame(SocketCommandContext ctx)
        {
            if (!GameController.CheckAvailability(Context.Guild))
            {
                await ReplyAsync("There are too many games running in this server");
                return null;
            }
            return new BattleRoyaleGame(ctx.User);
        }

        private async Task<SocketThreadChannel> TryCreateThread(SocketTextChannel channel, BattleRoyaleGame game)
        {
            SocketThreadChannel thread = await ThreadHandler.CreateNewThread(channel, game.UniqueId);
            if (thread == null)
            {
                await ReplyAsync("There was a problem creating the game");
                return null;
            }
            await thread.JoinAsync();
            await thread.SendMessageAsync($"Waiting for {Context.User.Username} to start the game.");
            return thread;
        }
    }
}
