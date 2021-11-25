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
        private static Dictionary<SocketThreadChannel, Game> _games { get; set; }
        public static void Initialize(int maxAllowedGamesInGuild)
        {
            _maxAllowedGamesInGuild = maxAllowedGamesInGuild;
            _games = new Dictionary<SocketThreadChannel, Game>();
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
            int runningGames = (guild.TextChannels.SelectMany(ch => ch.Threads.Where(t => _games.Keys.Contains(t)).Select(t => ch))).Count();
            //foreach (SocketTextChannel ch in guild.TextChannels)
            //    foreach (SocketThreadChannel t in ch.Threads)
            //        if (_games.Keys.Contains(t))
            //            runningGames++;
            return runningGames < _maxAllowedGamesInGuild;
        }
    }
}
