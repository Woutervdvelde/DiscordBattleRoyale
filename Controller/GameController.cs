using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

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
            }

            await component.DeferAsync();
        }

        private static async Task JoinGame(string id, SocketUser u)
        {
            if (u is not SocketGuildUser user) return;
            if (_games.TryGetValue(id, out Game game))
                await game.Join(user);
        }

        private static async Task StartGame(string id, SocketUser u)
        {
            if (_games.TryGetValue(id, out Game game))
                if (game.IsCreator(u))
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
                    if (option is GamePlayerNameOptions value)
                        game.Naming = value;
            }
        }
    }
}
