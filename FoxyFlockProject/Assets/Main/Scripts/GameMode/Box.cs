using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Collider thisBox;
    public List<GameObject> objectsInBox;

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
    }

    private void Update()
    {
        /*var collidiers = Physics.OverlapBox(center, halfSize,Quaternion.identity ,layerMask);
        for (int i = 0; i < collidiers.Length; i++)
        {

        }*/
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.layer == layerMask)
        {
            if (other.bounds.Contains(thisBox.bounds.min) && other.bounds.Contains(thisBox.bounds.max))
            {
                objectsInBox.Add(other.gameObject);
            }
            else
            {
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
