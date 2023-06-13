using Inventory;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Real_Prefabs.UI_Types
{
    public class WeaponSwitchUI : MonoBehaviour
    {
        private Image _slot1Image;
        private Image _slot2Image;

        private static Weapon _currentWeapon;
        private static Weapon _offhandWeapon;
        //private Sprite _sprite;
        //private Sprite _sprite1;

        public GameObject slot1;
        public GameObject slot2;
        
        private PlayerController _playerController;

        /// <summary> Sets the slot 2 Image and slot 1 Image to be equal to the image component of this game object.
        /// It also sets _sprite and _sprite 1 equal to their respective sprites, which are set in
        /// Unity's inspector.</summary>
        private void Start()
        {
            _playerController = GlobalController.Instance.player.GetComponent<PlayerController>();
            /*slot2Image = GetComponent<Image>();
            slot1Image = GetComponent<Image>();
            _sprite1 = _offhandWeapon.GetComponent<Sprite>();
            _sprite = _currentWeapon.GetComponent<Sprite>();
            */_currentWeapon = _playerController.equippedWeapon;
            _offhandWeapon = _playerController.offhandWeapon;

            _slot1Image = slot1.GetComponent<Image>();
            _slot2Image = slot2.GetComponent<Image>();
        }

        /// <summary> Updates the UI to reflect the current weapon and offhand weapon.</summary>
        /// <returns> The current weapon and offhand weapon with the sprite assigned to it.</returns>
        private void Update()
        {
            if (_currentWeapon is not null)
            {
                _currentWeapon.TryGetComponent(out Image currentwpn);
                _slot1Image.sprite = currentwpn.sprite;
                _slot1Image.fillAmount = 1;
            }
            else if (_offhandWeapon is not null)
            {
                _offhandWeapon.TryGetComponent(out Image offwpn);
                _slot2Image.sprite = offwpn.sprite;
                _slot2Image.fillAmount = 1;
            }
            else
            {
                _slot1Image.fillAmount = 0;
                _slot2Image.fillAmount = 0;
            }
        }

        /// <summary> Switches the current weapon with the offhand weapon.</summary>
        /// <returns> The current weapon and offhand weapon.</returns>
                
        public static void switchWeapons()
        {
            if (_currentWeapon != _offhandWeapon)
            {
                (_currentWeapon, _offhandWeapon) = (_offhandWeapon, _currentWeapon);
            }
        }

    }
}