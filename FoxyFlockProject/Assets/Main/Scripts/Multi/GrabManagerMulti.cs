using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;
using Mirror.Experimental;

public class GrabManagerMulti : GrabManager
{
    private GameModeSolo gameModeSolo;
    public int numberOfPool;
    [SerializeField] public GameObject fireBallPrefab;
    [SerializeField] public GameObject fireBallPrefabOut;
    private PlayerMovementMulti playerMovement;
    private ResetMulti resetMulti;
    public Representation fireballRepresentation;
    public bool nextIsFireBallBatche;
    int playerNumber;
    // Start is called before the first frame update
    public override IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

        sound = GetComponent<SoundReader>();
        for (int i = 0; i < grabableObjects.Count; i++)
        {
            Modifier _modifer = positiveModifiers[UnityEngine.Random.Range(0, positiveModifiers.Count)];
            Type type = _modifer.actions.GetType();
            var _object = GetComponent(type);
            grabableObjects[i].ChangeBehavior(_modifer, _object as ModifierAction, basicMats);
        }

        inputManager = GetComponentInParent<InputManager>();
        inputManager.OnSpawn.AddListener(UpdateBatche);
        playGround = inputManager.GetComponent<PlayerMovementMulti>().tableTransform.GetComponent<PlayGround>();
        gameModeSolo = playGround.GetComponentInChildren<GameModeSolo>();
        playerMovement = inputManager.GetComponent<PlayerMovementMulti>();
        resetMulti = inputManager.GetComponent<ResetMulti>();
        inputManager.OnGrabbingLeft.AddListener(OnGrabLeft);
        inputManager.OnGrabbingReleaseLeft.AddListener(OnRealeseLeft);
        inputManager.OnGrabbingRight.AddListener(OnGrabRight);
        inputManager.OnGrabbingReleaseRight.AddListener(OnRealeseRight);
        currentMilestone = playGround.CheckMilestones(out positionOfMilestoneIntersection, out numberOfMilestones, out nextMilestonePos);
        playerMovement.CmdMoveBubble(playGround.radius, nextMilestonePos.y,positionOfMilestoneIntersection, playGround.bonusOrbes, playGround.malusOrbes,playGround.fireBallOrbe);
        Vector3 headSettPos = inputManager.GetComponent<XRRig>().cameraFloorOffsetObject.transform.localPosition;
        transform.localPosition += headSettPos;
    }
    public virtual void InitPool(GameObject authority, PlayerMovementMulti player, int v)
    {
        playerNumber = v;
        if (ScenesManager.instance.IsLobbyScene() || ScenesManager.instance.IsMenuScene())
            return;
        mainPool = new List<pool>();
        ScenesManager.instance.numberOfFlocksInScene = 0;
        for (int i = 0; i < batches.Count; i++)
        {
            mainPool.Add(new pool());
            mainPool[i].floxes = new List<GameObject>();
            mainPool[i].isSelected = new List<bool>();

            for (int x = 0; x < batches[i].pieces.Count; x++)
            {
                // temp solution for attribution
                int random = UnityEngine.Random.Range(0, negativeModifiers.Count + positiveModifiers.Count + weightOfBasicInRandom);
                Modifier _modifier = baseModifier;
                if (random > 15 && random <= negativeModifiers.Count + weightOfBasicInRandom)
                    _modifier = negativeModifiers[random - weightOfBasicInRandom-1];
                else if (random > negativeModifiers.Count + weightOfBasicInRandom)
                    _modifier = positiveModifiers[random - weightOfBasicInRandom-1 - negativeModifiers.Count];
                Type type = _modifier.actions.GetType();
                var _object = GetComponent(type);
                player.InitBacth(authority,v, i, x, batches, _modifier, _object, basicMats, mainPool, out mainPool);
            }

            for (int x = 0; x < batches[i].batchModifier.negativeModifier.Count; x++)
            {
                Modifier _modifierPiece = negativeModifiers[UnityEngine.Random.Range(0, negativeModifiers.Count)];
                Type typePiece = _modifierPiece.actions.GetType();
                var _objectPiece = GetComponent(typePiece);
                player.InitModifier(authority,v, i,x, _modifierPiece, batches[i].batchModifier.negativeModifier[x], _objectPiece, basicMats, false, mainPool, out mainPool);
            }
            for (int x = 0; x < batches[i].batchModifier.positiveModifiers.Count; x++)
            {
                Modifier _modifierPiece = positiveModifiers[UnityEngine.Random.Range(0, positiveModifiers.Count)];
                Type typePiece = _modifierPiece.actions.GetType();
                var _objectPiece = GetComponent(typePiece);
                player.InitModifier(authority,v, i,x+2, _modifierPiece, batches[i].batchModifier.positiveModifiers[x], _objectPiece, basicMats, true, mainPool, out mainPool);
            }
            mainPool[i].isEmptyModifier = true;

        }
        player.InitFireBall(authority, fireBallPrefab, fireBallPrefabOut);
        numberOfPool = 1;
/*        for (int i = 0; i < positiveModifiers.Count; i++)
        {
            Type type = positiveModifiers[i].actions.GetType();
            var _object = GetComponent(type);
            if (_object)
            {
                Destroy(_object);
            }
        }
        for (int i = 0; i < negativeModifiers.Count; i++)
        {
            Type type = negativeModifiers[i].actions.GetType();
            var _object = GetComponent(type);
            if (_object)
            {
                Destroy(_object);
            }
        }*/
        inputManager.OnSpawn.AddListener(UpdateBatche);

    }

    protected override void UpdateMilestone()
    {
        nextMilestonePos = Vector3.zero;
        currentMilestone = playGround.CheckMilestones(out positionOfMilestoneIntersection, out numberOfMilestones, out nextMilestonePos);
        playerMovement.CmdChangeMilestoneValue(gameModeSolo.number, currentMilestone);
    }
    protected override void UpdateSpecial()
    {
        base.UpdateSpecial();
        if (fireBallNumber != null && fireBallNumber.Count > 0)
        {
            if(nextIsFireBallBatche)
                AllowFireBall();
            for (int i = 0; i < fireBallNumber.Count; i++)
            {
                if (fireBallNumber[i] != null)
                {
                    fireBallNumber[i].GetComponent<SoundReader>().Play();
                }
            }

        }
        nextIsFireBallBatche = !nextIsFireBallBatche;
    }
    public void GetPieceFireball(XRBaseInteractor baseInteractor)
    {
        if (baseInteractor.GetComponent<HandController>().controllerNode == UnityEngine.XR.XRNode.RightHand && !isGrabRight)
            return;
        if (baseInteractor.GetComponent<HandController>().controllerNode == UnityEngine.XR.XRNode.LeftHand && !isGrabLeft)
            return;
        fireballRepresentation.gameObject.SetActive(false);

        mainPool[currentPool].isFireballUsed = true;
        XRBaseInteractable baseInteractableBonus = fireBallInstantiated.GetComponent<GrabbableObject>();

        var grabableBonus = fireBallInstantiated.GetComponent<GrabablePhysicsHandler>();
        grabableBonus.enabled = true;

        StartCoroutine(WaiToSelect(baseInteractableBonus, baseInteractor, grabableBonus, true));
        fireBallInstantiated.transform.position = baseInteractor.transform.position;

        grabableBonus.OnHitGround.AddListener(RespawnFireball);
        return;

    }
    private void AllowFireBall()
    {
        if (!mainPool[currentPool].isFireballUsed)
        {
            fireballRepresentation.gameObject.SetActive(true);
            fireballRepresentation.manager = this;
            fireballRepresentation.image.texture = fireBallInstantiated.GetComponent<TextureForDispenser>().texture;
        }

    }
    protected override void FreezOfList(List<GameObject> flocksToFreez, int indeOfPool)
    {
        if (flocksToFreez == null)
            return;
        for (int i = 0; i < flocksToFreez.Count; i++)
        {
            resetMulti.AddFreezFlock(flocksToFreez[i],indeOfPool,i);
        }
    }
    protected override void UpdateBubble()
    {
        if (!doOnce)
        {
            doOnce = true;
            return;
        }
        foreach (GameObject orb in playGround.bonusOrbes)
        {
          playerMovement.CmdMoveBubble( playGround.radius, nextMilestonePos.y,positionOfMilestoneIntersection, playGround.bonusOrbes, playGround.malusOrbes, playGround.fireBallOrbe);
        }
        foreach (GameObject orb in playGround.malusOrbes)
        {
            playerMovement.CmdMoveBubble( playGround.radius, nextMilestonePos.y,positionOfMilestoneIntersection, playGround.bonusOrbes, playGround.malusOrbes, playGround.fireBallOrbe);
        }
        playerMovement.CmdMoveBubble( playGround.radius, nextMilestonePos.y,positionOfMilestoneIntersection, playGround.bonusOrbes, playGround.malusOrbes, playGround.fireBallOrbe); 
    }
    private void RespawnFireball(GameObject _object, Vector3 initPos, bool isGrab)
    {
        if (!isGrab)
        {

            _object.GetComponent<Rigidbody>().useGravity = false;
            _object.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _object.transform.rotation = Quaternion.identity;
            _object.transform.position = new Vector3(300 + 5 * 5, 300 + 5 * 5, 300);
            var grabableBonus = fireBallInstantiated.GetComponent<GrabablePhysicsHandler>();
            grabableBonus.OnHitGround.RemoveListener(RespawnFireball);
            grabableBonus.enabled = false;
            mainPool[currentPool].isFireballUsed = false;
            StartCoroutine(playGround.GetComponentInChildren<FireballManager>().TryClosePortal());
            AllowFireBall();

        }
    }

    public override void DestroyFlock(GameObject flock, int indexOfPool)
    {
        if (mainPool[indexOfPool].floxes.Contains(flock))
        {
            int indexOfFlock = mainPool[indexOfPool].floxes.IndexOf(flock);
            StartCoroutine(  flock.GetComponent<DissolveFlox>().StartDissolve(default, Vector3.zero, true, null, false, this));

            Modifier _modifier = baseModifier;
            Type type = _modifier.actions.GetType();
            var _object = GetComponent(type);

            playerMovement.InitBacth(playerMovement.gameObject, playerNumber, indexOfPool, indexOfFlock, batches, _modifier, _object, basicMats, mainPool, out mainPool);
        }

        else if (mainPool[indexOfPool].bonus.Contains(flock))
        {
            int indexOfFlock = mainPool[indexOfPool].bonus.IndexOf(flock);
            StartCoroutine(flock.GetComponent<DissolveFlox>().StartDissolve(default, Vector3.zero, true,null,false,this));

            Modifier _modifierPiece = negativeModifiers[UnityEngine.Random.Range(0, negativeModifiers.Count)];
            Type typePiece = _modifierPiece.actions.GetType();
            var _objectPiece = GetComponent(typePiece);
            mainPool[indexOfPool].malus.RemoveAt(indexOfFlock);

            playerMovement.InitModifier(playerMovement.gameObject, playerNumber, indexOfPool,indexOfFlock, _modifierPiece, batches[indexOfPool].batchModifier.negativeModifier[indexOfFlock], _objectPiece, basicMats, false, mainPool, out mainPool);

        }

        else if (mainPool[indexOfPool].malus.Contains(flock))
        {
            int indexOfFlock = mainPool[indexOfPool].malus.IndexOf(flock);
            StartCoroutine(flock.GetComponent<DissolveFlox>().StartDissolve(default, Vector3.zero, true, null, false, this));
            Modifier _modifierPiece = positiveModifiers[UnityEngine.Random.Range(0, positiveModifiers.Count)];
            Type typePiece = _modifierPiece.actions.GetType();
            var _objectPiece = GetComponent(typePiece);
            mainPool[indexOfPool].bonus.RemoveAt(indexOfFlock);
            playerMovement.InitModifier(playerMovement.gameObject, playerNumber,indexOfPool, indexOfFlock+2, _modifierPiece, batches[indexOfPool].batchModifier.positiveModifiers[indexOfFlock], _objectPiece, basicMats, true, mainPool, out mainPool);

        }
    }
    public void Destroy(GameObject flock)
    {
        resetMulti.GetComponent<ResetMulti>().StopAllCoroutines();
        resetMulti.CmdDestroy(flock);
    }
}
