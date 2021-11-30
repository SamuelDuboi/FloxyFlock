using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private float detectionThreshold = 0.1f;
    [SerializeField] private float deadzone = 0.025f;
    [SerializeField] private bool isRotation = false;

    private bool _isPressed;

    private Vector3 startPos;
    private Quaternion startRotation;

    private ConfigurableJoint joint;
    private Rigidbody rb;

    public UnityEvent onPressed, onReleased;

    private void Start()
    {
        if (!isRotation)
            startPos = transform.localPosition;
        else
            startRotation = transform.localRotation;

        joint = GetComponent<ConfigurableJoint>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_isPressed && GetValue() + detectionThreshold >= 1) 
            Pressed();
        if (_isPressed && GetValue() - detectionThreshold <= 0)
            Released();
    }

    private float GetValue()
    {
        var value = 0f;

        if (!isRotation)
        {
            value = Vector3.Distance(startPos, transform.localPosition) / joint.linearLimit.limit;
        }
        else
        {
            value = Quaternion.Angle(startRotation, transform.localRotation) / Mathf.Abs(joint.lowAngularXLimit.limit);
        }

        if (Mathf.Abs(value) < deadzone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed()
    {
        _isPressed = true;
        StartCoroutine(MakeKinematic());
        onPressed.Invoke();
    }

    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
    }

    private IEnumerator MakeKinematic()
    {
        rb.isKinematic = true;

        yield return new WaitForSeconds(0.5f);

        rb.isKinematic = false;
    }
}
