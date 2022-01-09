using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : ModifierAction
{
    [SerializeField] private float massScale = 3;
    [SerializeField] [Range(0, 20)] private float smoothPosition = 4;
    [SerializeField] [Range(0, 1f)] private float tightenPosition = 0.1f;
    [SerializeField] [Range(0, 20)] private float smoothRotation = 4;
    [SerializeField] [Range(0, 1f)] private float tightenRotation = 0.1f;

    public override void OnStarted(GameObject _object)
    {
        base.OnStarted(_object);

        flox = _object;

        GrabbableObject flockGrabbable = flox.GetComponent<GrabbableObject>();

        flockGrabbable.smoothPosition = true;
        flockGrabbable.smoothPositionAmount = smoothPosition;
        flockGrabbable.tightenPosition = tightenPosition;

        flockGrabbable.smoothRotation = true;
        flockGrabbable.smoothRotationAmount = smoothRotation;
        flockGrabbable.tightenRotation = tightenRotation;

        Rigidbody flockRb = flox.GetComponent<Rigidbody>();
        flockRb.mass *= massScale;
    }
    public override void OnEnterStasis(GameObject _object, bool isGrab, Rigidbody rgb)
    {
        base.OnEnterStasis(_object, isGrab,rgb);
    }
    public override void OnGrabed(GameObject _object)
    {
        grabSound = "MetalGrab";
        base.OnGrabed(_object);
    }
    public override void OnHitGround(GameObject _object, Vector3 initPos, bool isGrab)
    {
       
        base.OnHitGround(_object,initPos,isGrab);
    }
    public override void OnHitSomething(GameObject _object, Vector3 velocity, GameObject collision)
    {
        collisionSound = "MetalCollision";
        base.OnHitSomething(_object, velocity, collision);
    }
    public override void OnReleased(GameObject _object)
    {
        base.OnReleased(_object);
    }
}