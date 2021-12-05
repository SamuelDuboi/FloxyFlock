using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkManagerRace : NetworkRoomManager
{
   [HideInInspector] public int numberOfPlayer = 0;
    public Transform firstPlayer;
    public Transform secondPlayerTransform;

    public GameObject[] startSpawn;
    public PlayerMovementMulti playerController;
    private GrabManagerMulti[] grabManagers;
    public GameObject player2Canvas;
    public static NetworkManagerRace instance;
    private GameObject[] players = new GameObject[2];
    private int InitNumberOfPlayer;
    public override void Awake()
    {
        base.Awake();

        if (instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        InitNumberOfPlayer++;
    }
    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnection conn, GameObject roomPlayer)
    {
        int index = 0;
        firstPlayer = GameObject.FindGameObjectWithTag("FirstPlayerPos").transform;
        secondPlayerTransform = GameObject.FindGameObjectWithTag("SecondPlayerPos").transform;

        Transform start = numberOfPlayer == 0 ? firstPlayer : secondPlayerTransform;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);

        if (numberOfPlayer == 0)
        {
            playerController = player.GetComponent<PlayerMovementMulti>();
        }
        else
        {
            index = 1;
        }
        player.name = "player " + index;
        players[numberOfPlayer] = player;
        numberOfPlayer++;
        // player.GetComponent<ControllerKeyBoard>().playerId = numberOfPlayer;
        if(index ==InitNumberOfPlayer-1)
        StartCoroutine(WaitToSpawn(conn, players));
        return player;
    }
    IEnumerator WaitToSpawn(NetworkConnection conn, GameObject[] players)
    {
        yield return new WaitForSeconds(1f);
         //playerController.CmdSpawnManager(player);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < InitNumberOfPlayer; i++)
        {
            if (grabManagers == null)
                grabManagers = new GrabManagerMulti[2];
            grabManagers[i] = players[i].GetComponentInChildren<GrabManagerMulti>();
            if(InitNumberOfPlayer>1)
            playerController.CmdInitUI(i, players[i],true);
            else
            {
                playerController.CmdInitUI(i, players[i], false);
            }
            grabManagers[i].InitPool(players[i], playerController);
        }
       
    }

    public void Win(int playerId)
    {
        if (playerId == 0)
            playerController.CmdWin1();
        else
            playerController.CmdWin2();
    }
}
