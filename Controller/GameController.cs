using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using Discord;

namespace Controller
{
    public static class GameController
    {
        private static int _maxAllowedGamesInGuild;
        private static Dictionary<string, Game> _games { get; set; }

        public static void Initialize(int maxAllowedGamesInGuild)
        {
            _maxAllowedGamesInGuild = maxAllowedGamesInGuild;
            _games = new Dictionary<string, Game>();
        }

        /// <summary>
        /// Checks if the guild has more or less than the allowed amount of games running
        /// </summary>
        /// <param name="guild"></param>
        /// <returns>
        ///     true if there is a game available<br/>
        ///     false if there isn't a game available
        /// </returns>
        public static bool CheckAvailability(SocketGuild guild)
        {
            int runningGames = (_games.Where(g => g.Value.Thread.Guild.Id == guild.Id)).Count();
            return runningGames < _maxAllowedGamesInGuild;
        }

        public static Game GetGameByThread(SocketThreadChannel thread)
        {
            var result = _games.Where(g => g.Value.Thread.Name == thread.Name);
            return result.Any() ? result.First().Value : null;
        }

        public static async Task CheckMessage(SocketUserMessage message)
        {
            if (message.Author.IsBot) return;
            if (message.Channel is not SocketThreadChannel thread) return;

            Game game = GetGameByThread(thread);
            if (game == null) return;

            if (game.IsStarted || game.Naming != GamePlayerNameOptions.Custom) return;
            
            game.Suggestions.Add(message.Content);
            await UpdateAdminMessage(game);
        }

        public static void CreateGame(Game game, SocketThreadChannel thread, RestUserMessage settingsMessage, RestUserMessage inviteMessage)
        {
            game.Thread = thread;
            game.SettingsMessage = settingsMessage;
            game.InviteMessage = inviteMessage;
            game.GameFinished += OnGameFinished;
            _games.Add(game.UniqueId, game);
        }

        private static void OnGameFinished(GameEventArgs args)
        {
            if (!_games.Keys.Contains(args.Game.UniqueId)) return;

            _games.Remove(args.Game.UniqueId);
        }

        public static async Task ParseInteraction(SocketMessageComponent component)
        {
            switch (component.Data.CustomId)
            {
                case string s when s.StartsWith("invite_"):
                    await JoinGame(s[7..], component.User);
                    break;
                case string s when s.StartsWith("start_"):
                    await StartGame(s[6..], component.User);
                    break;
                case string s when s.StartsWith("cancel_"):
                    await CancelGame(s[7..], component.User);
                    break;
                case string s when s.StartsWith("naming_"):
                    await SetNaming(s[7..], component);
                    break;
                case string s when s.StartsWith("suggestions_"):
                    AddSuggestion(s[12..], component);
                    break;
            }

            await component.DeferAsync();
        }

        private static async Task JoinGame(string id, SocketUser u)
        {
            if (u is not SocketGuildUser user) return;
                if (_games.TryGetValue(id, out Game game))
                    if (!game.HasParticipant(u))
                        await game.Join(user);
        }

        private static async Task StartGame(string id, SocketUser u)
        {
            if (_games.TryGetValue(id, out Game game))
                if (game.IsCreator(u))
                    if (game.IsReady)
                        await game.Start();
        }

        private static async Task CancelGame(string id, SocketUser u)
        {
            if (_games.TryGetValue(id, out Game game))
                if (game.IsCreator(u))
                {
                    await game.Cancel();
                    _games.Remove(game.UniqueId);
                }
        }

        private static async Task SetNaming(string id, SocketMessageComponent component)
        {
            if (_games.TryGetValue(id, out Game game)) 
            {
                if (!game.IsCreator(component.User)) return;
                if (Enum.TryParse(typeof(GamePlayerNameOptions), component.Data.Values.First(), out object option))
                    if (option is GamePlayerNameOptions newNaming)
                    {
                        game.Naming = newNaming;

                        if (newNaming == GamePlayerNameOptions.Custom)
                        {
                            await game.Thread.SendMessageAsync("Custom naming has been turned on! All the messages you send from now on are able to be picked as participants by the admin.");
                            await UpdateAdminMessage(game);
                        }
                    }
            }
        }

