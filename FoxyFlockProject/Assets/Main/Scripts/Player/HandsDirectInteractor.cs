using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class HandsDirectInteractor : XRDirectInteractor
{

    public void Reset(GameObject flox)
    {
        StartCoroutine(WaitToFall(flox));
    }
    IEnumerator WaitToFall(GameObject flox)
    {
        yield return new WaitForSeconds(0.5f);
        var colliders = flox.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            OnTriggerExit(collider);
        }
    }

}
