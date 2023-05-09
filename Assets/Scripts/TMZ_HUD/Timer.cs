using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TMZ_HUD
{
    public class Timer : MonoBehaviour
    {
        public float startingTime;
        public TMP_Text timerText;
        
        public UnityEvent gameOver;
        internal float currentTime;

        private void Start()
        {
            currentTime = startingTime;
        }

        private void Update()
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(currentTime / 60);
                int seconds = Mathf.FloorToInt(currentTime % 60);
                timerText.text = $"{minutes:00}:{seconds:00}";
            }
            else
            {
                gameOver.Invoke();
                //GameOver();
            }
        }

        //private void GameOver()
        //{
        //  gameOverHandler.SetActive(true);
        //  Time.timeScale = 0;
        //}

        /// <summary> Resets the timer to its starting time.</summary>
        /// <returns> The current time.</returns>
        public void resetTimer()
        {
            currentTime = startingTime;
        }
    }
}
