using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Maps
{
    public class ClassicMap : Map
    {
        public ClassicMap(Loottable loot)
        {
            Loot = loot;
        }

        public void Carepackage(Player player)
        {
            if (Loot.Count > 2)
            {
                player.AddMessage("found a carepackage.");
                Loot.PlayerLoot(player);
                Loot.PlayerLoot(player);
                Loot.PlayerLoot(player);
            }
            else
                player.AddMessage("found an empty carepackage.");
        }

        public override void PlayerRoam(Player player)
        {
            int chance = new Random().Next(0, 13);

            if (chance >= 0 && chance < 6)
                player.Fight(GetRandomPlayer(player));
            if (chance >= 6 && chance < 9)
                Loot.PlayerLoot(player);
            if (chance >= 9 && chance < 11)
                player.Rest();
            if (chance >= 11 && chance < 12)
                player.Ambush(GetRandomPlayer(player));
            if (chance >= 12 && chance < 13)
                Carepackage(player);
        }
    }
}
