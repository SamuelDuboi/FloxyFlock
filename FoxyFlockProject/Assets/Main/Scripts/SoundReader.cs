using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundReader : MonoBehaviour
{
    public string clipName;
    public string secondClipName;
    public string ThirdClipName;
    public string ForthClipName;
    private AudioSource source;
    public bool applyAudioOnStart;
    public bool doOnce;
    private bool cantPLay;
    private void Start()
    {
        source = GetComponent<AudioSource>();
        if (SoundManager.instance != null)
        {
            SoundManager.instance.ApplyAudioClip(clipName, source);
            if (applyAudioOnStart)
                source.Play();
        }
        else
            cantPLay = true;
    }

    public void Play()
    {
        if (!cantPLay)
        {
            if (doOnce)
            {
                doOnce = false;
                return;
            }
            if(!source)
            source = GetComponent<AudioSource>();
            SoundManager.instance.ApplyAudioClip(clipName, source);
            source.Play();
        }
    }
    public void PlaySeconde()
    {
        if (!cantPLay)
        {
            if (!source)
                source = GetComponent<AudioSource>();
            SoundManager.instance.ApplyAudioClip(secondClipName, source);
            source.Play();
        }
    }
    public void PlayThird()
    {
        if (!cantPLay)
        {
            if (!source)
                source = GetComponent<AudioSource>();
            SoundManager.instance.ApplyAudioClip(ThirdClipName, source);
            source.Play();
        }
    }
    public void Playforth()
    {
        if (!cantPLay)
        {
            if (!source)
                source = GetComponent<AudioSource>();
            SoundManager.instance.ApplyAudioClip(ForthClipName, source);
            source.Play();
        }
    }
    public void StopSound()
    {
        source.Stop();
    }
}
