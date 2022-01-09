using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( SoundReader))]
public class HandSound : MonoBehaviour
{
    public string activationName;
    public string loopName;
    public SoundReader sound;
    public bool isLeft;
    private bool startActivated;
    public InputManager inputManager;
    public void Start()
    {
        sound = GetComponent<SoundReader>();
        if (isLeft)
        {
            inputManager.OnLeftTrigger.AddListener(HandStart);
            inputManager.OnLeftTriggerRelease.AddListener(HandStop);
        }
        else
        {
            inputManager.OnRightTrigger.AddListener(HandStart);
            inputManager.OnRightTriggerRelease.AddListener(HandStop);
        }
        inputManager.OnBothTrigger.AddListener(OnBothActivate);
    }
    private void HandStart(bool seeTable)
    {
        if (seeTable)
        {
            sound.clipName = activationName;
            sound.Play();
            startActivated = true;
        }
    }
    private void OnBothActivate(bool seeTable)
    {
        if (seeTable)
        {
            sound.clipName = loopName;
            sound.source.loop = true;
            sound.Play();
            startActivated = true;
        }
    }
    private void HandStop( )
    {
        if (startActivated )
        {
            startActivated = false;
            sound.source.loop = false;
            sound.StopSound();
        }
    }
}
