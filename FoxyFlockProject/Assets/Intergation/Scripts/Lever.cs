using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [SerializeField] private float detectionThreshold = 0.1f;
    [SerializeField] private float deadzone = 0.025f;

    private bool _isPressed;
    private Vector3 startRotation;

    private HingeJoint joint;
    private Rigidbody rb;

    public UnityEvent onPressed, onReleased;

    private void Start()
    {
        startRotation = transform.localEulerAngles;

        joint = GetComponent<HingeJoint>();
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
        var value = transform.localEulerAngles.x / Mathf.Abs(joint.limits.max);

        if (Mathf.Abs(value) < deadzone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed()
    {
        _isPressed = true;
        //StartCoroutine(MakeKinematic());
        onPressed.Invoke();
        Debug.Log(this + " isPressed");
    }

    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
        Debug.Log(this + " isReleased");
    }

    private IEnumerator MakeKinematic()
    {
        rb.isKinematic = true;

        yield return new WaitForSeconds(0.5f);

        rb.isKinematic = false;
    }
}
