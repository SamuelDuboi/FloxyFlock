using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    public Transform headeSett;
    public Transform m_transform;
    // Update is called once per frame
    void Update()
    {
        m_transform.rotation = Quaternion.Euler(m_transform.rotation.eulerAngles.x, headeSett.rotation.eulerAngles.y, m_transform.rotation.eulerAngles.z);
    }
}
