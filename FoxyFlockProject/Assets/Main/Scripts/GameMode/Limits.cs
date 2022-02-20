using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limits : MonoBehaviour
{
    public LayerMask detectionLayer;
    public bool triggered;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6 && other.transform.parent.GetComponentInParent<GrabbableObject>().isGrab == false)
        {
            triggered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer == 6 && other.transform.parent.GetComponentInParent<GrabbableObject>().isGrab == false)
        {
            triggered = false;
        }
    }
}
