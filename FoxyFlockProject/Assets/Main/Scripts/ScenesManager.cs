using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using NaughtyAttributes;
using UnityEngine.XR.Interaction.Toolkit;
public class ScenesManager : MonoBehaviour
{
    [Scene]
    public int[] menuScene;
    [Scene]
    public int[] lobbyScene;
    public static ScenesManager instance;
   [HideInInspector] public int numberOfFlocksInScene;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    public bool IsMenuScene()
    {
        for (int i = 0; i < menuScene.Length; i++)
        {
           if (SceneManager.GetActiveScene().buildIndex == menuScene[i])
                return true;

        }
        return false;
    }

    public bool IsLobbyScene()
    {
        for (int i = 0; i < lobbyScene.Length; i++)
        {
            if (SceneManager.GetActiveScene().buildIndex == lobbyScene[i])
                return true;

        }
        return false;
    }
    public void LunchScene(string sceneToLunch)
    {
        if ( IsMenuScene())
        {
            if(sceneToLunch != "Offline" && sceneToLunch != "MainMenus"&& sceneToLunch != "Room")
            {
                SoundManager.instance.LunchGame();
            }
        }
        else
        {
            if (sceneToLunch == "Offline" || sceneToLunch == "MainMenus" || sceneToLunch == "Room")
            {
                SoundManager.instance.LunchMenu();
            }
        }

        StartCoroutine(WaitToLunch(sceneToLunch));
    }
    public void LunchScene(int sceneToLunch)
    {
        if (IsMenuScene())
        {
            if (sceneToLunch >2)
            {
                SoundManager.instance.LunchGame();
            }
        }
        else
        {
            if (sceneToLunch <3)
            {
                SoundManager.instance.LunchMenu();
            }
        }

        StartCoroutine(WaitToLunch(sceneToLunch));
    }
    IEnumerator WaitToLunch(string sceneToLunch)
    {
        var async= SceneManager.LoadSceneAsync(sceneToLunch);
        async.allowSceneActivation = false;
        yield return new WaitForSeconds(0.5f);
        async.allowSceneActivation = true;

    }
    IEnumerator WaitToLunch(int sceneToLunch)
    {
        var async = SceneManager.LoadSceneAsync(sceneToLunch);
        async.allowSceneActivation = false;
        yield return new WaitForSeconds(0.5f);
        async.allowSceneActivation = true;

    }

}
