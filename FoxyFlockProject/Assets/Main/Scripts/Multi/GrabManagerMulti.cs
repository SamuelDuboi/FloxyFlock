using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class GrabManagerMulti : GrabManager
{
    private GameModeSolo gameModeSolo;
    public int numberOfPool;
 [SerializeField]   public GameObject fireBallPrefab;
 [SerializeField]   public GameObject fireBallPrefabOut;
    private PlayerMovementMulti playerMovement;
  
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
        inputManager.OnGrabbingLeft.AddListener(OnGrabLeft);
        inputManager.OnGrabbingReleaseLeft.AddListener(OnRealeseLeft);
        inputManager.OnGrabbingRight.AddListener(OnGrabRight);
        inputManager.OnGrabbingReleaseRight.AddListener(OnRealeseRight);
    }
    public virtual void InitPool(GameObject authority, PlayerMovementMulti player)
    {
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
                Modifier _modifier = baseModifier;
                Type type = _modifier.actions.GetType();
                var _object = GetComponent(type);
                player.InitBacth(authority, i, x, batches, _modifier, _object,basicMats,mainPool, out mainPool);
            }

            for (int x = 0; x < batches[i].batchModifier.negativeModifier.Count; x++)
            {
                Modifier _modifierPiece = negativeModifiers[UnityEngine.Random.Range(0,negativeModifiers.Count)];
                Type typePiece = _modifierPiece.actions.GetType();
                var _objectPiece = GetComponent(typePiece);
                player.InitModifier(authority, i,_modifierPiece, batches[i].batchModifier.negativeModifier[x], _objectPiece, basicMats,false, mainPool, out mainPool);
            }
            for (int x = 0; x < batches[i].batchModifier.positiveModifiers.Count; x++)
            {
                Modifier _modifierPiece = positiveModifiers[UnityEngine.Random.Range(0, positiveModifiers.Count)];
                Type typePiece = _modifierPiece.actions.GetType();
                var _objectPiece = GetComponent(typePiece);
                player.InitModifier(authority, i, _modifierPiece, batches[i].batchModifier.positiveModifiers[x], _objectPiece, basicMats,true, mainPool, out mainPool);
            }

        }
        player.InitFireBall(authority, fireBallPrefab, fireBallPrefabOut);
        numberOfPool = 1;
        for (int i = 0; i < positiveModifiers.Count; i++)
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
        }
        inputManager.OnSpawn.AddListener(UpdateBatche);
    }

    protected override void UpdateMilestone()
    {
        currentMilestone = playGround.CheckMilestones(out positionOfMilestoneIntersection, out numberOfMilestones);
       playerMovement.CmdChangeMilestoneValue(gameModeSolo.number, currentMilestone);
    }
    protected override IEnumerator WaiToDestroyBuble(int index, bool isBad)
    {
        if (isBad)
            malusNumber[index].GetComponent<SoundReader>().Play();
        else
            bonusNumber[index].GetComponent<SoundReader>().Play();

        yield return new WaitForSeconds(0.2f);

        if (isBad)
           playerMovement.CmdDestroyBubble(malusNumber[index]);
        else
            playerMovement.CmdDestroyBubble(bonusNumber[index]);
    }
}
