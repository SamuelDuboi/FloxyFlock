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
        m_transform.rotation = Quaternion.Euler(m_transform.rotation.eulerAngles.x, headeSett.rotation.eulerAngles.y, m_transform.rotation.eulerAngles.z);

        //m_transform.rotation = Quaternion.Lerp(m_transform.rotation, targetRotation, Time.deltaTime * followSpeed);
    }
}
