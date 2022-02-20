using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorielBubbleDetector : MonoBehaviour
{
    public SphereCollider spherCollider;
    public Transform m_transform;
    public LayerMask layerMask;
    public float radius;

    private GrabablePhysicsHandler _temp;
    bool hasFlocks;
    public GrabManager grabManager;


    public TutorielManager tutoMana;


    private void Start()
    {
        radius = spherCollider.radius * m_transform.lossyScale.x;

    }

    private void Update()
    {
        var collidiers = Physics.OverlapSphere(m_transform.position, radius, layerMask);


        for (int i = 0; i < collidiers.Length; i++)
        {
            _temp = collidiers[i].GetComponentInParent<GrabablePhysicsHandler>();
            if (_temp && !hasFlocks)
            {
                tutoMana.bubbleToFinish = true;
            }
        }
    }
}
