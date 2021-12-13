using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveFlox : MonoBehaviour
{

    public Material floxMaterial;
    public MeshRenderer flox;
    public float dissolveState;
    [Range(0.2f,10)]
    public float dissolveTime;
    public bool isDissolving;

    public float tempTime;

    public GrabablePhysicsHandler grabable;
    public MaterialPropertyBlock propBlock;
    public MaterialPropertyBlock initBlock;

    void Start()
    {
        propBlock = new MaterialPropertyBlock();
        flox.GetPropertyBlock(propBlock);
        initBlock = propBlock;
        grabable.OnHitGround.AddListener(StartDissolve);
        flox.SetPropertyBlock(propBlock);
    }
    void Update()
    {
        if (isDissolving)
        {
            propBlock.SetFloat("dissolveNoiseAmplitude", 1);
            flox.SetPropertyBlock(propBlock);
        }
        
    }
    public void StartDissolve(GameObject obj, Vector3 pos, bool isbool)
    {
        isDissolving = true;
        if (isDissolving)
        {
            propBlock.SetFloat("outlineActivation", 0);
            propBlock.SetInt("_SurfaceType", 1);
            propBlock.SetInt("_RenderQueueType", 4);
            flox.SetPropertyBlock(propBlock);

            while (dissolveState > 0)
            {
                tempTime += Time.deltaTime;
                dissolveState = 1 - (tempTime / dissolveTime);

                if (dissolveState < 0)
                    dissolveState = 0;
            }

        }
    }
    public void ResetDissolve() // A AJOUTER LORSQUE LES FLOX RESET
    {
        isDissolving = false;
        dissolveState = 1;
        tempTime = 0;
        flox.SetPropertyBlock(initBlock);
    }
}
