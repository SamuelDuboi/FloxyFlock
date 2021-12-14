using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

enum HeatState
{
    cool,
    burning,
    burned
}

public class HandBurn : MonoBehaviour
{
    [SerializeField] private float heatMaxValue = 100f;
    [SerializeField] private float burningSpeed = 10f;
    [SerializeField] private float coolingSpeed = 10f;
    [SerializeField] private float wiggleCoolingScale = 2f;
    [SerializeField] private Material handMaterial;

    private float heatCurrentValue = 0f;
    [HideInInspector] public float heatPourcentage = 0f;
    private float lastFrameHeatPourcentage = 0f;
    private Transform lastFrameTransform;

    private XRDirectInteractor interactor;
    private SkinnedMeshRenderer handRenderer;
    private MaterialPropertyBlock propBlock;


    private HeatState heatState =  HeatState.cool;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        propBlock = new MaterialPropertyBlock();

        interactor = this.GetComponent<XRDirectInteractor>();

        handRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
        if (handRenderer != null)
            handRenderer.material = handMaterial;
    }

    private void Update()
    {
        heatPourcentage = heatCurrentValue / heatMaxValue;

        if (heatPourcentage != lastFrameHeatPourcentage)
        {
            UpdateMatBurnValue();
            CoolEvent();
        }
    }

    private void UpdateMatBurnValue()
    {
        if (handRenderer != null)
        {
            //Recup Data
            handRenderer.GetPropertyBlock(propBlock);
            //EditZone
            propBlock.SetFloat("Burn", heatPourcentage);

            //Push Data
            handRenderer.SetPropertyBlock(propBlock);
        }
        else
        {
            handRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();

            if (handRenderer != null)
                handRenderer.material = handMaterial;
        }
    }

    public void BurnEvent(GrabbableObject flockInteractable)
    {
        heatState = HeatState.burning;

        heatCurrentValue += Time.deltaTime* burningSpeed;
        lastFrameTransform = this.transform;
        if (heatCurrentValue >= heatMaxValue)
        {
            heatCurrentValue = heatMaxValue;

            heatState = HeatState.burned;
            InteractionManager.instance.SelectExit(interactor, flockInteractable);
            interactor.allowSelect = false;
        }
    }

    private void CoolEvent()
    {
        if (heatState != HeatState.burning)
        {
            if (heatCurrentValue > 0)
            {
                heatCurrentValue -= Time.deltaTime*( coolingSpeed + (coolingSpeed * wiggleStrengh()));
                
                if (heatCurrentValue <= 0)
                {
                    if (heatState == HeatState.burned)
                        interactor.allowSelect = true;

                    heatState = HeatState.cool;
                }
            }  
        }
    }

    private float wiggleStrengh()
    {
        Vector3 lastFramePosition = lastFrameTransform.position;
        Quaternion lastFrameRotation = lastFrameTransform.rotation;

        float distanceCheck = Vector3.Distance(transform.position, lastFramePosition);
        float rotationCheck = Mathf.Abs(Quaternion.Angle(transform.rotation, lastFrameRotation)); //I don't know if quaternion.angle can be negative so i take the absolute as a security

        float wiggle = (distanceCheck + rotationCheck) * wiggleCoolingScale;

        lastFrameTransform = this.transform;

        return wiggle;
    }
}


