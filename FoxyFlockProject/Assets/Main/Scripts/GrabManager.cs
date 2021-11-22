using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GrabManager : MonoBehaviour
{

    public List<Modifier> modifiers;
    public List<GrabablePhysicsHandler> grabableObjects;
    public List<GameObject> grabableObjectsToSpawn;

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
            Modifier[] _modifer = new Modifier[2];
            _modifer[0] = modifiers[0];
            _modifer[1] = modifiers[UnityEngine.Random.Range(0, modifiers.Count)];
            Type type = modifiers[0].actions.GetType();
            Type type2 = modifiers[1].actions.GetType();
            var _object = GetComponent(type);
            var _object2 = GetComponent(type2);
            grabableObjects[i].ChangeBehavior(_modifer, _object as ModifierAction);
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
                Modifier[] _modifer = new Modifier[2];
                _modifer[0] = modifiers[0];
                _modifer[1] = modifiers[UnityEngine.Random.Range(0, modifiers.Count)];
                Type type = modifiers[i].actions.GetType();
                var _object = GetComponent(type);
                grabbable.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(_modifer,_object as ModifierAction);
            }
        }
    }
}
