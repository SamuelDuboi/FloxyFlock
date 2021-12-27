using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Reset : MonoBehaviour
{
    private List<GameObject> freezedFlocks = new List<GameObject>();

   public virtual void AddFreezFlock(GameObject flock)
    {
        freezedFlocks.Add(flock);
    }
    public virtual void RemoveFreezedFlock(GameObject flock)
    {
        if(freezedFlocks.Contains(flock))
        {
            freezedFlocks.Remove(flock);
            Destroy(flock);
        }
    }

    public virtual void ResetEvent()
    {
        for (int i = 0; i < freezedFlocks.Count; i++)
        {
            Destroy(freezedFlocks[i]);
        }
        freezedFlocks.Clear();
    }
}
