using UnityEngine;

namespace Level_Manager
{
    /// <summary>
    /// Spawns enemies per level, gets its data from the EnemyManager.cs
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        public EnemyManager enemyValue;
        private int _enemyNumber = 1;
        public GameObject[] typesToSpawn;
        private int[] _enemySpawnCounts;
        
        private void Start()
        {
            _enemySpawnCounts = new int[typesToSpawn.Length];
            SpawnEnemies();
        }

        /// <summary> Responsible for spawning enemies in the scene.        
        /// It uses a for loop to generate as many enemies as dictated by the level data in the EnemyManager.cs
        /// </summary>
        /// <returns> An array of GameObjects</returns>
        private void SpawnEnemies()
        {
            // for loop to generate as many enemies as dictated by the level data in the EnemyManager.cs
            for (int i = 0; i < enemyValue.enemiesPerLevel; i++)
            {
                GameObject enemyPrefab = RandomEnemyType();
                enemyPrefab.name = enemyValue.enemyName + _enemyNumber;
                GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

                int enemyIndex = System.Array.IndexOf(typesToSpawn, enemyPrefab);

                _enemySpawnCounts[enemyIndex]++;
                _enemyNumber++;
                i++;
            }
        }
        
        /// <summary>
        /// We want to try to spawn an equal amount of enemies per type; this is an attempt at it
        /// Chooses a random enemy type to spawn. It does this by first calculating the average number of spawns for
        /// each enemy type, then it calculates the chance that each enemy will be spawned based on how many times
        /// they have been spawned already. The more an enemy has been spawned, the less likely it is to be chosen
        /// again.</summary>
        /// <returns> A random GameObject from the TypesToSpawn array</returns>
        private GameObject RandomEnemyType()
        {
            int totalSpawns = 0;

            foreach (int spawnCount in _enemySpawnCounts)
            {
                totalSpawns += spawnCount;
            }

            float averageSpawns = (float)totalSpawns / typesToSpawn.Length;
            float[] diffSpawnChances = new[] { 0f };
            float totalChance = 0f;

            for (int i = 0; i < typesToSpawn.Length; i++)
            {
                float spawnChance = Mathf.Max(0f, 1f - ((float)_enemySpawnCounts[i] - averageSpawns) / averageSpawns);
                diffSpawnChances[i] = spawnChance;
                totalChance += spawnChance;
            }

            float randValue = Random.Range(0f, totalChance);
            for (int i = 0; i < typesToSpawn.Length; i++)
            {
                if (randValue < diffSpawnChances[i])
                {
                    return typesToSpawn[i];
                }

                randValue -= diffSpawnChances[i];
            }
            
            //If we get here something went wrong, lol
            Debug.LogError("Failed to choose an enemy type to spawn, somehow");
            return null;
        }
    }
}
