using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabManager : MonoBehaviour
{

    public List<Modifier> modifiers;
    public List<GrabablePhysicsHandler> grabableObjects;
    public List<GameObject> grabableObjectsToSpawn;


    //note dispplayed yet
#if UNITY_EDITOR
    public List< int> numberPerBatch;
    public bool modifierFoldout;
    public bool batcheFoldout;
   [SerializeField] public List<int> numbersPerModifer;
#endif
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < grabableObjects.Count; i++)
        {
            Modifier[] _modifer = new Modifier[2];
            _modifer[0] = modifiers[0];
            _modifer[1] = modifiers[Random.Range(0, modifiers.Count)];
            grabableObjects[i].ChangeBehavior(_modifer);
        }
    }

    public void SpawnBacth()
    {
        for (int i = 0; i < numberPerBatch.Count; i++)
        {
            for (int x = 0; x < numberPerBatch[i]; x++)
            {
                //instanitiate here
            }
        }
    }
}
