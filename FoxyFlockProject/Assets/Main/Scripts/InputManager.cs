using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using System.Collections;
public class InputManager : MonoBehaviour
{
    
    public HandController rightHand;
    public HandController leftHand;
    private bool rightHandIsTrigger;
    private bool rightHandIsTriggerRelease;
    private bool leftHandIsTrigger;
    private bool leftHandIsTriggerRelease;
    private bool leftHandIsGrab;
    private bool rightHandIsGrab;
    private bool rightHandIsGrabRelease;
    private bool leftHandIsGrabRelease;


    private bool snapTurnLeft;
    private bool snapTurnRight;
    private bool snapTurnRelease;

    private float triggerMinTreshold;
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

    public UnityEvent<Vector3> OnSnapTurn;
    public UnityEvent OnSnapTurnRelease;

    public static InputManager instance;
    private Vector3 snapTurnAngle;
    public PlayerMovement playerMovement;
    [HideInInspector] public bool seeTable;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(rightHand.inputDevice, InputHelpers.Button.PrimaryAxis2DRight, out snapTurnRight);
        InputHelpers.IsPressed(rightHand.inputDevice, InputHelpers.Button.PrimaryAxis2DLeft, out snapTurnLeft);
        if (snapTurnLeft && snapTurnRelease)
            OnSnapTurn.Invoke(snapTurnAngle*-1);
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
    }

    /// <summary>
    /// Call when the right trigger is pressed once
    /// </summary>
    private void OnRightTriggerListener(bool seeTable)
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
    private void OnLeftTriggerListener(bool seeTable)
    {
        leftHandIsTriggerRelease = false;
        if (!rightHandIsTriggerRelease)
        {
            StartCoroutine(WaitTonInvokeBoth(seeTable));
        }
    }

    IEnumerator WaitTonInvokeBoth(bool seeTable)
    {
        yield return new WaitForEndOfFrame();
        OnBothTrigger.Invoke(seeTable);
    }
    /// <summary>
    /// call when both trigger are pressed
    /// </summary>
    private void OnBothTriggerListener(bool seeTable)
    {
        canMove = true;
    }

    /// <summary>
    /// Call when the right trigger is pressed once
    /// </summary>
    private void OnRightTriggerReleaseListener()
    {
        rightHandIsTriggerRelease = true;
        canMove = false;
    }
    /// <summary>
    /// Call when the right trigger is relase once
    /// </summary>
    private void OnLeftTriggerReleaseListener()
    {
        leftHandIsTriggerRelease = true;
        canMove = false;
    }
    private void OnLeftHandGrabListener()
    {
        leftHandIsGrabRelease = false;
    }
    private void OnRightHandGrabListener()
    {
        rightHandIsGrabRelease = false;
    }
    private void OnLeftHandGrabReleaseListener()
    {
        leftHandIsGrabRelease = true;
    }
    private void OnRightHandGrabReleaseListener()
    {
        rightHandIsGrabRelease = false;
    }

    private void OnSnapTurnActiveListener(Vector3 direction)
    {
        snapTurnRelease = false;
    }
    private void OnSnapTurnReleaseListener( )
    {
        snapTurnRelease = true;

    }
}
