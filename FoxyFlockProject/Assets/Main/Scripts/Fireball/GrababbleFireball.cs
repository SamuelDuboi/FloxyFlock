using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrababbleFireball : GrabbableObject
{
    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        base.OnSelectEntering(interactor);
      //  currentInteractor = interactor;
      //  isGrab = true;
        //if (OnSelect != null)
        //    OnSelect.Invoke();
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        currentInteractor = null;
        isGrab = false;
    }
}
