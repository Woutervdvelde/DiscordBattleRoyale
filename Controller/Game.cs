using Discord.Rest;
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
        private List<SocketUser> _participants { get; set; }
        protected internal SocketThreadChannel Thread { get; set; }
        protected internal RestUserMessage InviteMessage { get; set; }

        public string UniqueId { get; private set; }

        public Game (SocketUser creator)
        {
            _creator = creator;
            _participants = new List<SocketUser>();

            UniqueId = GenerateUniqueId(creator.Username);
        }

        private string GenerateUniqueId(string user)
        {
            return $"{user.Normalize()}s Game ({DateTime.Now.ToString("yyyyMMddHHmmss")})";
        }

        public async Task Join(SocketGuildUser user)
        {
            await Thread.AddUserAsync(user);
            if (!_participants.Contains(user))
            {
                _participants.Add(user);
                await Thread.SendMessageAsync($"{user.Nickname} joined");
            }
        }

        public async Task Start()
        {
            await InviteMessage.DeleteAsync();
        }
    }
}
