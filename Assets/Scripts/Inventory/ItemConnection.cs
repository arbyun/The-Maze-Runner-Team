using Player;
using UnityEngine;

namespace Inventory
{
    public class ItemConnection : MonoBehaviour
    {
        internal Item slotItem;

        public void Drop()
        {
            Destroy(slotItem);
            slotItem = null;
        }

        public void Use()
        {
            foreach (var type in slotItem.types)
            {
                if (type == ItemType.Weapon)
                {
                    Weapon wpn = slotItem.itemGameObject.GetComponent<Weapon>();
                    GlobalController.Instance.player.GetComponent<PlayerController>().equippedWeapon = wpn;
                }
                else if (type == ItemType.Consumable)
                {
                    Consumable consumable = slotItem.itemGameObject.GetComponent<Consumable>();
                    consumable.Use();
                }
            }
        }
    }
}
