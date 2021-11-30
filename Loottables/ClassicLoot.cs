using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Loottables
{
    public class ClassicLoot : Loottable
    {
        public ClassicLoot()
        {
            Loot = new List<Equipment>
            {
                              //Name, ?Killmessage, Power, CritChance, Healing, Protection
                new Equipment("Knife", "stabbed {b} using their {equipment}.", 2, 0, 0, 0),
                new Equipment("Knife", "stabbed {b} using their {equipment}.", 2, 0, 0, 0),
                new Equipment("Knife", "stabbed {b} using their {equipment}.", 2, 0, 0, 0),
                new Equipment("Machete", 3, 0, 0, 0),
                new Equipment("Crowbar", 3, 5, 0, 0),
                new Equipment("Bandage", 0, 0, 2, 0),
                new Equipment("Bandage", 0, 0, 2, 0),
                new Equipment("Bandage", 0, 0, 2, 0),
                new Equipment("Canned Food", 0, 0, 2, 0),
                new Equipment("Canned Food", 0, 0, 2, 0),
                new Equipment("Canned Food", 0, 0, 2, 0),
                new Equipment("Helmet", 0, 0, 0, 1),
                new Equipment("Tactical Gloves", 0, 0, 0, 1),
                new Equipment("Light Armor", 0, 0, 0, 1),
                new Equipment("Medium Armor", 0, 0, 0, 2),
                new Equipment("Heavy Armor", 0, 0, 0, 4),
                new Equipment("Riot Shield", 2, 0, 0, 2),
                new Equipment("Medkit", 0, 0, 4, 0),
                new Equipment("RPG", "exploded {b} using their {equipment}.", 7, 0, 0, 0),
                new Equipment("L118A", "sniped {b} using their {equipment}.", 4, 70, 0, 0),
                new Equipment("Barrett 50CAL", "sniped {b} using their {equipment}.", 7, 0, 0, 0),
                new Equipment("Minigun", "mowed {b} down using their {equipment}.", 7, 50, 0, 0),
                new Equipment("AK47", "gave {b} a few piercings using their {equipment}.", 4, 30, 0, 0),
                new Equipment("M16", 4, 15, 0, 0),
                new Equipment("MP9", 3, 15, 0, 0),
                new Equipment("Skorpion", 3, 15, 0, 0),
                new Equipment("Five Seven", 4, 10, 0, 0),
                new Equipment("Scar L", 6, 0, 0, 0),
                new Equipment("Deagle", 5, 5, 0, 0)
            };
        }
    }
}
