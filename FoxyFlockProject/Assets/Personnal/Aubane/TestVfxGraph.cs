using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TestVfxGraph : MonoBehaviour
{
    private VisualEffect visualEffect;
    private VFXEventAttribute eventAttribute;

    public float NoiseStrength;
    public int Size;

    // Start is called before the first frame update
    void Start()
    {
        visualEffect = GetComponent<VisualEffect>();
        eventAttribute = visualEffect.CreateVFXEventAttribute();

        NoiseStrength = Shader.PropertyToID("Link Noise Strength");
        Size = Shader.PropertyToID("Link Size");

        visualEffect.SetInt(Size, 1);
        //visualEffect.SetFloat(NoiseStrength, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        visualEffect.SetInt(Size, 25);
        //visualEffect.SetFloat(NoiseStrength, 50.5f);
        visualEffect.SendEvent("MoveAroundPossible", eventAttribute);
    }
}
