using UnityEngine.SceneManagement;
using UnityEngine;
using NaughtyAttributes;
public class ScenesManager : MonoBehaviour
{
    [Scene]
    public int[] menuScene;
    [Scene]
    public int[] lobbyScene;
    public static ScenesManager instance;
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
}
