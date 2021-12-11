using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void CloseAndOpen(GameObject currentWindow, GameObject windowToOpen)
    {
        currentWindow.SetActive(true);
        windowToOpen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void loadScene()
    {
        //loadScene(); // la scène à load
    }

}
