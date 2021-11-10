
using UnityEngine.XR.Interaction.Toolkit;
public class HandController : XRController
{
    public HandPresence controllerPresence;
    protected override void UpdateController()
    {
        base.UpdateController();
        if (model && !controllerPresence)
            controllerPresence = model.GetComponent<HandPresence>();
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
