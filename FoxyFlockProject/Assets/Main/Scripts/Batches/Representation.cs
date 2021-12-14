using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Representation : MonoBehaviour
{
    public int index;
    public GrabManager manager;
    public RawImage image;

    private void OnTriggerStay(Collider other)
    {
        if (!manager.isOnCollision)
            manager.isOnCollision = true;
        manager.GetPiece(other.GetComponentInParent<XRDirectInteractor>(), index);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!manager.isOnCollision)
            manager.isOnCollision = true;
        manager.GetPiece(collision.gameObject.GetComponentInParent<XRDirectInteractor>(), index);
    }
    private void OnTriggerExit(Collider other)
    {
        if (manager.isOnCollision)
            manager.isOnCollision = false;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (manager.isOnCollision)
            manager.isOnCollision = false;
    }
}
