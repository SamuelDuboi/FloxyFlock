using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class TestVfxGraph : MonoBehaviour
{
    public VisualEffect visualEffect;
    public GameObject child;
    GameObject rightHand;

    private VFXEventAttribute movePossibleAttribute;
    private VFXEventAttribute moveImpossibleAttribute;
        
    public int Size;
    private float currentsize;
    
    public void OnTriggerPressListener(bool seeTable)
    {
        if (seeTable)
        {
            if (!visualEffect.enabled)
                visualEffect.enabled = true;
            visualEffect.SetInt(Size, 4);
            visualEffect.SetFloat("Link Noise Strength", 0.35f);
            visualEffect.SetFloat("Link Noise Speed", 0.3f);
            visualEffect.SetBool("Sparks Noise Changes", false);
            currentsize = 4;
        }
        
    }
    public void OnBothTriggerPressListener(bool seeTable)
    {
        if (seeTable)
        {
            currentsize = 10;
            visualEffect.SetInt(Size, 10);
            visualEffect.SendEvent("MoveAroundPossible", movePossibleAttribute);
            visualEffect.SetFloat("Link Noise Strength", 0.1f);
            visualEffect.SetFloat("Link Noise Speed", 0.5f);
            visualEffect.SetBool("Sparks Noise Changes", true);
        }
    }
    public  void OnTriggerPressReleaseListener()
    {
        if(currentsize == 10)
        {
            visualEffect.SetInt(Size, 4);
            currentsize = 4;
            visualEffect.SendEvent("MoveAroundNotPossible", moveImpossibleAttribute);
            visualEffect.SetFloat("Link Noise Strength", 0.35f);
            visualEffect.SetFloat("Link Noise Speed", 0.3f);
            visualEffect.SetBool("Sparks Noise Changes", false);
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
        movePossibleAttribute = visualEffect.CreateVFXEventAttribute();
        moveImpossibleAttribute = visualEffect.CreateVFXEventAttribute();

        visualEffect.SetFloat("Link Noise Strength", 0.35f);
        visualEffect.SetFloat("Link Noise Speed", 0.3f);
        Size = Shader.PropertyToID("Link Size");
        visualEffect.SetBool("Sparks Noise Changes", false);

        visualEffect.SetInt(Size, 4);
        visualEffect.enabled = false;
        

    }
}
