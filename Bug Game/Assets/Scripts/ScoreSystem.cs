using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public GameObject scoreText;
    public GameObject timerText;
    public static int bugCount = 0;
    public int score = 0;
    public static float timeLimit = 300;
    public static float timeRemaining = 0;
    public PauseMenuController pauseMenuController;
    public bool timerEnabled = false;

    private void Awake() {
        timeRemaining = timeLimit;
    }

    private void Start() {
        timerEnabled = true;
    }

    private void Update() {
        if(bugCount == 42) {
            ScoreCount();
            pauseMenuController.WinGame(score);
        }

        if (timerEnabled) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            }
            else {
                timerEnabled = false;
                timeRemaining = 0;
                ScoreCount();
                pauseMenuController.GameOver(score);
            }
        }
        DisplayTime(timeRemaining);
    }

    void DisplayTime(float timeToDisplay) {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.GetComponent<Text>().text = minutes + ":" + seconds.ToString("00");
    }

    void ScoreCount() {
        score = bugCount * 100;
        score += (int)timeRemaining * 100;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Collectible") {
            bugCount++;
            scoreText.GetComponent<Text>().text = bugCount + "/42";
            Destroy(other.gameObject);
        }
    }
}
