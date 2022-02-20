using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HotPotatoV2 : ModifierAction
{
    public float timeBeforeAutoRelease;
    public string collisionClipName;
    public string idleClipName;
    private bool hasDoneFirstGrab;
    private DissolveFlox dissolveFlox;
    private GrabablePhysicsHandler grabablePhysicsHandler;
    public float timeToWaitBeforFreez = 5;
    private float currentTimeBeforFreez;
    private void Update()
    {
        if (isGrab)
        {
            if(currentInteractor)
            currentInteractor.GetComponent<HandBurn>().BurnEvent(flockInteractable);
        }
        if(hasDoneFirstGrab &&!isGrab && grabablePhysicsHandler && !isOnStasis&& grabablePhysicsHandler.enabled && dissolveFlox.dissolveState ==1 && flockInteractable.enabled && rgb.velocity.magnitude < 0.1f)
        {
            currentTimeBeforFreez += Time.deltaTime;
            grabablePhysicsHandler.SetFreezValue(currentTimeBeforFreez, timeToWaitBeforFreez);
            if(currentTimeBeforFreez>= timeToWaitBeforFreez)
            {
                if (inputManager)
                {
                    inputManager.GetComponentInChildren<GrabManager>().FreezHotPotato(gameObject);
                    currentTimeBeforFreez = 0;
                }
            }
        }
        else if(currentTimeBeforFreez != 0)
        {
            currentTimeBeforFreez = 0;
            grabablePhysicsHandler.SetFreezValue(currentTimeBeforFreez, timeToWaitBeforFreez);
        }
    }
    public override void OnStarted(GameObject _object)
    {
        base.OnStarted(_object);
        dissolveFlox = GetComponent<DissolveFlox>();
        grabablePhysicsHandler = GetComponent<GrabablePhysicsHandler>();
       // sound.clipName = collisionClipName;
    }
    public override void OnEnterStasis(GameObject _object, bool isGrab, Rigidbody rgb)
    {
        base.OnEnterStasis(_object, isGrab,rgb);
    }
    public override void OnGrabed(GameObject _object)
    {
        hasDoneFirstGrab = true;
        currentTimeBeforFreez = 0;
        timerSlow = 0;
        grabSound = "HotGrab";
        base.OnGrabed(_object);
        if(currentInteractor)
        currentInteractor.GetComponent<HandBurn>().doOnce = false;
    }
    public override void OnHitGround(GameObject _object, Vector3 initPos, bool isGrab)
    {
        base.OnHitGround(_object, initPos, isGrab);
    }
    public override void OnHitSomething(GameObject _object, Vector3 velocity, GameObject collision)
    {
        collisionSound = "HotCollision";

        base.OnHitSomething(_object, velocity, collision);
    }
    public override void OnReleased(GameObject _object)
    {
        base.OnReleased(_object);
        if(currentInteractor)
        currentInteractor.GetComponent<HandBurn>().DropEvent();
    }
}
