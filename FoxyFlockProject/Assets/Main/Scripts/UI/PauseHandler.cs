using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{

    public GameObject UI;
    public InputManager inputManager;
    // Start is called before the first frame update
    void Start()
    {
        inputManager.OnMenuPressed.AddListener(MenuPressed);   
    }
    private void MenuPressed()
    {
        if (UI.activeSelf)
            UI.SetActive(false);
        else
            UI.SetActive(true);
    }
    public void Resume()
    {
        inputManager.OnMenuPressed.Invoke();
    }       

}
