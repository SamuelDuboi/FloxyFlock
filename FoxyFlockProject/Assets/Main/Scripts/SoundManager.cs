using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public bool mute;
    public static SoundManager instance;
    [SerializeField] private SoundList soundList;
    private List<AudioSource> sources;
    public List< InputManager> inputManagers;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

 
    public void AddInputManager(InputManager inputManager)
    {
        if (inputManagers == null)
            inputManagers = new List<InputManager>();
        inputManagers.Add(inputManager);
        inputManager.ActiveSound.AddListener(ActiveSound);
    }
    public void ApplyAudioClip(string name, AudioSource audioSource)
    {
        bool isSelected = false;
        foreach (Music soundClassic in soundList.music)
        {
            if (soundClassic.name == name)
            {
                audioSource.clip = soundClassic.clip;
                audioSource.volume = soundClassic.volume;
                if (mute)
                    audioSource.mute = true;
                isSelected = true;
                if (sources == null)
                    sources = new List<AudioSource>();
                if (!sources.Contains(audioSource))
                    sources.Add(audioSource);
            }
        }
        if (!isSelected)
        {
            Debug.LogError("No sound have this name "+ name);
        }
    }

    public void ActiveSound(bool isActive)
    {
        mute = isActive;
        if (isActive)
        {
            foreach (var sound in sources)
            {
                sound.mute = true;
            }
        }
        else
        {
            foreach (var sound in sources)
            {
                sound.mute = false;
            }
        }
    }

    public void ToogleSound(int index)
    {
        inputManagers[index].OnActiveSound();
    }

    public void ClearSound()
    {
        if(sources != null)
        sources.Clear();
    }
}
