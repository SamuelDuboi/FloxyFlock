using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;
public class GrabManager : MonoBehaviour
{
    public List<Modifier> modifiers;
    public List<GrabablePhysicsHandler> grabableObjects;
    public List<Batch> batches;
    public PhysicMaterial[] basicMats = new PhysicMaterial[2];
    public Representation[] representations;
    private List<pool> mainPool;
    private int currentPool;
#if UNITY_EDITOR
    public bool modifierFoldout;
    public List<ModifierAction> actions;
    [SerializeField] public List<int> numbersPerModifer;
     public GameObject[]allflocks;
#endif
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < grabableObjects.Count; i++)
        {
            Modifier _modifer = modifiers[UnityEngine.Random.Range(0, modifiers.Count)];
            Type type = _modifer.actions.GetType();
            var _object = GetComponent(type);
            grabableObjects[i].ChangeBehavior(_modifer, _object as ModifierAction, basicMats);
        }
        InputManager.instance.OnSpawn.AddListener(SpawnBacth);
        for (int i = 0; i < modifiers.Count; i++)
        {
            Type type = modifiers[i].actions.GetType();
            var _object = GetComponent(type);
            if (_object)
            {
                Destroy(_object);
            }
        }
        InitPool();
    }
    private void InitPool()
    {
        mainPool = new List<pool>();
        for (int i = 0; i < batches.Count; i++)
        {
            mainPool.Add(new pool());
            mainPool[i].floxes = new List<GameObject>();
            for (int x = 0; x < batches[i].pieces.Count; x++)
            {
                GameObject flock = Instantiate(batches[i].pieces[x], new Vector3(300+x*5, 300, 300), Quaternion.identity);
                Modifier _modifer = modifiers[UnityEngine.Random.Range(0, modifiers.Count)];
                Type type = _modifer.actions.GetType();
                var _object = GetComponent(type);
                flock.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(_modifer, _object as ModifierAction, basicMats);
                flock.GetComponent<GrabablePhysicsHandler>().enabled = false;

                flock.GetComponent<Rigidbody>().useGravity= false;
                mainPool[i].floxes.Add(flock);

            }
        }
    }
    public void SpawnBacth()
    {
        int totalWeight = 0;
        for (int i = 0; i < batches.Count; i++)
        {
            totalWeight += batches[i].weight;
        }
        int random = UnityEngine.Random.Range(0, totalWeight);
        int currentWeight;
        for (int i = 0; i < batches.Count; i++)
        {
            currentWeight = batches[i].weight;
            if (currentWeight > random)
            {
                currentPool = i;
                break;
            }
        }
        for (int i = 0; i < batches[currentPool].pieces.Count; i++)
        {
            representations[i].gameObject.SetActive(true);
            representations[i].index = i;
            representations[i].manager = this;
            representations[i].image.texture = mainPool[currentPool].floxes[i].GetComponent<TextureForDispenser>().texture;

         
        }
        /*for (int i = 0; i < numberPerBatch.Count; i++)
        {
            for (int x = 0; x < numberPerBatch[i]; x++)
            {
                GameObject grabbable = Instantiate(grabableObjectsToSpawn[i], grabableObjectsToSpawn[i].transform.position, Quaternion.identity);
                Modifier _modifer =  modifiers[UnityEngine.Random.Range(0, modifiers.Count)];
                Type type = _modifer.actions.GetType();
                var _object = GetComponent(type);
                grabbable.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(modifiers[UnityEngine.Random.Range(0, modifiers.Count)], _object as ModifierAction, basicMats);
            }
        }*/
    }

    public void GetPiece(XRBaseInteractor baseInteractor, int index)
    {

        mainPool[currentPool].floxes[index].GetComponent<GrabablePhysicsHandler>().enabled = true;
        XRBaseInteractable baseInteractable = mainPool[currentPool].floxes[index].GetComponent<GrabbableObject>();
        mainPool[currentPool].floxes[index].transform.position = baseInteractor.transform.position;

        InteractionManager.instance.ForceSelect(baseInteractor, baseInteractable);
        representations[index].gameObject.SetActive(false);
    }
}

class pool
{
    public List<GameObject> floxes ;
}