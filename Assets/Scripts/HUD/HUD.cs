using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOver;
    public GameObject inventory;
    private PlayerController pc;

    private void Start()
    {
        pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.SetActive(!inventory.activeSelf);
        }

        if (pc.isDead)
        {
            gameOver.SetActive(!gameOver.activeSelf);
            Time.timeScale = 0;
        }
    }

    public void loadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void restartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
