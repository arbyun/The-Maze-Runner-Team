using UnityEngine;

namespace Level_Manager
{
    /// <summary>
    /// This is here to help my classmates set up the levels more easily;
    /// Its purpose is to set up how many enemies we want to spawn on a specific level
    /// </summary>
    [CreateAssetMenu(fileName = "LevelEnemies", menuName = "The Maze Runner/New Enemy Manager", order = 1)]
    public class EnemyManager : ScriptableObject
    {
        public string enemyName;
        public int enemiesPerLevel;
    }
}
