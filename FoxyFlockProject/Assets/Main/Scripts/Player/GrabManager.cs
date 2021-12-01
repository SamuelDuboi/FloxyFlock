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
    private bool isGrabLeft;
    private bool isGrabRight;
    private bool isFirstBacthPassed;
    private int currentPool;
    public PlayGround playGround;
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
        InputManager.instance.OnGrabbingLeft.AddListener(OnGrabLeft);
        InputManager.instance.OnGrabbingReleaseLeft.AddListener(OnRealeseLeft);
        InputManager.instance.OnGrabbingRight.AddListener(OnGrabRight);
        InputManager.instance.OnGrabbingReleaseRight.AddListener(OnRealeseRight);
       
    }
    private void InitPool()
    {
        mainPool = new List<pool>();
        ScenesManager.instance.numberOfFlocksInScene=0;
        for (int i = 0; i < batches.Count; i++)
        {
            mainPool.Add(new pool());
            mainPool[i].floxes = new List<GameObject>();
            mainPool[i].isSelected = new List<bool>();
            for (int x = 0; x < batches[i].pieces.Count; x++)
            {
                GameObject flock = Instantiate(batches[i].pieces[x], new Vector3(300+x*20+i*5, 300 + x * 20 + i * 5, 300 + x * 20 + i * 5), Quaternion.identity);
                Modifier _modifer = modifiers[UnityEngine.Random.Range(0, modifiers.Count)];
                Type type = _modifer.actions.GetType();
                var _object = GetComponent(type);
                flock.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(_modifer, _object as ModifierAction, basicMats);
                flock.GetComponent<GrabablePhysicsHandler>().enabled = false;

                flock.GetComponent<Rigidbody>().useGravity= false;
                mainPool[i].floxes.Add(flock);
                mainPool[i].isSelected.Add(false);
                ScenesManager.instance.numberOfFlocksInScene++;
            }
        }
    }
    public void SpawnBacth()
    {
        if (isFirstBacthPassed)
        {
            if (!mainPool[currentPool].isEmpty )
            return;

        }
        else
        {
            isFirstBacthPassed = true;
        }

        int totalWeight = 0;
        for (int i = 0; i < batches.Count; i++)
        {
            totalWeight += batches[i].weight;

        }
        int numberOfRound = 0;
       //no one need to read that
        StartLoop:
        int random = UnityEngine.Random.Range(0, totalWeight);
        int currentWeight=0;
        numberOfRound++;
        if (numberOfRound > batches.Count)
            return;
            for (int i = 0; i < batches.Count; i++)
            {
                currentWeight += batches[i].weight;
                if (currentWeight > random)
                {
                    if (batches[i].isEmpty)
                    {
                        goto StartLoop;
                    }
                    currentPool = i;
                    break;
                }
            }
        for (int i = 0; i < representations.Length; i++)
        {
            representations[i].gameObject.SetActive(false);
        }
        
        for (int i = 0; i < mainPool[currentPool].floxes.Count; i++)
        {
            if (mainPool[currentPool].isSelected[i])
                return;
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
    public void LockBatche(GameObject gameObject)
    {

    }


    public void GetPiece(XRBaseInteractor baseInteractor, int index)
    {
        if (!isGrabLeft && !isGrabRight)
            return;
        var grabable = mainPool[currentPool].floxes[index].GetComponent<GrabablePhysicsHandler>();
        grabable.enabled = true;
    
        XRBaseInteractable baseInteractable = mainPool[currentPool].floxes[index].GetComponent<GrabbableObject>();
        mainPool[currentPool].floxes[index].transform.position = baseInteractor.transform.position;
        StartCoroutine(WaiToSelect(baseInteractable, baseInteractor, index, grabable));
        representations[index].gameObject.SetActive(false);
        mainPool[currentPool].isSelected[index] = true;
        grabable.OnHitGround.AddListener(RespawnPiece);
        for (int i = 0; i < mainPool[currentPool].isSelected.Count; i++)
        {
            if (!mainPool[currentPool].isSelected[i])
                return;
        }
        batches[currentPool].isEmpty = true;
        mainPool[currentPool].isEmpty = true;
    }
    IEnumerator WaiToSelect(XRBaseInteractable baseInteractable,XRBaseInteractor baseInteractor, int index, GrabablePhysicsHandler grabable)
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        InteractionManager.instance.ForceSelect(baseInteractor, baseInteractable);
        grabable.timeToSlow = playGround.timeBeforFall;
        grabable.slowForce = playGround.slowForce;
        grabable.OnEnterStasis.Invoke(grabable.gameObject, true, grabable.m_rgb);
    }
    private void OnCollisionStay(Collision collision)
    {
        var coll = collision.gameObject.GetComponentInParent<GrabbableObject>();
        if (coll)
        {
            IsInCurrentPool(coll);
        }
    }
    private bool IsInCurrentPool(GrabbableObject coll)
    {
        for (int i = 0; i < mainPool[currentPool].floxes.Count; i++)
        {
            if (mainPool[currentPool].floxes[i] == coll.gameObject && !coll.isGrab)
            {
                mainPool[currentPool].isSelected[i] = false;
                mainPool[currentPool].isEmpty = false;
                representations[i].gameObject.SetActive(true);
                coll.GetComponent<Rigidbody>().useGravity = false;
                coll.GetComponent<Rigidbody>().velocity = Vector3.zero;
                coll.transform.rotation = Quaternion.identity;
                coll.transform.position = new Vector3(300 + i * 5, 300 + i * 5, 300);
                return true;
            }
        }
        return false;
    }
    private void OnGrabLeft()
    {
        isGrabLeft = true;
    }
    private void OnRealeseLeft()
    {
        isGrabLeft = false;
    }
    private void OnGrabRight() 
    { 
        isGrabRight = true;
    }
    private void OnRealeseRight()
    {
        isGrabRight = false;
    }

    private void RespawnPiece(GameObject _object, Vector3 initPos,bool isGrab)
    {
        if (!isGrab)
        {
            if (!IsInCurrentPool(_object.GetComponent<GrabbableObject>()))
            {
                for (int i = 0; i < mainPool.Count; i++)
                {
                    if(i!= currentPool)
                    {
                        for (int x = 0; x < mainPool[i].floxes.Count; x++)
                        {
                            if(mainPool[i].floxes[x]== _object)
                            {
                                mainPool[i].isSelected[x] = false;
                                batches[i].isEmpty = false;
                                mainPool[i].isEmpty = true;
                                _object.GetComponent<Rigidbody>().useGravity = false;
                                _object.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                _object.transform.rotation = Quaternion.identity;
                                _object.transform.position = new Vector3(300 + i * 5, 300 + i * 5, 300);
                            }

                            //find a way to add recreat a bacth with only one piece and nee to add this piece with others
                           // if(mainPool)
                        }
                    }
                }
            }
        }
    }

}

class pool
{
    public List<GameObject> floxes ;
    public List<bool> isSelected;
    public bool isEmpty;
}