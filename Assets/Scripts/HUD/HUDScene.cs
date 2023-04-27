using UnityEngine;
using UnityEngine.SceneManagement;

namespace HUD
{
    public class HUDScene : MonoBehaviour
    {
        public GameObject pauseMenu;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(!pauseMenu.activeSelf);
                Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
            }
        }

        public void LoadMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
        }

    }
}
