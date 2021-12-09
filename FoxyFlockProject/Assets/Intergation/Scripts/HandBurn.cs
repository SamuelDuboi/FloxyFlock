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

    private float heatCurrentValue = 0f;
    [HideInInspector] public float heatPourcentage = 0f;
    private Transform lastFrameTransform;

    private SkinnedMeshRenderer handRenderer;
    private XRDirectInteractor interactor;

    private HeatState heatState =  HeatState.cool;

    private void Start()
    {
        interactor = this.GetComponent<XRDirectInteractor>();
        handRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        heatPourcentage = heatCurrentValue / heatMaxValue; //TODO : Use this for material strengh

        CoolEvent();
    }

    public void BurnEvent(GrabbableObject flockInteractable)
    {
        heatState = HeatState.burning;

        heatCurrentValue += burningSpeed;

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
                heatCurrentValue -= coolingSpeed + (coolingSpeed * wiggleStrengh());
                
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


