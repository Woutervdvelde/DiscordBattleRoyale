using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Equipment
    {
        public string Name { get; set; }
        public string Killmessage { get; set; }
        public int Power { get; set; }
        public int CritChance { get; set; }
        public int Healing { get; set; }
        public int Protection { get; set; }

        public int Damage
        {
            get
            {
                if (CritChance > 0)
                    if (new Random().Next(1, 100) < CritChance)
                        return Power * 2;
                return Power;
            }
        }

        public static Equipment Fists { get => new Equipment("fists", 1, 0, 0, 0); }

        public Equipment(string Name, string Killmessage, int Power, int CritChance, int Healing, int Protection)
        {
            this.Name = Name;
            this.Killmessage = Killmessage;
            this.Power = Power;
            this.CritChance = CritChance;
            this.Healing = Healing;
            this.Protection = Protection;
        }

        public Equipment(string Name, int Power, int CritChance, int Healing, int Protection) : this(Name, null, Power, CritChance, Healing, Protection) { }
    }
}
