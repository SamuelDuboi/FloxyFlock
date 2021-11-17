using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierAction 
{
    public virtual void OnGrabed(GameObject objcet)
    {
        Debug.Log("yo");
    }
    public virtual void OnReleased(GameObject objcet)
    {

    }
    public virtual void OnHitSomething(GameObject objcet, Vector3 velocity, GameObject collision)
    {

    }
    public virtual void OnHitGround(GameObject objcet)
    {

    }
    public virtual void OnEnterStasis(GameObject objcet, bool isGrab)
    {

    }
}
