using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Milestone : MonoBehaviour
{
   [HideInInspector] public bool isFinale;
    private Vector3 flocksPoint;
    public Transform _transform;
    public BoxCollider boxCollider;
    public LayerMask layerMask;
    private Vector3 halfExtend;
    private void Start()
    {
        halfExtend = boxCollider.size / 2;
        Destroy(boxCollider);
    }

   
    public bool CheckCollision(out Vector3 point)
    {
        var colliders=  Physics.OverlapBox(_transform.position, halfExtend,Quaternion.identity,layerMask);
        //get the closest point of the collider center in the given box
        if (colliders != null && colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[0].tag != "Piece")
                   continue;
                flocksPoint = colliders[0].ClosestPoint(new Vector3(colliders[0].transform.position.x, _transform.position.y, _transform.position.z));
                point = flocksPoint;
                return true;
            }
           
        }
        point = Vector3.zero;
        return false;
    }
}
