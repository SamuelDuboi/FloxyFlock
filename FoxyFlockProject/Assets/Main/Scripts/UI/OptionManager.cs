using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using NaughtyAttributes;
public class OptionManager : MonoBehaviour
{
    private bool isAliasing;
    private bool castShadow;
    public enum ResulutionQuality { high,medium,low};
    private ResulutionQuality currentResolution;
    
    [Foldout("Rendering")]public RenderPipelineAsset AllOn;
    [Foldout("Rendering")]public RenderPipelineAsset AllOnNoAliasing;
    [Foldout("Rendering")]public RenderPipelineAsset AllOnNoShadow;
    [Foldout("Rendering")]public RenderPipelineAsset AllOnShadowResolutionLow;
    [Foldout("Rendering")]public RenderPipelineAsset AllOnShadowResolutionMedium;
    [Foldout("Rendering")]public RenderPipelineAsset NoAliasingNoShadow;
    [Foldout("Rendering")]public RenderPipelineAsset NoAliasingShadowResolutionLow;
    [Foldout("Rendering")] public RenderPipelineAsset NoAliasingShadowResolutionMedium;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IsAlliasing(bool _aliasing)
    {
        isAliasing = _aliasing;
        ApplyRender();
    }
    public void IsShadow(bool _isShadow)
    {
        castShadow = _isShadow;
        ApplyRender();
    }
    public void ChangeEnum(int value)
    {
        currentResolution = (ResulutionQuality) value;
        ApplyRender();
    }
    private void ApplyRender()
    {
        if (castShadow)
        {
            if (isAliasing)
            {
                switch (currentResolution)
                {
                    case ResulutionQuality.high:
                        GraphicsSettings.renderPipelineAsset = AllOn;
                        break;
                    case ResulutionQuality.medium:
                        GraphicsSettings.renderPipelineAsset = AllOnShadowResolutionMedium;
                        break;
                    case ResulutionQuality.low:
                        GraphicsSettings.renderPipelineAsset = AllOnShadowResolutionLow;
                        break;
                }
            }
            else
            {
                switch (currentResolution)
                {
                    case ResulutionQuality.high:
                        GraphicsSettings.renderPipelineAsset = AllOnNoAliasing;
                        break;
                    case ResulutionQuality.medium:
                        GraphicsSettings.renderPipelineAsset = NoAliasingShadowResolutionMedium;
                        break;
                    case ResulutionQuality.low:
                        GraphicsSettings.renderPipelineAsset = NoAliasingShadowResolutionLow;
                        break;
                }
            }
        }
        else
        {
            if (isAliasing)
            {
                GraphicsSettings.renderPipelineAsset = AllOnNoShadow;
            }
            else
            {
                GraphicsSettings.renderPipelineAsset = NoAliasingNoShadow;
            }
        }
    }
}
