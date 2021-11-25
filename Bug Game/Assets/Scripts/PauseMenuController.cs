using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;

    public static bool isPlaying;

    private void Start() {
        ResumeGame();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPlaying) {
                PauseGame();
            } else {
                ResumeGame();
            }
        }
    }

    public void PauseGame() {
        isPlaying = false;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame() {
        isPlaying = true;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }
}
