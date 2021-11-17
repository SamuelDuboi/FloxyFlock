using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GrabablePhysicsHandler : MonoBehaviour
{

    public Modifier[] modifiers;
    public Rigidbody m_rgb;
    public Collider[] colliders;
    public GrabbableObject m_grabbable;

    public UnityEvent<GameObject> OnGrabed;
    public UnityEvent<GameObject> OnReleased;
    public UnityEvent<GameObject,Vector3, GameObject> OnHitSomething;
    public UnityEvent<GameObject> OnHitGround;
    public UnityEvent<GameObject,bool> OnEnterStasis;
    void Start()
    {
        

    }

  

    void OnGrabListener() 
    {
        Debug.Log("Grab");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            OnHitGround.Invoke(gameObject);
            
        }
        else if(collision.gameObject.layer == 9)
        {
            OnEnterStasis.Invoke(gameObject, m_grabbable.isGrab);
            
        }
        else
        {
            OnHitSomething.Invoke(gameObject, m_rgb.velocity, collision.gameObject); ;
            
        }
        
    }
   public void InvokeOngrab()
    {
       OnGrabed.Invoke(gameObject);
        
    }
    public void InvokeOnRelease()
    {
       OnReleased.Invoke(gameObject);
        
    }

    public void ChangeBehavior(Modifier[] modifier)
    {
        for (int i = 0; i < modifiers.Length; i++)
        {
            //faire un manager de modifier comme ça ca les instantie
            OnGrabed.RemoveListener(modifiers[i].actions.OnGrabed);
            OnReleased.RemoveListener(modifiers[i].actions.OnReleased);
            OnHitSomething.RemoveListener(modifiers[i].actions.OnHitSomething);
            OnHitGround.RemoveListener(modifiers[i].actions.OnHitGround);
            OnEnterStasis.RemoveListener(modifiers[i].actions.OnEnterStasis);
        }

        modifiers = modifier;
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].material = modifiers[0].physiqueMaterial;
        }
        for (int i = 0; i < modifiers.Length; i++)
        {
            //faire un manager de modifier comme ça ca les instantie
            OnGrabed.AddListener(modifiers[i].actions.OnGrabed);
            OnReleased.AddListener(modifiers[i].actions.OnReleased);
            OnHitSomething.AddListener(modifiers[i].actions.OnHitSomething);
            OnHitGround.AddListener(modifiers[i].actions.OnHitGround);
            OnEnterStasis.AddListener(modifiers[i].actions.OnEnterStasis);
        }

    }
}
