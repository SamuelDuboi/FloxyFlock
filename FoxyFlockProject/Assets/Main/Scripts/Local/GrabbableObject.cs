using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
[RequireComponent(typeof(ChangeMatOnSelect))]
public class GrabbableObject : XRGrabInteractable
{
    public bool isGrab;
    public XRBaseInteractor currentInteractor;
#pragma warning disable CS0672 // Un membre se substitue au membre obsolète
#pragma warning disable CS0618 // Le type ou le membre est obsolète

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
#pragma warning restore CS0618 // Le type ou le membre est obsolète
#pragma warning restore CS0672 // Un membre se substitue au membre obsolète


}

