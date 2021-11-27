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
    public class Game
    {
        private SocketUser _creator { get; set; }
        private List<SocketUser> _participants { get; set; }
        private List<string> _names { get; set; }
        private List<string> _suggestions { get; set; }
        
        protected internal SocketThreadChannel Thread { get; set; }
        protected internal RestUserMessage SettingsMessage { get; set; }
        protected internal RestUserMessage InviteMessage { get; set; }

        public string UniqueId { get; private set; }

        public GamePlayerNameOptions Naming { get; set; }

        public Game (SocketUser creator)
        {
            _creator = creator;
            _participants = new List<SocketUser>();
            _names = new List<string>();
            _suggestions = new List<string>();

            UniqueId = GenerateUniqueId(creator.Username);
            Naming = GamePlayerNameOptions.Username;
        }

        private string GenerateUniqueId(string user)
        {
            return $"{user.Normalize()}s Game ({DateTime.Now.ToString("yyyyMMddHHmmss")})";
        }

        public bool IsCreator(SocketUser u)
        {
            return u.Id == u.Id;
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

        public async Task Cancel()
        {
            await Thread.DeleteAsync();
            await SettingsMessage?.DeleteAsync();
            await InviteMessage?.DeleteAsync();
        }
    }
}
