using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MoveAroundVignette : MonoBehaviour
{
    public Volume postProcessVolume = null;
    public float vignetteIntensity = 0.66f;
    public float vignetteFadeDuration = 0.5f;
    private Vignette vignette = null;
    [SerializeField] private InputManager inputManager;
    private bool isVignetteActive = false;

    private void Start()
    {
        if (!ScenesManager.instance.IsLobbyScene() && !ScenesManager.instance.IsMenuScene())
        {
            inputManager.OnBothTrigger.AddListener(OnBothTriggerState);
            inputManager.OnLeftTriggerRelease.AddListener(VignetteFadeOut);
            inputManager.OnRightTriggerRelease.AddListener(VignetteFadeOut);
        }

        if (postProcessVolume != null && postProcessVolume.profile.TryGet(out Vignette _vignette))
            vignette = _vignette;
    }

    public void OnBothTriggerState(bool seeTable)
    {
       if (seeTable)
            VignetteFadeIn();
    }


    private void VignetteFadeIn()
    {
        isVignetteActive = true;
        StartCoroutine(FadeValue(0f, vignetteIntensity));
    }

    private void VignetteFadeOut()
    {
        if (isVignetteActive)
        {
            isVignetteActive = false;
            StartCoroutine(FadeValue(vignetteIntensity, 0f));
        }
    }

    private IEnumerator FadeValue(float startvalue, float endValue)
    {
        float elapsedTime = 0f;

        while (elapsedTime <= vignetteFadeDuration)
        {
            float blend = elapsedTime / vignetteFadeDuration;
            elapsedTime += Time.deltaTime;

            float intensity = Mathf.Lerp(startvalue, endValue, blend);
            ApplyValue(intensity);

            yield return null;
        }
    }

    private void ApplyValue(float value)
    {
        vignette.intensity.Override(value);
    }
}
