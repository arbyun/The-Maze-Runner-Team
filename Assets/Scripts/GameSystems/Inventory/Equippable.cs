using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Inventory
{
    public class Equippable: ItemContainer
    {
        /// <summary>
        /// Defines what kinds of items can be equipped.
        /// </summary>
        public List<ItemSlot> slots;

        /// <summary>
        /// Used as parent object for new item instances.
        /// </summary>
        public Transform grid;

        /// <summary>
        /// Equipped items will be instantiated in front of equipment slots.
        /// </summary>
        public GameObject itemPrefab;

        private readonly List<InventoryItem> _inventoryItems = new List<InventoryItem>(); 

        public void OnValidate()
        {
            slots = GetComponentsInChildren<ItemSlot>().ToList();
        }

        public override void Refresh()
        {
            Reset();

            foreach (var slot in slots)
            {
                var item = FindItem(slot);

                slot.gameObject.SetActive(item == null);

                if (item != null)
                {
                    var inventoryItem = Instantiate(itemPrefab, grid).GetComponent<InventoryItem>();

                    inventoryItem.item = item;
                    inventoryItem.count.text = null;
                    inventoryItem.transform.position = slot.transform.position;
                    inventoryItem.transform.SetSiblingIndex(slot.transform.GetSiblingIndex());
                    _inventoryItems.Add(inventoryItem);
                    CopyDragReceiver(inventoryItem, slot);
                }
            }
        }

        private void Reset()
        {
            foreach (var inventoryItem in _inventoryItems)
            {
                Destroy(inventoryItem.gameObject);
            }

            _inventoryItems.Clear();
        }

        private Item FindItem(ItemSlot slot)
        {
            var index = slots.Where(i => i.itemType == slot.itemType).ToList().IndexOf(slot);
            var items = Items.Where(i => i.itemType == slot.itemType).ToList();

            return index < items.Count ? items[index] : null;
        }

        private static void CopyDragReceiver(InventoryItem inventoryItem, ItemSlot slot)
        {
            var copy = inventoryItem.gameObject.AddComponent<DragReceiver>();
            var sample = slot.GetComponent<DragReceiver>();

            copy.tweenTargets = new List<Image> { inventoryItem.icon, inventoryItem.frame };
            copy.colorDropAllowed = sample.colorDropAllowed;
            copy.colorDropDenied = sample.colorDropDenied;
            copy.itemTypes = sample.itemTypes;
            copy.itemTags = sample.itemTags;
            copy.group = sample.group;
        }
    }
}