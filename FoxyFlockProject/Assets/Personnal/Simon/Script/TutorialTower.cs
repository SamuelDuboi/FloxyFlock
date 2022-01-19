using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTower : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Material initialMat;

    public MaterialPropertyBlock propBlock;
    public void Start()
    {
        propBlock = new MaterialPropertyBlock();
        if (meshRenderer == null || propBlock == null || initialMat == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            initialMat = meshRenderer.material;
            propBlock = new MaterialPropertyBlock();
        }

        meshRenderer.material = initialMat;
        propBlock.Clear();

        //Recup Data
        meshRenderer.GetPropertyBlock(propBlock);
        //EditZone
        propBlock.SetFloat("IsFrozen", 1);

        //Push Data
        meshRenderer.SetPropertyBlock(propBlock);

        gameObject.layer = 14;
    }
}
