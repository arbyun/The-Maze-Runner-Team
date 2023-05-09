using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

namespace Utilities
{
    public class DoorController3 : MonoBehaviour
    {
        public GameObject player;
        public GameObject door;
        public GameObject[] objectsToMakeTransparent;
        public float transparencyChangeSpeed = 0.5f;

        private bool _isPlayerColliding;
        private static bool _isEnteredFromDoor = false;
        private const string EnteredFromDoorKey = "enteredFromDoor";

        void Start()
        {
            // Read the value of isEnteredFromDoor from PlayerPrefs
            if (PlayerPrefs.HasKey(EnteredFromDoorKey))
            {
                _isEnteredFromDoor = PlayerPrefs.GetInt(EnteredFromDoorKey) == 1;
            }
        }

        void Update()
        {
            if (_isPlayerColliding && Input.GetKeyDown(KeyCode.I))
            {
                StartCoroutine(zoomIn());
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == player)
            {
                _isPlayerColliding = true;
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == player)
            {
                _isPlayerColliding = false;
            }
        }

        IEnumerator zoomIn()
        {
            // Set isEnteredFromDoor to true if the player entered the level through a door
            _isEnteredFromDoor = true;

            // Zoom in on the player and the door using Cinemachine
            Cinemachine.CinemachineVirtualCamera vcam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
            vcam.Follow = player.transform;
            vcam.LookAt = door.transform;

            Cinemachine.CinemachineBasicMultiChannelPerlin noise =
                vcam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = 2.0f;

            yield return new WaitForSeconds(1.5f);

            noise.m_AmplitudeGain = 0.0f;

            // Save the value of isEnteredFromDoor to PlayerPrefs
            PlayerPrefs.SetInt(EnteredFromDoorKey, _isEnteredFromDoor ? 1 : 0);

            // Load the next scene
            SceneManager.LoadScene("NextLevelScene");
        }

        void setTransparent(GameObject obj)
        {
            Color objColor = obj.GetComponent<SpriteRenderer>().color;
            objColor.a = 0.0f;
            obj.GetComponent<SpriteRenderer>().color = objColor;
        }
        
        static void setVisible(GameObject obj, float alpha)
        {
            Color objColor = obj.GetComponent<SpriteRenderer>().color;
            objColor.a = alpha;
            obj.GetComponent<SpriteRenderer>().color = objColor;
        }

        void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject == player)
            {
                // Gradually make the objects in the "objectsToMakeTransparent" array visible as the player moves through the door
                for (int i = 0; i < objectsToMakeTransparent.Length; i++)
                {
                    float alpha = (i + 1) / (float)objectsToMakeTransparent.Length;

                    if (_isEnteredFromDoor)
                    {
                        alpha = 1.0f - alpha; // Reverse alpha value if entered through a door
                    }

                    setVisible(objectsToMakeTransparent[i], alpha * transparencyChangeSpeed);
                }
            }
        }
    }
}

   
