using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTurn : MonoBehaviour
{
    public Transform m_transform;
    public Transform pantoufleTransform;
    public InputManager inputManager;
    private void Start()
    {
        inputManager.OnSnapTurn.AddListener(Turn);
    }
    
    void Turn(Vector3 direction)
    {
        m_transform.Rotate(direction);
        pantoufleTransform.rotation = Quaternion.Euler(pantoufleTransform.rotation.eulerAngles.x, pantoufleTransform.rotation.eulerAngles.y+inputManager.characterStats.snapTurnAngle, pantoufleTransform.rotation.eulerAngles.z);
    }

}
