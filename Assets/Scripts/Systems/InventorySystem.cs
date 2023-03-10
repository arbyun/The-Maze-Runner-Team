using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public class InventorySystem : MonoBehaviour
    {
        private static List<GameObject> _items;
        public static GameObject EquippedWeapon;

        /// <summary>
        /// TODO: Initialize the inventory sys
        /// </summary>
        void Start()
        {
        
        }

        /// <summary>
        /// TODO: Add listener here
        /// </summary>
        void Update()
        {
        
        }

        /// <summary>
        /// Self explanatory
        /// </summary>
        /// <param name="weapon">Weapon to be equipped</param>
        public void EquipWeapon(GameObject weapon)
        {
            // If we have the weapon on our inventory
            if (_items.Contains(weapon))
            {
                EquippedWeapon = weapon;
            }
        }

        public void UnequipWeapon()
        {
            EquippedWeapon = null;
        }

        public void AddItem(GameObject item)
        {
            _items.Add(item);
        }
    }
}
