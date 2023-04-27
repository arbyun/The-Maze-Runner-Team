using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.Inventory
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "The Maze Runner/Create", order = 0)]
    public class Item : ScriptableObject
    {
        public bool stackable;
        public int Count;
        public string itemName;
        public Sprite itemIcon;
        public ItemType itemType;
        public Guid id = Guid.NewGuid();
        public List<ItemTags> tags = new List<ItemTags>();
        public float quality;
    }
}