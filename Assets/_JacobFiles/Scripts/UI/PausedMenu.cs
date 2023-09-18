using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedMenu : MonoBehaviour
{
    public static bool Paused = false;
    public GameObject PauseMenu;
    private PlayerController controller;

    private void Start()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;

        // Toggle cursor visibility and lock state using the PlayerController script.
        controller.ToggleCursor(true);
    }

    public void Resume()
    {
        if (Cursor.visible) // Check if the cursor is currently visible.
        {
            controller.ToggleCursor(false); // Hide the cursor and lock it.
        }

        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
