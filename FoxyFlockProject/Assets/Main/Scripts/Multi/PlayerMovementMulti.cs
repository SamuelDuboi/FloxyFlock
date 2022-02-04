using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
public class PlayerMovementMulti : NetworkBehaviour
{
    public InputManager inputManager;
    public Transform vrHeadSett;
    public Renderer tableRenderer;
    public Transform tableTransform;
    [HideInInspector] public Transform zClampMin;
    [HideInInspector] public Transform zClampMax;
    [HideInInspector] public Transform upClamp;
    [HideInInspector] public Transform downClamp;
    private Vector3 movementMidle;
    private Vector3 deltaPose;
    private Vector3 initMidle;
    private CharacterStats characterStats;
    private Vector3 forwardOffset;
    private Vector3 rotateOffset;
    private float upOffset;
    private float yPower;
    private float xPower;
    private float zPower;
    public Transform tempLeftHand;
    public Transform tempRighttHand;
    public GameObject grabManager;
    [HideInInspector] public List<Batch>batches;
    private GameObject tempFlock;
    private GameObject tempFlock2;
    bool doOnce;
    public MoveBubble moveBubble;
    public int intOfPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (!isServer)
        {
            tableTransform = GameObject.FindGameObjectWithTag("Table2").transform;
            tableTransform.GetComponentInChildren<GameModeSolo>().number = 1;
        }
        else
        {
            tableTransform = GameObject.FindGameObjectWithTag("Table").transform;
            tableTransform.GetComponentInChildren<GameModeSolo>().number = 0;
        }
        if (isLocalPlayer)
        {
            tableTransform.GetComponentInChildren<GameModeSolo>().hands = GetComponentInChildren<HandsPlayground>();
            tableTransform.GetComponentInChildren<FireballManager>().rig = transform;

            tableTransform.GetComponentInChildren<GameModeSolo>().playerMovement = this;
        }
        
        tableRenderer = tableTransform.GetComponentInChildren<Renderer>();
        TableGetClamp temp = tableTransform.GetComponent<TableGetClamp>();
        zClampMin = temp.zClampMin;
        zClampMax = temp.zClampMax;
        upClamp = temp.upClamp;
        downClamp = temp.downClamp;


