using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloxBurn : MonoBehaviour
{
    [SerializeField] private string burnClipName = "";
    [SerializeField] private float deathDuration = 2f;
    [SerializeField] private Material deathMaterial;
    public DissolveFlox dissolveFlox;
    public SoundReader soundReader;

    private void Start()
    {
        soundReader.clipName = burnClipName;
    }

    public void BurnEvent()
    {
        dissolveFlox.StartDissolve(default, Vector3.zero, false);
    }

}
