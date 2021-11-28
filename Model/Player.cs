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
        public List<Equipment> Equipment { get; set; }
        private int _health { get; set; }
        public int Health { get => _health; set => _health = _health + value <= MaxHealth ? _health += value : _health = MaxHealth ; }
        public int Kills { get; set; }
        public List<string> CurrentMessage { get; set; }
        public Equipment BestFightEquipment { 
            get 
            {
                Equipment best = Equipment.OrderBy(e => e.Power)?.First();
                if (best == null || best.Power == 0)
                    return new Equipment("fists", 1, 0, 0, 0);
                return best;
            }
        }

        public Equipment BestConsumableEquipment
        {
            get
            {
                Equipment equipment = Equipment.OrderBy(e => e.Healing)?.First();
                if (equipment != null && equipment.Healing > 0)
                    return equipment;
                else
                    return null;
            }
        }

        public Player(string name, List<Equipment> equipment)
        {
            Name = name;
            Equipment = equipment;
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
            int armor = Equipment.Sum(e => e.Protection);
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
                Equipment.Remove(equipment);
        } 
    }
}
