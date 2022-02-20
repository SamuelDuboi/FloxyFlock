using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private Transform pressTransform;
    [SerializeField] private Rigidbody pressRBody;
    [SerializeField] private MeshRenderer pressRenderer;
    [SerializeField] private Collider pressCollider;
    [SerializeField] private Transform upLimitTransform;
    [SerializeField] private Transform lowerLimitTransform;
    [SerializeField] private Animator buttonAnimator;

    [SerializeField] private bool hold = true;
    [SerializeField] private float holdTime = 3f;

    [SerializeField] private Collider[] collidersToIgnore;

    [SerializeField] [Range(0f, 1f)] private float threshold = 0.1f;
    [SerializeField] private float springForce = 10f;

    private bool isPressed;
    private float lowerUpDistance;

    public UnityEvent onPressed, onReleased, onHoldCompleted;

    private float timeSinceBeginHold;
    private float holdStartTime;
    private bool canHoldTimer;

    private MaterialPropertyBlock propBlock;

    private void Start()
    {
        if (collidersToIgnore.Length > 0)
        {
            foreach (Collider collider in collidersToIgnore)
            {
                Physics.IgnoreCollision(pressCollider, collider);
            }
        }

        propBlock = new MaterialPropertyBlock();

        lowerUpDistance = (upLimitTransform.position - lowerLimitTransform.position).magnitude;
    }

    private void FixedUpdate()
    {
        PressMovement();
    }

    private void PressMovement()
    {
        float distance = (pressTransform.position - lowerLimitTransform.position).magnitude;

        if (pressTransform.position.y > upLimitTransform.position.y)
        {
            pressTransform.position = new Vector3(pressTransform.position.x, upLimitTransform.transform.position.y, pressTransform.transform.position.z);
            pressRBody.isKinematic = true;
        }
        else if (pressTransform.position.y < lowerLimitTransform.position.y)
        {
            pressTransform.position = new Vector3(pressTransform.position.x, lowerLimitTransform.transform.position.y, pressTransform.transform.position.z);
            pressCollider.isTrigger = true;
        }
        else
        {
            pressRBody.AddForce(pressTransform.up * springForce * (Mathf.Clamp(1f - distance, 0.01f, 1f)) * Time.fixedDeltaTime);
            pressCollider.isTrigger = false;
            pressRBody.isKinematic = false;
        }


        if (distance < lowerUpDistance * threshold)
        {
            if (!isPressed)
            {
                Pressed();
                StartHoldTimer();
            }

            if (hold && canHoldTimer)
                HoldTimer();
        }

        else if (pressTransform.position.y > lowerLimitTransform.position.y)
        {
            if (isPressed)
            {
                Released();
            }
        }
    }

    private void Pressed()
    {
        isPressed = true;
        onPressed.Invoke();
        ChangeMatOnPressState(1f);
        print("press");
    }

    private void Released()
    {
        isPressed = false;
        onReleased.Invoke();
        ChangeMatOnPressState(0f);
        Debug.Log(this + " isReleased");
    }

    private void HoldCompleted()
    {
        canHoldTimer = false;
        onHoldCompleted.Invoke();
        Debug.Log(this + " isReleased");
    }


    private void StartHoldTimer()
    {
        holdStartTime = (float)AudioSettings.dspTime;
        canHoldTimer = true;
    }

    private void HoldTimer()
    {
        timeSinceBeginHold = (float)AudioSettings.dspTime - holdStartTime;

        if (timeSinceBeginHold >= holdTime)
        {
            HoldCompleted();
        }
    }

    private void ChangeMatOnPressState(float state)
    {
        //Recup Data
        pressRenderer.GetPropertyBlock(propBlock);
        //EditZone
        propBlock.SetFloat("SelectedOutlineColor", state);
        //Push Data
        pressRenderer.SetPropertyBlock(propBlock);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            buttonAnimator.SetBool("IsOpen", true);
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            buttonAnimator.SetBool("IsOpen", false);
        }
    }
}
