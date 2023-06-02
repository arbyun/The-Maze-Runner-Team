using System.Collections.Generic;
using System.IO;
using Player;
using UnityEngine;

namespace Utilities
{
    public class DataManager : MonoBehaviour
    {
        internal static DataManager instance;
        private static KeyBindingManager _keyBindings = new();
        private static PlayerController _playerController = new();
        private static GameConstants _gameConstants = new();

        #region ------------------- Saving Paths ----------------------

        private string _settingsFilePath;
        private string _playerFilePath;
        private string _constantsPath;
        private Dictionary<string, IDataType> _pathAndData;
        private static EnemyController _enemyController;
        private static FallingTrap _trap;
        private static UnstablePlatform _unstablePlatform;
        

        #endregion


        private void Awake()
        {
            _settingsFilePath = Path.Combine(Application.persistentDataPath, "settings.json");
            _playerFilePath = Path.Combine(Application.persistentDataPath, "playerdata.json");
            _constantsPath = Path.Combine(Application.persistentDataPath, "constants.json");
            
            _pathAndData.Add(_settingsFilePath, _keyBindings);
            _pathAndData.Add(_playerFilePath, _playerController);
            _pathAndData.Add(_constantsPath, _gameConstants);
            
            load();
            
            
        }

        private void load()
        {
            foreach (var kvp in _pathAndData)
            {
                if (File.Exists(kvp.Key))
                {
                    string json = File.ReadAllText(kvp.Key);
                    JsonUtility.FromJsonOverwrite(json, kvp.Value);
                }
            }
        }

        public void save()
        {
            foreach (var kvp in _pathAndData)
            {
                string json = JsonUtility.ToJson(kvp.Value);
                File.WriteAllText(kvp.Key, json);
            }
            
        }

        public void saveLevelState(List<EnemyController> enemyHealth, int playerHealth, Trap[] traps)
        {
            Debug.Log("Ayo mah men i ACtually initialize");
            foreach (var enemy in enemyHealth)
            {
                PlayerPrefs.SetInt($"EnemyHealth{enemy.id}", enemy.health);
            }

            foreach (var trap in traps)
            {
                PlayerPrefs.SetString($"TrapState{trap.id}", trap.isDestroyed.ToString());
            }
            
            Debug.Log("yo everythin fine here at datamanager");
            
            PlayerPrefs.SetInt("Health", playerHealth);
        }

        public static void getLevelState()
        {
            foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
            {
                if (PlayerPrefs.HasKey($"EnemyHealth{enemy.id}"))
                {
                    enemy.health = PlayerPrefs.GetInt($"EnemyHealth{enemy.id}");
                }
            }
            
            foreach (Trap trap in FindObjectsOfType<Trap>())
            {
                if (PlayerPrefs.HasKey($"TrapState{trap.name}"))
                {
                    bool isDestroyed = bool.Parse(PlayerPrefs.GetString($"TrapState{trap.name}"));
                    trap.isDestroyed = isDestroyed;
                }
            }

            if (PlayerPrefs.HasKey("Health"))
            {
                _playerController.health = PlayerPrefs.GetInt("Health");
            }
        }

    }
}