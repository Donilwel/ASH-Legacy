using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("TestGame");
    }
    public void ExitGame()
    {
        Debug.Log("Close the game");
        Application.Quit();
    }
}
