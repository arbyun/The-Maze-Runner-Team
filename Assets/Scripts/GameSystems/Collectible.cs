using UnityEngine;

namespace GameSystems
{
    public class Collectible : MonoBehaviour
    {
        private int _quantity;

        /// <summary> Adds the item to the inventory system.</summary>
        /// <returns> The gameobject</returns>
        private void AddToInventory()
        {
            _quantity++;

            for (int i = 0; i < _quantity; i++)
            {
                InventorySystem.AddItem(gameObject);
            }
        }

        /// <summary> Pick up collectible when you collide with it </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Player player = other.GetComponent<Player>();

                if (player != null)
                {
                    AddToInventory();
                }
            }
        }
    }
}