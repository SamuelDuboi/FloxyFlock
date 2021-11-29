using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Collider thisBox;
    public List<GameObject> objectsInBox;
    public List<GrabbableObject> grabbableObjects;
    public Material baseMat;
    public Material winMat;
    public Material defeatMat;

    public LayerMask layerMask; 
    private Vector3 center;
    public Vector3 halfSize;
    void Start()
    {
        thisBox = gameObject.GetComponent<Collider>();
        center = gameObject.transform.position;
        grabbableObjects = new List<GrabbableObject>();
    }

    private void Update()
    {
        /*var collidiers = Physics.OverlapBox(center, halfSize,Quaternion.identity ,layerMask);
        for (int i = 0; i < collidiers.Length; i++)
        {

        }*/
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            GrabbableObject parent = other.GetComponentInParent<GrabbableObject>();
            if (grabbableObjects.Contains(parent))
                return;
            else
            {
                for (int i = 0; i < parent.colliders.Count; i++)
                {
                    if(!thisBox.bounds.Contains(parent.colliders[i].bounds.max) || !thisBox.bounds.Contains(parent.colliders[i].bounds.max))
                    {
                        return;
                    }
                }
                grabbableObjects.Add(parent);
            }
        }
 }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.layer == 6)
        {
          

            if (other.bounds.Contains(thisBox.bounds.min) && other.bounds.Contains(thisBox.bounds.max))
            {
                objectsInBox.Add(other.gameObject);
            }
            else
            {
                if (objectsInBox.Count < 1)
                    return;
                objectsInBox.RemoveAt(objectsInBox.BinarySearch(other.gameObject));
            }

        }
    }
    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == layerMask)
        {
            objectsInBox.RemoveAt(objectsInBox.BinarySearch(other.gameObject));
        }
    }*/
}
