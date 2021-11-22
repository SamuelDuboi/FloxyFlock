using UnityEngine;
using System.Collections.Generic;

public class InteractableEvents : MonoBehaviour
{
    public Material hoverMat;
    public PhysicMaterial basePMat;
    public PhysicMaterial grabPMat;
    
    [HideInInspector] public bool isGrab = false;
    private MeshRenderer meshRenderer;
    private Material baseMat;
    private List<Collider> colliderList;

    private void Start()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        baseMat = meshRenderer.material;

        colliderList = new List<Collider>();

        for (int i = 0; i < this.transform.GetChild(0).childCount; i++)
        {
            colliderList.Add(this.transform.GetChild(0).GetChild(i).GetComponent<Collider>());
        }
    }

    public void ChangePhysicsMaterialOnSelectEntering()
    {
        foreach (Collider collider in colliderList)
        {
            collider.material = grabPMat;
        }
    }

    public void ChangePhysicsMaterialOnSelectExiting()
    {
        foreach (Collider collider in colliderList)
        {
            collider.material = basePMat;
        }
    }

    public void ChangeMatOnHoverEntering()
    {
        meshRenderer.material = hoverMat;
    }

    public void ChangeMatOnHoverExiting()
    {
        meshRenderer.material = baseMat;
    }

    public void ChangePieceState()
    {
        isGrab = !isGrab;
    }
}
