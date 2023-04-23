using UnityEngine;

namespace GameSystems.Inventory
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "The Maze Runner/Create", order = 0)]
    public class Item : ScriptableObject
    {
        public bool stackable;
        public string itemName;
        public Sprite itemIcon;
        public ItemType itemType;
        public float id;

        public void Use()
        {
            switch (itemType)
            {
                case ItemType.Weapon:
                    //GetUseWeapon();
                    break;
                case ItemType.Buff:
                    //Add();
                    break;
                case ItemType.Consumable:
                    //Consume();
                    break;
            }
        }
    }
}