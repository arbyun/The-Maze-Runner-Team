using UnityEngine;

namespace Enemies_NPCs
{
    /// <summary>
    /// Types of enemies; each will eventually get its own script and prefab
    /// </summary>
    public enum ETypes
    {
        Stationary,
        Ranged,
        Follower,
        Jumper
    }
    
    public class EnemyTypes : MonoBehaviour
    {
        public ETypes enemyType;
    }
}
