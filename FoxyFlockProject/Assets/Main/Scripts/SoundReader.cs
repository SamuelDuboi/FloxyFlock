using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundReader : MonoBehaviour
{
    public string clipName;
    public string secondClipName;
    public string ThirdClipName;
    public string ForthClipName;
    [HideInInspector] public AudioSource source;
    public bool applyAudioOnStart;
    public bool randomSelectionOnStart;
    public bool doOnce;
    private bool cantPLay;
    private void Start()
    {
        source = GetComponent<AudioSource>();
        if (SoundManager.instance != null && clipName != string.Empty && clipName != null)
        {
            SoundManager.instance.ApplyAudioClip(clipName, source);
            if (applyAudioOnStart)
            {
                if(randomSelectionOnStart)
                {
                    int random = 0;
                    if (clipName != string.Empty)
                        random++;
                    if (secondClipName != string.Empty)
                        random++;
                    if (ThirdClipName != string.Empty)
                        random++;
                    if (ForthClipName != string.Empty)
                        random++;
                    int random2 = Random.Range(0, random );
                    switch (random2)
                    {
                        case 0:
                            Play();
                            break;
                        case 1:
                            PlaySeconde();
                            break;
                        case 2:
                            PlayThird();
                            break;
                        case 3:
                            Playforth();
                            break;
                        default:
                            break;
                    }
                    return;
                }
                else
                source.Play();
            }
        }
        else
            cantPLay = true;
    }
    private void OnEnable()
    {
        source = GetComponent<AudioSource>();
        if (SoundManager.instance != null && clipName != string.Empty && clipName != null)
        {
            SoundManager.instance.ApplyAudioClip(clipName, source);
            if (applyAudioOnStart)
            {
                if (randomSelectionOnStart)
                {

                    int random = 0;
                    if (clipName != string.Empty)
                        random++;
                    if (secondClipName != string.Empty)
                        random++;
                    if (ThirdClipName != string.Empty)
                        random++;
                    if (ForthClipName != string.Empty)
                        random++;
                    int random2 = Random.Range(0, random);
                    switch (random2)
                    {
                        case 0:
                            Play();
                            break;
                        case 1:
                            PlaySeconde();
                            break;
                        case 2:
                            PlayThird();
                            break;
                        case 3:
                            Playforth();
                            break;
                        default:
                            break;
                    }
                    return;
                }
                else
                    source.Play();
            }
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
    public void Play(string name)
    {
        if (!cantPLay)
        {
            if (doOnce)
            {
                doOnce = false;
                return;
            }
            if (!source)
                source = GetComponent<AudioSource>();
            clipName = name;
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
