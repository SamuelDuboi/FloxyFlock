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
    private MeshRenderer handRenderer;
    private XRDirectInteractor interactor;

    private HeatState heatState =  HeatState.cool;

    private void Start()
    {
        interactor = this.GetComponent<XRDirectInteractor>();
    }

    private void Update()
    {
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
                heatCurrentValue -= coolingSpeed;
                
                if (heatCurrentValue <= 0)
                {
                    if (heatState == HeatState.burned)
                        interactor.allowSelect = true;

                    heatState = HeatState.cool;
                }
            }
                
        }
    }
}


