/*using GameSystems;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WeaponSwitchUI : MonoBehaviour
    {
        public Image slot1Image;
        public Image slot2Image;

        private static GameObject _currentWeapon;
        private static GameObject _offhandWeapon;
        private Sprite _sprite;
        private Sprite _sprite1;

        /// <summary> Sets the slot 2 Image and slot 1 Image to be equal to the image component of this game object.
        /// It also sets _sprite and _sprite 1 equal to their respective sprites, which are set in
        /// Unity's inspector.</summary>
        private void Start()
        {
            slot2Image = GetComponent<Image>();
            slot1Image = GetComponent<Image>();
            _sprite1 = _offhandWeapon.GetComponent<Sprite>();
            _sprite = _currentWeapon.GetComponent<Sprite>();
            _currentWeapon = InventorySystem.EquippedWeapon;
            _offhandWeapon = InventorySystem.SecondWeapon;

            slot1Image.color = new Color(255, 255, 255, 0);
            slot2Image.color = new Color(255, 255, 255, 0);
        }

        /// <summary> Updates the UI to reflect the current weapon and offhand weapon.</summary>
        /// <returns> The current weapon and offhand weapon with the sprite assigned to it.</returns>
        private void Update()
        {
            if (_currentWeapon is not null)
            {
                slot1Image.sprite = _sprite;
                slot1Image.color = new Color(255, 255, 255, 255);
            }
            else if (_offhandWeapon is not null)
            {
                slot2Image.sprite = _sprite1;
                slot2Image.color = new Color(255, 255, 255, 255);
            }
            else
            {
                slot1Image.color = new Color(255, 255, 255, 0);
                slot2Image.color = new Color(255, 255, 255, 0);
            }
        }

        /// <summary> Switches the current weapon with the offhand weapon.</summary>
        /// <returns> The current weapon and offhand weapon.</returns>
        public static void SwitchWeapons()
        {
            (_currentWeapon, _offhandWeapon) = (_offhandWeapon, _currentWeapon);
            
            InventorySystem.EquipWeapon(_offhandWeapon);
            InventorySystem.OffHandWeapon(_currentWeapon);
        }
    }
}*/