        public static void AddSuggestion(string id, SocketMessageComponent component)
        {
            if (_games.TryGetValue(id, out Game game))
                if (game.IsCreator(component.User))
                    game.ChosenSuggestions = component.Data.Values.ToList();
        }

        public static async Task<RestUserMessage> SendInviteMessage(SocketTextChannel channel, Game game)
        {
            var inviteBuilder = new ComponentBuilder()
                .WithButton("Join game", $"invite_{game.UniqueId}");

            return await channel.SendMessageAsync(
                $"║■■■■■■■■■ PLAYER ■■■■■■■■■║\n" +
                $" Click on the button to join {((SocketGuildUser)game.Creator).Nickname}'s game",
                component: inviteBuilder.Build()
            );

        }

        public static async Task<RestUserMessage> SendAdminMessage(SocketTextChannel channel, Game game)
        {
            if (game.SettingsMessage != null)
            {
                await UpdateAdminMessage(game); 
                return null;
            }

            ComponentBuilder settingsBuilder = new ComponentBuilder()
                .WithButton("Start", $"start_{game.UniqueId}")
                .WithButton("Cancel", $"cancel_{game.UniqueId}", ButtonStyle.Danger)
                .WithSelectMenu(
                    $"naming_{game.UniqueId}",
                    ConvertEnumToSelectMenuOptions(GamePlayerOptions.GamePlayerNameDescriptions, game.Naming.ToString()),
                    "Naming",
                    1, 1
                );

            return await channel.SendMessageAsync(
                $"║■■■■■■■■■ ADMIN ■■■■■■■■■║\n" +
                $"Only {((SocketGuildUser)game.Creator).Nickname} can manage the game.\n" +
                $"You have access to the following controls\n",
                component: settingsBuilder.Build()
            );
        }

        public static async Task UpdateAdminMessage(Game game)
        {
            ComponentBuilder settingsBuilder = new ComponentBuilder()
            .WithButton("Start", $"start_{game.UniqueId}")
            .WithButton("Cancel", $"cancel_{game.UniqueId}", ButtonStyle.Danger)
            .WithSelectMenu(
                $"naming_{game.UniqueId}",
                ConvertEnumToSelectMenuOptions(GamePlayerOptions.GamePlayerNameDescriptions, game.Naming.ToString()),
                "Naming",
                1, 1
            );

            if (game.Naming == GamePlayerNameOptions.Custom)
            {
                var suggestionsBuilder = new SelectMenuBuilder()
                    .WithPlaceholder("Select suggestions as naming")
                    .WithCustomId($"suggestions_{game.UniqueId}")
                    .WithMinValues(1)
                    .WithMaxValues(1 + game.Suggestions.Count)
                    .AddOption("Suggestions", "Suggestions", "Here you'll see all the suggestions");

                game.Suggestions.ForEach(s => suggestionsBuilder.AddOption(s, s));

                settingsBuilder.WithSelectMenu(suggestionsBuilder);
            }

            await game.SettingsMessage.ModifyAsync(m => m.Components = settingsBuilder.Build());
        }

        public static List<SelectMenuOptionBuilder> ConvertEnumToSelectMenuOptions<K>(Dictionary<K, string> descriptions, string selected = null)
        {
            List<SelectMenuOptionBuilder> options = new List<SelectMenuOptionBuilder>();
            foreach (string value in Enum.GetNames(typeof(K)))
            {
                descriptions.TryGetValue((K)Enum.Parse(typeof(K), value), out string description);
                options.Add(new SelectMenuOptionBuilder()
                {
                    Label = value,
                    Value = value,
                    Description = description,
                    IsDefault = selected == value
                }); ;
            };

            return options;
        }
    }
}
