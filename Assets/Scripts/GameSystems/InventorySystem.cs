using System.Collections.Generic;
using UnityEngine;

namespace GameSystems
{
    public class InventorySystem : MonoBehaviour
    {
        private static List<GameObject> _items;
        public static GameObject EquippedWeapon;
        public static GameObject SecondWeapon;

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
        /// Method to equip a weapon via script.
        /// </summary>
        /// <param name="weapon">Weapon to be equipped.</param>
        public static void EquipWeapon(GameObject weapon)
        {
            // If we have the weapon on our inventory
            if (_items.Contains(weapon))
            {
                EquippedWeapon = weapon;
            }
        }

        /// <summary> Used to equip a weapon in the offhand slot.        
        /// </summary>
        /// <param name="weapon"> /// this is the weapon you want to equip in your offhand.
        /// </param>
        /// <returns> The second weapon.</returns>
        public static void OffHandWeapon(GameObject weapon)
        {
            if (_items.Contains(weapon))
            {
                SecondWeapon = weapon;
            }
        }

        /// <summary> Used to unequip a weapon from the player.        
        /// If the weapon being unequipped is the EquippedWeapon, then it will be set to null.
        /// If the weapon being unequipped is not EquippedWeapon, but instead SecondWeapon, then that will be set
        /// to null instead.</summary>
        /// <param name="weapon"> The weapon to unequip </param>
        public static void UnequipWeapon(GameObject weapon)
        {
            if (EquippedWeapon == weapon)
            {
                EquippedWeapon = null;
            }
            else if (SecondWeapon == weapon)
            {
                SecondWeapon = null;
            }
        }

        /// <summary> Adds an item to the list of items.</summary>        
        /// <param name="item"> /// the item to be added to the list of items.
        /// </param>
        public static void AddItem(GameObject item)
        {
            _items.Add(item);
        }
    }
}
