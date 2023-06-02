using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities.Test_Scripts
{
    public class LevelManager : GlobalController
    {
        /* OBJECTIVES FOR THIS SCRIPT:
         > Preserve the position of the player and camera when entering a sublevel by a door.
         > Fade in sublevel, fade out level.
         > Persistant states between levels.
         > Preload sublevel in the background. */

        public enum LevelType
        {
            MainLevel,
            Sublevel
        }

        #region -------- PRESERVE PLAYER AND CAMERA'S STATE ---------

        private Vector3 _cameraPosition;
        private Quaternion _cameraRotation;
        private Camera _camera;

        #endregion

        #region -------- LOAD IN BACKGROUND / PRELOAD --------

        internal AsyncOperation loadingOperation;
        public string nextScene;
        public LevelType levelType;

        #endregion

        #region --------- FADING EFFECTS ---------
        
        [Tooltip("Check twice if the level gameObject has a CanvasGroup that covers it fully and has an alpha value" +
                 "of 0")]
        public GameObject levelParent;
        public CanvasGroup fadePanel; 

        #endregion

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            var nextObjs = SceneManager.GetSceneByName(nextScene).GetRootGameObjects();
            GameObject panel = GameObject.FindGameObjectWithTag("Panel");

            if (panel == null)
            {
                foreach (var t in nextObjs)
                {
                    if (t.CompareTag($"Panel")) 
                    { 
                        panel = t; 
                        break;
                    }
                }
            }

            var panelGroup = panel.GetComponent<CanvasGroup>();
            panelGroup.alpha = 1f;
            //fadeInLevel(panelGroup);
            player.transform.position = GameObject.FindWithTag("Door").transform.position;
            OnTriggerExitWithDoor();
        }

        #region -------- DOOR HELPER ---------

        internal void OnTriggerEnterWithDoor()
        {
            //if (other.CompareTag("Player"))
            
            // Store the current camera position and rotation
            _cameraPosition = _camera.transform.position;
            _cameraRotation = _camera.transform.rotation;

            // Fade out this level
            StartCoroutine(fadeOutLevel());

            // Preload the next level
            loadingOperation = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
            loadingOperation.allowSceneActivation = false;
            
        }

        internal void OnTriggerExitWithDoor()
        {
            // Set the camera position and rotation to the stored values
            _camera.transform.position = _cameraPosition;
            _camera.transform.rotation = _cameraRotation;

            // Activate the new level
            loadingOperation.allowSceneActivation = true;

            // Fade in the new level
            StartCoroutine(fadeInLevel());
            
        }

        #endregion

        #region -------- FADING OPERATIONS ---------

        private IEnumerator fadeOutLevel()
        {
            fadePanel.gameObject.SetActive(true);

            float elapsedTime = 0;
            while (elapsedTime < 1.0f)
            {
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime);
                fadePanel.alpha = alpha;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            fadePanel.alpha = 1f;

            // Unload this level
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        }

        private IEnumerator fadeInLevel()
        {
            float elapsedTime = 0;
            while (elapsedTime < 1.0f)
            {
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime);
                fadePanel.alpha = alpha;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            fadePanel.alpha = 0f;

            fadePanel.gameObject.SetActive(false);
        }

        #endregion
        
    }
}