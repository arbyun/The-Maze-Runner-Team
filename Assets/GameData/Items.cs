using System.Collections.Generic;
using System.Linq;
using GameSystems.Inventory;
using UnityEngine;

namespace GameData
{
    public class Items: MonoBehaviour
    {
        internal Dictionary<string, Item> ItemDataDictionary = new Dictionary<string, Item>();
        internal List<Item> OwnedItems = new List<Item>();

        private void Start()
        {
            LoadItems();
        }

        private void Update()
        {
            LoadItems();
        }

        private void GiveItem(Item item, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                OwnedItems.Add(item);
            }
        }

        private void UseConsumableItem(Item item)
        {
            OwnedItems.Remove(item);
        }

        private void LoadItems()
        {
            Item[] itemDataArray = Resources.LoadAll<Item>(GameConstants.FolderItems); // load all .asset files in the folder

            foreach (Item itemData in itemDataArray)
            {
                ItemDataDictionary[itemData.itemName] = itemData; // add item data to the dictionary using the item name as the key
            }
        }

        internal List<Item> GetItemsByQuality(float quality)
        {
            List<Item> items = ItemDataDictionary.Values.ToList(); // get all item data as a list
            items.Sort((a, b) => a.quality.CompareTo(b.quality)); // sort items by weight
            return items.FindAll(item => item.quality == quality); // find items with the specified weight
        }
        
        //call it in rewards script by using
        //List<Item> itemsByQuality = GetComponent<Items>().GetItemsByQuality(10f);
    }
}