using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Experimental;
public class ResetMulti : NetworkBehaviour
{
    private List<GameObject> freezedFlocks = new List<GameObject>();

    public virtual void AddFreezFlock(GameObject flock)
    {
        freezedFlocks.Add(flock);
        CmdFreezFlock(flock);
    }
    public virtual void RemoveFreezedFlock(GameObject flock)
    {
        if (freezedFlocks.Contains(flock))
        {
            freezedFlocks.Remove(flock);
            CmdDestroyFlock(flock);
        }
    }
   
    public virtual void ResetEvent()
    {
        for (int i = 0; i < freezedFlocks.Count; i++)
        {
            CmdDestroyFlock(freezedFlocks[i]);
        }
        freezedFlocks.Clear();
    }
    [Command]
    public void CmdDestroyFlock(GameObject flock)
    {
        Destroy(flock);
        RpcDestroyFlock(flock);
    }
    [ClientRpc]
    public void RpcDestroyFlock(GameObject flock)
    {
        Destroy(flock);
    }

    [Command(requiresAuthority = false)]
    public void CmdFreezFlock(GameObject flock)
    {
        flock.GetComponent<GrabablePhysicsHandler>().OnFreeze();

        Destroy(flock.GetComponent<GrabbableObject>());
        Destroy(flock.GetComponent<GrabablePhysicsHandler>());
        Destroy(flock.GetComponent<Rigidbody>());
        NetworkRigidbody rgb;
        if (flock.TryGetComponent<NetworkRigidbody>(out rgb))
            Destroy(rgb);
        RpcFreezFlock(flock);
    }
    [ClientRpc]
    public void RpcFreezFlock(GameObject flock)
    {
        flock.GetComponent<GrabablePhysicsHandler>().OnFreeze();

        Destroy(flock.GetComponent<GrabbableObject>());
        Destroy(flock.GetComponent<GrabablePhysicsHandler>());
        Destroy(flock.GetComponent<Rigidbody>());
        NetworkRigidbody rgb;
        if (flock.TryGetComponent<NetworkRigidbody>(out rgb))
            Destroy(rgb);
    }
}
