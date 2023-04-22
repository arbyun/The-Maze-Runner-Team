using UnityEngine;

namespace GameSystems
{
    public enum ItemType
    {
        Buff,
        Weapon,
        Consumable
    }
    
    [CreateAssetMenu(fileName = "ItemData", menuName = "The Maze Runner/Items", order = 0)]
    public class ItemData : ScriptableObject
    {
        public string nameKey;
        public string descriptionKey;
        public Texture2D image;
        public ItemType type;
        public int stackSize;
    }
}