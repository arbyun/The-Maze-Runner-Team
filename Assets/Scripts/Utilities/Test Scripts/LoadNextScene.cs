using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities.Test_Scripts
{
    public class LoadNextScene : MonoBehaviour
    {
        [SerializeField] private string mainScene;
        [SerializeField] private string subScene;
        private GameObject _player;
        private string _activeScene;

        private void Awake()
        {
            _player = GlobalController.Instance.player;
            _activeScene = SceneManager.GetActiveScene().name;
        }

        /*private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == _player && _activeScene == mainScene)
            {
                Debug.Log("Main Working");
                loadSubScene();
                unloadMain();
            }
            else //(collision.gameObject == _player && _activeScene == subScene)
            {
                Debug.Log("Working");
                loadMainScene();
                unloadSub();
            }
        }*/

        public void switchScenes()
        {
            if (_activeScene == mainScene)
            {
                Debug.Log("Main Working");
                loadSubScene();
                unloadMain();
            }
            else //(collision.gameObject == _player && _activeScene == subScene)
            {
                Debug.Log("Working");
                loadMainScene();
                unloadSub();
            }   
        }

        void loadSubScene()
        {      
            SceneHelper.LoadScene(subScene, true, true);
            Debug.Log("Loading scene");
        }

        void unloadMain()
        {
            //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            //SceneManager.UnloadSceneAsync(currentSceneIndex - 1);
            SceneManager.UnloadSceneAsync(mainScene);
            Debug.Log("Unloading scene");
        }

        void loadMainScene()
        {
            SceneHelper.LoadScene(mainScene, true, true);
            Debug.Log("Loading scene");
        }

        void unloadSub()
        {
            SceneManager.UnloadSceneAsync(subScene);
            Debug.Log("Unloading scene");
        }

    }
}