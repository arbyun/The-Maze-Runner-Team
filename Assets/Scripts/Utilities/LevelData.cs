using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Utilities
{
    [CreateAssetMenu(fileName = "New Level Data", menuName = "TMZ/New.../LevelData")]
    class LevelData : ScriptableObject
    {
        internal enum Subset
        {
            MainScene,
            SubScene,
            Clareira
        }

        public Subset levelType;
        public string winLevelScene;
        public EnemyController[] enemiesInLevel;
        public string[] connectedScenes;
        
        private PlayerController _player;

        private int _undirtyPh;
        private Dictionary<EnemyController, int> _undirtyEn;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            var helper = GameObject.FindGameObjectsWithTag("Enemy");
            var counter = helper.Length;
            enemiesInLevel = new EnemyController[counter];

            /*for (int i = 0; i < counter; i++)
            {
                foreach (var enemy in helper)
                {
                    enemy.TryGetComponent<>(out EnemyController ctrl); // :)

                    if (ctrl != null)
                    {
                        enemiesInLevel[i] = ctrl;
                    }
                }
            }*/
        }

        #region Level Data Storer

        public void save()
        {
            _undirtyPh = _player.health;

            foreach (var enemy in enemiesInLevel)
            {
                _undirtyEn.Add(enemy, enemy.health);
            }
        }

        public void load()
        {
            _player.health = _undirtyPh;

            foreach (var kvp in _undirtyEn)
            {
                var ec = kvp.Key;
                ec.health = kvp.Value;
            }
        }

        #endregion
    }
}