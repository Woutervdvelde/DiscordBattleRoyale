using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public delegate void GameEventHandler(GameEventArgs game);

    public class GameEventArgs : EventArgs
    {
        public Game Game;

        public GameEventArgs(Game game)
        {
            Game = game;
        }
    }
}
