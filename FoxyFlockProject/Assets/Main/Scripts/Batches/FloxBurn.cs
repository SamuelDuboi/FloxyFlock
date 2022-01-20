using UnityEngine;

public class FloxBurn : MonoBehaviour
{
    [SerializeField] private string burnClipName = "";
    //[SerializeField] private float deathDuration = 2f;
    //[SerializeField] private Material deathMaterial;
    public DissolveFlox dissolveFlox;
    public SoundReader soundReader;

    private void Start()
    {
        soundReader.clipName = burnClipName;
    }

    public void BurnEvent()
    {
       StartCoroutine(dissolveFlox.StartDissolve(default, Vector3.zero, true));
    }

}
