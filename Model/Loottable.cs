using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class Loottable
    {
        public List<Equipment> Loot { get; set; }
        public int Count { get => Loot.Count; }

        public void RemoveAt(int position)
        {
            Loot.RemoveAt(position);
        }

        public Equipment GetAt(int position)
        {
            return Count > position ? Loot[position] : null;
        }

        public void PlayerLoot(Player player)
        {
            if (Count > 0)
            {
                int position = new Random().Next(Count);
                Equipment equipment = GetAt(position);
                player.Equipment.Add(equipment);
                player.AddMessage($"found {equipment.Name}.");
                RemoveAt(position);
            }
            else
                player.AddMessage("is roaming.");
        }
    }
}
