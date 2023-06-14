using UnityEngine;

namespace Inventory
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager Instance;
        internal InventoryInstance inventory;
        [SerializeField] private UIInventory uiInventory;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            inventory = new InventoryInstance();
            uiInventory.SetInventory(inventory);
        }
    }
}