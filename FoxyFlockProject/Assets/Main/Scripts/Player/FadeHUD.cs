using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeHUD : MonoBehaviour
{
    [SerializeField] private Image image; //Set in hierarchy
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float timeBeforeFade = 1.5f;
    [SerializeField] private float timeBeforeFadeScaleOnGameplay = 2f;
    [SerializeField] private SoundReader soundReader;

    private bool canLerp = false;
    private float startTime;
    private bool canPlaySoundOnStart = false;

    private void Start()
    {
        image.color = Color.black;

        if (!ScenesManagement.instance.IsLobbyScene() && !ScenesManagement.instance.IsMenuScene())
        {
            timeBeforeFade *= timeBeforeFadeScaleOnGameplay;
            
            int random = Random.Range(1, 4);
            canPlaySoundOnStart = true;
        }

        Invoke("StartFade", timeBeforeFade);
    }

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

        if (canPlaySoundOnStart)
        {
            int random = Random.Range(1, 4);

            switch (random)
            {
                case 1:
                    soundReader.Play();
                    break;
                case 2:
                    soundReader.PlaySeconde();
                    break;
                case 3:
                    soundReader.PlayThird();
                    break;
            }

            canPlaySoundOnStart = false;
        }
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
