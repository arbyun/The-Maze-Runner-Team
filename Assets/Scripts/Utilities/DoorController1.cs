using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

namespace Utilities
{
    public class DoorController1 : MonoBehaviour
    {
        public GameObject player;
        
        public float zoomDuration = 1.0f;
        public float zoomAmount = 2.0f;
        
        public string nextSceneName = "Level2";

        public GameObject level;
        private SpriteRenderer[] _objectsToMakeTransparent;

        private bool _isZooming = false;
        private CinemachineVirtualCamera _virtualCamera;
        private Collider2D _doorCollider;

        private void Start()
        {
            // Get the virtual camera component
            _virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();

            // Get the door's collider
            _doorCollider = GetComponent<Collider2D>();

            SpriteRenderer[] otmt = level.GetComponentsInChildren<SpriteRenderer>();

            _objectsToMakeTransparent = otmt;

            // Set all objects to transparent
            foreach (SpriteRenderer obj in _objectsToMakeTransparent)
            {
                setTransparent(obj);
            }
        }

        private void Update()
        {
            // Check for collision and key press
            if (_isZooming || !Input.GetKeyDown(KeyCode.I) || !_doorCollider.IsTouching(player.GetComponent<Collider2D>()))
            {
                return;
            }

            // Start the zoom
            StartCoroutine(zoomIn());
        }

        private IEnumerator zoomIn()
        {
            Debug.Log("We zoomin'");
            _isZooming = true;

            // Zoom in on the player and the door
            _virtualCamera.Follow = player.transform;
            _virtualCamera.LookAt = _doorCollider.transform;
            _virtualCamera.m_Lens.OrthographicSize += zoomAmount;

            // Wait for the zoom to finish
            yield return new WaitForSeconds(zoomDuration);

            // Load the next scene
            SceneManager.LoadScene(nextSceneName);
        }

        private static void setTransparent(SpriteRenderer obj)
        {
            // Set an object to transparent
            Color c = obj.color;
            c.a = 0.0f;
            obj.color = c;
        }

        private static void setVisible(SpriteRenderer obj, float alpha)
        {
            // Set an object to visible with the given alpha value
            Color c = obj.color;
            c.a = alpha;
            obj.color = c;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Make the objects gradually visible as the player moves through the door
                float alpha = 0.0f;
                float step = 1.0f / _objectsToMakeTransparent.Length;

                foreach (SpriteRenderer obj in _objectsToMakeTransparent)
                {
                    alpha += step;
                    setVisible(obj, alpha);
                }
            }
        }
    }
}
