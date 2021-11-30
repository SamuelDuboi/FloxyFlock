using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : ModifierAction
{
    [SerializeField] private float breakThreshold = 10;
    [SerializeField] private GameObject shardsParticleSystem;
    [SerializeField] private string clipName = "";

    public override void OnStarted(GameObject _object)
    {
        base.OnStarted(_object);

        sound = _object.AddComponent<SoundReader>();
        sound.clipName = clipName;
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
        base.OnHitGround(_object,initPos,isGrab);
    }
    public override void OnHitSomething(GameObject _object, Vector3 velocity, GameObject collision)
    {
        base.OnHitSomething(_object, velocity, collision);

        if (velocity.magnitude >= breakThreshold || (collision.GetComponent<Rigidbody>() != null && collision.GetComponent<Rigidbody>().velocity.magnitude >= breakThreshold)) // TODO : Find a way to make this work with any colliding object
        {
            Instantiate(shardsParticleSystem, this.transform.position, Quaternion.identity);

            if (sound)
                sound.Play();

            InteractionManager.instance.SelectExit(currentInteractor, flockInteractable); // Should be working on main
            _object.GetComponent<MeshRenderer>().enabled = false;
            _object.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public override void OnReleased(GameObject _object)
    {
        base.OnReleased(_object);
    }

}