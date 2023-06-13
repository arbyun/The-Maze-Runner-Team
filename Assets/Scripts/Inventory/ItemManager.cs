using UnityEngine;

namespace Inventory
{
    public class ItemManager : MonoBehaviour
    {
        private InventoryInstance _inventory;
        [SerializeField] private UIInventory uiInventory;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _inventory = new InventoryInstance();
            uiInventory.SetInventory(_inventory);
        }
    }
}