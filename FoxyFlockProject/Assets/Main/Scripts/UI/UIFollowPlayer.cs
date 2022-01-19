using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    public Transform headeSett;
    public Transform m_transform;

    public float canvasDistanceToCamera;
    public float offsetThreshold;
    public float followSpeed;


    // Update is called once per frame
    void Update()
    {
        Vector3 followPos = (headeSett.position + headeSett.forward * canvasDistanceToCamera);
        Vector3 canvasToFollowPos = followPos - m_transform.position;

        if (canvasToFollowPos.magnitude > offsetThreshold)
        {
            m_transform.Translate(followPos * followSpeed);
        }

        m_transform.rotation = Quaternion.Euler(m_transform.rotation.eulerAngles.x, headeSett.rotation.eulerAngles.y, m_transform.rotation.eulerAngles.z);
    }
}
