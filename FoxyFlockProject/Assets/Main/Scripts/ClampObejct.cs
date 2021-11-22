using UnityEngine;
using UnityEditor;
public class ClampObejct : MonoBehaviour
{
    public GameObject table;
    public bool UpDownClamp;
    public bool forwardClamp;
    public bool isMin;
    public CapsuleCollider sphere;
    private void OnDrawGizmos()
    {
        if (table )
        {
            if (forwardClamp)
            {
                Handles.color = Color.red;
                float distance = (transform.position - table.transform.position).magnitude;
                Handles.DrawWireDisc(table.transform.position, transform.up, distance);
                if (isMin)
                {
                    sphere.radius = distance;
                }
            }
            if (UpDownClamp)
            {
                 Color handlColor = new Color(1, 0, 0, 0.5f);
                Gizmos.color = handlColor;
                Gizmos.DrawCube (transform.position,new Vector3(15,0,15));
            }
        }
    }
}
