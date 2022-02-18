using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class InteractionManager : XRInteractionManager
{
    public static InteractionManager instance;
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public override void HoverEnter(XRBaseInteractor interactor, XRBaseInteractable interactable)
    {
        if (Mathf.Abs(Vector3.Distance(interactable.transform.position, interactor.transform.position)) > 1f)
            return;
        base.HoverEnter(interactor, interactable);
    }
    public override void SelectEnter(XRBaseInteractor interactor, XRBaseInteractable interactable)
    {
        if (Mathf.Abs(Vector3.Distance(interactable.transform.position, interactor.transform.position)) > 1f)
            return;
        base.SelectEnter(interactor, interactable);
    }
}
