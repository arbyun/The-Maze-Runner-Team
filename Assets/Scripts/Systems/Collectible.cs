using UnityEngine;

namespace Systems
{
    public class Collectible : MonoBehaviour
    {
        private int _quantity;

        private void AddToInventory()
        {
            _quantity++;

            for (int i = 0; i < _quantity; i++)
            {
                InventorySystem.AddItem(gameObject);
            }
        }

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