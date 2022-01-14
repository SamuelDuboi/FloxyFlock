using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.Audio;
public class OptionManager : MonoBehaviour
{
    private ResolutionQuality currentResolution;
    public RenderPipelineAsset[] qualitySettings;
    public TextMeshProUGUI textResolution;

    public AudioMixer master;
    public float maxSoundValue = 0;
    public float minSoundValue = -80;
    public void Right()
    {
        if ((int)currentResolution < 2)
            currentResolution++;
        else
            currentResolution = 0;
        GraphicsSettings.renderPipelineAsset = qualitySettings[(int)currentResolution];
        textResolution.text = currentResolution.ToString();
    }
    public void Left()
    {
        if ((int)currentResolution >0)
            currentResolution--;
        else
            currentResolution = ResolutionQuality.low;
        GraphicsSettings.renderPipelineAsset = qualitySettings[(int)currentResolution];
        textResolution.text = currentResolution.ToString();
    }

    public void Actualise(TextMeshProUGUI text)
    {
        text.text = currentResolution.ToString();
    }

    public void ChangeSoundMaster(float value)
    {
        master.SetFloat("MasterVolume", minSoundValue * value / 100);
    }
    public void ChangeSoundMusic(float value)
    {
        master.SetFloat("MusicVolume", minSoundValue * value / 100);
    }
    public void ChangeSoundFlox(float value)
    {
        master.SetFloat("FloxVolume", minSoundValue * value / 100);
    }
    public void ChangeSoundSFX(float value)
    {
        master.SetFloat("SFXVolume", minSoundValue * value / 100);
    }
}

    public enum ResolutionQuality { high,medium,low};