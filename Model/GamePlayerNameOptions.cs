using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum GamePlayerNameOptions
    {
        Username, Nickname, Custom
    }

    public class GamePlayerOptions
    {
        public static Dictionary<GamePlayerNameOptions, string> GamePlayerNameDescriptions {
            get
            {
                Dictionary<GamePlayerNameOptions, string> descriptions = new();
                descriptions.Add(GamePlayerNameOptions.Username, "Players based on joined players usernames");
                descriptions.Add(GamePlayerNameOptions.Nickname, "Players based on joined players nicknames");
                descriptions.Add(GamePlayerNameOptions.Custom, "Players based on suggestions made by players (new menu)");

                return descriptions;
            }
        }
    }
}
