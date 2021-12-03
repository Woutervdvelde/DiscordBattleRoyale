using Discord;
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


            var settingsBuilder = new ComponentBuilder()
                .WithButton("Start", $"start_{game.UniqueId}")
                .WithButton("Cancel", $"cancel_{game.UniqueId}", ButtonStyle.Danger)
                .WithSelectMenu(
                    $"naming_{game.UniqueId}",
                    InteractionHandler.ConvertEnumToSelectMenuOptions<GamePlayerNameOptions>(GamePlayerOptions.GamePlayerNameDescriptions, game.Naming.ToString()),
                    "Naming",
                    1, 1
                );

            RestUserMessage settingsMessage = await channel.SendMessageAsync(
                $"║■■■■■■■■■ ADMIN ■■■■■■■■■║\n" +
                $"Only {Context.User.Username} can manage the game.\n" +
                $"You have access to the following controls\n",
                component: settingsBuilder.Build()
            );

            var inviteBuilder = new Discord.ComponentBuilder()
                .WithButton("Join game", $"invite_{game.UniqueId}");
            
            RestUserMessage inviteMessage = await channel.SendMessageAsync(
                $"║■■■■■■■■■ PLAYER ■■■■■■■■■║\n" +
                $" Click on the button to join {Context.User.Username}'s game",
                component: inviteBuilder.Build()
            );
            
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
