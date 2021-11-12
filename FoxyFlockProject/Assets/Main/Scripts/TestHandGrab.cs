using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class TestHandGrab : MonoBehaviour
{
    public XRInteractionManager interactionManager;
    public XRBaseInteractor baseInteractor;
    public XRBaseInteractable baseInteractable;
    // Start is called before the first frame update
    void Start()
    {
        InputManager.instance.OnLeftGrab.AddListener(Grav);
    }

    void Grav()
    {
        interactionManager.ForceSelect(baseInteractor, baseInteractable);
    }
}
