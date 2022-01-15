using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMic : MonoBehaviour
{
    void Start()
    {
        Application.runInBackground = true;
        AudioSource audioSource = GetComponent<AudioSource>();
     
        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            if(Microphone.devices[i].Contains("Oculus"))
            {
                audioSource.clip = Microphone.Start(Microphone.devices[i], true, 1000, 44100);
                audioSource.Play();
                if (Microphone.IsRecording(Microphone.devices[i]))
                {
                    while (!(Microphone.GetPosition(Microphone.devices[i]) > 0)) { }
                }
                else
                {
                    Debug.Log("is not recording");

                }
            }
        }
       
    }
    private void Update()
    {
        
    }
}
