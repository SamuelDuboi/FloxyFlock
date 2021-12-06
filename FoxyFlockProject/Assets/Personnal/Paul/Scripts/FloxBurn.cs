using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloxBurn : MonoBehaviour
{
    [SerializeField] private string burnClipName = "";
    [SerializeField] private float deathDuration = 2f;

    private SoundReader soundReader;

    private void Start()
    {
        soundReader.clipName = burnClipName;
    }

    public void BurnEvent()
    {
        StartCoroutine(BurnCoroutine());
    }

    private IEnumerator BurnCoroutine()
    {
        //Activate disolve here and set disolve duration to deathTime;

        if (soundReader)
            soundReader.Play();

        yield return new WaitForSeconds(deathDuration); //Change this to match disolve and burn sound time 

        this.gameObject.SetActive(false); // TODO? : Go back into whatever pool
    }
}
