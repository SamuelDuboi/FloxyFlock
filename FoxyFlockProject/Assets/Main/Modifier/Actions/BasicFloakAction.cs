using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFloakAction : ModifierAction
{
    
    public override void OnEnterStasis(GameObject _object, bool isGrab, Rigidbody rgb)
    {
        base.OnEnterStasis(_object, isGrab,rgb);
    }
    public override void OnGrabed(GameObject _object)
    {
        timerSlow = 0;
        grabSound = "BasicGrab";
        base.OnGrabed(_object);
    }
    public override void OnHitGround(GameObject _object, Vector3 initPos, bool isGrab)
    {
        base.OnHitGround(_object, initPos, isGrab);
    }
    public override void OnHitSomething(GameObject _object, Vector3 velocity, GameObject collision)
    {
        collisionSound = "BasicCollisionTable";

        base.OnHitSomething(_object, velocity, collision);
    }
    public override void OnReleased(GameObject _object)
    {
        base.OnReleased(_object);
    }
}
