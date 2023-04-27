using UnityEngine;
using UnityEngine.SceneManagement;

namespace HUD
{
    public class HUDLoader : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }
    }
}
