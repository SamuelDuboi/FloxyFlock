using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFloakAction : ModifierAction
{
    public override void OnEnterStasis(GameObject _object, bool isGrab)
    {
        base.OnEnterStasis(_object, isGrab);
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
        Debug.Log("hit");
        base.OnHitSomething(_object, velocity, collision);
    }
    public override void OnReleased(GameObject _object)
    {
        base.OnReleased(_object);
    }
}
