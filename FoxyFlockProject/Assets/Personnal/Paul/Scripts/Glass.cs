using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : ModifierAction
{
    [SerializeField] private GameObject shardsParticleSystem;
    [SerializeField] private float breakThreshold = 10;

    public override void OnStarted(GameObject _object)
    {
        base.OnStarted(_object);
    }
    public override void OnEnterStasis(GameObject _object, bool isGrab, Rigidbody rgb)
    {
        base.OnEnterStasis(_object, isGrab,rgb);
    }
    public override void OnGrabed(GameObject _object)
    {
        base.OnGrabed(_object);
    }
    public override void OnHitGround(GameObject _object, Vector3 initPos, bool isGrab)
    {
        base.OnHitGround(_object,initPos,isGrab);
    }
    public override void OnHitSomething(GameObject _object, Vector3 velocity, GameObject collision)
    {
        base.OnHitSomething(_object, velocity, collision);

        if (velocity.magnitude >= breakThreshold)
        {
            Instantiate(shardsParticleSystem, this.transform.position, Quaternion.identity);
        }

    }
    public override void OnReleased(GameObject _object)
    {
        base.OnReleased(_object);
    }
}