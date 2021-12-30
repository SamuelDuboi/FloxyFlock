using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutFireball : MonoBehaviour
{
    [HideInInspector] public FireballManager fireballManager; //set by the manager

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Portal")
        {
            //init exit portal event
        }
    }
}
