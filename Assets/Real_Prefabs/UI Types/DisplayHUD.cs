using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Real_Prefabs.UI_Types
{
    public class DisplayHUD : MonoBehaviour
    {
        public Image healthFill;
        private PlayerController _playerController;
        [SerializeField] private static Image[] _speedBlocks;
        private float _maxSpeed;
        public GameObject healthBarFill;
        
        private void Awake()
        {
            healthFill = healthBarFill.GetComponent<Image>();
            _playerController = GlobalController.Instance.player.GetComponent<PlayerController>();
            _maxSpeed = _playerController.moveSpeed;
        }

        /// <summary> Updates the health bar and speed blocks.</summary>
        private void Update()
        {
            int healthRemain = _playerController.health;
            float currentSpeed = _playerController.moveSpeed;
            float fill = (float)healthRemain / 10;
            healthFill.fillAmount = fill;
            
            bool canSprint = _playerController.isSprintable;
            bool sprintCdDown = _playerController.isSprintReset;
            
            for (int i = 0; i < _speedBlocks.Length; ++i)
            {
                if (!canSprint && !sprintCdDown)
                {
                    _speedBlocks[i].fillAmount = 0;
                }
                else if (canSprint && !sprintCdDown)
                {
                    _speedBlocks[i].fillAmount = 0;
                }
                else if (canSprint && sprintCdDown)
                {
                    _speedBlocks[i].fillAmount = 1;
                }
            }
        }
    }
}