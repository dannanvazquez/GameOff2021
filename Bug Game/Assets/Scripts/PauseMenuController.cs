using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject overlayEffect;

    public GameObject gameOverMenu;
    public GameObject gameOverScore;

    public GameObject winGameMenu;
    public GameObject winGameScore;

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
        overlayEffect.SetActive(true);
    }

    public void ResumeGame() {
        isPlaying = true;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        overlayEffect.SetActive(false);
    }

    public void GameOver(int finalScore) {
        isPlaying = false;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameOverScore.GetComponent<Text>().text = "Score: " + finalScore;
        gameOverMenu.SetActive(true);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        overlayEffect.SetActive(true);
    }

    public void WinGame(int finalScore) {
        isPlaying = false;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        winGameScore.GetComponent<Text>().text = "Score: " + finalScore;
        winGameMenu.SetActive(true);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        overlayEffect.SetActive(true);
    }
}
