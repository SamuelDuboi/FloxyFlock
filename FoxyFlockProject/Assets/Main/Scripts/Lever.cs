using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [SerializeField] private float detectionThreshold = 0.1f;
    [SerializeField] private float deadzone = 0.025f;

    private bool _isPressed;
    private bool canPress;

    private HingeJoint joint;
    private Rigidbody rb;

    public UnityEvent onPressed, onReleased;

    private void Start()
    {

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
        float tempAngle=0;
        if (transform.localEulerAngles.x > 190)
            tempAngle = 360 - transform.eulerAngles.x;
        else
            tempAngle = transform.eulerAngles.x;
        if (tempAngle > joint.limits.max)
            return 0.5f;
        var value = tempAngle / Mathf.Abs(joint.limits.max);

        if (Mathf.Abs(value) < deadzone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed()
    {
        _isPressed = true;
       
        onPressed.Invoke();
        Debug.Log(this + " isPressed");
    }

    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
        StartCoroutine(MakeKinematic());
        Debug.Log(this + " isReleased");
    }

    private IEnumerator MakeKinematic()
    {
        rb.isKinematic = true;
        yield return new WaitForSeconds(0.5f);
        
        rb.isKinematic = false;
    }
}
