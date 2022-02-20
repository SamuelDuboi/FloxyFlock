using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Reset : MonoBehaviour
{
    private List<GameObject> freezedFlocks = new List<GameObject>();
    private List<int> freezdFlockPoolIndex = new List<int>();
    private List<int> freezdFlockIndex = new List<int>();
    public GrabManager grabManager;
    private SoundReader soundReader;
   public virtual void AddFreezFlock(GameObject flock, int poolIndex, int flockIndex)
    {
        if (freezedFlocks.Contains(flock))
            return;
        StartCoroutine( flock.GetComponent<GrabablePhysicsHandler>().Freez());

        freezdFlockPoolIndex.Add(poolIndex);
        freezdFlockIndex.Add(flockIndex);
        flock.GetComponentInChildren<FloxExpressionManager>().isFrozen = true;
        Destroy(flock.GetComponent<GrabbableObject>());
        Destroy( flock.GetComponent<GrabablePhysicsHandler>());
        Destroy(flock.GetComponent<Rigidbody>());

        freezedFlocks.Add(flock);
    }
    public virtual void AddFreezFlock(GameObject flock, int poolIndex, int flockIndex, bool isHotPotato)
    {
        if (freezedFlocks.Contains(flock))
            return;

        freezdFlockPoolIndex.Add(poolIndex);
        freezdFlockIndex.Add(flockIndex);
        flock.GetComponentInChildren<FloxExpressionManager>().isFrozen = true;
        Destroy(flock.GetComponent<GrabbableObject>());
        Destroy(flock.GetComponent<GrabablePhysicsHandler>());
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
        StartCoroutine(WaitToSeeTable());
    }
    IEnumerator WaitToSeeTable()
    {
        yield return new WaitUntil(() => grabManager.GetComponentInParent<PlayerMovement>().SeeTable());
        for (int i = 0; i < freezedFlocks.Count; i++)
        {
            grabManager.DestroyFlock(freezedFlocks[i], freezdFlockPoolIndex[i]);
        }
        if (soundReader == null)
            soundReader = GetComponent<SoundReader>();
        soundReader.secondClipName = "StartReset";
        soundReader.PlaySeconde();
        if (freezedFlocks != null && freezedFlocks.Count > 0)
            StartCoroutine(LastFlockIsDestroy(freezedFlocks[freezedFlocks.Count - 1].GetComponent<DissolveFlox>()));
        freezedFlocks.Clear();
        freezdFlockIndex.Clear();
        freezdFlockPoolIndex.Clear();
    }
    IEnumerator LastFlockIsDestroy(DissolveFlox dissolveFlox)
    {
        grabManager.playGround.soundReader.Play("Dissolve");
        yield return new WaitForSeconds(dissolveFlox.dissolveTime);
        soundReader.ThirdClipName = "EndReset";
        soundReader.PlayThird();
        grabManager.UpdateIntersectionPos();

    }
}
