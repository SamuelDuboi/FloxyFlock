using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GrabManager : MonoBehaviour
{

    public List<Modifier> modifiers;
    public List<GrabablePhysicsHandler> grabableObjects;
    public List<GameObject> grabableObjectsToSpawn;
    public PhysicMaterial[] basicMats = new PhysicMaterial[2];
#if UNITY_EDITOR
    public List<int> numberPerBatch;
    public bool modifierFoldout;
    public bool batcheFoldout;
    public List<ModifierAction> actions;
    [SerializeField] public List<int> numbersPerModifer;
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
    }

    public void SpawnBacth()
    {
        for (int i = 0; i < numberPerBatch.Count; i++)
        {
            for (int x = 0; x < numberPerBatch[i]; x++)
            {
                GameObject grabbable = Instantiate(grabableObjectsToSpawn[i], grabableObjectsToSpawn[i].transform.position, Quaternion.identity);
                Modifier _modifer =  modifiers[UnityEngine.Random.Range(0, modifiers.Count)];
                Type type = _modifer.actions.GetType();
                var _object = GetComponent(type);
                grabbable.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(modifiers[UnityEngine.Random.Range(0, modifiers.Count)], _object as ModifierAction, basicMats);
            }
        }
    }
}
