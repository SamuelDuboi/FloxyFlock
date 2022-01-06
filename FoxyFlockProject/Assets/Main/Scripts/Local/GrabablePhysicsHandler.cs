using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Reflection;

public class GrabablePhysicsHandler : MonoBehaviour
{

    public Modifier modifier;
    public Rigidbody m_rgb;
    public List<Collider> colliders;
    public GrabbableObject m_grabbable;
    private MeshRenderer meshRenderer;
    private Material initialMat;
    private Material mats;
    public UnityEvent<GameObject> OnGrabed;
    public UnityEvent<GameObject> OnStart;
    public UnityEvent<GameObject> OnReleased;
    public UnityEvent<GameObject, Vector3, GameObject> OnHitSomething;
    public UnityEvent<GameObject, Vector3, bool> OnHitGround;
    public UnityEvent<GameObject, bool, Rigidbody> OnEnterStasis;
    public UnityEvent<GameObject> OnExitStasis;
    private Vector3 initPos;
    //enter on playgroundValue
    [HideInInspector] public float slowForce;
    [HideInInspector] public float timeToSlow;
    private PhysicMaterial[] physicMaterials;
    public bool isOnPlayground;
    public bool isOnStasisOnce;
    public float timerToExit;
    private ModifierAction[] actions = new ModifierAction[1];
    MaterialPropertyBlock propBlock;
    public InputManager inputManager;
    public Modifier initialModifier;
    public IEnumerator Start()
    {

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        meshRenderer = GetComponent<MeshRenderer>();
        initialMat = meshRenderer.material;

        //overrid mat without creat new instance or modify it
        propBlock = new MaterialPropertyBlock();

        
        colliders = m_grabbable.colliders;
        initPos = transform.position;
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
        if (!isOnPlayground)
            OnExitStasis.Invoke(gameObject);
        isOnPlayground = true;
        timerToExit = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {

            OnHitSomething.Invoke(gameObject, m_rgb.velocity, collision.gameObject); ;

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            OnHitGround.Invoke(gameObject, initPos, m_grabbable.isGrab);

        }
    }

    public void InvokeOnStart()
    {
        m_grabbable.OnHover += OnHover;
        m_grabbable.OnHoverExit += OnHoverExit;
        m_grabbable.OnSelect += OnSelect;
        OnStart.Invoke(gameObject);

    }
    public void InvokeOngrab()
    {
        OnGrabed.Invoke(gameObject);
        ChangePhysicMatsOnDeSelect();
    }
    public void InvokeOnRelease()
    {
        OnReleased.Invoke(gameObject);
        ChangePhysicMatsOnSelect();
    }

