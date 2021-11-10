using UnityEngine.SceneManagement;
using UnityEngine;

public class ScenesManager : MonoBehaviour
{
    public int[] menuScene;
    public static ScenesManager instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
