using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
[System.Serializable]
public class ModifierAction : MonoBehaviour
{
    protected GameObject flock;
    protected XRBaseInteractor currentInteractor;
    protected GrabbableObject flockInteractable;
    protected SoundReader sound;
    protected bool isOnStasis;
    protected bool canEnter;
    protected bool isSlowingDown;
    protected float timerSlow;
    public float timerToSlowInStasis = 1.0f;
    public float slowForce = 1.2f;
    public virtual void OnStarted(GameObject _object)
    {

    }
    public virtual void OnGrabed(GameObject _object)
    {
        if (isSlowingDown)
            isSlowingDown = false;
    }
    public virtual void OnReleased(GameObject _object)
    {

    }
    public virtual void OnHitSomething(GameObject _object, Vector3 velocity, GameObject collision)
    {

    }
    public virtual void OnHitGround(GameObject _object)
    {

    }
    public virtual void OnEnterStasis(GameObject _object, bool isGrab, Rigidbody rgb )
    {
        isSlowingDown = true;
        timerSlow = 0;
        timerToSlowInStasis = _object.GetComponent<GrabablePhysicsHandler>().timeToSlow;
        slowForce = _object.GetComponent<GrabablePhysicsHandler>().slowForce;
        StartCoroutine(SlowCoroutine(rgb));
    }
    public IEnumerator SlowCoroutine(Rigidbody rgb)
    {
        while(isSlowingDown && timerSlow < timerToSlowInStasis && rgb.velocity.magnitude>0.1f)
        {
            rgb.velocity /= slowForce;
            timerSlow += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        isSlowingDown = false;
        timerSlow = 0;
        rgb.velocity = Vector3.zero;
    }
}
