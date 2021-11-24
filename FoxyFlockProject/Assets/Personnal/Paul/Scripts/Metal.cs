using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : ModifierAction
{
    [SerializeField] private float massScale = 3;
    [SerializeField] [Range(0, 20)] private float smoothPosition = 4;

    public override void OnStarted(GameObject _object)
    {
        base.OnStarted(_object);

        flock = _object;

        GrabbableObject flockGrabbable = flock.GetComponent<GrabbableObject>();
        flockGrabbable.smoothPosition = true;
        flockGrabbable.smoothPositionAmount = smoothPosition;
        flockGrabbable.tightenPosition = 0;

        Rigidbody flockRb = flock.GetComponent<Rigidbody>();
        flockRb.mass = flockRb.mass * massScale;
    }
    public override void OnEnterStasis(GameObject _object, bool isGrab, Rigidbody rgb)
    {
        base.OnEnterStasis(_object, isGrab,rgb);
    }
    public override void OnGrabed(GameObject _object)
    {
        base.OnGrabed(_object);
    }
    public override void OnHitGround(GameObject _object)
    {
        base.OnHitGround(_object);
    }
    public override void OnHitSomething(GameObject _object, Vector3 velocity, GameObject collision)
    {
        base.OnHitSomething(_object, velocity, collision);
    }
    public override void OnReleased(GameObject _object)
    {
        base.OnReleased(_object);
    }
}