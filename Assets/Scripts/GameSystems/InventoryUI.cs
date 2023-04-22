using System;
using UnityEngine;

namespace GameSystems
{
    [RequireComponent(typeof(Collider2D))]
    public class InventoryUI : MonoBehaviour
    {
        private void Start()
        {
            throw new NotImplementedException();
        }

        /// <summary> Called when the player clicks on a weapon in their inventory.        
        /// It will equip that weapon and unequip any other weapons currently equipped.</summary>
        /// <param name="weapon"> /// </param>
        /// <returns> The weapon that is clicked on.</returns>
        public void OnWeaponClicked(GameObject weapon)
        {
            InventorySystem inventory = GetComponent<InventorySystem>();
            InventorySystem.EquipWeapon(weapon);
        }
    }
}