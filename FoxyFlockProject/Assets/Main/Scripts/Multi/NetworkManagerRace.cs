using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkManagerRace : NetworkManager
{
    int numberOfPlayer = 0;
    public Transform firstPlayer;
    public Transform secondPlayerTransform;


    public GameObject[] startSpawn;
    private PlayerMovementMulti playerController;
    private GrabManagerMulti[] grabManagers;

    public static NetworkManagerRace instance;
    public override void Awake()
    {
        base.Awake();

        if (instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
    }
    /// <summary>
    /// Called on the server when a client adds a new player with ClientScene.AddPlayer.
    /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
    /// </summary>
    /// <param name="conn">Connection from client.</param>
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        int index = 0;
        Transform start = numPlayers == 0 ? firstPlayer : secondPlayerTransform;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        if (numberOfPlayer == 0)
        {
            playerController = player.GetComponent<PlayerMovementMulti>();
        }
        else
        {
            index = 1;
        }
        numberOfPlayer++;
        // player.GetComponent<ControllerKeyBoard>().playerId = numberOfPlayer;
        NetworkServer.AddPlayerForConnection(conn, player);
        StartCoroutine(WaitToSpawn(conn, player,index));
    }
    IEnumerator WaitToSpawn(NetworkConnection conn, GameObject player, int index)
    {
        yield return new WaitForSeconds(1f);
         playerController.CmdSpawnManager();
        yield return new WaitForSeconds(1f);
        if (grabManagers == null)
            grabManagers = new GrabManagerMulti[2];
        grabManagers[numberOfPlayer-1] = player.GetComponent<PlayerMovementMulti>().grabManager.GetComponent<GrabManagerMulti>();

        grabManagers[index].InitPool(player,playerController);
    }

    public void Win(int playerId)
    {
       /* if (playerId == 0)
            playerController.CmdEndTurn1();
        else
            playerController.CmdEndTurn();*/
    }
}
