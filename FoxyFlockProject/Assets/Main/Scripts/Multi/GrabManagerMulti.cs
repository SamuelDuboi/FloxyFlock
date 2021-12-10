using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class GrabManagerMulti : GrabManager
{
    public int numberOfPool;
 [SerializeField]   public GameObject fireBallPrefab;
 [SerializeField]   public GameObject fireBallPrefabOut;
    // Start is called before the first frame update
    public override void Start()
    {
       for (int i = 0; i < grabableObjects.Count; i++)
        {
            Modifier _modifer = modifiers[UnityEngine.Random.Range(0, modifiers.Count)];
            Type type = _modifer.actions.GetType();
            var _object = GetComponent(type);
            grabableObjects[i].ChangeBehavior(_modifer, _object as ModifierAction, basicMats);
        }

        inputManager = GetComponentInParent<InputManager>();
        inputManager.OnSpawn.AddListener(SpawnBacth);
        playGround = inputManager.GetComponent<PlayerMovementMulti>().tableTransform.GetComponent<PlayGround>();
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
                Modifier _modifier = modifiers[UnityEngine.Random.Range(0, modifiers.Count)];
                Type type = _modifier.actions.GetType();
                var _object = GetComponent(type);
                player.InitBacth(authority, i, x, batches, _modifier, _object,basicMats,mainPool, out mainPool);
            }
            Modifier _modifierPiece = batches[i].positiveModifier.modifier;
            Type typePiece = _modifierPiece.actions.GetType();
            var _objectPiece = GetComponent(typePiece);
            player.InitModifier(authority, i, batches[i].positiveModifier, _objectPiece, basicMats, mainPool, out mainPool);
        }
        player.InitFireBall(authority, fireBallPrefab, fireBallPrefabOut);
        numberOfPool = 1;
        for (int i = 0; i < modifiers.Count; i++)
        {
            Type type = modifiers[i].actions.GetType();
            var _object = GetComponent(type);
            if (_object)
            {
                Destroy(_object);
            }
        }
        inputManager.OnSpawn.AddListener(SpawnBacth);
    }
    

}
