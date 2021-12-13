using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.Events;
using System.Collections;
public class InputManager : MonoBehaviour
{
    
    public HandController rightHand;
    public HandController leftHand;
    protected bool rightHandIsTrigger;
    protected bool rightHandIsTriggerRelease;
    protected bool leftHandIsTrigger;
    protected bool leftHandIsTriggerRelease;
    protected bool leftHandIsGrab;
    protected bool rightHandIsGrab;
    protected bool rightHandIsGrabRelease;
    protected bool leftHandIsGrabRelease;


    protected bool snapTurnLeft;
    protected bool snapTurnRight;
    protected bool snapTurnRelease;
    protected float triggerMinTreshold;
    [HideInInspector] public bool canMove;
    public CharacterStats characterStats;
    // spawn
    private bool canSpawn;


    public UnityEvent<bool> OnLeftTrigger;
    public UnityEvent<bool> OnRightTrigger;
    public UnityEvent<bool> OnBothTrigger;
    public UnityEvent OnLeftTriggerRelease;
    public UnityEvent OnRightTriggerRelease;
    public UnityEvent OnCanMove;
    public UnityEvent OnSpawn;
    public UnityEvent OnLeftGrab;
    public UnityEvent OnRightGrab;
    public UnityEvent OnRightGrabRelease;
    public UnityEvent OnLeftGrabRelease;
    public UnityEvent OnGrabbingRight;
    public UnityEvent OnGrabbingLeft;
    public UnityEvent OnGrabbingReleaseLeft;
    public UnityEvent OnGrabbingReleaseRight;
    public UnityEvent<float, float> OnHapticImpulseRight;
    public UnityEvent<float,float> OnHapticImpulseLeft;

    public UnityEvent<Vector3> OnSnapTurn;
    public UnityEvent OnSnapTurnRelease;

    public UnityEvent OnMenuPressed;

    //
    public UnityEvent<bool> ActiveSound;
    protected bool isActiveSound;


    protected Vector3 snapTurnAngle;
    public PlayerMovement playerMovement;
    [HideInInspector] public bool seeTable;
    protected SoundReader sound;
    public bool isMenuPressed;
    public bool isMenuPressing;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rightHandIsTriggerRelease = true;
        leftHandIsTriggerRelease = true;
        leftHandIsGrabRelease = true;
        rightHandIsGrabRelease = true;
        snapTurnRelease = true;
        triggerMinTreshold = characterStats.minTriggerTreshold;
        OnRightTrigger.AddListener(OnRightTriggerListener);
        OnLeftTrigger.AddListener(OnLeftTriggerListener);
        OnBothTrigger.AddListener(OnBothTriggerListener);
        OnRightTriggerRelease.AddListener(OnRightTriggerReleaseListener);
        OnLeftTriggerRelease.AddListener(OnLeftTriggerReleaseListener);

        OnLeftGrab.AddListener(OnLeftHandGrabListener);
        OnRightGrab.AddListener(OnRightHandGrabListener);
        OnLeftGrabRelease.AddListener(OnLeftHandGrabReleaseListener);
        OnRightGrabRelease.AddListener(OnRightHandGrabReleaseListener);

        OnSnapTurn.AddListener(OnSnapTurnActiveListener);
        OnSnapTurnRelease.AddListener(OnSnapTurnReleaseListener);
        snapTurnAngle = Vector3.up * characterStats.snapTurnAngle;

        OnHapticImpulseLeft.AddListener(LeftHapticListener);
        OnHapticImpulseRight.AddListener(RightHapticListener);
        sound = GetComponent<SoundReader>();
        SoundManager.instance.AddInputManager(this);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        InputHelpers.IsPressed(rightHand.inputDevice, InputHelpers.Button.PrimaryAxis2DRight, out snapTurnRight);
        InputHelpers.IsPressed(rightHand.inputDevice, InputHelpers.Button.PrimaryAxis2DLeft, out snapTurnLeft);
        if (snapTurnLeft && snapTurnRelease)
        {
            OnSnapTurn.Invoke(snapTurnAngle*-1);           
        }
        if (snapTurnRight && snapTurnRelease)
            OnSnapTurn.Invoke(snapTurnAngle);
        if (!snapTurnLeft && !snapTurnRight && !snapTurnRelease)
            OnSnapTurnRelease.Invoke();

        #region Grab
        InputHelpers.IsPressed(rightHand.inputDevice, characterStats.gripBtton, out rightHandIsGrab);
        InputHelpers.IsPressed(leftHand.inputDevice, characterStats.gripBtton, out leftHandIsGrab);