        if (!ScenesManagement.instance.IsLobbyScene() && !ScenesManagement.instance.IsMenuScene())
        {
            inputManager.OnCanMove.AddListener(Movement);
            inputManager.OnBothTrigger.AddListener(InitMovement);
        }
        characterStats = inputManager.characterStats;
        forwardOffset = new Vector3(0, characterStats.forwardYOffset, characterStats.forwardZOffset);
        rotateOffset = new Vector3(characterStats.RotateXOffset, characterStats.RotateYOffset, 0);
        upOffset = characterStats.upOffset;
        yPower = characterStats.yPower;
        xPower = characterStats.xPower;
        zPower = characterStats.zPower;
        //grabManager.transform.position = vrHeadSett.position;
        //CmdSpawnManager();
    }
   /* [Command]
    public void CmdSpawnManager(GameObject parent)
    {
        grabManager = Instantiate(grabManagerPrefab, grabManagerPrefab.transform.position,grabManagerPrefab.transform.rotation);
        grabManager.GetComponent<GrabManagerMulti>().inputManager = inputManager;
        grabManager.GetComponent<GrabManagerMulti>().playGround = tableTransform.GetComponent<PlayGround>();
        grabManager.transform.SetParent(parent.transform);
        grabManager.transform.localPosition = grabManagerPrefab.transform.position;
        NetworkServer.Spawn(grabManager, parent);
    }*/

    private void InitMovement(bool seeTable = false)
    {
        tempLeftHand.position = inputManager.leftHand.transform.position;
        tempRighttHand.position = inputManager.rightHand.transform.position;

        initMidle = Vector3.Lerp(tempLeftHand.localPosition, tempRighttHand.localPosition, 0.5f);
    }
    public bool SeeTable()
    {
        Vector3 pointOnScreen = vrHeadSett.GetComponent<Camera>().WorldToScreenPoint(tableRenderer.bounds.center);
        if ((pointOnScreen.x < 0) || (pointOnScreen.x > Screen.width) ||
            (pointOnScreen.y < 0) || (pointOnScreen.y > Screen.height))
        {
            InitMovement();
            return false;
        }
        if (pointOnScreen.z < 0)
        {
            InitMovement();
            return false;
        }
        return true;
    }
    private void Movement()
    {
        //check if table is on screen
        SeeTable();

        tempLeftHand.position = inputManager.leftHand.transform.position;
        tempRighttHand.position = inputManager.rightHand.transform.position;
        movementMidle = Vector3.Lerp(tempRighttHand.localPosition, tempLeftHand.localPosition, 0.5f);
        //get delat pos 
        deltaPose = new Vector3((movementMidle.x - initMidle.x), (movementMidle.y - initMidle.y), (movementMidle.z - initMidle.z));
        #region testWithSnapTurn
        /* movementMidle = Vector3.Lerp(tempLeftHand.position  , tempLeftHand.position, 0.5f);
         deltaPose = new Vector3((movementMidle.x - initMidle.x), (movementMidle.y - initMidle.y), (movementMidle.z - initMidle.z));

         var zAngle = Vector3.Dot(vrHeadSett.forward, deltaPose);

         var xAngle = Vector3.Dot(vrHeadSett.right, deltaPose);

         if (Mathf.Abs(xAngle) > 1f || Mathf.Abs(zAngle) > 1)
             return;
           //if the amplutde of the movemnt is mor forward then side
         if (Mathf.Abs(zAngle) > Mathf.Abs(xAngle))
         {

             // if the movemnt is enough to move
             if (forwardOffset.z < Mathf.Abs(zAngle))
             {
                 //if is to close and want to come closer, return
                 if (Vector2.Distance(ToVector2XZ(transform.position), ToVector2XZ(tableTransform.position)) < Vector2.Distance(ToVector2XZ(zClampMin.position), ToVector2XZ(tableTransform.position)) && zAngle*characterStats.zPower*-1 < 0)
                 {
                     return;
                 }
                 //if is to far and want to go farer (is that english ?)
                 else if (Vector2.Distance(ToVector2XZ(transform.position), ToVector2XZ(tableTransform.position)) > Vector2.Distance(ToVector2XZ(zClampMax.position), ToVector2XZ(tableTransform.position)) && zAngle * characterStats.zPower * -1 > 0)
                     return;
                 //move the pos to the table
                 transform.position += new Vector3(tableTransform.position.x - vrHeadSett.transform.position.x, 0, tableTransform.position.z - vrHeadSett.transform.position.z).normalized * zAngle * 50*zPower*Time.deltaTime;
                 // the movement is enough to move forward and/or move upward
                 //MoveUp(forwardOffset);
                 Debug.Log(zAngle);
             }
             //else if can move up
             else
             {
                 MoveUp(upOffset);

             }

         }
         else if (Mathf.Abs(zAngle) < Mathf.Abs(xAngle))
         {
             // if the movemnt is enough to rotate
             if (rotateOffset.x < Mathf.Abs(xAngle))
             {
                 //move the pos to the table
                 transform.RotateAround(tableTransform.position, Vector3.up, xAngle * 180* xPower *Time.deltaTime);
                 // the movement is enough to move forward and/or move upward
                 MoveUp(rotateOffset);
             }
             //else if can move up
             else
             {
                 MoveUp(upOffset);
             }
         }
         //in the weird case that the player move the same amount in x and in z
         //if move enough in y axis
         else
         {
             MoveUp(upOffset);
         }*/
        #endregion

        //if the amplutde of the movemnt is mor forward then side
        if (Mathf.Abs(deltaPose.x) < Mathf.Abs(deltaPose.z))
        {

            // if the movemnt is enough to move
            if (forwardOffset.z < Mathf.Abs(deltaPose.z))
            {
                //if is to close and want to come closer, return
                if (Vector2.Distance(ToVector2XZ(transform.position), ToVector2XZ(tableTransform.position)) < Vector2.Distance(ToVector2XZ(zClampMin.position), ToVector2XZ(tableTransform.position)) && deltaPose.z * characterStats.zPower * -1 < 0)
                {
                    return;
                }
                //if is to far and want to go farer (is that english ?)
                else if (Vector2.Distance(ToVector2XZ(transform.position), ToVector2XZ(tableTransform.position)) > Vector2.Distance(ToVector2XZ(zClampMax.position), ToVector2XZ(tableTransform.position)) && deltaPose.z * characterStats.zPower * -1 > 0)
                    return;
                //move the pos to the table
                transform.position += new Vector3(tableTransform.position.x - vrHeadSett.transform.position.x, 0, tableTransform.position.z - vrHeadSett.transform.position.z).normalized * deltaPose.z * zPower;
                // the movement is enough to move forward and/or move upward
                MoveUp(forwardOffset);
            }
            //else if can move up
            else
            {
                MoveUp(upOffset);

            }

        }
        else if (Mathf.Abs(deltaPose.x) > Mathf.Abs(deltaPose.z))
        {
            // if the movemnt is enough to rotate
            if (rotateOffset.x < Mathf.Abs(deltaPose.x))
            {
                //move the pos to the table
                transform.RotateAround(tableTransform.position, Vector3.up, deltaPose.x * 180 * xPower);
                // the movement is enough to move forward and/or move upward
                MoveUp(rotateOffset);
            }
            //else if can move up
            else
            {
                MoveUp(upOffset);
            }
        }
        //in the weird case that the player move the same amount in x and in z
        //if move enough in y axis
        else
        {
            MoveUp(upOffset);
        }

        initMidle = movementMidle;
    }
    private void MoveUp(Vector3 offset)
    {

        if (vrHeadSett.position.y - upClamp.position.y > 0 && deltaPose.y < 0)
            return;

        else if (vrHeadSett.position.y - downClamp.position.y < 0 && deltaPose.y > 0)
            return;

        transform.position += Vector3.up * (deltaPose.y) * yPower;
    }
    private void MoveUp(float offset)
    {

        if (vrHeadSett.position.y - upClamp.position.y > 0 && deltaPose.y < 0)
            return;
        else if (vrHeadSett.position.y - downClamp.position.y < 0 && deltaPose.y > 0)
            return;
        transform.position += Vector3.up * (deltaPose.y) * yPower;
    }
    private Vector2 ToVector2XZ(Vector3 a)
    {
        return new Vector2(a.x, a.z);
    }
    Modifier tempModifier;
    Component tempComponent;
    private Type _tempComponent;
    PhysicMaterial[] tempbasicMats;
    public void InitBacth(GameObject authority,int v, int i, int x, List<Batch> _batches, Modifier _modifier, Component _object, PhysicMaterial[] basicMats, List<pool> _mainPool1,  out List<pool> _mainPool)
    {
        batches = _batches;
      
        doOnce = !doOnce;
        GameObject flock = Instantiate(batches[i].pieces[x], new Vector3(300 + (x+v*6) * 20 * + i * 5, 300 + (x+v*6) * 20  + i * 5, 300 + (x+v*6) * 20  + i * 5), Quaternion.identity);
        tempModifier = _modifier;
        tempbasicMats = basicMats;
        flock.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(_modifier, _object as ModifierAction, basicMats);

        flock.GetComponent<GrabablePhysicsHandler>().inputManager = inputManager;
        flock.GetComponent<GrabablePhysicsHandler>().initPos = flock.transform.position;
        flock.GetComponent<GrabablePhysicsHandler>().m_rgb.velocity = Vector3.zero;
        v++;
        _mainPool1[i].floxes.Add(flock);
        _mainPool1[i].isSelected.Add(false);
        
        _mainPool1[i].isEmpty = false;
        ScenesManagement.instance.numberOfFlocksInScene++;
        _mainPool = _mainPool1;
        tempFlock = flock;
        int index = 0;
        bool hasFounded = false;
        for (int w = 0; w < components.Count; w++)
        {
            if (components[w].GetType() == _object.GetType())
            {
                index = w;
                hasFounded = true;
            }
        }
        if (!hasFounded)
        {
            components.Add(_object);
            index = components.Count - 1;
        }
        CmdSpawnPiece(authority,_mainPool1, _object.GetType().ToString(), tempbasicMats,index,false);
        
    }
    private List<Component> components = new List<Component>();
    public void InitModifier(GameObject authority,int v, int i,int x,Modifier modifier, GameObject piece , Component _object, PhysicMaterial[] basicMats,bool isBonus, List<pool> _mainPool1, out List<pool> _mainPool)
    {
   
        doOnce = !doOnce;
        GameObject flock = Instantiate(piece, new Vector3(-300 + (x + v*6) * 20 * +i * 5, 300 + (x + v*6) * 20 + i * 5, 300 + (x + v*6) * 20 + i * 5), Quaternion.identity);
        tempModifier = modifier;
        int index = 0;
        bool hasFounded = false;
        for (int w = 0;w < components.Count; w++)
        {
            if (components[w].GetType() == _object.GetType())
            {
                 index = w;
                hasFounded = true;
            }
        }
        
        if(!hasFounded)
        {
            components.Add(_object);
            index = components.Count -1;
        }
        _tempComponent = _object.GetType();
        tempbasicMats = basicMats;
       
        flock.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(tempModifier, _object as ModifierAction, basicMats);
        flock.GetComponent<GrabablePhysicsHandler>().inputManager = inputManager;
        flock.GetComponent<GrabablePhysicsHandler>().initPos = flock.transform.position;
        flock.GetComponent<GrabablePhysicsHandler>().m_rgb.velocity = Vector3.zero;
        if (isBonus)
        {
            if (_mainPool1[i].bonus == null)
                _mainPool1[i].bonus = new List<GameObject>();
            _mainPool1[i].bonus.Add(flock);

        }
        else
        {
            if (_mainPool1[i].malus == null)
                _mainPool1[i].malus = new List<GameObject>();
            _mainPool1[i].malus.Add(flock);
        }
        _mainPool1[i].isEmptyModifier = true;

        _mainPool = _mainPool1;
        tempFlock = flock;
        CmdSpawnPiece(authority, _mainPool1, _tempComponent.ToString(), tempbasicMats, index, true);
        
    }
    public void InitFireBall(GameObject authority, GameObject _fireBall, GameObject outFireBall)
    {
        
        int rand = UnityEngine.Random.Range(0, 100);
        GameObject fireBall = Instantiate(_fireBall, new Vector3(300 + 20 * rand, 300 + 20 * rand, 300 + 20 * rand), Quaternion.identity);
        tempFlock = fireBall;
        if (authority.name== "player 0")
        {
            CmdSpawnObject(authority, tempFlock,0);
            tableTransform.GetComponentInChildren<FireballManager>().inFireball = fireBall;
            tableTransform.GetComponentInChildren<FireballManager>().grabManager = grabManager.GetComponent<GrabManager>();
            tableTransform.GetComponentInChildren<FireballManager>().detectionHUD = vrHeadSett.GetComponentInChildren<DetectionHUD>();
            fireBall.GetComponent<Rigidbody>().useGravity = false;
            fireBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else
        {
            CmdSpawnObjectSecondFireBall(authority, fireBall, 0);
        }
        GameObject fireBall2 = Instantiate(outFireBall, new Vector3(300 + 20 * rand, 300 + 20 * rand, 300 + 20 * rand), Quaternion.identity);
        tempFlock2 = fireBall2;
        if (authority.name == "player 0")
        {
            CmdSpawnObject(authority, tempFlock, 1);
            fireBall2.GetComponent<GrabablePhysicsHandler>().m_rgb.useGravity = false;
            fireBall2.GetComponent<GrabablePhysicsHandler>().m_rgb.velocity = Vector3.zero;
            fireBall2.GetComponent<GrabablePhysicsHandler>().inputManager = inputManager;
            fireBall2.GetComponent<GrabablePhysicsHandler>().enabled = false;
            tableTransform.GetComponentInChildren<FireballManager>().outFireball = fireBall2;
            tableTransform.GetComponentInChildren<FireballManager>().grabManager = grabManager.GetComponent<GrabManager>();
            tableTransform.GetComponentInChildren<FireballManager>().detectionHUD = vrHeadSett.GetComponentInChildren<DetectionHUD>();
            grabManager.GetComponent<GrabManagerMulti>().fireBallInstantiated = fireBall2;
            tableTransform.GetComponentInChildren<FireballManager>().Initialize();
        }
        else
        {
            CmdSpawnObjectSecondFireBall(authority, fireBall, 1);
        }
    }
    [Command]
    private void CmdSpawnObject(GameObject authority,GameObject fireBall, int i)
    {
        if (i == 0)
        {
            NetworkServer.Spawn(tempFlock, authority);
        }
        else
        {
            tempFlock2.GetComponent<GrabablePhysicsHandler>().m_rgb.useGravity = false;
            tempFlock2.GetComponent<GrabablePhysicsHandler>().m_rgb.velocity = Vector3.zero;
            //tempFlock.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(tempModifier, tempComponent as ModifierAction, tempbasicMats);
            tempFlock2.GetComponent<GrabablePhysicsHandler>().enabled = false;
            NetworkServer.Spawn(tempFlock2, authority);
        }
    }

    [Command]
    private void CmdSpawnObjectSecondFireBall(GameObject authority, GameObject fireBall, int i)
    {
        if (i == 0)
        {
            NetworkServer.Spawn(tempFlock, authority);
            NetworkIdentity opponentIdentity = authority.GetComponent<NetworkIdentity>();
            TargetGetFireBallSpawn(opponentIdentity.connectionToClient, tempFlock, authority, i);

        }
        else
        {
            tempFlock2.GetComponent<GrabablePhysicsHandler>().m_rgb.useGravity = false;
            tempFlock2.GetComponent<GrabablePhysicsHandler>().m_rgb.velocity = Vector3.zero;
            //tempFlock.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(tempModifier, tempComponent as ModifierAction, tempbasicMats);
            tempFlock2.GetComponent<GrabablePhysicsHandler>().enabled = false;
            NetworkServer.Spawn(tempFlock2, authority);
            NetworkIdentity opponentIdentity = authority.GetComponent<NetworkIdentity>();
            TargetGetFireBallSpawn(opponentIdentity.connectionToClient, tempFlock2, authority, i);
        }
    }
    [TargetRpc]
    private void TargetGetFireBallSpawn(NetworkConnection target,GameObject fireBall,GameObject authority , int i)
    {
        authority.GetComponentInChildren<GrabManager>().fireBallInstantiated = fireBall;
        if (i == 0)
        {
            authority.GetComponent<PlayerMovementMulti>().tableTransform.GetComponentInChildren<FireballManager>().inFireball = fireBall;
            authority.GetComponent<PlayerMovementMulti>().tableTransform.GetComponentInChildren<FireballManager>().grabManager = grabManager.GetComponent<GrabManager>();
            authority.GetComponent<PlayerMovementMulti>().tableTransform.GetComponentInChildren<FireballManager>().detectionHUD = vrHeadSett.GetComponentInChildren<DetectionHUD>();
            fireBall.GetComponent<Rigidbody>().useGravity = false;
            fireBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else
        {
            fireBall.GetComponent<GrabablePhysicsHandler>().m_rgb.useGravity = false;
            fireBall.GetComponent<GrabablePhysicsHandler>().m_rgb.velocity = Vector3.zero;
            fireBall.GetComponent<GrabablePhysicsHandler>().inputManager = inputManager;
            fireBall.GetComponent<GrabablePhysicsHandler>().enabled = false;
            authority.GetComponent<PlayerMovementMulti>().tableTransform.GetComponentInChildren<FireballManager>().outFireball = fireBall;
            authority.GetComponent<PlayerMovementMulti>().tableTransform.GetComponentInChildren<FireballManager>().grabManager = grabManager.GetComponent<GrabManager>();
            authority.GetComponent<PlayerMovementMulti>().tableTransform.GetComponentInChildren<FireballManager>().detectionHUD = vrHeadSett.GetComponentInChildren<DetectionHUD>();
            authority.GetComponent<PlayerMovementMulti>().tableTransform.GetComponentInChildren<FireballManager>().Initialize();
            authority.GetComponentInChildren<GrabManagerMulti>().fireBallInstantiated = fireBall;

        }
       
    }
    void RpcSyncFireBall(GameObject fireBall, GameObject authority, int i)
    {
      

    }
    [Command]
    private void CmdSpawnPiece(GameObject authority, List<pool> mainPool, string _tempComponent, PhysicMaterial[] _tempbasicMats, int index, bool isModifier)
    {
        tempFlock.GetComponent<GrabablePhysicsHandler>().m_rgb.useGravity = false;
        tempFlock.GetComponent<GrabablePhysicsHandler>().m_rgb.velocity = Vector3.zero;
        //tempFlock.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(tempModifier, tempComponent as ModifierAction, tempbasicMats);
        tempFlock.GetComponent<GrabablePhysicsHandler>().enabled = false;
        NetworkServer.Spawn(tempFlock, authority);
        RpcSyncUnits(tempFlock,mainPool,authority, _tempComponent, tempbasicMats,  index,isModifier);
    }

    [ClientRpc]
    void RpcSyncUnits(GameObject x, List<pool> mainPool, GameObject authority,string _tempComponent,PhysicMaterial[] _tempbasicMats, int index, bool isModifier)
    {
        bool hasFounded = false;
        components.Add(grabManager.GetComponent( _tempComponent));
        
        tempFlock = x;
        tempFlock.GetComponent<GrabablePhysicsHandler>().m_rgb.useGravity = false;
        tempFlock.GetComponent<GrabablePhysicsHandler>().m_rgb.velocity = Vector3.zero;
        tempFlock.GetComponent<GrabablePhysicsHandler>().inputManager = inputManager;
        tempFlock.GetComponent<GrabablePhysicsHandler>().initPos = tempFlock.transform.position;

        if (_tempComponent == "BasicFloakAction")
            tempFlock.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(components[components.Count-1] as ModifierAction, _tempbasicMats);
        else
        {
            var manager = grabManager.GetComponent<GrabManagerMulti>();
            for (int i = 0; i < manager.negativeModifiers.Count; i++)
            {
                if(manager.negativeModifiers[i].name == _tempComponent)
                    tempFlock.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(manager.negativeModifiers[i],components[components.Count - 1] as ModifierAction, _tempbasicMats);
            }
            for (int i = 0; i < manager.positiveModifiers.Count; i++)
            {
                if (manager.positiveModifiers[i].name == _tempComponent)
                    tempFlock.GetComponent<GrabablePhysicsHandler>().ChangeBehavior(manager.positiveModifiers[i], components[components.Count - 1] as ModifierAction, _tempbasicMats);
            }

        }

        tempFlock.GetComponent<GrabablePhysicsHandler>().enabled = false;

        authority.GetComponentInChildren<GrabManagerMulti>().mainPool = mainPool;
        if(!isModifier)
        authority.GetComponentInChildren<GrabManagerMulti>().AddFlox();
        authority.GetComponentInChildren<GrabManagerMulti>().numberOfPool = mainPool.Count;
    }

    [Command(requiresAuthority = false)]
    public void CmdWin1()
    {
        RcpWin1();
        StartCoroutine(WaitToCallMenu());
    }
    [Command(requiresAuthority = false)]
    public void CmdWin2()
    {
        RcpWin2();
        StartCoroutine(WaitToCallMenu());
    }

    [ClientRpc]
    void RcpWin1()
    {
        UIGlobalManager.instance.Win(0);
        StartCoroutine(WaitToCallMenu());

    }
    [ClientRpc]
    void RcpWin2()
    {  
        UIGlobalManager.instance.Win(1);
        StartCoroutine(WaitToCallMenu());
    }

    IEnumerator WaitToCallMenu()
    {
        yield return new WaitForSeconds(5.0f);
        inputManager.OnMenuPressed.Invoke();
    }

    [Command(requiresAuthority = false)]
    public void CmdInitUI(int index, GameObject player, bool activate, int indexOFSprite, GameObject roomPLayer1, GameObject roomPLayer2 )
    {

        if(!activate)
        RcpInitUI(index,player,activate,indexOFSprite,roomPLayer1,roomPLayer2);
        Destroy(roomPLayer1);
        if(roomPLayer2)
        Destroy(roomPLayer2);
    }


    [ClientRpc]
    void RcpInitUI(int index, GameObject player,bool activate, int indexOFSprite,GameObject roomPLayer1, GameObject roomPLayer2)
    {
        if (roomPLayer1 != null)
            Destroy(roomPLayer1);
        if (roomPLayer2 != null)
            Destroy(roomPLayer2);
        var assigntToUi = player.GetComponentInChildren<AssignToUI>();
        UIGlobalManager.instance.PlayerImage(index, indexOFSprite);
    }

    [Command(requiresAuthority = false)]
    public void CmdInitGrabManager( GameObject grabManager, int index)
    {
        if (index == 0)
            NetworkManagerRace.instance.grabManagers = new GrabManagerMulti[2];
        NetworkManagerRace.instance.grabManagers[index] = grabManager.GetComponentInChildren<GrabManagerMulti>();
        RcpInitGrabManager(grabManager, index);
    }


    [ClientRpc]
    void RcpInitGrabManager(GameObject grabManager, int index)
    {
        if(index ==0)
            NetworkManagerRace.instance.grabManagers = new GrabManagerMulti[2];
        NetworkManagerRace.instance.grabManagers[index] = grabManager.GetComponentInChildren<GrabManagerMulti>();
    }

    [Command(requiresAuthority = false)]
    public void CmdDestroyBubble(GameObject bubble)
    {
        NetworkServer.Destroy(bubble);
    }

    [Command(requiresAuthority =false)]
    public void CmdSpawnInFireBall(GameObject target)
    {
        RpcGetFireBall();
    }
    [ClientRpc(includeOwner =false)]
    private void RpcGetFireBall()
    {
        StartCoroutine(tableTransform.GetComponentInChildren<FireballManager>().FireballIncoming());
    }
   
    [ClientRpc]
    public void RpcDebugLog(string text)
    {
        Debug.Log(text);
    }
    /// <summary>
    /// will be replace by the actual graph
    /// </summary>
    /// <param name="text"></param>
    [ClientRpc]
    public void RpcTempPosition(string text)
    {
        UIGlobalManager.instance.SetRulesMode(text);
    }
    [Command]
    public void CmdChangeMilestoneValue(int index, int value)
    {
        NetworkManagerRace.instance.ChangeMilestonValue(index, value);
        RpcChangeMilestonValue(index, value);
    }
    [ClientRpc]
    private void RpcChangeMilestonValue(int index, int value)
    {
        NetworkManagerRace.instance.ChangeMilestonValue(index, value);
    }


    [Command(requiresAuthority = true)]
    public void CmdMoveBubble(float _playgroundRayon,float tposition,Vector3 pPos,List<GameObject> bonus, List<GameObject> malus, GameObject fireball, List<Vector3> direction)
    {
       // moveBubble.MoveBubbles(_playgroundRayon, tposition, pPos, bonus, malus, fireball, direction);
        RcpMoveBubble( _playgroundRayon, tposition,pPos, bonus, malus, fireball, direction);
    }


    [ClientRpc]
    void RcpMoveBubble(float _playgroundRayon, float tposition, Vector3 pPos, List<GameObject> bonus, List<GameObject> malus, GameObject fireball, List<Vector3> direction)
    {
        moveBubble.GetComponent<MoveBubble>().MoveBubbles(_playgroundRayon, tposition,pPos, bonus, malus, fireball,direction);
    }

    [Command(requiresAuthority =false)]
    public void CmdMoveObject(GameObject objectToMove, Vector3 newPos)
    {
        objectToMove.transform.position = newPos;
        RpcMoveObject(objectToMove, newPos);
    }

    [ClientRpc]
    private void RpcMoveObject(GameObject objectToMove, Vector3 newPos)
    {
        objectToMove.transform.position = newPos;

    }

    [Command(requiresAuthority = false)]
    public void CmdExplosion(Vector3 position)
    {
        RpcExplosion(position);
    }
    [ClientRpc]
    public void RpcExplosion(Vector3 position)
    {
        tableTransform.GetComponentInChildren<FireballManager>().Explosion(position);
    }

    
  
}
