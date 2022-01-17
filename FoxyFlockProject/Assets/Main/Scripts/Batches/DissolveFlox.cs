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
    public MaterialPropertyBlock initBlock;

    void Start()
    {
        grabable.propBlock = new MaterialPropertyBlock();
        flox.GetPropertyBlock(grabable.propBlock);
        initBlock = grabable.propBlock;
        // grabable.OnHitGround.AddListener(StartDissolve);
        flox.SetPropertyBlock(grabable.propBlock);
    }
    void Update()
    {
        if (isDissolving)
        {
            grabable.propBlock.SetFloat("dissolveNoiseAmplitude", 1);
            flox.SetPropertyBlock(grabable.propBlock);
        }

    }
    public IEnumerator StartDissolve(GameObject obj, Vector3 pos, bool isDestroy,GrabManager grabManager = null,bool isGrab = false, GrabManagerMulti grabManagerMulti = default)
    {
        tempTime = 0;
        dissolveState = 1;
        isDissolving = true;
        grabable.propBlock.Clear();
        flox.material = floxMaterialT;
        grabable.propBlock = new MaterialPropertyBlock();
        flox.GetPropertyBlock(grabable.propBlock);
        if (isDestroy)
        {
            grabable.propBlock.SetFloat("IsFrozen", 1);
            flox.SetPropertyBlock(grabable.propBlock);
        }

        while (dissolveState > 0)
        {
            tempTime += 0.01f;
            dissolveState = 1 - (tempTime / dissolveTime);
            if (flox == null)
                break;
            flox.GetPropertyBlock(grabable.propBlock);
            grabable.propBlock.SetFloat("dissolveNoiseAmplitude", dissolveState);
            flox.SetPropertyBlock(grabable.propBlock);
           
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
            grabable.propBlock.Clear();
            grabable.propBlock = new MaterialPropertyBlock();
            flox.GetPropertyBlock(grabable.propBlock);
            flox.SetPropertyBlock(grabable.propBlock);
            dissolveState = 0;
            yield break ;
        }
        grabable.propBlock.Clear();
        flox.material = floxMaterial;
        flox.GetPropertyBlock(grabable.propBlock);
        flox.SetPropertyBlock(grabable.propBlock);
        gameObject.SetActive(false);
        dissolveState = 0;
        
    }

    public void ResetDissolve() 
    {
        isDissolving = false;
        dissolveState = 1;
        tempTime = 0;
        flox.SetPropertyBlock(initBlock);
    }
}
