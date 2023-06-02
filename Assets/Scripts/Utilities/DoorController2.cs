using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

namespace Utilities
{
    public class DoorController2 : MonoBehaviour
    {
        public GameObject player;
        public GameObject door;
        
        public float zoomDuration = 1.0f;
        public float zoomAmount = 2.0f;
        
        public string nextSceneName = "Level2";

        public GameObject level;
        private SpriteRenderer[] _objectsToMakeTransparent;
        
        public float transparencyChangeSpeed = 0.5f;

        private CinemachineVirtualCamera _virtualCamera;

        private bool _isPlayerColliding;
        private static bool _isEnteredFromDoor = false;

        private void Start()
        {
            // Get the virtual camera component
            _virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();

            // Get the door's collider
            var _doorCollider = door.GetComponent<Collider2D>();

            SpriteRenderer[] otmt = level.GetComponentsInChildren<SpriteRenderer>();

            _objectsToMakeTransparent = otmt;
            //or _objectsToMakeTransparent = otmt;
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
                Debug.Log("You're colliding.");
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == player)
            {
                _isPlayerColliding = false;
                Debug.Log("Exited trigger.");
            }
        }

        IEnumerator zoomIn()
        {
            // Set isEnteredFromDoor to true if the player entered the level through a door
            _isEnteredFromDoor = true;
            Debug.Log("Zoomin' in");

            // Zoom in on the player and the door using Cinemachine
            _virtualCamera.Follow = player.transform;
            _virtualCamera.LookAt = door.transform;

            //CinemachineBasicMultiChannelPerlin noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            //noise.m_AmplitudeGain = zoomAmount;
            //or
            var temp = _virtualCamera.m_Lens.OrthographicSize;
            _virtualCamera.m_Lens.OrthographicSize /= zoomAmount;
            Debug.Log("Adding zoom in"); 

            yield return new WaitForSeconds(1.5f);

            //noise.m_AmplitudeGain = 0.0f;
            //or
            _virtualCamera.m_Lens.OrthographicSize = temp;
            Debug.Log("Adding zoom out");

            // Load the next scene and set its objects to be transparent by default
            SceneManager.LoadScene(nextSceneName);
            Debug.Log("Loading next scene");

            foreach (SpriteRenderer obj in _objectsToMakeTransparent)
            {
                setTransparent(obj);
            }
        }

        static void setTransparent(SpriteRenderer obj)
        {
            Color objColor = obj.color;
            objColor.a = 0.0f;
            obj.color = objColor;
            Debug.Log("Making transparent");
        }

        static void setVisible(SpriteRenderer obj, float alpha)
        {
            Color objColor = obj.color;
            objColor.a = alpha;
            obj.color = objColor;
            Debug.Log("Making visible");
        }

        void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject == player && SceneManager.GetSceneByName(nextSceneName).isLoaded)
            {
                // Gradually make the objects in the "objectsToMakeTransparent" array visible as the player moves through the door
                for (int i = 0; i < _objectsToMakeTransparent.Length; i++)
                {
                    float alpha = (i + 1) / (float)_objectsToMakeTransparent.Length;

                    if (_isEnteredFromDoor)
                    {
                        alpha = 1.0f - alpha; // Reverse alpha value if entered through a door
                    }

                    setVisible(_objectsToMakeTransparent[i], alpha * transparencyChangeSpeed);
                }
            }
        }
    }
}
