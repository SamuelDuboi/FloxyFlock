using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMatOnSelect : MonoBehaviour
{
    public MeshRenderer mesh;
    public Material[] mats;

    public void OnSelect()
    {
        mesh.material = mats[1];
    }

    public void OnDeselet()
    {
        mesh.material = mats[0];
    }
}
