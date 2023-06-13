using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryInstance
    {
        private List<Item> _itemList;
        public event EventHandler OnItemListChanged;

        public InventoryInstance()
        {
            _itemList = new List<Item>();
        }

        public void AddItem(Item item)
        {
            if (item.stackable)
            {
                bool itemAlreadyInInventory = false;
                
                foreach (var inventoryItem in _itemList)
                {
                    if (inventoryItem == item)
                    {
                        inventoryItem.quantity += item.quantity;
                        itemAlreadyInInventory = true;
                    }
                }

                if (!itemAlreadyInInventory)
                {
                    _itemList.Add(item);
                }
            }
            else
            {
                _itemList.Add(item);
            }
            
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }

        public List<Item> GetItemList()
        {
            return _itemList;
        }
    }
}
