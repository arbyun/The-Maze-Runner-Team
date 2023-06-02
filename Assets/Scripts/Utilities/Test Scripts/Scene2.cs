using UnityEngine;

namespace Utilities.Test_Scripts
{
    public class Scene2 : MonoBehaviour
    {
        public GameObject[] objectsToHide;

        private void Start()
        {
            // Get the bool value from PlayerPrefs and check if it's true
            bool objectsInvisible = PlayerPrefs.GetInt("objectsInvisible", 0) == 1;

            // Hide all objects if the bool value is true
            if (objectsInvisible)
            {
                foreach (GameObject obj in objectsToHide)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}