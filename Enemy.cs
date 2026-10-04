using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp50
{
    using System.Windows.Media.Imaging;

    public class Enemy
    {
        private string name;
        private BigNumber maxHitPoints;
        private BigNumber currentHitPoints;
        private BigNumber goldReward;
        private bool isDead;
        private EnemyIcon icon;

        public string Name { get { return name; } }
        public BigNumber MaxHitPoints { get { return maxHitPoints; } }
        public BigNumber GoldReward { get { return goldReward; } }
        public BigNumber CurrentHitPoints { get { return currentHitPoints; } }
        public bool IsDead { get { return isDead; } }
        public EnemyIcon Icon { get { return icon; } }

        // создаёт нового противника на основе шаблона
        public Enemy(CEnemyTemplate template, EnemyIcon icon)
        {
            name = template.Name;
            maxHitPoints = new BigNumber(template.BaseLife.ToString());
            currentHitPoints = maxHitPoints;
            goldReward = new BigNumber(template.BaseGold.ToString());
            isDead = false;
            this.icon = icon;
        }

        // обрабатывает урон; true — если противник побеждён этой атакой
        public bool TakeDamage(BigNumber dmg, out BigNumber goldReward)
        {
            if (currentHitPoints < dmg || currentHitPoints.ToString() == dmg.ToString())
            {
                // противник погибает
                currentHitPoints = new BigNumber("0");
                goldReward = this.goldReward;
                Die();
                return true;
            }

            currentHitPoints = currentHitPoints - dmg;
            goldReward = new BigNumber("0");
            return false;
        }

        // помечает противника как мёртвого
        private void Die()
        {
            isDead = true;
        }
    }
}
