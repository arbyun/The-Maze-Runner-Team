using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utilities
{
    public class SceneLoader : MonoBehaviour
    {
        [Tooltip("Foreground: The current active, main, scene.")]
        public string foregroundSceneName;

        [Tooltip("Background: The scene we can switch to.")]
        public string backgroundSceneName;

        [Tooltip("Transition time. Usually not to touch.")]
        public float fadeDuration = 1.0f;

        private bool _isForegroundActive = true;
        private Image _fadeImage;

        private AsyncOperation _foregroundSceneLoadingOperation;
        private AsyncOperation _backgroundSceneLoadingOperation;
        private Canvas _canvas;
        private Transform _fadePanel;
        private IEnumerator _enumerator;
        private IEnumerator _enumerator1;

        /*private void Start()
        {
            // Load the foreground scene
            SceneManager.LoadScene(foregroundSceneName, LoadSceneMode.Additive);

            // Get the fade Image component
            _fadeImage = GetComponentInChildren<Image>();
        }

        internal void switchLayers()
        {
            // Start fading out the current layer
            StartCoroutine(fadeOutLayer());

            // Load the new layer
            if (!_isBackgroundActive)
            {
                SceneManager.LoadScene(backgroundSceneName, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(foregroundSceneName, LoadSceneMode.Additive);
            }

            // Start fading in the new layer
            StartCoroutine(fadeInLayer());

            // Set the current layer flag
            _isBackgroundActive = !_isBackgroundActive;
        }

        private IEnumerator fadeOutLayer()
        {
            float elapsedTime = 0.0f;
            while (elapsedTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(0.0f, 1.0f, elapsedTime / fadeDuration);
                _fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        }

        private IEnumerator fadeInLayer()
        {
            float elapsedTime = 0.0f;
            while (elapsedTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
                _fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        }*/

        /*private void Start()
        {
            _enumerator1 = fadeOut(backgroundSceneName, fadeDuration);
            _enumerator = fadeOut(foregroundSceneName, fadeDuration);
            _fadePanel = _canvas.transform.Find("FadePanel");
            _canvas = FindObjectOfType<Canvas>();
        }

        public void switchLayers()
        {
            if (_isForegroundActive)
            {
                loadScene(backgroundSceneName);
                StartCoroutine(_enumerator);
            }
            else
            {
                loadScene(foregroundSceneName);
                StartCoroutine(_enumerator1);
            }

            _isForegroundActive = !_isForegroundActive;
        }

        private IEnumerator fadeOut(string sceneName, float duration)
        {
            _fadePanel.gameObject.SetActive(true);

            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                _fadePanel.GetComponent<CanvasGroup>().alpha = alpha;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _fadePanel.GetComponent<CanvasGroup>().alpha = 1f;

            _foregroundSceneLoadingOperation.allowSceneActivation = false;
            _backgroundSceneLoadingOperation.allowSceneActivation = false;

            unloadScene(sceneName);

            if (_isForegroundActive)
            {
                _foregroundSceneLoadingOperation = loadSceneAsync(foregroundSceneName);
            }
            else
            {
                _backgroundSceneLoadingOperation = loadSceneAsync(backgroundSceneName);
            }

            while (!_isForegroundActive && !_backgroundSceneLoadingOperation.isDone)
            {
                yield return null;
            }

            while (_isForegroundActive && !_foregroundSceneLoadingOperation.isDone)
            {
                yield return null;
            }

            _fadePanel.GetComponent<CanvasGroup>().alpha = 0f;

            _foregroundSceneLoadingOperation.allowSceneActivation = true;
            _backgroundSceneLoadingOperation.allowSceneActivation = true;

            yield return null;
        }

        private static void loadScene(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);

            if (!scene.isLoaded)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }

        private static AsyncOperation loadSceneAsync(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);

            if (!scene.isLoaded)
            {
                return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }

            return null;
        }

        private static void unloadScene(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);

            if (scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
        }*/
    }
}