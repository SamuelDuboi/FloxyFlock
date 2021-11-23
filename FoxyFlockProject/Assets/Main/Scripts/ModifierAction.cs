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
    protected Rigidbody rgb;
    protected bool isOnStasis;
    protected bool canEnter;
    protected bool isSlowingDown;
    protected float timerSlow;
    public float timerToSlowInStasis = 1.0f;
    public float slowForce = 1.2f;
    public float vibrationForce = 0.5f;
    public float vibrationTime = 0.2f;
    protected bool isGrab;
    public virtual void OnStarted(GameObject _object)
    {

    }
    public virtual void OnGrabed(GameObject _object)
    {
        flock = _object;
        flockInteractable = flock.GetComponent<GrabbableObject>();
        currentInteractor = flockInteractable.currentInteractor;
        timerSlow = 0;
        isGrab = true;
       
    }
    public virtual void OnReleased(GameObject _object)
    {
        isGrab = false;

        if (isOnStasis)
            StartCoroutine(SlowCoroutine());
        else 
        {
            isSlowingDown = false;
            rgb.useGravity = true;
        }
    }
    public virtual void OnHitSomething(GameObject _object, Vector3 velocity, GameObject collision)
    {

    }
    public virtual void OnHitGround(GameObject _object, Vector3 initPos, bool isGrab)
    {
        if (!isGrab)
        {
            _object.transform.position = initPos;
        }
    }
    public virtual void OnExitStasis(GameObject _object)
    {
        isOnStasis = false;
        if(rgb)
        rgb.useGravity = true;
        if (isGrab)
        {
            if (currentInteractor.name == "RightHand Controller")
            {
                InputManager.instance.OnHapticImpulseRight.Invoke(vibrationForce, vibrationTime);
            }
            else if (currentInteractor.name == "LeftHand Controller")
            {
                InputManager.instance.OnHapticImpulseLeft.Invoke(vibrationForce, vibrationTime);

            }
            else { Debug.LogError("Your hand dont have the good name, please name it RightHand Controller and LeftHand Controller"); }
        }
    }
    public virtual void OnEnterStasis(GameObject _object, bool isGrab, Rigidbody _rgb )
    {
        isOnStasis = true;
        isSlowingDown = true;
        timerSlow = 0;
        timerToSlowInStasis = _object.GetComponent<GrabablePhysicsHandler>().timeToSlow;
        slowForce = _object.GetComponent<GrabablePhysicsHandler>().slowForce;
        rgb = _rgb;
        if(!isGrab)
        StartCoroutine(SlowCoroutine());
        else
        {
            if(currentInteractor.name == "RightHand Controller")
            {
                InputManager.instance.OnHapticImpulseRight.Invoke(vibrationForce,vibrationTime);
            }
            else if(currentInteractor.name == "LeftHand Controller")
            {
                InputManager.instance.OnHapticImpulseLeft.Invoke(vibrationForce, vibrationTime);

            }
            else { Debug.LogError("Your hand dont have the good name, please name it RightHand Controller and LeftHand Controller"); }
        }
    }

    public IEnumerator SlowCoroutine( )
    {
        rgb.useGravity = false;
        while(isSlowingDown && timerSlow < timerToSlowInStasis)
        {
            rgb.velocity /= slowForce;
            timerSlow += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        rgb.useGravity = true;

        isSlowingDown = false;
        timerSlow = 0;
        rgb.velocity = Vector3.zero;
    }
}
