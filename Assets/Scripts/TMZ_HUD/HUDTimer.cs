using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TMZ_HUD
{
    public class HUDTimer : MonoBehaviour
    {
        public Timer timer;
        public TMP_Text timerText;

        private void Start()
        {
            //timerText = GetComponent<TMP_Text>();
            timer = GetComponent<Timer>();
        }

        /// <summary> Updates the timer text.</summary>
        /// <returns> The timer in minutes and seconds.</returns>
        private void Update()
        {
            int minutes = Mathf.FloorToInt(timer.currentTime / 60);
            int seconds = Mathf.FloorToInt(timer.currentTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
