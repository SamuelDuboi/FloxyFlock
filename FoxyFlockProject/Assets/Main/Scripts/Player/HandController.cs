using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class HandController : XRController
{
    public Collider spherCollider;
    public HandPresence controllerPresence;
    public InputManager inputManager;
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
            inputManager.OnGrabbingLeft.AddListener(Grab);
            inputManager.OnGrabbingReleaseLeft.AddListener(GrabRelease);
        }
        else if (controllerNode == UnityEngine.XR.XRNode.RightHand)
        {
            inputManager.OnGrabbingRight.AddListener(Grab);
            inputManager.OnGrabbingReleaseRight.AddListener(GrabRelease);
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
