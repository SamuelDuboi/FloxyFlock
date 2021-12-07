using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HotPotato1 : ModifierAction
{
    public float timeBeforeAutoRelease;
    public string collisionClipName;
    public string idleClipName;

    private void Update()
    {
        if (isGrab)
            currentInteractor.GetComponent<HandBurn>().BurnEvent(flockInteractable);
    }
    public override void OnStarted(GameObject _object)
    {
        base.OnStarted(_object);
        sound= _object.AddComponent<SoundReader>();
        sound.clipName = collisionClipName;
    }
    public override void OnEnterStasis(GameObject _object, bool isGrab, Rigidbody rgb)
    {
        base.OnEnterStasis(_object, isGrab,rgb);
    }
    public override void OnGrabed(GameObject _object)
    {
        base.OnGrabed(_object);
    }
    public override void OnHitGround(GameObject _object, Vector3 initPos, bool isGrab)
    {
        base.OnHitGround(_object, initPos, isGrab);
    }
    public override void OnHitSomething(GameObject _object, Vector3 velocity, GameObject collision)
    {
        base.OnHitSomething(_object, velocity, collision);
    }
    public override void OnReleased(GameObject _object)
    {
        base.OnReleased(_object);
    }
}
