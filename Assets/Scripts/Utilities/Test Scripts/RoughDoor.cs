using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utilities.Test_Scripts
{
    public class RoughDoor : GlobalController
    {
        public string nextSceneName;
        //public Transform spawnPoint;
        private DataManager _data;
        
        private Vector3 _cameraPosition;
        private Quaternion _cameraRotation;
        public CanvasGroup _fadePanel;

        public float fadingTime;
        public GameObject interactTextPrefab;
        private bool _canInteract;
        private KeyBindingManager _kbm;
        private KeyCode _interactKey;
        private GameObject _interactText;
        private bool _canLoadScene = true;
        private Trap[] _traps;

        //private SendTriggerToChildren _triggerChildren;

        private bool _cvm;


        private void Start()
        {
            _traps = FindObjectsOfType<Trap>();
            _cvm = FindObjectOfType<CamTest>().zoomActive;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("YO IM COLLIDING");
            
            if (other.CompareTag("Player"))
            {
                _canInteract = true;
                showInteractText();
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _canInteract = false;
                hideInteractText();
            }
        }

        private void Update()
        {
            if (_canInteract && _canLoadScene && Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log("Colliding");
                //_triggerChildren.trigger();
                _cvm = true;
                _cameraPosition = Camera.main.transform.position;
                _cameraRotation = Camera.main.transform.rotation;
                StartCoroutine(loadSceneAsync());
                _canLoadScene = false;
            }
        }
        
        private void showInteractText()
        {
            if (_interactText == null)
            {
                _interactText = Instantiate(interactTextPrefab, transform.position + new Vector3(0, 1.5f, 0),
                    Quaternion.identity);
                _interactText.GetComponentInChildren<Text>().text = $"Press {_interactKey} to Interact";
            }
        
            _interactText.GetComponentInChildren<Text>().text = $"Press {_interactKey} to Interact";
        }

        /// <summary> The hideInteractText function destroys the interact text object.</summary>    
        ///
        ///
        /// <returns> Nothing.</returns>
        private void hideInteractText()
        {
            if (_interactText != null)
            {
                Destroy(_interactText);
                _interactText = null;
            }
        }

        private IEnumerator loadSceneAsync()
        {
            Debug.Log("Starting to load");
    
            // Start saving the level state in a separate thread
            yield return StartCoroutine(saveLevelStateCoroutine());
    
            Debug.Log("SAVED ASSETS DATA");

            Debug.Log("Loading Scene");

            yield return new WaitForSeconds(3);
            
            // Fade out current scene
            yield return fadeOut();

            Debug.Log("It's returning");
            // Load next scene asynchronously
    
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
            //asyncLoad.allowSceneActivation = false;
            Debug.Log("Loaded Scene");

            // Wait until scene is fully loaded
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
    
            Debug.Log("Finished loading");
    
            Camera.main.transform.position = _cameraPosition;
            Camera.main.transform.rotation = _cameraRotation;
    
            // Set player position and camera
            Vector3 spawnPoint = GameObject.FindWithTag("Door").transform.position;
            player.transform.position = spawnPoint;
            Debug.Log("Teleporting player");

            // Fade in next scene
            yield return fadeIn();
            Debug.Log("Was able to fade in :)");

            // Unload previous scene
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            asyncLoad.allowSceneActivation = true;
            Debug.Log("Allowed scene active yay");
    
            // Load the level state after the scene has finished loading
            getLevelState();
            Debug.Log("LOADED ASSETS DATA");

            _canLoadScene = true;
        }

        private IEnumerator saveLevelStateCoroutine()
        {
            // Get the necessary data to save
            GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
            List<EnemyController> enemies = new List<EnemyController>();

            foreach (GameObject enemyObject in enemyObjects)
            {
                if (enemyObject.TryGetComponent(out EnemyController ctrl))
                {
                    EnemyController enemy = ctrl;
                    
                    if (enemy != null)
                    {
                        enemies.Add(enemy);
                    }
                }
            }
            
            Debug.Log("Yay! You've reached wait for end of frame!");

            // Save the level state in a separate thread
            yield return new WaitForEndOfFrame();
            Debug.Log("YAY! END OF FRAME IS NOT PROBLEM");
            saveLevelState(enemies, player.GetComponent<PlayerController>().health, _traps);
            Debug.Log("saved it");
        }

        public void saveLevelState(List<EnemyController> enemyHealth, int playerHealth, Trap[] traps)
        {
            Debug.Log("Ayo mah men i ACtually initialize");
            foreach (var enemy in enemyHealth)
            {
                PlayerPrefs.SetInt($"EnemyHealth{enemy.id}", enemy.health);
            }

            foreach (var trap in traps)
            {
                PlayerPrefs.SetString($"TrapState{trap.id}", trap.isDestroyed.ToString());
            }
            
            Debug.Log("yo everythin fine here at datamanager");
            
            PlayerPrefs.SetInt("Health", playerHealth);
        }
        
        public void getLevelState()
        {
            foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
            {
                if (PlayerPrefs.HasKey($"EnemyHealth{enemy.id}"))
                {
                    enemy.health = PlayerPrefs.GetInt($"EnemyHealth{enemy.id}");
                }
            }
            
            foreach (Trap trap in FindObjectsOfType<Trap>())
            {
                if (PlayerPrefs.HasKey($"TrapState{trap.name}"))
                {
                    bool isDestroyed = bool.Parse(PlayerPrefs.GetString($"TrapState{trap.name}"));
                    trap.isDestroyed = isDestroyed;
                }
            }

            if (PlayerPrefs.HasKey("Health"))
            {
                player.GetComponent<PlayerController>().health = PlayerPrefs.GetInt("Health");
            }
            
            Debug.Log("gotcha, loaded");
        }
        
        private IEnumerator fadeOut()
        {
            
            _fadePanel.gameObject.SetActive(true);
            hideInteractText();
            SceneManager.UnloadSceneAsync("HUD");
            
            CanvasGroup canvasGroup = _fadePanel.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;

            Debug.Log("Fading out");
            // Fade out over 1 second
            float elapsedTime = 0f;
            while (elapsedTime < fadingTime)
            {
                Debug.Log("FaDING STILL");
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / 1f);
                canvasGroup.alpha = alpha;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Debug.Log("Should've faded by now lol");
            // Set alpha to 1f
            canvasGroup.alpha = 1f;
            yield return new WaitForSeconds(3);
        }

        internal IEnumerator fadeIn()
        {
            // Find fade panel in next scene
            Scene nextScene = SceneManager.GetSceneByName(nextSceneName);
            GameObject fadePanel = GameObject.FindWithTag("Panel");
            
            /*var nextObjs = nextScene.GetRootGameObjects();

            if (fadePanel == null)
            {
                foreach (var t in nextObjs)
                {
                    if (t.CompareTag($"Panel")) 
                    { 
                        fadePanel = t; 
                        break;
                    }
                }
            }*/
            
            // Set alpha to 1f
            CanvasGroup canvasGroup = fadePanel.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
            
            Debug.Log("Fading In");

            // Fade in over 1 second
            float elapsedTime = 0f;
            while (elapsedTime < fadingTime)
            {
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / 1f);
                canvasGroup.alpha = alpha;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Set alpha to 0f
            canvasGroup.alpha = 0f;
        }
    }
}