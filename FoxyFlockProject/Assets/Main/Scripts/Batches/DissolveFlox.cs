using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveFlox : MonoBehaviour
{
    public Material floxMaterial;
    public Material floxMaterialT;
    public MeshRenderer flox;
    public float dissolveState;
    [Range(0.2f, 10)]
    public float dissolveTime;
    public bool isDissolving;

    private float tempTime;

    public GrabablePhysicsHandler grabable;
    public MaterialPropertyBlock propBlock;
    public MaterialPropertyBlock initBlock;

    void Start()
    {
        propBlock = new MaterialPropertyBlock();
        flox.GetPropertyBlock(propBlock);
        initBlock = propBlock;
        // grabable.OnHitGround.AddListener(StartDissolve);
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
    public IEnumerator StartDissolve(GameObject obj, Vector3 pos, bool isDestroy,GrabManager grabManager = null,bool isGrab = false, GrabManagerMulti grabManagerMulti = default)
    {
        tempTime = 0;
        dissolveState = 1;
        isDissolving = true;
        flox.material = floxMaterialT;
        flox.GetPropertyBlock(propBlock);
        if (isDestroy)
        {
            propBlock.SetFloat("IsFrozen", 1);
            flox.SetPropertyBlock(propBlock);
        }

        while (dissolveState > 0)
        {
            tempTime += 0.01f;
            dissolveState = 1 - (tempTime / dissolveTime);
            if (flox == null)
                break;
            flox.GetPropertyBlock(propBlock);
            propBlock.SetFloat("dissolveNoiseAmplitude", dissolveState);
            flox.SetPropertyBlock(propBlock);
           
            yield return new WaitForSeconds(0.01f);
        }
        if (isDestroy)
        {
            if (grabManagerMulti != default && gameObject != null)
                grabManagerMulti.Destroy(gameObject);
            else
                Destroy(gameObject);
        }
        if(grabManager != null)
        {
            grabManager.ResetInInventory(obj, pos, isGrab);
            flox.material = floxMaterial;
            dissolveState = 0;
            yield break ;
        }
        flox.material = floxMaterial;
        
            gameObject.SetActive(false);
            dissolveState = 0;
        
    }

    public void ResetDissolve() // A AJOUTER LORSQUE LES FLOX RESET <- n'a pas été fait 
    {
        isDissolving = false;
        dissolveState = 1;
        tempTime = 0;
        flox.SetPropertyBlock(initBlock);
    }
}
