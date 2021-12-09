using UnityEngine.SceneManagement;
using UnityEngine;
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
        SceneManager.LoadScene(sceneToLunch);
    }
    public void LunchScene(int sceneToLunch)
    {
        SceneManager.LoadScene(sceneToLunch);
    }
}
