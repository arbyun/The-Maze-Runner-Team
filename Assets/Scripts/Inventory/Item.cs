using System;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "TMZ/New.../ItemData", order = 1)]
    public class Item : ScriptableObject
    {
        public string itemName;
        public Sprite itemInventoryImage;
        public Sprite itemInGameImage;
        public GameObject itemGameObject;
        public bool stackable;
        public int quantity;
        public bool equippable;
        public Guid id = new Guid();
        public ItemType[] types; [Tooltip("Choose only two at max.")]

        private void OnValidate()
        {
            if (!itemGameObject)
            {
                GameObject gm = new GameObject($"{itemName}");
                itemGameObject = gm;
            }
        }
    }
}