using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
[RequireComponent(typeof(ChangeMatOnSelect))]
public class GrabbableObject : XRGrabInteractable
{
    public bool isGrab;
    public XRBaseInteractor currentInteractor;
    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        base.OnSelectEntering(interactor);
        currentInteractor = interactor;
        isGrab = true;
    }
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        currentInteractor = null;
        isGrab = false;
    }


}

