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
    public override void Awake()
    {
        base.Awake();

        if (instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
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
        numberOfPlayer++;
        // player.GetComponent<ControllerKeyBoard>().playerId = numberOfPlayer;
        NetworkServer.AddPlayerForConnection(conn, player);
        StartCoroutine(WaitToSpawn(conn, player, index));
        return player;
    }
    IEnumerator WaitToSpawn(NetworkConnection conn, GameObject player, int index)
    {
        yield return new WaitForSeconds(1f);
         //playerController.CmdSpawnManager(player);
        yield return new WaitForSeconds(1f);
        if (grabManagers == null)
            grabManagers = new GrabManagerMulti[2];
        grabManagers[numberOfPlayer - 1] = player.GetComponentInChildren<GrabManagerMulti>();
        playerController.CmdInitUI(index, player);
        grabManagers[index].InitPool(player,playerController);
    }

    public void Win(int playerId)
    {
        if (playerId == 0)
            playerController.CmdWin1();
        else
            playerController.CmdWin2();
    }
}
