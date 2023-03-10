using System;
using UnityEngine;

namespace Systems
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