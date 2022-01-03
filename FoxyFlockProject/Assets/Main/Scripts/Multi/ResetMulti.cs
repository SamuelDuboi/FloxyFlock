using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Experimental;
public class ResetMulti : NetworkBehaviour
{
    private List<GameObject> freezedFlocks = new List<GameObject>();
    private List<int> freezdFlockPoolIndex = new List<int>();
    private List<int> freezdFlockIndex = new List<int>();
    public GrabManager grabManager;
    public InputManager inputManager;
    public virtual void AddFreezFlock(GameObject flock, int poolIndex, int flockIndex)
    {
        freezedFlocks.Add(flock);

        freezdFlockPoolIndex.Add(poolIndex);
        freezdFlockIndex.Add(flockIndex);

        CmdFreezFlock(flock);
    }
    public virtual void RemoveFreezedFlock(GameObject flock, int indexOfPool, int indexOfFLock)
    {
        if (freezedFlocks.Contains(flock))
        {
            var index = freezedFlocks.IndexOf(flock);
            CmdDestroyFlock(flock,indexOfPool, indexOfFLock);
            freezedFlocks.RemoveAt(index);
            freezdFlockPoolIndex.RemoveAt(index);
            freezdFlockIndex.RemoveAt(index);

        }
    }
   
    public virtual void ResetEvent()
    {
        for (int i = 0; i < freezedFlocks.Count; i++)
        {
            CmdDestroyFlock(freezedFlocks[i], freezdFlockPoolIndex[i], freezdFlockIndex[i]);
        }
        freezedFlocks.Clear();
        freezdFlockPoolIndex.Clear();
        freezdFlockIndex.Clear();
    }
    [Command]
    public void CmdDestroyFlock(GameObject flock, int indexOfPool, int indexOfFLock)
    {
        grabManager.DestroyFlock(flock, indexOfPool,indexOfFLock);
        RpcDestroyFlock(flock, indexOfPool, indexOfFLock);
        Destroy(flock);

    }
    [ClientRpc]
    public void RpcDestroyFlock(GameObject flock, int indexOfPool, int indexOfFLock)
    {
        grabManager.DestroyFlock(flock, indexOfPool, indexOfFLock);
        Destroy(flock);
    }

    [Command(requiresAuthority = false)]
    public void CmdFreezFlock(GameObject flock)
    {
/*        flock.GetComponent<GrabablePhysicsHandler>().OnFreeze();

        Destroy(flock.GetComponent<GrabbableObject>());
        Destroy(flock.GetComponent<GrabablePhysicsHandler>());
        Destroy(flock.GetComponent<Rigidbody>());
        NetworkRigidbody rgb;
        if (flock.TryGetComponent<NetworkRigidbody>(out rgb))
            Destroy(rgb);*/
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
