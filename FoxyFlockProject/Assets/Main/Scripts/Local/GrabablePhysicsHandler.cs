using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Reflection;

public class GrabablePhysicsHandler : MonoBehaviour
{

    public Modifier[] modifiers;
    public Rigidbody m_rgb;
    public Collider[] colliders;
    public GrabbableObject m_grabbable;

    public UnityEvent<GameObject> OnGrabed;
    public UnityEvent<GameObject> OnStart;
    public UnityEvent<GameObject> OnReleased;
    public UnityEvent<GameObject,Vector3, GameObject> OnHitSomething;
    public UnityEvent<GameObject> OnHitGround;
    public UnityEvent<GameObject,bool, Rigidbody> OnEnterStasis;

    //enter on playgroundValue
   [HideInInspector] public float slowForce;
   [HideInInspector] public float timeToSlow;
    public bool isOnPlayground;
    public bool isOnStasisOnce;
    public float timerToExit;
    private ModifierAction[] actions = new ModifierAction[2];
    public IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        InvokeOnStart();
    }

    private void Update()
    {
        if (isOnPlayground)
        {
            isOnStasisOnce = false;
            timerToExit += Time.deltaTime;
            if (timerToExit > 0.1f)
            {
                isOnPlayground = false;
                timerToExit = 0;
            }
        }
        else if (!isOnStasisOnce)
        {
            isOnStasisOnce = true;
            OnEnterStasis.Invoke(gameObject, m_grabbable.isGrab, m_rgb);
        }
    }
    public void SetIsOnPlayGround(float _lowForce, float _timeToSlow)
    {
        slowForce = _lowForce;
        timeToSlow = _timeToSlow;
        isOnPlayground = true;
        timerToExit = 0;
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

        else
        {
            OnHitSomething.Invoke(gameObject, m_rgb.velocity, collision.gameObject); ;
            
        }
        
    }
    public void InvokeOnStart()
    {
        OnStart.Invoke(gameObject);

    }
    public void InvokeOngrab()
    {
       OnGrabed.Invoke(gameObject);
        
    }
    public void InvokeOnRelease()
    {
       OnReleased.Invoke(gameObject);
        
    }

    public void ChangeBehavior(Modifier[] modifier, ModifierAction action)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            if (!actions[i])
                continue;

            OnGrabed.RemoveListener(actions[i].OnGrabed);
            OnReleased.RemoveListener(actions[i].OnReleased);
            OnHitSomething.RemoveListener(actions[i].OnHitSomething);
            OnHitGround.RemoveListener(actions[i].OnHitGround);
            OnEnterStasis.RemoveListener(actions[i].OnEnterStasis);
            OnStart.RemoveListener(actions[i].OnStarted);
            actions[i].enabled = false;
        }

        modifiers = modifier;
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].material = modifiers[0].physiqueMaterial;
        }
        for (int i = 0; i < actions.Length; i++)

        {
            Type type = modifiers[i].actions.GetType();
            actions[i] = gameObject.AddComponent(type) as ModifierAction;
            MyExtension.GetCopyOf(actions[i], action);
            OnGrabed.AddListener(actions[i].OnGrabed);
            OnReleased.AddListener(actions[i].OnReleased);
            OnHitSomething.AddListener(actions[i].OnHitSomething);
            OnHitGround.AddListener(actions[i].OnHitGround);
            OnEnterStasis.AddListener(actions[i].OnEnterStasis);
            OnStart.AddListener(actions[i].OnStarted);
        }

    }
}
public static class MyExtension
{
    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType())
            return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch
                {
                } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }
}