using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp50
{
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    using System.Collections.Generic;
    public class CEnemyTemplateList
    {
        // Список противников из класса CEnemyTemplate
        private List<CEnemyTemplate> enemies;

        public CEnemyTemplateList()
        {
            enemies = new List<CEnemyTemplate>();
        }

        // Добавление нового противника в список
        public void AddEnemy(string name, string iconName, int baseLife,
                              double lifeModifier, int baseGold,
                              double goldModifier, double spawnChance)
        {
            CEnemyTemplate enemy = new CEnemyTemplate(name, iconName, baseLife,
                                                        lifeModifier, baseGold,
                                                        goldModifier, spawnChance);
            enemies.Add(enemy);
        }

        // Поиск противника по имени
        public CEnemyTemplate GetEnemyByName(string name)
        {
            foreach (CEnemyTemplate enemy in enemies)
            {
                if (enemy.Name == name)
                    return enemy;
            }
            return null; // если не нашли — вернём "пусто"
        }

        // Поиск противника по индексу (порядковому номеру в списке)
        public CEnemyTemplate GetEnemyByIndex(int id)
        {
            if (id >= 0 && id < enemies.Count)
                return enemies[id];
            return null;
        }

        // Удаление противника по имени
        public void DeleteEnemyByName(string name)
        {
            CEnemyTemplate enemy = GetEnemyByName(name);
            if (enemy != null)
                enemies.Remove(enemy);
        }

        // Удаление противника по индексу
        public void DeleteEnemyByIndex(int id)
        {
            if (id >= 0 && id < enemies.Count)
                enemies.RemoveAt(id);
        }

        // Получить список всех имён противников (пригодится для отображения в интерфейсе)
        public List<string> GetListOfNames()
        {
            List<string> names = new List<string>();
            foreach (CEnemyTemplate enemy in enemies)
            {
                names.Add(enemy.Name);
            }
            return names;
        }
        // Сохранение списка противников в файл
        public void SaveToJson(string path)
        {
            string jsonString = JsonSerializer.Serialize(enemies);
            File.WriteAllText(path, jsonString);
        }

        // Загрузка списка противников из файла
        public void LoadFromJson(string path)
        {
            string jsonFromFile = File.ReadAllText(path);
            enemies = new List<CEnemyTemplate>();

            JsonDocument doc = JsonDocument.Parse(jsonFromFile);
            foreach (JsonElement element in doc.RootElement.EnumerateArray())
            {
                string name = element.GetProperty("Name").GetString();
                string iconName = element.GetProperty("IconName").GetString();
                int baseLife = element.GetProperty("BaseLife").GetInt32();
                double lifeModifier = element.GetProperty("LifeModifier").GetDouble();
                int baseGold = element.GetProperty("BaseGold").GetInt32();
                double goldModifier = element.GetProperty("GoldModifier").GetDouble();
                double spawnChance = element.GetProperty("SpawnChance").GetDouble();

                CEnemyTemplate enemy = new CEnemyTemplate(name, iconName, baseLife,
                                                            lifeModifier, baseGold,
                                                            goldModifier, spawnChance);
                enemies.Add(enemy);
            }
        }
    }
}
