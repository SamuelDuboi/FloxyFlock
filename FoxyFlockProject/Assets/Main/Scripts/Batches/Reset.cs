using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Reset : MonoBehaviour
{
    private List<GameObject> freezedFlocks = new List<GameObject>();
    private List<int> freezdFlockPoolIndex = new List<int>();
    private List<int> freezdFlockIndex = new List<int>();
    public GrabManager grabManager;
   public virtual void AddFreezFlock(GameObject flock, int poolIndex, int flockIndex)
    {
        flock.GetComponent<GrabablePhysicsHandler>().OnFreeze();

        freezdFlockPoolIndex.Add(poolIndex);
        freezdFlockIndex.Add(flockIndex);

        Destroy(flock.GetComponent<GrabbableObject>());
        Destroy( flock.GetComponent<GrabablePhysicsHandler>());
        Destroy(flock.GetComponent<Rigidbody>());

        freezedFlocks.Add(flock);
    }
    public virtual void RemoveFreezedFlock(GameObject flock, int indexOfPool, int indexOfFlock)
    {
        if(freezedFlocks.Contains(flock))
        {
            var index = freezedFlocks.IndexOf(flock);
            grabManager.DestroyFlock(flock, indexOfPool); 
            freezedFlocks.Remove(flock);
            freezdFlockPoolIndex.RemoveAt(index);
            freezdFlockIndex.RemoveAt(index);
        }
    }

    public virtual void ResetEvent()
    {
        for (int i = 0; i < freezedFlocks.Count; i++)
        {
            grabManager.DestroyFlock(freezedFlocks[i], freezdFlockPoolIndex[i]);
        }
        freezedFlocks.Clear();
        freezdFlockIndex.Clear();
        freezdFlockPoolIndex.Clear();
    }
}
