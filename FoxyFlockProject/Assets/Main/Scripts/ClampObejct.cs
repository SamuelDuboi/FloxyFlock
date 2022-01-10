using UnityEngine;
using UnityEditor;
public class ClampObejct : MonoBehaviour
{
    public GameObject table;
    public bool UpDownClamp;
    public bool forwardClamp;
    public bool isMin;
    public CapsuleCollider sphere;
    public Transform zone;
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (table )
        {
            if (forwardClamp)
            {
                Handles.color = Color.red;
                float distance = (new Vector3( transform.position.x,table.transform.position.y,transform.position.z) - table.transform.position).magnitude;
                Handles.DrawWireDisc(table.transform.position, transform.up, distance);
                if (isMin)
                {
                    sphere.radius = distance;
                    float value = zone.transform.localScale.y;
                    zone.transform.localScale = new Vector3(1,0,1)*distance;
                    zone.transform.localScale += Vector3.up *value;
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
#endif
}
