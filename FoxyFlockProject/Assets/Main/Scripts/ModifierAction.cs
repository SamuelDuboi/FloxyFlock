using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ModifierAction : MonoBehaviour
{
    public virtual void OnGrabed(GameObject objcet)
    {
        
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