        if(rightHandIsGrabRelease && rightHandIsGrab)
        {
            OnRightGrab.Invoke();
        }
        if (leftHandIsGrabRelease && leftHandIsGrab)
        {
            OnLeftGrab.Invoke();
        }
        if (!leftHandIsGrabRelease && !leftHandIsGrab)
        {
            OnLeftGrabRelease.Invoke();
        }
        if (!rightHandIsGrabRelease && !rightHandIsGrabRelease)
        {
            OnRightGrabRelease.Invoke();
        }
        #endregion
        #region Trigger
        InputHelpers.IsPressed(rightHand.inputDevice, characterStats.moveTrigger, out rightHandIsTrigger, triggerMinTreshold);
        InputHelpers.IsPressed(leftHand.inputDevice, characterStats.moveTrigger, out leftHandIsTrigger, triggerMinTreshold);

        //if right trigger is press, only call once like a press enter
        if (rightHandIsTriggerRelease && rightHandIsTrigger )
        {
            seeTable = playerMovement.SeeTable();
            OnRightTrigger.Invoke(seeTable);
        }
        //if left trigger is press, only call once like a press enter
        if (leftHandIsTriggerRelease && leftHandIsTrigger )
        {
            seeTable = playerMovement.SeeTable();
            OnLeftTrigger.Invoke(seeTable);
        }
        //if right trigger is reales, only call once like a press exit
        if (!rightHandIsTriggerRelease && !rightHandIsTrigger )
        {
            OnRightTriggerRelease.Invoke();

        }
        //if right trigger is reales, only call once like a press exit
        if (!leftHandIsTriggerRelease && !leftHandIsTrigger )
        {
            OnLeftTriggerRelease.Invoke();
        }
        if (canMove)
        {
            OnCanMove.Invoke();
        }
        #endregion

        InputHelpers.IsPressed(leftHand.inputDevice, InputHelpers.Button.SecondaryButton, out isMenuPressed);
        if (isMenuPressed && !isMenuPressing)
        {
            isMenuPressing = true;
            Debug.Log("menusPressed");
            OnMenuPressed.Invoke();
        }
        if (!isMenuPressed && isMenuPressing)
        {
            isMenuPressing = false;
        }
    }
    #region TriggerListener

    /// <summary>
    /// Call when the right trigger is pressed once
    /// </summary>
    protected virtual void OnRightTriggerListener(bool seeTable)
    {
        rightHandIsTriggerRelease = false;
        if (!leftHandIsTriggerRelease)
        {
            StartCoroutine(WaitTonInvokeBoth(seeTable));
        }
    }
    /// <summary>
    /// call when the left trigger is pressed once
    /// </summary>
    protected virtual void OnLeftTriggerListener(bool seeTable)
    {
        leftHandIsTriggerRelease = false;
        if (!rightHandIsTriggerRelease)
        {
            StartCoroutine(WaitTonInvokeBoth(seeTable));
        }
    }

    protected virtual IEnumerator WaitTonInvokeBoth(bool seeTable)
    {
        yield return new WaitForEndOfFrame();
        OnBothTrigger.Invoke(seeTable);
    }
    /// <summary>
    /// call when both trigger are pressed
    /// </summary>
    protected virtual void OnBothTriggerListener(bool seeTable)
    {
        canMove = true;
        sound.clipName = "MoveAroundLoop";
        sound.Play();
    }

    /// <summary>
    /// Call when the right trigger is pressed once
    /// </summary>
    protected virtual void OnRightTriggerReleaseListener()
    {
        rightHandIsTriggerRelease = true;
        canMove = false;
        sound.StopSound();
    }
    /// <summary>
    /// Call when the right trigger is relase once
    /// </summary>
    protected virtual void OnLeftTriggerReleaseListener()
    {
        leftHandIsTriggerRelease = true;
        canMove = false;
        sound.StopSound();

    }
    #endregion
    #region GrabListeners
    protected virtual void OnLeftHandGrabListener()
    {
        leftHandIsGrabRelease = false;
    }
    protected virtual void OnRightHandGrabListener()
    {
        rightHandIsGrabRelease = false;
    }
    protected virtual void OnLeftHandGrabReleaseListener()
    {
        leftHandIsGrabRelease = true;
    }
    protected virtual void OnRightHandGrabReleaseListener()
    {
        rightHandIsGrabRelease = false;
    }
    #endregion
    #region SnapeTurnListener
    protected virtual void OnSnapTurnActiveListener(Vector3 direction)
    {
        snapTurnRelease = false;
    }
    protected virtual void OnSnapTurnReleaseListener( )
    {
        snapTurnRelease = true;

    }
    #endregion
    protected virtual void LeftHapticListener(float force, float timer)
    {
        leftHand.SendHapticImpulse(force, timer);
    }
    protected virtual void RightHapticListener(float force, float timer)
    {
        rightHand.SendHapticImpulse(force, timer);
    }
    public virtual void OnActiveSound()
    {
        isActiveSound = !isActiveSound;
        ActiveSound.Invoke(isActiveSound);
    }
    public virtual void Sound()
    {
        SoundManager.instance.ActiveSound(!SoundManager.instance.mute);
    }
    public virtual void SpawnInvoke()
    {
        OnSpawn.Invoke();
    }
}
