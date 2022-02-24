using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
[System.Serializable]
public class ModifierAction : MonoBehaviour
{
    protected GameObject flox;
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
    public float vibrationForce = 0.2f;
    public float vibrationTime = 0.2f;
    protected bool isGrab;
    public string grabSound;
    public string collisionSound;
    public string dissolvSound;
    private bool cantPlaySound;
    private bool hasDoneStart;
    public InputManager inputManager;
    public virtual void OnStarted(GameObject _object)
    {
        sound = _object.AddComponent<SoundReader>();
        sound.secondClipName = "EnterStasis";
        sound.ForthClipName = "Dissolve";
        rgb = GetComponent<Rigidbody>();
        hasDoneStart = true;
    }
    public virtual void OnGrabed(GameObject _object)
    {
        if (!hasDoneStart)
            OnStarted(_object);
        flox = _object;
        flockInteractable = flox.GetComponent<GrabbableObject>();
        currentInteractor = flockInteractable.currentInteractor;
        timerSlow = 0;
        if (isOnStasis)
            isSlowingDown = true;
        StopCoroutine(SlowCoroutine());
        isGrab = true;
        sound.clipName = grabSound;
        sound.Play();
        cantPlaySound = false;
    }
    public virtual void OnReleased(GameObject _object)
    {
        if (!hasDoneStart)
            OnStarted(_object);
        isGrab = false;
        timerSlow = 0;
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
        if (!hasDoneStart)
            OnStarted(_object);
        if (!cantPlaySound && !isGrab)
        {
            if(collision.tag == "Table"    || collision.tag == "TagDestroyed" || collision.tag == "Table2" || collision.tag == "Piece")
            {
                cantPlaySound = true;
                sound.ThirdClipName = collisionSound;
                sound.PlayThird();
            }
        }
    }

    public virtual void OnHitGround(GameObject _object, Vector3 initPos, bool isGrab)
    {
        if (sound == null)
            sound = GetComponent<SoundReader>();
        sound.ForthClipName = "Dissolve";
        sound.Playforth();
        if(SaveSystem.instance)
            SaveSystem.instance.Saving(gameObject.name, new Vector2(transform.position.x, transform.position.z));
    }
    public virtual void OnExitStasis(GameObject _object)
    {
        if (!hasDoneStart)
            OnStarted(_object);
        isOnStasis = false;
        sound.secondClipName = "ExitStasis";
        sound.PlaySeconde();
        if(rgb)
        rgb.useGravity = true;
        if (isGrab)
        {
            if(!inputManager)
                inputManager = _object.GetComponent<GrabablePhysicsHandler>().inputManager;
            if (currentInteractor.name == "RightHand Controller")
            {
                inputManager.OnHapticImpulseRight.Invoke(vibrationForce, vibrationTime);
            }
            else if (currentInteractor.name == "LeftHand Controller")
            {
                inputManager.OnHapticImpulseLeft.Invoke(vibrationForce, vibrationTime);

            }
            else { Debug.LogError("Your hand dont have the good name, please name it RightHand Controller and LeftHand Controller"); }
        }
    }
    public virtual void OnEnterStasis(GameObject _object, bool isGrab, Rigidbody _rgb )
    {
        if (!hasDoneStart)
            OnStarted(_object);
        sound.secondClipName = "EnterStasis";
        sound.PlaySeconde();
        isOnStasis = true;
        isSlowingDown = true;
        timerSlow = 0;
        timerToSlowInStasis = _object.GetComponent<GrabablePhysicsHandler>().timeToSlow;
        if (!inputManager)
            inputManager = _object.GetComponent<GrabablePhysicsHandler>().inputManager;
        inputManager = _object.GetComponent<GrabablePhysicsHandler>().inputManager;
        slowForce = _object.GetComponent<GrabablePhysicsHandler>().slowForce;
        if (_object.transform.position.x > 200)
            return;
        rgb = _rgb;
        if(!isGrab)
        StartCoroutine(SlowCoroutine());
        else
        {
            if(currentInteractor.name == "RightHand Controller")
            {
                inputManager.OnHapticImpulseRight.Invoke(vibrationForce,vibrationTime);
            }
            else if(currentInteractor.name == "LeftHand Controller")
            {
                inputManager.OnHapticImpulseLeft.Invoke(vibrationForce, vibrationTime);

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
