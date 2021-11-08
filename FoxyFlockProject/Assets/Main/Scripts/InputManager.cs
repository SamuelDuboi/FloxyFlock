using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
public class InputManager : MonoBehaviour
{

    public HandController rightHand;
    public HandController leftHand;
    private bool rightHandIsTrigger;
    private bool rightHandIsTriggerRelease;
    private bool leftHandIsTrigger;
    private bool leftHandIsTriggerRelease;
    private float triggerMinTreshold;
    [HideInInspector] public bool canMove;
    public CharacterStats characterStats;

    
    public UnityEvent OnLeftTrigger;
    public UnityEvent OnRightTrigger;
    public UnityEvent OnBothTrigger;
    public UnityEvent OnLeftTriggerRelease;
    public UnityEvent OnRightTriggerRelease;
    public UnityEvent OnCanMove;
    // Start is called before the first frame update
    void Start()
    {
        rightHandIsTriggerRelease = true;
        leftHandIsTriggerRelease = true;
        triggerMinTreshold = characterStats.minTriggerTreshold;
        OnRightTrigger.AddListener(OnRightTriggerListener);
        OnLeftTrigger.AddListener(OnLeftTriggerListener);
        OnBothTrigger.AddListener(OnBothTriggerListener);
        OnRightTriggerRelease.AddListener(OnRightTriggerReleaseListener);
        OnLeftTriggerRelease.AddListener(OnLeftTriggerReleaseListener);
    }

    // Update is called once per frame
    void Update()
    {
        InputHelpers.IsPressed(rightHand.inputDevice, characterStats.moveTrigger, out rightHandIsTrigger, triggerMinTreshold);
        InputHelpers.IsPressed(leftHand.inputDevice, characterStats.moveTrigger, out leftHandIsTrigger, triggerMinTreshold);

        //if right trigger is press, only call once like a press enter
        if (rightHandIsTriggerRelease && rightHandIsTrigger )
        {
            OnRightTrigger.Invoke();
        }
        //if left trigger is press, only call once like a press enter
        if (leftHandIsTriggerRelease && leftHandIsTrigger )
        {
            OnLeftTrigger.Invoke();
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
    }

    /// <summary>
    /// Call when the right trigger is pressed once
    /// </summary>
    private void OnRightTriggerListener()
    {
        rightHandIsTriggerRelease = false;
        if (!leftHandIsTriggerRelease)
        {
            OnBothTrigger.Invoke();
        }
    }
    /// <summary>
    /// call when the left trigger is pressed once
    /// </summary>
    private void OnLeftTriggerListener()
    {
        leftHandIsTriggerRelease = false;
        if (!rightHandIsTriggerRelease)
        {
            OnBothTrigger.Invoke();
        }
    }
    /// <summary>
    /// call when both trigger are pressed
    /// </summary>
    private void OnBothTriggerListener()
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
}
