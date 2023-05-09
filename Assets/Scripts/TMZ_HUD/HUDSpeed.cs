using Player;
using UnityEngine;
using UnityEngine.UI;

namespace TMZ_HUD
{
    public class HUDSpeed : MonoBehaviour
    {
        public Image sprintFill;
        private PlayerController _playerController;

        // Start is called before the first frame update
        void Start()
        {
            _playerController = GlobalController.Instance.player.GetComponent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            bool canSprint = _playerController.isSprintable;
            bool sprintCDDown = _playerController.isSprintReset;

            if (!canSprint && !sprintCDDown)
            {
                sprintFill.fillAmount = 0;
            }
            else if (canSprint && !sprintCDDown)
            {
                sprintFill.fillAmount = 0;
            }
            else if (canSprint && sprintCDDown)
            {
                sprintFill.fillAmount = 1;
            }
        }
    }
}
