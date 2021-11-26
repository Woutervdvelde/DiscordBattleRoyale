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

        public static bool CheckThread(SocketThreadChannel thread)
        {
            return _games.Keys.Contains(thread);
        }

        /// <summary>
        /// Creates a unique game name based on time.
        /// This way the <see cref="ThreadHandler.GetThread"/> can retrieve the thread.
        /// </summary>
        /// <param name="user">User that creates the game</param>
        /// <returns></returns>
        public static string GenerateThreadName(SocketUser user)
        {
            return $"{user.Username.Normalize()}s Game ({DateTime.Now.ToString("yyyyMMddHHmmss")})";
        }
    }
}
