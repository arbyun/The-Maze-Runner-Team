using UnityEngine;

namespace UI
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private MenuButtonController menuButtonController;
        [SerializeField] private Animator animator;
        [SerializeField] private ScreenAnimator animatorFunctions;
        [SerializeField] private int thisIndex;
        private static readonly int Selected = Animator.StringToHash("selected");
        private static readonly int Pressed = Animator.StringToHash("pressed");

        /// <summary> Checks if the button is selected, and if it is, it plays the animation for being selected.
        /// If the button has been pressed (the player has hit enter), then it plays a different animation.</summary>
        private void Update()
        {
            if (menuButtonController.index == thisIndex)
            {
                animator.SetBool (Selected, true);
                if (Input.GetAxis ("Submit") is 1)
                {
                    animator.SetBool (Pressed, true);
                }
                else if (animator.GetBool (Pressed))
                {
                    animator.SetBool (Pressed, false);
                    animatorFunctions.disableOnce = true;
                }
            }
            else
            {
                animator.SetBool (Selected, false);
            }
        }
    }
}