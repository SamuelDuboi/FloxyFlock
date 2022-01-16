using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXMoveAround : MonoBehaviour
{
    public VisualEffect visualEffect;
    private VFXEventAttribute eventAttribute;
    public GameObject child;
    private float NoiseStrength;
    private int Size;
    private float currentsize;
    GameObject rightHand;
    public InputManager inputManager;

    public void OnTriggerPressListener(bool seeTable)
    {
        if (seeTable)
        {
            if (!visualEffect.enabled)
                visualEffect.enabled = true;

            visualEffect.SetInt(Size, 4);
            currentsize = 4;
        }
        
    }
    public void OnBothTriggerPressListener(bool seeTable)
    {
        if (seeTable)
        {
            currentsize = 10;
            visualEffect.SetInt(Size, 10);
            visualEffect.SendEvent("MoveAroundPossible", eventAttribute);
        }
    }
    public  void OnTriggerPressReleaseListener()
    {
        if(currentsize == 10)
        {
            visualEffect.SetInt(Size, 4);
            currentsize = 4;
        }
        else
        {
            visualEffect.enabled = false;
        }
    }
    private void Update()
    {
        if (!rightHand)
        {
            if (!inputManager)
                inputManager = GetComponentInParent<InputManager>();
            if (inputManager.rightHand.model.gameObject)
            {
                rightHand = inputManager.rightHand.model.gameObject;
                inputManager.OnLeftTrigger.AddListener(OnTriggerPressListener);
                inputManager.OnRightTrigger.AddListener(OnTriggerPressListener);
                inputManager.OnBothTrigger.AddListener(OnBothTriggerPressListener);
                inputManager.OnRightTriggerRelease.AddListener(OnTriggerPressReleaseListener);
                inputManager.OnLeftTriggerRelease.AddListener(OnTriggerPressReleaseListener);

                child.transform.SetParent(rightHand.transform.GetChild(0));
                child.transform.localPosition = Vector3.zero;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        eventAttribute = visualEffect.CreateVFXEventAttribute();

        NoiseStrength = Shader.PropertyToID("Link Noise Strength");
        Size = Shader.PropertyToID("Link Size");

        visualEffect.SetInt(Size, 4);
        visualEffect.enabled = false;
        

        //visualEffect.SetFloat(NoiseStrength, 0.5f);
    }
}
