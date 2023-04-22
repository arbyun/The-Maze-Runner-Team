using UnityEngine;

namespace UI
{
    public class ScreenAnimator : MonoBehaviour
    {
        [SerializeField] private MenuButtonController menuButtonController;
        [SerializeField] private AudioClip mainScreenFX;
        public bool disableOnce;

        /// <summary> Plays a sound effect. Takes an AudioClip as its only parameter.</summary>
        /// <param name="whichSound"> /// this is the sound that will be played.
        /// </param>
        private void PlaySound(AudioClip whichSound)
        {
            if (!disableOnce)
            {
                menuButtonController.audioSource.PlayOneShot(whichSound);
            }
            else
            {
                disableOnce = false;
            }
        }
    }
}