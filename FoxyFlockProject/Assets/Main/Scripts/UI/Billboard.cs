using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera billboardCamera;
    private void Start()
    {
        if(GetComponentInParent<PlayerMovement>())
        billboardCamera = GetComponentInParent<PlayerMovement>().vrHeadSett.GetComponent<Camera>();
        else
        {
            billboardCamera = GetComponentInParent<PlayerMovementMulti>().vrHeadSett.GetComponent<Camera>();
        }
    }
    private void Update()
    {
        if (!billboardCamera)
        {
            if (GetComponentInParent<PlayerMovement>())
                billboardCamera = GetComponentInParent<PlayerMovement>().vrHeadSett.GetComponent<Camera>();
            else
            {
                billboardCamera = GetComponentInParent<PlayerMovementMulti>().vrHeadSett.GetComponent<Camera>();
            }
        }
        else
            transform.forward = billboardCamera.transform.forward;
    }
}