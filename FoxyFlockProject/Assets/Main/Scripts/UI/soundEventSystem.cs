
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class soundEventSystem : MonoBehaviour
{
    public SoundReader sound;
    public EventSystem eventSystem;
    private GameObject currentSelectedObject;
    // Update is called once per frame
    void Update()
    {
        if(eventSystem.currentSelectedGameObject != currentSelectedObject)
        {
            currentSelectedObject = eventSystem.currentSelectedGameObject;
            sound.PlaySeconde();
        }
    }
}
