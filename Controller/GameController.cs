using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static void CreateGame(Game game, SocketThreadChannel thread, RestUserMessage message)
        {
            game.Thread = thread;
            game.InviteMessage = message;
            _games.Add(game.UniqueId, game);
        }

        public static async Task ParseInteraction(SocketMessageComponent component)
        {
            switch(component.Data.CustomId)
            {
                case string s when s.StartsWith("invite_"):
                    await JoinGame(s[7..], component.User);
                    break;
            }
        }

        private static async Task JoinGame(string id, SocketUser u)
        {
            if (u is not SocketGuildUser user) return;
            if (_games.TryGetValue(id, out Game game))
                await game.Join(user);
        }
    }
}
