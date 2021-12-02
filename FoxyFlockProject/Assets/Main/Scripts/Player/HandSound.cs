using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( SoundReader))]
public class HandSound : MonoBehaviour
{
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
