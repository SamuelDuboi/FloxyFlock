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
        manager.GetPiece(other.GetComponentInParent<XRDirectInteractor>(), index);
    }
}
