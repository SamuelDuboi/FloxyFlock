
using UnityEngine;

public class DontFollowHead : MonoBehaviour
{

    public Transform m_transform;
    public Transform headTransform;


    // Update is called once per frame
    void Update()
    {
        m_transform.localPosition = -headTransform.localPosition;
    }
}
