using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static void SwitchScene(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public static void QuitGame() {
        Application.Quit();
    }
}
