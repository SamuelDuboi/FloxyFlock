using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTurn : MonoBehaviour
{
    public Transform m_transform;
    private void Start()
    {
        InputManager.instance.OnSnapTurn.AddListener(Turn);
    }

    void Turn(Vector3 direction)
    {
        
        m_transform.Rotate(direction);
    }
}
