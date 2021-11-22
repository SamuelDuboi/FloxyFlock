using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class HotPotato : ModifierAction
{
    private float timer;
    private bool isGrab;
    public float timerBeforAutoRelease;
    private GameObject flock;
    private XRBaseInteractor currentInteractor;
    private GrabbableObject flockInteractable;
    private MeshRenderer mesh;
    public Material[] mats;
    private int stateIndex;
    private SoundReader sound;
    public string clipName;
    private bool isCooling;
    public float cooldDown;

    private void Update()
    {
        if (isGrab)
        {
            timer += Time.deltaTime;
            if (timer > timerBeforAutoRelease)
            {
                InteractionManager.instance.SelectExit(currentInteractor, flockInteractable);
                flockInteractable.enabled = false;
                isCooling = true;
                timer = cooldDown;
                stateIndex = 3;
                return;
                
            }
            if (timer>timerBeforAutoRelease*stateIndex/4 )
            {
                mesh.material = mats[stateIndex];
                stateIndex++;
            }
            
        }
        if (isCooling)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                flockInteractable.enabled = true;
                isCooling = false;
                timer = 0;
                return;
            }
            if (timer < cooldDown * stateIndex / 4)
            {
                mesh.material = mats[stateIndex];
                stateIndex--;
            }
        }
    }
    public override void OnStarted(GameObject _object)
    {
        base.OnStarted(_object);
       sound= _object.AddComponent<SoundReader>();
        sound.clipName = clipName;
    }
    public override void OnEnterStasis(GameObject _object, bool isGrab)
    {
        base.OnEnterStasis(_object, isGrab);
    }
    public override void OnGrabed(GameObject _object)
    {
        base.OnGrabed(_object);
        isGrab = true;
        flock = _object;
        flockInteractable = flock.GetComponent<GrabbableObject>();
        currentInteractor = flockInteractable.currentInteractor;
        mesh = GetComponent<MeshRenderer>();
        stateIndex = 0;
    }
    public override void OnHitGround(GameObject _object)
    {
        base.OnHitGround(_object);
    }
    public override void OnHitSomething(GameObject _object, Vector3 velocity, GameObject collision)
    {
        base.OnHitSomething(_object, velocity, collision);
        if(sound)
        sound.Play();
    }
    public override void OnReleased(GameObject _object)
    {
        base.OnReleased(_object);
        currentInteractor = null;
        isGrab = false;
    }
}
