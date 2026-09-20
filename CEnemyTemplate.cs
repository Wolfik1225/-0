using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp50
{
    public class CEnemyTemplate
    {
        // Свойства класса — доступны для чтения снаружи,
        // изменяются только изнутри класса (через конструктор)
        public string Name { get; private set; }
        public string IconName { get; private set; }

        public int BaseLife { get; private set; }
        public double LifeModifier { get; private set; }

        public int BaseGold { get; private set; }
        public double GoldModifier { get; private set; }

        public double SpawnChance { get; private set; }

        // Конструктор класса
        public CEnemyTemplate(string name, string iconName, int baseLife,
                               double lifeModifier, int baseGold,
                               double goldModifier, double spawnChance)
        {
            Name = name;
            IconName = iconName;
            BaseLife = baseLife;
            LifeModifier = lifeModifier;
            BaseGold = baseGold;
            GoldModifier = goldModifier;
            SpawnChance = spawnChance;
        }
    }
}
