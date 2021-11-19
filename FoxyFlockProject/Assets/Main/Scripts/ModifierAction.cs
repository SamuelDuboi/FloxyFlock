using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ModifierAction : MonoBehaviour
{
    public virtual void OnGrabed(GameObject _object)
    {
        
    }
    public virtual void OnReleased(GameObject _object)
    {

    }
    public virtual void OnHitSomething(GameObject _object, Vector3 velocity, GameObject collision)
    {

    }
    public virtual void OnHitGround(GameObject _object)
    {

    }
    public virtual void OnEnterStasis(GameObject _object, bool isGrab)
    {

    }
}
