using Level_Manager;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TimerUI : MonoBehaviour
    {
        public Timer timer;
        public TMP_Text timerText;

        private void Start()
        {
            timerText = GetComponent<TMP_Text>();
            timer = GetComponent<Timer>();
        }

        /// <summary> Updates the timer text.</summary>
        /// <returns> The timer in minutes and seconds.</returns>
        private void Update()
        {
            int minutes = Mathf.FloorToInt(timer.CurrentTime / 60);
            int seconds = Mathf.FloorToInt(timer.CurrentTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
