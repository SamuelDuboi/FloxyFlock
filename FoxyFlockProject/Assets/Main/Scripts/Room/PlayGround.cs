using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayGround : MonoBehaviour
{
    public CapsuleCollider spherCollider;
    public Transform m_transform;
    public LayerMask layerMask;
    public Transform minClampTransform;
    public Transform maxClampTransform;
    [Range(1,2)]
    public float slowForce;
    public float timeBeforFall;
    private float radius;
    private RaycastHit hit;
    private float distance;
    private Vector3 point1;
    private Vector3 extend;
    private Vector3 point2;
    private GrabablePhysicsHandler _temp;
    // Start is called before the first frame update
    void Start()
    {
        radius = spherCollider.radius;
        extend = new Vector3(0, spherCollider.bounds.extents.y, 0) ;
        point1 =  m_transform.position - extend;
        point2 = m_transform.position + extend ;
        spherCollider.enabled = false;
        distance = Vector3.Distance(point1, point2);
    }

    // Update is called once per frame
    void Update()
    {
        var collidiers = Physics.OverlapCapsule(point1, point2, radius, layerMask);
        for (int i = 0; i < collidiers.Length; i++)
        {
            _temp = collidiers[i].GetComponentInParent<GrabablePhysicsHandler>();
            if (_temp)
            {
                _temp.SetIsOnPlayGround(slowForce,timeBeforFall);
            }
        }
       
       
    }
    
}
