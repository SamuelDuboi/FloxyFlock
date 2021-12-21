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
    private Vector3 halfExtend;
    private void Start()
    {
        halfExtend = boxCollider.size / 2;
        Destroy(boxCollider);
    }
    private bool CheckCollision()
    {
        var colliders=  Physics.OverlapBox(_transform.position, halfExtend);
        //get the closest point of the collider center in the given box
        if (colliders != null && colliders.Length > 0)
        {
            flocksPoint = colliders[0].ClosestPoint(new Vector3(colliders[0].transform.position.x, _transform.position.y, _transform.position.z));

        }
    }
}
