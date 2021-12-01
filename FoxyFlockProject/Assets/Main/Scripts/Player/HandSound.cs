using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( SoundReader))]
public class HandSound : MonoBehaviour
{
    public SoundReader sound;
    public bool isLeft;
    private bool startActivated;
    public void Start()
    {
        sound = GetComponent<SoundReader>();
        if (isLeft)
        {
            InputManager.instance.OnLeftTrigger.AddListener(HandStart);
            InputManager.instance.OnLeftTriggerRelease.AddListener(HandStop);
        }
        else
        {
            InputManager.instance.OnRightTrigger.AddListener(HandStart);
            InputManager.instance.OnRightTriggerRelease.AddListener(HandStop);
        }
    }
    private void HandStart(bool seeTable)
    {
        if (seeTable)
        {
            sound.clipName = "HandMoveStart";
            sound.Play();
            startActivated = true;
        }
    }
    private void HandStop( )
    {
        if (startActivated )
        {
            startActivated = false;
            sound.clipName = "HandMoveStop";
            sound.Play();
        }
    }
}
