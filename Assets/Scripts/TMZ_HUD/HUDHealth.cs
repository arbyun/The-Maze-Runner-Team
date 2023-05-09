using Player;
using UnityEngine;
using UnityEngine.UI;

namespace TMZ_HUD
{
    public class HUDHealth : MonoBehaviour
    {
        public Image healthFill;
        private PlayerController _playerController;
    
        // Start is called before the first frame update
        void Start()
        {
            _playerController = GlobalController.Instance.player.GetComponent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            int healthRemain = _playerController.health;
            float fill = (float)healthRemain / 10;
            healthFill.fillAmount = fill;
        }
    }
}
