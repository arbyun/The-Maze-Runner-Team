using Player;

namespace Inventory
{
    public class Consumable: InventoryItem
    {
        public int restoreHpAmount;
        
        public void Use()
        {
            GlobalController.Instance.player.GetComponent<PlayerController>().health += restoreHpAmount;
            itemData.quantity -= 1;
        }
    }
}