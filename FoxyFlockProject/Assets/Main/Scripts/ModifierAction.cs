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
    public float vibrationForce = 0.5f;
    public float vibrationTime = 0.2f;
    protected bool isGrab;
    public string grab;
    public string collisionTable;
    private bool cantPlaySound;
    private bool hasDoneStart;
    public InputManager inputManager;
    public virtual void OnStarted(GameObject _object)
    {
        sound = _object.AddComponent<SoundReader>();
        int rand = Random.Range(0, 2);

        if (rand == 0)
            grab = "Grab1";
        else
            grab = "Grab2";
        sound.clipName = grab;
        collisionTable = "CollisionTable";
        sound.secondClipName = "EnterStasis";
        sound.ThirdClipName = collisionTable;
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
        isGrab = true;
        int rand = Random.Range(0, 2);
        if (rand == 0)
            grab = "Grab1";
        else
            grab = "Grab2";
        sound.clipName = grab;
        sound.Play();
    }
    public virtual void OnReleased(GameObject _object)
    {
        if (!hasDoneStart)
            OnStarted(_object);
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
        if (!cantPlaySound)
        {
            if(collision.tag == "Table"    || collision.tag == "TableComponent" || collision.tag == "Table2")
            {
              //  sound.PlayThird();
                StartCoroutine(WaiToPlaySoundTable());
            }
        }
    }
    IEnumerator WaiToPlaySoundTable()
    {
        cantPlaySound = true;
        yield return new WaitForSeconds(0.5f);
        cantPlaySound = false;
    }
    public virtual void OnHitGround(GameObject _object, Vector3 initPos, bool isGrab)
    {
        /*if (!isGrab)
        {
            _object.transform.position = initPos;
        }*/
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
