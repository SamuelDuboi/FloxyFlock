using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private Transform pressTransform;
    [SerializeField] private Rigidbody pressRBody;
    [SerializeField] private Collider pressCollider;
    [SerializeField] private Transform upLimitTransform;
    [SerializeField] private Transform lowerLimitTransform;
    [SerializeField] private Animator buttonAnimator;

    [SerializeField] private Collider[] collidersToIgnore;

    [SerializeField] [Range(0f, 1f)] private float threshold = 0.1f;
    [SerializeField] private float springForce = 10f;

    private bool isPressed;
    private float lowerUpDistance;

    private bool handAreInProximity;

    public UnityEvent onPressed, onReleased;

    private void Start()
    {
        if (collidersToIgnore.Length > 0)
        {
            foreach (Collider collider in collidersToIgnore)
            {
                Physics.IgnoreCollision(pressCollider, collider);
            }
        }

        lowerUpDistance = (lowerLimitTransform.position - upLimitTransform.position).magnitude;
    }

    private void FixedUpdate()
    {
        PressMovement();
    }

    private void PressMovement()
    {
        float distance = (lowerLimitTransform.position - pressTransform.position).magnitude;

        if (pressTransform.position.y >= upLimitTransform.position.y)
        {
            pressTransform.position = new Vector3(pressTransform.position.x, upLimitTransform.transform.position.y, pressTransform.transform.position.z);
        }
        else if (pressTransform.position.y <= lowerLimitTransform.position.y)
        {
            pressTransform.position = new Vector3(pressTransform.position.x, lowerLimitTransform.transform.position.y, pressTransform.transform.position.z);
        }
        else
        {
            pressRBody.AddForce(Vector3.up * springForce * (distance + 0.01f) * Time.fixedDeltaTime);
        }

        if (distance < lowerUpDistance * threshold)
        {
            if (!isPressed)
            {
                Pressed();
            }
        }
        else
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
    }

    private void Released()
    {
        isPressed = false;
        onReleased.Invoke();
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
