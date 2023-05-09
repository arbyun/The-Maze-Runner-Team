using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities.Test_Scripts
{
    public class Scene1 : MonoBehaviour
    {
        private void Start()
        {
            // Set the bool value to true
            PlayerPrefs.SetInt("objectsInvisible", 1);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Load Scene 2
                SceneManager.LoadScene("Scene2");
            }
        }
    }
}