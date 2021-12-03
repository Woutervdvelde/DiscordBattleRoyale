using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maps;
using Model;
using Loottables;
using Discord;

namespace Controller
{
    public class Game
    {
        private SocketUser _creator { get; set; }
        private List<SocketUser> _participants { get; set; }
        private Map _map { get; set; }
        private List<string> _names { get; set; }
        private List<string> _suggestions { get; set; }

        public bool IsReady { get => _participants.Count > 1; }
        
        protected internal SocketThreadChannel Thread { get; set; }
        protected internal RestUserMessage SettingsMessage { get; set; }
        protected internal RestUserMessage InviteMessage { get; set; }

        public string UniqueId { get; private set; }

        public GamePlayerNameOptions Naming { get; set; }

        public event GameEventHandler GameFinished;

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
            return _creator.Id == u.Id;
        }

        public bool HasParticipant(SocketUser u)
        {
            return _participants.Where(su => su.Id == u.Id).Count() > 0;
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
            await Thread.SendMessageAsync($"{_creator.Username} started the game");
            await SettingsMessage?.DeleteAsync();
            await InviteMessage?.DeleteAsync();
            GenerateNames();
            await Thread.SendMessageAsync($"Participants are: ```{string.Join(", ", _names)}```");
            await InitializeGame();
        }

        public async Task Cancel()
        {
            await Thread.DeleteAsync();
            await SettingsMessage?.DeleteAsync();
            await InviteMessage?.DeleteAsync();
        }

        public async Task Close()
        {
            await Thread.SendMessageAsync("Game finished");
            GameFinished?.Invoke(new GameEventArgs(this));
            GameFinished = null;
        }

        public void GenerateNames()
        {
            switch (Naming)
            {
                case GamePlayerNameOptions.Username:
                    _participants.ForEach(p => _names.Add(p.Username));
                    break;
                case GamePlayerNameOptions.Nickname:
                    _participants.ForEach(p => _names.Add(((SocketGuildUser)p).Nickname));
                    break;
            }
        }

        private async Task InitializeGame()
        {
            //Initializing game with classic game stuff
            _map = new ClassicMap(new ClassicLoot());
            _map.Players = _names.Select(p => new Player(p, new List<Equipment>())).ToList();
            _ = PlayGame(_map);
        }

        private async Task PlayGame(Map map)
        {

            while (true)
            {
                StringBuilder message = new StringBuilder();

                foreach (Player player in map.Players)
                {
                    if (!player.IsAlive) continue;

                    player.CurrentMessage.Clear();
                    map.PlayerRoam(player);
                    player.CurrentMessage.ForEach(m => message.AppendLine(m));
                }

                map.RoundCount++;
                var embed = new EmbedBuilder() { Title = $"Round {map.RoundCount}", Description = message.ToString() };
                await Thread.SendMessageAsync(embed: embed.Build());

                if (map.LivingPlayers.Count > 1)
                    await Task.Delay(5000);
                else break;
            }
            await Close();
        }
    }
}
