using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class UIInventory : MonoBehaviour
    {
        private InventoryInstance _inventory;
        private Transform _itemSlotContainer;
        private Transform _itemSlotTemplate;
        [SerializeField] private Item[] debugItems;
        
        private int _maxContainers;

        public void SetInventory(InventoryInstance inventory)
        {
            this._inventory = inventory;
            inventory.OnItemListChanged += Inventory_OnItemListChanged;
            RefreshInventoryItems();
        }

        private void Awake()
        {
            _itemSlotContainer = transform.Find("Inventory Slots");
            _itemSlotTemplate = transform.Find("Item Slots");
            _maxContainers = _itemSlotContainer.transform.childCount;
            
            foreach (var item in debugItems)
            {
                _inventory.AddItem(item);
            }
        }
        
        private void RefreshInventoryItems()
        {
            int containerIndex = 0;

            // Loop through each item
            foreach (var item in _inventory.GetItemList())
            {
                // Check if all containers are already full
                if (containerIndex >= _maxContainers)
                {
                    Debug.Log("Error: No space left in containers.");
                    return;
                }

                // Get the next available container
                var container = _itemSlotContainer.transform.GetChild(containerIndex).gameObject;

                // Activate the container
                container.SetActive(true);

                TextMeshProUGUI text = container.GetComponent<TextMeshProUGUI>();

                text.SetText(item.quantity > 1 ? item.quantity.ToString() : "");

                var it = container.GetComponent<ItemConnection>();

                it.slotItem = item;


                // Get the Image component of the container
                var image = container.GetComponent<Image>();

                // Set the sprite of the container to the item's inventory image
                image.sprite = item.itemInventoryImage;

                containerIndex++;
            }
        }

        private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
        {
            RefreshInventoryItems();
        }
    }
}
