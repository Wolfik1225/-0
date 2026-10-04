using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp50
{
    public class Player
    {
        private int lvl;
        private BigNumber gold;
        private BigNumber damage;
        private double damageModifier;
        private BigNumber upgradeCost;
        private double upgradeModifier;

        public int Lvl { get { return lvl; } }
        public BigNumber Gold { get { return gold; } }
        public BigNumber Damage { get { return damage; } }
        public double DamageModifier { get { return damageModifier; } }
        public BigNumber UpgradeCost { get { return upgradeCost; } }
        public double UpgradeModifier { get { return upgradeModifier; } }

        public Player()
        {
            lvl = 1;
            gold = new BigNumber("0");
            damage = new BigNumber("1");
            damageModifier = 1.2;
            upgradeCost = new BigNumber("100");
            upgradeModifier = 1.2;
        }
        // добавляет золото игроку
        public void AddGold(BigNumber amount)
        {
            gold = gold + amount;
        }

        // возвращает урон, который наносит игрок за клик
        public BigNumber DealDamage()
        {
            return damage;
        }

        // пытается улучшить урон за золото; true — если получилось, false — если золота не хватило
        public bool TryUpgrade()
        {
            if (!TrySpendGold(upgradeCost))
                return false;

            lvl++;
            RecalculateStats();
            return true;
        }

        // списывает золото, если его достаточно; true — если получилось
        private bool TrySpendGold(BigNumber amount)
        {
            if (gold < amount)
                return false;

            gold = gold - amount;
            return true;
        }

        // пересчитывает урон и стоимость следующего апгрейда после успешного улучшения
        private void RecalculateStats()
        {
            damage = CalculateTotalDamage();
            upgradeCost = CalculateNextUpgradeCost();
        }

        // считает новый урон с учётом модификатора
        private BigNumber CalculateTotalDamage()
        {
            return damage * damageModifier;
        }

        // считает стоимость следующего апгрейда (растёт быстрее, чем урон — множитель зависит от уровня)
        private BigNumber CalculateNextUpgradeCost()
        {
            return upgradeCost * (upgradeModifier * lvl);
        }
    }
}
