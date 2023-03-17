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

        public void OnWeaponClicked(GameObject weapon)
        {
            InventorySystem inventory = GetComponent<InventorySystem>();
            inventory.EquipWeapon(weapon);
        }
    }
}