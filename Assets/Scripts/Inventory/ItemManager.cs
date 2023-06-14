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
            _inventory = new InventoryInstance();
            uiInventory.SetInventory(_inventory);
        }
    }
}