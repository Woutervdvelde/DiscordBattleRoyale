using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class Map
    {
        public int RoundCount { get; set; }
        public List<Player> Players { get; set; }
        public Loottable Loot { get; set; }
        public string PlayerNames { get => string.Join(", ", Players.Select(p => p.Name)); }
        public List<Player> LivingPlayers { get => Players.Where(p => p.Health > 0).ToList(); }
        
        public Player GetRandomPlayer(Player player)
        {
            Player randomPlayer = null;
            while (randomPlayer == null)
            {
                Player p = Players[new Random().Next(Players.Count)];
                if (p.Health > 0 && p != player) 
                    randomPlayer = p;
            }
            return randomPlayer;
        }

        public abstract void playerRoam(Player player);
    }
}
