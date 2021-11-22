using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class HandController : XRController
{
    public Collider spherCollider;
    public HandPresence controllerPresence;
    protected override void UpdateController()
    {
        base.UpdateController();
        if (model && !controllerPresence)
            controllerPresence = model.GetComponent<HandPresence>();
    }
    private void Start()
    {
        if(controllerNode == UnityEngine.XR.XRNode.LeftHand)
        {
            InputManager.instance.OnGrabbingLeft.AddListener(Grab);
            InputManager.instance.OnGrabbingReleaseLeft.AddListener(GrabRelease);
        }
        else if (controllerNode == UnityEngine.XR.XRNode.RightHand)
        {
            InputManager.instance.OnGrabbingRight.AddListener(Grab);
            InputManager.instance.OnGrabbingReleaseRight.AddListener(GrabRelease);
        }
    }
    private void Grab()
    {
        if (!spherCollider.enabled)
            return;
        spherCollider.enabled = false;
    }
    private void GrabRelease()
    {
        if (spherCollider.enabled)
            return;
        spherCollider.enabled = true;

    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 6 && controllerPresence)
            controllerPresence.isGrab = true;
    }
    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 6 && controllerPresence)
            controllerPresence.isGrab = false;
    }
}
