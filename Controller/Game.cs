using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Game
    {
        private SocketUser _creator { get; set; }
        private SocketThreadChannel _thread { get; set; }

        public Game (SocketUser creator, SocketThreadChannel thread)
        {
            _creator = creator;
            _thread = thread;
        }
    }
}
