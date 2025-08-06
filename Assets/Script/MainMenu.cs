using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("Game");
        GameManager.Instance.isGameActive = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
