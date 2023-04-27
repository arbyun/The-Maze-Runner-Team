using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Inventory
{
    /// <summary>
    /// High-level inventory interface.
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        public Equippable equipment;
        public ScrollInventory bag;
        public Button equipButton;
        public Button removeButton;
        public AudioSource audioSource;
        public AudioClip equipSound;
        public AudioClip removeSound;
        public GameData.Items data;
        //public ItemInfo ItemInfo;

        protected Item SelectedItem;
        protected List<ItemTags> SelectedItemTags;
        protected ItemType SelectedItemType;

        public void Refresh()
        {
            if (SelectedItemTags.Contains(ItemTags.Undefined))
            {
                //ItemInfo.Reset();
                equipButton.interactable = removeButton.interactable = false;
            }
            else
            {
                if (CanEquip())
                {
                    equipButton.interactable = bag.Items.Any(i => i.id == SelectedItem.id)
                                               && equipment.slots.Count(i => i.itemType == SelectedItemType) > equipment.Items.Count(i => i.id == SelectedItem.id);
                    removeButton.interactable = equipment.Items.Any(i => i.id == SelectedItem.id);
                }
                else
                {
                    equipButton.interactable = removeButton.interactable = false;
                }
            }
        }

        protected void Reset()
        {
            if (SelectedItem == null)
            {
                equipButton.interactable = removeButton.interactable = false;
            }
            else
            {
                if (CanEquip())
                {
                    equipButton.interactable = bag.Items.Any(i => i.id == SelectedItem.id)
                                               && equipment.slots.Count(i => i.itemType == SelectedItemType) > 
                                               equipment.Items.Count(i => i.id == SelectedItem.id);
                    removeButton.interactable = equipment.Items.Any(i => i.id == SelectedItem.id);
                }
                else
                {
                    equipButton.interactable = removeButton.interactable = false;
                }
            }
        }

        protected void MoveItem(Item item, ItemContainer from, ItemContainer to)
        {
            if (to.expanded)
            {
                to.Items.Add(item);
            }
            else
            {
                var target = to.Items.SingleOrDefault(i => i.id == item.id);

                if (target == null)
                {
                    to.Items.Add(item);
                }
                else
                {
                    target.Count++;
                }
            }

            if (from.expanded)
            {
                from.Items.Remove(from.Items.Last(i => i.id == item.id));
            }
            else
            {
                var target = from.Items.Single(i => i.id == item.id);

                if (target.Count > 1)
                {
                    target.Count--;
                }
                else
                {
                    from.Items.Remove(target);
                }
            }

            Refresh();
            from.Refresh();
            to.Refresh();
        }

        /// <summary>
        /// Initialize owned items (just for example).
        /// </summary>
        public void Awake()
        {
            var inventory = data.OwnedItems;

            var equipped = new List<Item>();

            bag.Initialize(ref inventory);
            equipment.Initialize(ref equipped);
        }

        protected void Start()
        {
            Reset();
            equipButton.interactable = removeButton.interactable = false;

            // TODO: Assigning static callbacks. Don't forget to set null values when UI will be closed. You can also
            // use events instead.
            InventoryItem.OnItemSelected = SelectItem;
            InventoryItem.OnDragStarted = SelectItem;
            InventoryItem.OnDragCompleted = InventoryItem.OnDoubleClick = item => { if (bag.Items.Contains(item)) 
                Equip(); else Remove(); };
        }

        /// <summary> The SelectItem function is used to select an item from the inventory.        
        /// &lt;para&gt;The function takes in a parameter of type ItemId, which is an enum that contains all the items in the game.&lt;/para&gt;
        /// &lt;para&gt;The SelectedItem variable is set to whatever item was passed into this function.&lt;/para&gt;
        /// &lt;para&gt;SelectedItemParams are then set equal to Items.Params[itemId], which returns a dictionary containing all of that specific item's parameters.&lt;/para&gt;</summary>
        /// <param name="item"> The item to select.</param>
        /// <returns> The selecteditem.</returns>
        public void SelectItem(Item item)
        {
            Guid itemId = item.id;
            SelectedItem.id = itemId;
            SelectedItemTags = item.tags;
            SelectedItemType = item.itemType;
            Refresh();
        }

        /// <summary> The Equip function is called when the player clicks on an item in their inventory.        
        /// It checks if there is already an item equipped of that type, and if so, it moves it to the bag.
        /// If the selected item has a TwoHanded tag, then any shield currently equipped will be moved to the bag.
        /// If a shield is being equipped and there's a two-handed weapon currently equipped, that weapon will be moved to the bag.</summary>
        ///
        ///
        /// <returns> The last item in the equipment list that has the same type as selected item</returns>
        public void Equip()
        {
            var equipped = equipment.Items.LastOrDefault(i => i.itemType == SelectedItemType);

            if (equipped != null)
            {
                AutoRemove(SelectedItemType, equipment.slots.Count(i => i.itemType == SelectedItemType));
            }

            MoveItem(SelectedItem, bag, equipment);
            audioSource.PlayOneShot(equipSound);
        }

        /// <summary> The Remove function removes the selected item from the equipment and places it in the bag.</summary>        
        ///
        ///
        /// <returns> The item that was removed</returns>
        public void Remove()
        {
            MoveItem(SelectedItem, equipment, bag);
            SelectItem(equipment.Items.FirstOrDefault(i => i.id == SelectedItem.id) ?? bag.Items.Single(i => 
                i.id == SelectedItem.id));
            audioSource.PlayOneShot(removeSound);
        }

        /// <summary> The CanEquip function checks if the selected item can be equipped.        
        /// It does this by checking if any of the equipment slots have an ItemType that matches the SelectedItemParams.Type, and also has all of the tags in SelectedItemParams.Tags.</summary>
        ///
        ///
        /// <returns> A boolean value, which is true if the item type and tags of the selected item are contained in any of the equipment slots.</returns>
        private bool CanEquip()
        {
            return equipment.slots.Any(i => i.itemType == SelectedItemType && i.itemTags.All(j => 
                SelectedItemTags.Contains(j)));
        }

        /// <summary>
        /// Automatically removes items if target slot is busy.
        /// </summary>
        /// <summary> The AutoRemove function is used to automatically remove items from the player's equipment when they reach a certain amount.        
        /// &lt;para&gt;The function takes two parameters: an ItemType and an int.&lt;/para&gt;
        /// &lt;list type=&quot;bullet&quot;&gt;
        /// &lt;item&gt;&lt;description&gt;ItemType - The item type that will be removed if there are too many of them in the player's equipment.&lt;/description&gt;&lt;/item&gt;
        /// &lt;item&gt;&lt;description&gt;int - The maximum number of items allowed in the player's equipment before one is removed.&lt;/description&gt;&lt;/item&gt;&lt;/list&gt;</summary>
        ///
        /// <param name="itemType"> The type of item to be removed</param>
        /// <param name="max"> The maximum number of items allowed in the inventory</param>
        ///
        /// <returns> The last item in the list if it's not selected, otherwise it returns the last item in the list.</returns>
        private void AutoRemove(ItemType itemType, int max)
        {
            var items = equipment.Items.Where(i => i.itemType == itemType).ToList();
            long sum = 0;

            foreach (var p in items)
            {
                sum += p.Count;
            }

            if (sum == max)
            {
                MoveItem(items.LastOrDefault(i => i.id != SelectedItem.id) ?? items.Last(), equipment, bag);
            }
        }
    }
}
