using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public string _nextScene;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject != GlobalController.Instance.player)
            return;

        //PlayerPrefs.SetString("Milestone", GlobalController.Instance.victoryScene);
        SceneManager.LoadScene(_nextScene);
    }
}
