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
    private InputManager inputManager;

    private void Start()
    {
        if (!ScenesManager.instance.IsLobbyScene() && !ScenesManager.instance.IsMenuScene())
        {
            inputManager.OnBothTrigger.AddListener(TriggersState);
        }

        if (postProcessVolume != null && postProcessVolume.profile.TryGet(out Vignette _vignette))
            vignette = _vignette;
    }

    public void TriggersState(bool areBothTriggerPressed)
    {
        if (areBothTriggerPressed)
        {
            
        }
    }

    private void VignetteFadeIn(bool canFade)
    {
        if (canFade)
            StartCoroutine(FadeValue(0f, vignetteIntensity));
    }

    private void VignetteFadeOut()
    {
        StartCoroutine(FadeValue(vignetteIntensity, 0f));
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
