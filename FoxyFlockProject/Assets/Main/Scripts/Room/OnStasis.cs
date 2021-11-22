using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStasis : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            Debug.Log("On stasis");
        }
    }
}
