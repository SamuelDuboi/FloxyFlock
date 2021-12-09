using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HotPotato : ModifierAction
{
    private float timer;
    public float timerBeforAutoRelease;
   
    private MeshRenderer mesh;
    public Material[] mats;

    private bool isCooling;
    public float cooldDown;
    private int stateIndex;

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
       
    }
    public override void OnEnterStasis(GameObject _object, bool isGrab, Rigidbody rgb)
    {
        base.OnEnterStasis(_object, isGrab,rgb);
    }
    public override void OnGrabed(GameObject _object)
    {
        base.OnGrabed(_object);
        isGrab = true;
        mesh = GetComponent<MeshRenderer>();
        stateIndex = 0;
    }
    public override void OnHitGround(GameObject _object, Vector3 initPos, bool isGrab)
    {
        base.OnHitGround(_object, initPos, isGrab);
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