    public void ChangeBehavior(Modifier _modifier, ModifierAction action, PhysicMaterial[] grabedMat)
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
            OnExitStasis.RemoveListener(actions[i].OnExitStasis);
            actions[i].enabled = false;
        }

        modifier = _modifier;
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].material = modifier.physiqueMaterial;
        }
        for (int i = 0; i < actions.Length; i++)

        {
            Type type = modifier.actions.GetType();
            actions[i] = gameObject.AddComponent(type) as ModifierAction;
            MyExtension.GetCopyOf(actions[i], action);
            OnGrabed.AddListener(actions[i].OnGrabed);
            OnReleased.AddListener(actions[i].OnReleased);
            OnHitSomething.AddListener(actions[i].OnHitSomething);
            OnHitGround.AddListener(actions[i].OnHitGround);
            OnEnterStasis.AddListener(actions[i].OnEnterStasis);
            OnExitStasis.AddListener(actions[i].OnExitStasis);
            OnStart.AddListener(actions[i].OnStarted);
        }
        PhysicMaterial[] tempMat = new PhysicMaterial[2];
        tempMat[0] = grabedMat[0];
        if (_modifier.hasPhysiqueMaterial)
            tempMat[1] = _modifier.physiqueMaterial;
        else
            tempMat[1] = grabedMat[1];
        if (!modifier.isBasciFloat)
            ChangeMat(_modifier.material,_modifier.materialt, tempMat);
        else
            ChangeMat(tempMat);

    }
    public void ChangeBehavior( ModifierAction action, PhysicMaterial[] grabedMat)
    {

        for (int i = 0; i < actions.Length; i++)
        {
            if (!actions[i])
                continue;
            Destroy(gameObject.GetComponent(actions[i].GetType()));
            OnGrabed.RemoveListener(actions[i].OnGrabed);
            OnReleased.RemoveListener(actions[i].OnReleased);
            OnHitSomething.RemoveListener(actions[i].OnHitSomething);
            OnHitGround.RemoveListener(actions[i].OnHitGround);
            OnEnterStasis.RemoveListener(actions[i].OnEnterStasis);
            OnStart.RemoveListener(actions[i].OnStarted);
            OnExitStasis.RemoveListener(actions[i].OnExitStasis);
            actions[i].enabled = false;
        }

        modifier = initialModifier;
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].material = modifier.physiqueMaterial;
        }
        for (int i = 0; i < actions.Length; i++)

        {
            actions[i] = gameObject.AddComponent(action.GetType()) as ModifierAction;

            OnGrabed.AddListener(actions[i].OnGrabed);
            OnReleased.AddListener(actions[i].OnReleased);
            OnHitSomething.AddListener(actions[i].OnHitSomething);
            OnHitGround.AddListener(actions[i].OnHitGround);
            OnEnterStasis.AddListener(actions[i].OnEnterStasis);
            OnExitStasis.AddListener(actions[i].OnExitStasis);
            OnStart.AddListener(actions[i].OnStarted);
        }
        PhysicMaterial[] tempMat = new PhysicMaterial[2];
        tempMat[0] = grabedMat[0];
        if (initialModifier.hasPhysiqueMaterial)
            tempMat[1] = initialModifier.physiqueMaterial;
        else
            tempMat[1] = grabedMat[1];
        if (!modifier.isBasciFloat)
            ChangeMat(initialModifier.material, null, tempMat);
        else
            ChangeMat(tempMat);

    }
    
    private void OnHover()
    {
        //Recup Data
        meshRenderer.GetPropertyBlock(propBlock);
        //EditZone
        propBlock.SetInt("inHand", 1);

        //Push Data
        meshRenderer.SetPropertyBlock(propBlock);
    }
    private void OnHoverExit()
    {
        //Recup Data
        meshRenderer.GetPropertyBlock(propBlock);
        //EditZone
        propBlock.SetInt("inHand", 0);

        //Push Data
        meshRenderer.SetPropertyBlock(propBlock);
    }

    private void OnSelect()
    {
        //Recup Data
        meshRenderer.GetPropertyBlock(propBlock);
        //EditZone
        propBlock.SetInt("inHand", 1);

        //Push Data
        meshRenderer.SetPropertyBlock(propBlock);
    }
    public void OnFreeze()
    {
        if(meshRenderer == null || propBlock == null || initialMat ==  null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            initialMat = meshRenderer.material;
            propBlock = new MaterialPropertyBlock();
        }

        //Recup Data
        meshRenderer.GetPropertyBlock(propBlock);
        //EditZone
        propBlock.SetFloat("IsFrozen", 1);

        //Push Data
        meshRenderer.SetPropertyBlock(propBlock);
    }

    private void ChangePhysicMatsOnSelect()
    {
        if (physicMaterials == null || physicMaterials[0] == null)
            return;
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].material = physicMaterials[0];
        }
    }
    private void ChangePhysicMatsOnDeSelect()
    {
        if (physicMaterials == null || physicMaterials[0] == null)
            return;
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].material = physicMaterials[1];
        }
    }
    private void ChangeMat(Material _mat, Material _matT, PhysicMaterial[] _physicMaterial)
    {
        physicMaterials = _physicMaterial;
        mats = _mat;
        if(_matT != null)
        {
            GetComponent<DissolveFlox>().floxMaterial = mats;
            GetComponent<DissolveFlox>().floxMaterialT = _matT;
        }
        
        if (!meshRenderer)
            meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = mats;
        //change value in shader
    }
    private void ChangeMat( PhysicMaterial[] _physicMaterial)
    {
        physicMaterials = _physicMaterial;
        if (!meshRenderer)
            meshRenderer = GetComponent<MeshRenderer>();
        if (!initialMat)
            initialMat = meshRenderer.material;
        mats = initialMat;
        meshRenderer.material = mats;
        //change value in shader
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