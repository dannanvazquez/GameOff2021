using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public GameObject scoreText;
    public static int score = 0;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Collectible") {
            score++;
            scoreText.GetComponent<Text>().text = "Bugs: " + score;
            Destroy(other.gameObject);
        }
    }
}
