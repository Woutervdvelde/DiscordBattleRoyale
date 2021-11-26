using BattleRoyale.Services;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Discord.WebSocket;
using Discord.Rest;
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

            if (!GameController.CheckAvailability(Context.Guild))
            {
                await ReplyAsync("There are too many games running in this server");
                return;
            }
            Game game = new Game(Context.User);

            SocketThreadChannel thread = await ThreadHandler.CreateNewThread(channel, game.UniqueId);
            if (thread == null)
            {
                await ReplyAsync("There was a problem creating the game");
                return;
            }
            await thread.JoinAsync();
            await thread.SendMessageAsync($"Waiting for {Context.User.Username} to start the game.");

            var builder = new Discord.ComponentBuilder()
                .WithButton("Join game", $"invite_{game.UniqueId}");

            RestUserMessage message = await channel.SendMessageAsync($"Click on the button to join {Context.User.Username}'s game", component: builder.Build());

            GameController.CreateGame(game, thread, message);

            var optionBuilder = new Discord.ComponentBuilder()
                .WithSelectMenu(
                    customId: $"invite_{game.UniqueId}",
                    options: InteractionHandler.ConvertEnumToSelectMenuOptions<GamePlayerNameOptions>(GamePlayerOptions.GamePlayerNameDescriptions),
                    minValues: 1,
                    maxValues: 1
                );
            await channel.SendMessageAsync("Creator options:", component: optionBuilder.Build());
        }
    }
}
