using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;
using Mirror.Experimental;
public class GrabManager : MonoBehaviour
{
    public Modifier baseModifier;
    public List<Modifier> positiveModifiers;
    public List<Modifier> negativeModifiers;
    public List<GrabablePhysicsHandler> grabableObjects;
    public List<Batch> batches;
    public PhysicMaterial[] basicMats = new PhysicMaterial[2];
    public Representation[] representations;
    public Representation[] representationsModifiers;
    public List<pool> mainPool;
    protected bool isGrabLeft;
    protected bool isGrabRight;
    protected bool isFirstBacthPassed;
    protected int currentPool;
    protected List<GameObject> malusNumber;
    protected List<GameObject> fireBallNumber;
    protected List<GameObject> bonusNumber;
    public Reset reset;
    public PlayGround playGround;
    public InputManager inputManager;
    public GameObject fireBallInstantiated;
    protected SoundReader sound;
    public int currentMilestone = -1;
    protected Vector3 positionOfMilestoneIntersection;
    protected int numberOfMilestones;

    protected MaterialPropertyBlock propBlock;

    // public Buble[] bubles;
#if UNITY_EDITOR
    public bool modifierFoldout;
    public List<ModifierAction> actions;
    [SerializeField] public List<int> numbersPerModifer;
    public GameObject[] allflocks;
#endif
    // Start is called before the first frame update
    public virtual IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        propBlock = new MaterialPropertyBlock();
        sound = GetComponent<SoundReader>();
        inputManager = GetComponentInParent<InputManager>();
        for (int i = 0; i < grabableObjects.Count; i++)
        {
            Modifier _modifer = positiveModifiers[UnityEngine.Random.Range(0, positiveModifiers.Count)];
            Type type = _modifer.actions.GetType();
            var _object = GetComponent(type);
            grabableObjects[i].ChangeBehavior(_modifer, _object as ModifierAction, basicMats);
        }
        inputManager.OnSpawn.AddListener(UpdateBatche);
        for (int i = 0; i < positiveModifiers.Count; i++)
        {
            Type type = positiveModifiers[i].actions.GetType();
            var _object = GetComponent(type);
            
        }
        for (int i = 0; i < negativeModifiers.Count; i++)
        {
            Type type = negativeModifiers[i].actions.GetType();
            var _object = GetComponent(type);
            
        }
        InitPool();
        inputManager.OnGrabbingLeft.AddListener(OnGrabLeft);
        inputManager.OnGrabbingReleaseLeft.AddListener(OnRealeseLeft);
        inputManager.OnGrabbingRight.AddListener(OnGrabRight);
        inputManager.OnGrabbingReleaseRight.AddListener(OnRealeseRight);
    }
    public virtual void InitPool()
    {
        mainPool = new List<pool>();
        ScenesManager.instance.numberOfFlocksInScene = 0;
        for (int i = 0; i < batches.Count; i++)
        {
            mainPool.Add(new pool());
            mainPool[i].floxes = new List<GameObject>();
            mainPool[i].isSelected = new List<bool>();
            for (int x = 0; x < batches[i].pieces.Count; x++)
            {
                GameObject flock = Instantiate(batches[i].pieces[x], new Vector3(300 + (x * 5 + 1) * 20 * (i * 5 + 1), 300 + x * 20, 300 + x), Quaternion.identity);
                Modifier _modifer = baseModifier;
                Type type = _modifer.actions.GetType();
                var _object = GetComponent(type);
                flock.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(_modifer, _object as ModifierAction, basicMats);
                flock.GetComponent<GrabablePhysicsHandler>().enabled = false;
                flock.GetComponent<GrabablePhysicsHandler>().inputManager = inputManager;

                flock.GetComponent<Rigidbody>().useGravity = false;
                mainPool[i].floxes.Add(flock);
                mainPool[i].isSelected.Add(false);
                ScenesManager.instance.numberOfFlocksInScene++;
            }
            for (int x = 0; x < batches[i].batchModifier.negativeModifier.Count; x++)
            {
                GameObject flock2 = Instantiate(batches[i].batchModifier.negativeModifier[x], new Vector3(300 + (15 * 5 + 1) * 20 * (i * 5 + 1), 300 + 15 * 20, 300 + 15), Quaternion.identity);
                Modifier _modifierPiece = negativeModifiers[UnityEngine.Random.Range(0, negativeModifiers.Count)];
                Type typePiece = _modifierPiece.actions.GetType();
                var _objectPiece = GetComponent(typePiece);
                flock2.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(_modifierPiece, _objectPiece as ModifierAction, basicMats);
                flock2.GetComponent<GrabablePhysicsHandler>().enabled = false;
                flock2.GetComponent<GrabablePhysicsHandler>().inputManager = inputManager;

                flock2.GetComponent<Rigidbody>().useGravity = false;
                if (mainPool[i].malus == null)
                    mainPool[i].malus = new List<GameObject>();
                mainPool[i].malus.Add(flock2);
            }
            for (int x = 0; x < batches[i].batchModifier.positiveModifiers.Count; x++)
            {
                GameObject flock2 = Instantiate(batches[i].batchModifier.positiveModifiers[x], new Vector3(300 + (15 * 5 + 1) * 20 * (i * 5 + 1), 300 + 15 * 20, 300 + 15), Quaternion.identity);
                Modifier _modifierPiece = positiveModifiers[UnityEngine.Random.Range(0, positiveModifiers.Count)];
                Type typePiece = _modifierPiece.actions.GetType();
                var _objectPiece = GetComponent(typePiece);
                flock2.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(_modifierPiece, _objectPiece as ModifierAction, basicMats);
                flock2.GetComponent<GrabablePhysicsHandler>().enabled = false;
                flock2.GetComponent<GrabablePhysicsHandler>().inputManager = inputManager;

                flock2.GetComponent<Rigidbody>().useGravity = false;
                if (mainPool[i].bonus == null)
                    mainPool[i].bonus = new List<GameObject>();
                mainPool[i].bonus.Add(flock2);
            }


        }
    }


    #region Update
    public void UpdateBatche()
    {
        int previousPool = currentPool;
        #region security test
        if (isFirstBacthPassed)
        {
            if (!mainPool[currentPool].isEmpty)
            {
                int randomSound = UnityEngine.Random.Range(1, 3);
                sound.clipName = "FloxMachineBad" + randomSound.ToString();
                sound.Play();
                Debug.LogError("There are still flock on the dispenser");
                return;
            }
            if (!mainPool[currentPool].isEmptyModifier)
            {
                int randomSound = UnityEngine.Random.Range(1, 3);
                sound.clipName = "FloxMachineBad" + randomSound.ToString();
                sound.Play();
                Debug.LogError("There are still bonus or malus on the dispenser");
                return;
            }

        }
        else
        {
            previousPool = 5000;
            isFirstBacthPassed = true;
        }
        for (int i = 0; i < mainPool[currentPool].floxes.Count; i++)
        {
            if (mainPool[currentPool].floxes[i].GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
            {
                int randomSound = UnityEngine.Random.Range(1, 3);
                sound.clipName = "FloxMachineBad" + randomSound.ToString();
                sound.Play();
                Debug.LogError("Your floxes are still moving");
                return;
            }
            if (mainPool[currentPool].floxes[i].GetComponent<GrabbableObject>().isGrab)
            {
                int randomSound = UnityEngine.Random.Range(1, 3);
                sound.clipName = "FloxMachineBad" + randomSound.ToString();
                sound.Play();
                Debug.Log("grabbed");
                return;
            }
            if (mainPool[currentPool].floxes[i].GetComponent<GrabablePhysicsHandler>().enabled && !mainPool[currentPool].floxes[i].GetComponent<GrabablePhysicsHandler>().isOnPlayground)
            {
                int randomSound = UnityEngine.Random.Range(1, 3);
                sound.clipName = "FloxMachineBad" + randomSound.ToString();
                sound.Play();
                Debug.Log("Flock in stasis");
                return;
            }
        }
        #endregion

        int totalWeight = 0;
        for (int i = 0; i < batches.Count - 1; i++)
        {
            if (batches[i].isEmpty)
                continue;
            totalWeight += batches[i].weight;

        }
    //  int numberOfRound = 0;
    //no one need to read that
    StartLoop:
        int random = UnityEngine.Random.Range(0, totalWeight);
        int currentWeight = 0;
        /* numberOfRound++;
         if (numberOfRound > batches.Count)
             return;*/
        for (int i = 0; i < batches.Count - 1; i++)
        {
            if (batches[i].isEmpty)
                continue;
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
        if (currentWeight == 0)
        {
            sound.PlaySeconde();
            //inputManager.OnSpawn.RemoveListener(SpawnBacth);
            return;
        }
        //in testing
        if (previousPool < 5000)
            Freez(previousPool);

        //In testing
        UpdateMilestone();

        //need to be done
        UpdateBoard();

        //done
        UpdateInventory();

        //In testing
        UpdateSpecial();

        //In testing
        UpdateBubble();


        int randomSound3 = UnityEngine.Random.Range(1, 2);
        sound.ThirdClipName = "FloxMachineGood" + randomSound3.ToString();
        sound.PlayThird();
    }
    private void Freez(int previousPool)
    {
        FreezOfList(mainPool[previousPool].floxes, previousPool);
        FreezOfList(mainPool[previousPool].bonus, previousPool);
        FreezOfList(mainPool[previousPool].malus,previousPool);
    }

    protected virtual void FreezOfList(List<GameObject> flocksToFreez,int indexOfPool)
    {
        if (flocksToFreez == null)
            return;
        for (int i = 0; i < flocksToFreez.Count; i++)
        {
            reset.AddFreezFlock(flocksToFreez[i],indexOfPool,i);
        }
    }
    protected virtual void UpdateMilestone()
    {
        currentMilestone = playGround.CheckMilestones(out positionOfMilestoneIntersection, out numberOfMilestones);
        Debug.Log("You have reache " + currentMilestone.ToString() + " / " + (numberOfMilestones + 1).ToString() + "Milestones");
        if (currentMilestone == numberOfMilestones)
            UIGlobalManager.instance.Win(1);
    }
    protected virtual void UpdateBoard()
    {

    }
    protected virtual void UpdateInventory()
    {
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
    }
    protected virtual void UpdateSpecial()
    {
        if (malusNumber != null && malusNumber.Count > 0)
        {
            AllowMalus();
            for (int i = 0; i < malusNumber.Count; i++)
            {
                if (malusNumber[i] != null)
                {
                    malusNumber[i].GetComponent<SoundReader>().Play();
                }
            }

        }
        if (bonusNumber != null && bonusNumber.Count > 0)
        {
            AllowBonus();
            for (int i = 0; i < bonusNumber.Count; i++)
            {
                if (bonusNumber[i] != null)
                    bonusNumber[i].GetComponent<SoundReader>().Play();
            }
            StartCoroutine(ResetModifierCount());
        }
    }
    protected virtual void UpdateBubble()
    {
        foreach (GameObject orb in playGround.bonusOrbes)
        {
            orb.transform.position += Vector3.up * playGround.milestoneManager.distance;
        }
        foreach (GameObject orb in playGround.malusOrbes)
        {
            orb.transform.position += Vector3.up * playGround.milestoneManager.distance;
        }
        playGround.fireBallOrbe.transform.position += Vector3.up * playGround.milestoneManager.distance;

    }
    #endregion
    IEnumerator ResetModifierCount()
    {
        yield return new WaitForSeconds(1.6f);
        if (malusNumber != null && malusNumber.Count != 0)
            malusNumber.Clear();
    }

    public void AddBubble(bool isMalus, GameObject bubble)
    {
        if (isMalus)
        {
            if (malusNumber == null)
                malusNumber = new List<GameObject>();
            if (malusNumber.Contains(bubble))
                return;
            malusNumber.Add(bubble);

        }
        else
        {
            if (bonusNumber == null)
                bonusNumber = new List<GameObject>();
            if (bonusNumber.Contains(bubble))
                return;
            bonusNumber.Add(bubble);
        }
    }
    public void AddFireBall(GameObject bubble)
    {
        if (fireBallNumber == null)
            fireBallNumber = new List<GameObject>();
        if (fireBallNumber.Contains(bubble))
            return;
        fireBallNumber.Add(bubble);
    }
    public void RemoveBubble(bool isMalus, GameObject bubble)
    {
        if (isMalus)
        {
            if (malusNumber.Count == 0)
                return;
            if (malusNumber.Contains(bubble))
                malusNumber.Remove(bubble);
        }
        else
        {
            if (bonusNumber.Count == 0)
                return;
            if (bonusNumber.Contains(bubble))
                bonusNumber.Remove(bubble);
        }
    }
    private void AllowMalus()
    {

        if (mainPool[currentPool].numberOfModifiersActivated > representationsModifiers.Length)
        {
            //if there is no bonus, return, else override the bonus
            if (mainPool[currentPool].bonusIndex == null || mainPool[currentPool].bonusIndex.Count == 0)
                return;
            else
            {
                representationsModifiers[mainPool[currentPool].bonusIndex[0]].gameObject.SetActive(true);
                representationsModifiers[mainPool[currentPool].bonusIndex[0]].index = mainPool[currentPool].bonusIndex[0];
                representationsModifiers[mainPool[currentPool].bonusIndex[0]].manager = this;
                representationsModifiers[mainPool[currentPool].bonusIndex[0]].image.texture = mainPool[currentPool].malus[mainPool[currentPool].bonusIndex.Count - 1].GetComponent<TextureForDispenser>().texture;
                if (mainPool[currentPool].malusIndex == null)
                    mainPool[currentPool].malusIndex = new List<int>();
                mainPool[currentPool].malusIndex.Add(mainPool[currentPool].bonusIndex[0]);
                representationsModifiers[mainPool[currentPool].bonusIndex[0]].indexInList = mainPool[currentPool].malusIndex.Count - 1;
                representationsModifiers[mainPool[currentPool].bonusIndex[0]].isMalus = true;

                mainPool[currentPool].bonusIndex.RemoveAt(0);
                return;
            }
        }
        representationsModifiers[mainPool[currentPool].numberOfModifiersActivated].gameObject.SetActive(true);
        representationsModifiers[mainPool[currentPool].numberOfModifiersActivated].index = mainPool[currentPool].numberOfModifiersActivated;
        representationsModifiers[mainPool[currentPool].numberOfModifiersActivated].manager = this;
        representationsModifiers[mainPool[currentPool].numberOfModifiersActivated].image.texture = mainPool[currentPool].malus[mainPool[currentPool].bonusIndex.Count - 1].GetComponent<TextureForDispenser>().texture;
        if (mainPool[currentPool].malusIndex == null)
            mainPool[currentPool].malusIndex = new List<int>();
        mainPool[currentPool].malusIndex.Add(mainPool[currentPool].numberOfModifiersActivated);
        representationsModifiers[mainPool[currentPool].numberOfModifiersActivated].indexInList = mainPool[currentPool].malusIndex.Count - 1;
        representationsModifiers[mainPool[currentPool].numberOfModifiersActivated].isMalus = true;

        mainPool[currentPool].numberOfModifiersActivated++;

        if (mainPool[currentPool] == null)
            mainPool[currentPool].isSelected = new List<bool>();
        mainPool[currentPool].isSelected.Add(false);
    }

    private void AllowBonus()
    {
        if (mainPool[currentPool].numberOfModifiersActivated > representationsModifiers.Length)
            return;
        representations[representations.Length - 1].gameObject.SetActive(true);
        representations[representations.Length - 1].index = representations.Length - 1;
        representations[representations.Length - 1].manager = this;
        representations[representations.Length - 1].image.texture = mainPool[currentPool].bonus[mainPool[currentPool].bonusIndex.Count - 1].GetComponent<TextureForDispenser>().texture;

        if (mainPool[currentPool].bonusIndex == null)
            mainPool[currentPool].bonusIndex = new List<int>();
        mainPool[currentPool].bonusIndex.Add(mainPool[currentPool].numberOfModifiersActivated);
        representationsModifiers[mainPool[currentPool].numberOfModifiersActivated].indexInList = mainPool[currentPool].bonus.Count - 1;
        representationsModifiers[mainPool[currentPool].numberOfModifiersActivated].isMalus = false;
        mainPool[currentPool].numberOfModifiersActivated++;

        if (mainPool[currentPool] == null)
            mainPool[currentPool].isSelected = new List<bool>();
        mainPool[currentPool].isSelected.Add(false);

    }
    public bool isOnCollision;
    public void GetPiece(XRBaseInteractor baseInteractor, int index)
    {
        if (baseInteractor.GetComponent<HandController>().controllerNode == UnityEngine.XR.XRNode.RightHand && !isGrabRight)
            return;
        if (baseInteractor.GetComponent<HandController>().controllerNode == UnityEngine.XR.XRNode.LeftHand && !isGrabLeft)
            return;
        representations[index].gameObject.SetActive(false);

        //how to fireball
        /* if (index == representations.Length - 2)
         {
             mainPool[currentPool].isFireballUsed = true;
             XRBaseInteractable baseInteractableBonus = fireBallInstantiated.GetComponent<GrabbableObject>();

             var grabableBonus = fireBallInstantiated.GetComponent<GrabablePhysicsHandler>();
             grabableBonus.enabled = true;

             StartCoroutine(WaiToSelect(baseInteractableBonus, baseInteractor, grabableBonus, true));
             fireBallInstantiated.transform.position = baseInteractor.transform.position;

             grabableBonus.OnHitGround.AddListener(RespawnFireball);
             return;
         }*/

        XRBaseInteractable baseInteractable = mainPool[currentPool].floxes[index].GetComponent<GrabbableObject>();

        var grabable = mainPool[currentPool].floxes[index].GetComponent<GrabablePhysicsHandler>();
        grabable.enabled = true;

        StartCoroutine(WaiToSelect(baseInteractable, baseInteractor, grabable, false));
        grabable.OnHitGround.AddListener(RespawnPiece);


        mainPool[currentPool].floxes[index].transform.position = baseInteractor.transform.position;
        mainPool[currentPool].isSelected[index] = true;
        for (int i = 0; i < mainPool[currentPool].isSelected.Count; i++)
        {
            if (!mainPool[currentPool].isSelected[i])
                return;
        }
        batches[currentPool].isEmpty = true;
        mainPool[currentPool].isEmpty = true;
    }

    public void GetPieceModifier(XRBaseInteractor baseInteractor, int index, bool isMalus, int indexInList)
    {
        if (isMalus)
        {
            XRBaseInteractable baseInteractableBonus = mainPool[currentPool].malus[indexInList].GetComponent<GrabbableObject>();

            var grabableBonus = mainPool[currentPool].malus[indexInList].GetComponent<GrabablePhysicsHandler>();
            grabableBonus.enabled = true;

            StartCoroutine(WaiToSelect(baseInteractableBonus, baseInteractor, grabableBonus, true));
            mainPool[currentPool].malus[indexInList].transform.position = baseInteractor.transform.position;

            grabableBonus.OnHitGround.AddListener(RespawnModifier);
        }
        else
        {
            XRBaseInteractable baseInteractableBonus = mainPool[currentPool].bonus[index - 5].GetComponent<GrabbableObject>();
            var grabableBonus = mainPool[currentPool].bonus[indexInList].GetComponent<GrabablePhysicsHandler>();
            grabableBonus.enabled = true;

            StartCoroutine(WaiToSelect(baseInteractableBonus, baseInteractor, grabableBonus, false));
            grabableBonus.OnHitGround.AddListener(RespawnModifier);
            mainPool[currentPool].bonus[indexInList].transform.position = baseInteractor.transform.position;
        }
        for (int i = 0; i < mainPool[currentPool].isSelectedModifier.Count; i++)
        {
            if (!mainPool[currentPool].isSelectedModifier[i])
                return;
        }
        mainPool[currentPool].isEmptyModifier = true;
    }

    protected IEnumerator WaiToSelect(XRBaseInteractable baseInteractable, XRBaseInteractor baseInteractor, GrabablePhysicsHandler grabable, bool isFireBall)
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        InteractionManager.instance.ForceSelect(baseInteractor, baseInteractable);
        grabable.timeToSlow = playGround.timeBeforFall;
        grabable.slowForce = playGround.slowForce;
        grabable.OnEnterStasis.Invoke(grabable.gameObject, true, grabable.m_rgb);
        yield return new WaitForSeconds(0.5f);
        if (isFireBall)
        {
            playGround.GetComponentInChildren<FireballManager>().canAct = true;
        }

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
        for (int i = 0; i < mainPool[currentPool].bonus.Count; i++)
        {
            if (mainPool[currentPool].bonus[i] == coll.gameObject && !coll.isGrab)
            {
                mainPool[currentPool].isSelectedModifier[mainPool[currentPool].bonusIndex[i]] = false;
                mainPool[currentPool].isEmptyModifier = false;
                representationsModifiers[mainPool[currentPool].bonusIndex[i]].gameObject.SetActive(true);
                coll.GetComponent<Rigidbody>().useGravity = false;
                coll.GetComponent<Rigidbody>().velocity = Vector3.zero;
                coll.transform.rotation = Quaternion.identity;
                coll.transform.position = new Vector3(300 + i * 5, 300 + i * 5, 300);
                return true;
            }
        }
        for (int i = 0; i < mainPool[currentPool].malus.Count; i++)
        {
            if (mainPool[currentPool].malus[i] == coll.gameObject && !coll.isGrab)
            {
                mainPool[currentPool].isSelectedModifier[mainPool[currentPool].malusIndex[i]] = false;
                mainPool[currentPool].isEmptyModifier = false;
                representationsModifiers[mainPool[currentPool].malusIndex[i]].gameObject.SetActive(true);
                coll.GetComponent<Rigidbody>().useGravity = false;
                coll.GetComponent<Rigidbody>().velocity = Vector3.zero;
                coll.transform.rotation = Quaternion.identity;
                coll.transform.position = new Vector3(300 + i * 5, 300 + i * 5, 300);
                return true;
            }
        }
        if (fireBallInstantiated == coll.gameObject && !coll.isGrab)
        {
            representations[representations.Length - 2].gameObject.SetActive(true);
            representations[representations.Length - 2].index = representations.Length - 1;
            representations[representations.Length - 2].manager = this;
            representations[representations.Length - 2].image.texture = fireBallInstantiated.GetComponent<TextureForDispenser>().texture;
            return true;
        }
        return false;
    }
    public virtual void OnGrabLeft()
    {
        isGrabLeft = true;
    }
    public virtual void OnRealeseLeft()
    {
        isGrabLeft = false;
    }
    public virtual void OnGrabRight()
    {
        isGrabRight = true;
    }
    public virtual void OnRealeseRight()
    {
        isGrabRight = false;
    }

    private void RespawnPiece(GameObject _object, Vector3 initPos, bool isGrab)
    {
        if (!isGrab)
        {
            if (!IsInCurrentPool(_object.GetComponent<GrabbableObject>()))
            {
                for (int i = 0; i < mainPool.Count; i++)
                {
                    if (i != currentPool)
                    {
                        for (int x = 0; x < mainPool[i].floxes.Count; x++)
                        {
                            if (mainPool[i].floxes[x] == _object)
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
    private void RespawnModifier(GameObject _object, Vector3 initPos, bool isGrab)
    {
        if (!isGrab)
        {
            if (!IsInCurrentPool(_object.GetComponent<GrabbableObject>()))
            {
                for (int i = 0; i < mainPool.Count; i++)
                {
                    if (i != currentPool)
                    {
                        if (mainPool[i].bonus != null)
                        {
                            for (int x = 0; x < mainPool[i].bonus.Count; x++)
                            {
                                if (mainPool[i].bonus[x] == _object)
                                {
                                    mainPool[i].isSelectedModifier[mainPool[i].bonusIndex[i]] = false;
                                    mainPool[i].isEmptyModifier = false;
                                    _object.GetComponent<Rigidbody>().useGravity = false;
                                    _object.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                    _object.transform.rotation = Quaternion.identity;
                                    _object.transform.position = new Vector3(300 + i * 5, 300 + i * 5, 300);
                                }
                            }
                        }
                        if (mainPool[i].malus != null)
                        {
                            for (int x = 0; x < mainPool[i].malus.Count; x++)
                            {
                                if (mainPool[i].malus[x] == _object)
                                {
                                    mainPool[i].isSelectedModifier[mainPool[i].malusIndex[i]] = false;
                                    mainPool[i].isEmptyModifier = false;
                                    _object.GetComponent<Rigidbody>().useGravity = false;
                                    _object.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                    _object.transform.rotation = Quaternion.identity;
                                    _object.transform.position = new Vector3(300 + i * 5, 300 + i * 5, 300);
                                }
                            }
                        }

                    }
                }
            }
        }
    }

    public virtual void DestroyFlock(GameObject flock, int indexOfPool, int indexOfFlock)
    {
        if(mainPool[indexOfPool].floxes[indexOfFlock] == flock)
        {
            Destroy(flock);
            GameObject _flock = Instantiate(batches[indexOfPool].pieces[indexOfFlock], new Vector3(300 + (indexOfFlock * 5 + 1) * 20 * (indexOfPool * 5 + 1), 300 + indexOfFlock * 20, 300 + indexOfFlock), Quaternion.identity);
            Modifier _modifer = baseModifier;
            Type type = _modifer.actions.GetType();
            var _object = GetComponent(type);
            _flock.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(_modifer, _object as ModifierAction, basicMats);
            _flock.GetComponent<GrabablePhysicsHandler>().enabled = false;
            _flock.GetComponent<GrabablePhysicsHandler>().inputManager = inputManager;

            _flock.GetComponent<Rigidbody>().useGravity = false;
            mainPool[indexOfPool].floxes[indexOfFlock] = _flock;
            mainPool[indexOfPool].isSelected[indexOfFlock] = false;
            mainPool[indexOfPool].isEmpty = false;
        }
        else if (mainPool[indexOfPool].bonus[indexOfFlock] == flock)
        {
            Destroy(flock);
            GameObject _flock = Instantiate(batches[indexOfPool].batchModifier.positiveModifiers[indexOfFlock], new Vector3(300 + (indexOfFlock * 5 + 1) * 20 * (indexOfPool * 5 + 1), 300 + indexOfFlock * 20, 300 + indexOfFlock), Quaternion.identity);
            Modifier _modifer = baseModifier;
            Type type = _modifer.actions.GetType();
            var _object = GetComponent(type);
            _flock.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(_modifer, _object as ModifierAction, basicMats);
            _flock.GetComponent<GrabablePhysicsHandler>().enabled = false;
            _flock.GetComponent<GrabablePhysicsHandler>().inputManager = inputManager;

            _flock.GetComponent<Rigidbody>().useGravity = false;
            mainPool[indexOfPool].bonus[indexOfFlock] = _flock;
            mainPool[indexOfPool].isEmptyModifier = false;
        }
        else if (mainPool[indexOfPool].malus[indexOfFlock] == flock)
        {
            Destroy(flock);
            GameObject _flock = Instantiate(batches[indexOfPool].batchModifier.negativeModifier[indexOfFlock], new Vector3(300 + (indexOfFlock * 5 + 1) * 20 * (indexOfPool * 5 + 1), 300 + indexOfFlock * 20, 300 + indexOfFlock), Quaternion.identity);
            Modifier _modifer = baseModifier;
            Type type = _modifer.actions.GetType();
            var _object = GetComponent(type);
            _flock.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(_modifer, _object as ModifierAction, basicMats);
            _flock.GetComponent<GrabablePhysicsHandler>().enabled = false;
            _flock.GetComponent<GrabablePhysicsHandler>().inputManager = inputManager;

            _flock.GetComponent<Rigidbody>().useGravity = false;
            mainPool[indexOfPool].malus[indexOfFlock] = _flock;
            mainPool[indexOfPool].isEmptyModifier = false;
        }
    }

}

public class pool
{
    public List<GameObject> floxes;
    public List<bool> isSelected;
    public List<bool> isSelectedModifier;
    public List<GameObject> bonus;
    public List<GameObject> malus;
    public List<int> bonusIndex;
    public List<int> malusIndex;
    public int numberOfModifiersActivated;
    public bool isEmpty;
    public bool isEmptyModifier;
    public bool isFireballUsed;
}
