using System;
using System.IO;
using UnityEditor;
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
        public int quantity = 1;
        public bool equippable;
        public Guid id = new Guid();
        public ItemType[] types;
    }
}