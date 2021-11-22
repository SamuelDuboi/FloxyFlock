using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class TestVfxGraph : MonoBehaviour
{
    public VisualEffect visualEffect;
    private VFXEventAttribute eventAttribute;
    public GameObject child;
    public float NoiseStrength;
    public int Size;
    private float currentsize;
    GameObject rightHand;
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
            if (InputManager.instance.rightHand.model.gameObject)
            {
                rightHand = InputManager.instance.rightHand.model.gameObject;
                InputManager.instance.OnLeftTrigger.AddListener(OnTriggerPressListener);
                InputManager.instance.OnRightTrigger.AddListener(OnTriggerPressListener);
                InputManager.instance.OnBothTrigger.AddListener(OnBothTriggerPressListener);
                InputManager.instance.OnRightTriggerRelease.AddListener(OnTriggerPressReleaseListener);
                InputManager.instance.OnLeftTriggerRelease.AddListener(OnTriggerPressReleaseListener);
                child.transform.SetParent(rightHand.transform);
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
