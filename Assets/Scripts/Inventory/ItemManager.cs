using UnityEngine;

namespace Inventory
{
    public class ItemManager : MonoBehaviour
    {
        public ItemManager Instance;
        internal InventoryInstance inventory;
        [SerializeField] private UIInventory uiInventory;

        private ItemManager()
        {
            Instance = this;
        }

        private void Awake()
        {
            if (Instance == null) 
            {
                Instance = this;
            }

            DontDestroyOnLoad(gameObject);
            inventory = new InventoryInstance();
            uiInventory.SetInventory(inventory);
        }
    }
}