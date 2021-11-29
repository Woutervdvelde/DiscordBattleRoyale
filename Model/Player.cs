using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Player
    {
        public static int MaxHealth = 10;

        public string Name { get; set; }
        public List<Equipment> Inventory { get; set; }
        private int _health { get; set; }
        //Calculate health doesn't work correctly
        public int Health { get => _health; set => _health = value > MaxHealth ? value : MaxHealth; }
        public int Kills { get; set; }
        public bool IsAlive { get => _health > 0; }
        public List<string> CurrentMessage { get; set; }
        public Equipment BestFightEquipment { 
            get 
            {
                if (Inventory.Count == 0) return Equipment.Fists;
                Equipment best = Inventory.OrderBy(e => e.Power)?.First();
                if (best.Power == 0)
                    return Equipment.Fists;
                return best;
            }
        }

        public Equipment BestConsumableEquipment
        {
            get
            {
                if (Inventory.Count == 0) return null;
                Equipment equipment = Inventory.OrderBy(e => e.Healing)?.First();
                if (equipment != null && equipment.Healing > 0)
                    return equipment;
                else
                    return null;
            }
        }

        public Player(string name, List<Equipment> equipment)
        {
            Name = name;
            Inventory = equipment;
            Health = MaxHealth;
            Kills = 0;
            CurrentMessage = new List<string>();
        }

        public void AddMessage(string message)
        {
            CurrentMessage.Add($"{Name}[{Health}] {message}");
        }

        public void Hurt(int damage)
        {
            int armor = Inventory.Sum(e => e.Protection);
            Health -= damage;
        }

        public void Rest()
        {
            if (Health < 10)
            {
                Equipment equipment = BestConsumableEquipment;
                if (equipment != null)
                    Heal(equipment);
                else
                    Health++; AddMessage("took a rest.");
            }
            else
                AddMessage("is roaming.");
        }

        private void Heal(Equipment equipment)
        {
            Health += equipment.Healing;
            AddMessage($"used {equipment.Name} to heal({equipment.Healing})");
            if (equipment.Protection == 0)
                Inventory.Remove(equipment);
        }

        public void Kill(Player b)
        {
            Equipment equipment = BestFightEquipment;
            if (equipment.Killmessage == null)
                AddMessage($"killed {b.Name}[{b.Health}] using their {equipment.Name}.");
            else
                AddMessage("");
        }

        public void Fight(Player b)
        {
            b.Hurt(BestFightEquipment.Damage);
            if (b.Health <= 0)
                Kill(b);
            else
            {
                Hurt(b.BestFightEquipment.Damage);
                if (Health <= 0)
                    b.Kill(this);
                else
                    AddMessage($"and {b.Name}[{b.Health}] fight.");
            }
        }

        public void Ambush(Player b)
        {
            b.Hurt(BestFightEquipment.Damage);
            if (b.Health <= 0)
            {
                AddMessage($"ambushed and killed {b.Name}[{b.Health}] using their {BestFightEquipment.Name}.");
                Kills++;
                b.Inventory.ForEach(e => Inventory.Add(e));
            }
            else
            {
                AddMessage($"ambushed {b.Name}[{b.Health}");
            }

        }
    }
}
