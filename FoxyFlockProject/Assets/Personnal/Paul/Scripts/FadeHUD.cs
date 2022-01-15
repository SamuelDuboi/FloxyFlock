using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeHUD : MonoBehaviour
{
    [SerializeField] private Image image; //Set in hierarchy
    [SerializeField] private float fadeDuration;

    private bool canLerp = false;
    private float startTime;

    private void Update()
    {
        if (canLerp)
        {
            AlphaLerp();
        }
    }

    public void StartFade()
    {
        canLerp = true;
        startTime = (float)AudioSettings.dspTime;
    }

    private void AlphaLerp()
    {
        float progression = ((float)AudioSettings.dspTime - startTime) / fadeDuration;

        if (progression > 1f)
        {
            image.color = new Color(0f, 0f, 0f, 0f);
            canLerp = false;
            return;
        }

        float alpha = Mathf.Lerp(1f, 0f, progression);
        image.color = new Color(0f, 0f, 0f, alpha);
    }

}
