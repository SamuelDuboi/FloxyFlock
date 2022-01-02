using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutFireball : MonoBehaviour
{
    [HideInInspector] public FireballManager fireballManager; //set by the manager

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == fireballManager.portal)
        {
            fireballManager.FireballHitPortal();
        }
    }

    public void OpenPortal()
    {
        StartCoroutine(fireballManager.TryOpenPortal());
    }
}
