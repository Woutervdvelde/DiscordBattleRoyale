using NUnit.Framework;
using System.Collections.Generic;
using Model;

namespace GameTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BestFightEquipment_ReturnStrongest()
        {
            Equipment e1 = new Equipment("NUnit_Sword", 10, 0, 0, 0);
            Equipment e2 = new Equipment("NUnit_Knife", 8, 0, 0, 0);
            Equipment e3 = new Equipment("NUnit_Crowbar", 5, 0, 0, 0);
            Player player = new Player("NUnit_Player", new List<Equipment>() { e1, e2, e3 });

            Assert.AreEqual(e1.Name, player.BestFightEquipment.Name);
        }

        [Test]
        public void BestFightEquipment_ReturnFistWhenNoEquipment()
        {
            Player player = new Player("NUnit_Player", new List<Equipment>());

            Assert.AreEqual(Equipment.Fists.Name, player.BestFightEquipment.Name);
        }
    }
}