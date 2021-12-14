using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using System.Collections;
public class InputManagerMulti : InputManager
{

    public PlayerMovementMulti playerMovementMulti;

    // Start is called before the first frame update
   protected override void Start()
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
    protected override void Update()
    {
        InputHelpers.IsPressed(rightHand.inputDevice, InputHelpers.Button.PrimaryAxis2DRight, out snapTurnRight);
        InputHelpers.IsPressed(rightHand.inputDevice, InputHelpers.Button.PrimaryAxis2DLeft, out snapTurnLeft);
        if (snapTurnLeft && snapTurnRelease)
        {
            OnSnapTurn.Invoke(snapTurnAngle * -1);
        }
        if (snapTurnRight && snapTurnRelease)
            OnSnapTurn.Invoke(snapTurnAngle);
        if (!snapTurnLeft && !snapTurnRight && !snapTurnRelease)
            OnSnapTurnRelease.Invoke();

        #region Grab
        InputHelpers.IsPressed(rightHand.inputDevice, characterStats.gripBtton, out rightHandIsGrab);
        InputHelpers.IsPressed(leftHand.inputDevice, characterStats.gripBtton, out leftHandIsGrab);

        if (rightHandIsGrabRelease && rightHandIsGrab)
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
        if (rightHandIsTriggerRelease && rightHandIsTrigger)
        {
            seeTable = playerMovementMulti.SeeTable();
            OnRightTrigger.Invoke(seeTable);
        }
        //if left trigger is press, only call once like a press enter
        if (leftHandIsTriggerRelease && leftHandIsTrigger)
        {
            seeTable = playerMovementMulti.SeeTable();
            OnLeftTrigger.Invoke(seeTable);
        }
        //if right trigger is reales, only call once like a press exit
        if (!rightHandIsTriggerRelease && !rightHandIsTrigger)
        {
            OnRightTriggerRelease.Invoke();

        }
        //if right trigger is reales, only call once like a press exit
        if (!leftHandIsTriggerRelease && !leftHandIsTrigger)
        {
            OnLeftTriggerRelease.Invoke();
        }
        if (canMove)
        {
            OnCanMove.Invoke();
        }
        #endregion
        InputHelpers.IsPressed(leftHand.inputDevice, InputHelpers.Button.MenuButton, out isMenuPressed);
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
    protected override void OnRightTriggerListener(bool seeTable)
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
    protected override void OnLeftTriggerListener(bool seeTable)
    {
        leftHandIsTriggerRelease = false;
        if (!rightHandIsTriggerRelease)
        {
            StartCoroutine(WaitTonInvokeBoth(seeTable));
        }
    }

    protected override IEnumerator WaitTonInvokeBoth(bool seeTable)
    {
        yield return new WaitForEndOfFrame();
        OnBothTrigger.Invoke(seeTable);
    }
    /// <summary>
    /// call when both trigger are pressed
    /// </summary>
    protected override void OnBothTriggerListener(bool seeTable)
    {
        canMove = true;
        sound.clipName = "MoveAroundLoop";
        sound.Play();
    }

    /// <summary>
    /// Call when the right trigger is pressed once
    /// </summary>
    protected override void OnRightTriggerReleaseListener()
    {
        rightHandIsTriggerRelease = true;
        canMove = false;
        sound.StopSound();
    }
    /// <summary>
    /// Call when the right trigger is relase once
    /// </summary>
    protected override void OnLeftTriggerReleaseListener()
    {
        leftHandIsTriggerRelease = true;
        canMove = false;
        sound.StopSound();

    }
    #endregion
    #region GrabListeners
    protected override void OnLeftHandGrabListener()
    {
        leftHandIsGrabRelease = false;
    }
    protected override void OnRightHandGrabListener()
    {
        rightHandIsGrabRelease = false;
    }
    protected override void OnLeftHandGrabReleaseListener()
    {
        leftHandIsGrabRelease = true;
    }
    protected override void OnRightHandGrabReleaseListener()
    {
        rightHandIsGrabRelease = false;
    }
    #endregion
    #region SnapeTurnListener
    protected override void OnSnapTurnActiveListener(Vector3 direction)
    {
        snapTurnRelease = false;
    }
    protected override void OnSnapTurnReleaseListener()
    {
        snapTurnRelease = true;

    }
    #endregion
    protected override void LeftHapticListener(float force, float timer)
    {
        leftHand.SendHapticImpulse(force, timer);
    }
    protected override void RightHapticListener(float force, float timer)
    {
        rightHand.SendHapticImpulse(force, timer);
    }
    public override void OnActiveSound()
    {
        isActiveSound = !isActiveSound;
        ActiveSound.Invoke(isActiveSound);
    }
    public override void Sound()
    {
        SoundManager.instance.ActiveSound(!SoundManager.instance.mute);
    }
    public override void SpawnInvoke()
    {
        OnSpawn.Invoke();
    }
}
