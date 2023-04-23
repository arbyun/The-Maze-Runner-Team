using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Level_Manager
{
    public class Timer : MonoBehaviour
    {
        public float startingTime;
        public TMP_Text timerText;
        
        public UnityEvent gameOver;
        internal float CurrentTime;

        private void Start()
        {
            CurrentTime = startingTime;
        }

        private void Update()
        {
            if (CurrentTime > 0)
            {
                CurrentTime -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(CurrentTime / 60);
                int seconds = Mathf.FloorToInt(CurrentTime % 60);
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
        public void ResetTimer()
        {
            CurrentTime = startingTime;
        }
    }
}
