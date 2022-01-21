using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMic : MonoBehaviour
{
    void Start()
    {
        Application.runInBackground = true;
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(Microphone.devices[1], true, 1000, 44100);
        audioSource.Play();
        if (Microphone.IsRecording(Microphone.devices[1]))
        {
            while (!(Microphone.GetPosition(Microphone.devices[1]) > 0)) { }
            Debug.Log("is recording");
        }
        else
        {
            Debug.Log("is not recording");

        }

    }
    private void Update()
    {
        
    }
}
