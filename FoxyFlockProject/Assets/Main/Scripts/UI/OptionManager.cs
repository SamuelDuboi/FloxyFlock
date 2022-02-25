using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private InputManager inputManager;
    public Slider masterSlider;
    public Slider floxSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    
    private void Start()
    {
        inputManager = GetComponentInParent<InputManager>();
        if(inputManager)
        inputManager.OnMenuPressed.AddListener(OnMenuPresse);
    }
    void OnMenuPresse()
    {
        float value = 0;
        master.GetFloat("MasterVolume", out value);
        masterSlider.value = 100 - value * 100 / minSoundValue;
        master.GetFloat("FloxVolume", out value);
        floxSlider.value = 100 - value * 100 / minSoundValue;
        master.GetFloat("MusicVolume", out value);
        musicSlider.value = 100 - value * 100 / minSoundValue;
        master.GetFloat("SFXVolume", out value);
        sfxSlider.value = 100 - value * 100 / minSoundValue;
        textResolution.text = currentResolution.ToString();
    }


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
    public void ChangeSoundMaster(float value)
    {
        master.SetFloat("MasterVolume",minSoundValue - minSoundValue * value / 100);
    }
    public void ChangeSoundMusic(float value)
    {
        master.SetFloat("MusicVolume", minSoundValue - minSoundValue * value / 100);
    }
    public void ChangeSoundFlox(float value)
    {
        master.SetFloat("FloxVolume", minSoundValue- minSoundValue * value / 100);
    }
    public void ChangeSoundSFX(float value)
    {
        master.SetFloat("SFXVolume", minSoundValue-  minSoundValue * value / 100);
    }
    public void LunchScene(int index)
    {
        if (SaveSystem.instance)
            StartCoroutine(SaveSystem.instance.Send(index));
        else
        ScenesManagement.instance.LunchScene(index);
    }
    public void OnReturnLobby()
    {
        NetworkManagerRace.instance.OnReset();
    }
}

    public enum ResolutionQuality { high,medium,low};