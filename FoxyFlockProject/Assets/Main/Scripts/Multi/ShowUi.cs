using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShowUi : NetworkBehaviour
{
    public RoomPlayer player;
    public List<Sprite> avatars;
    [Command(requiresAuthority = false)]
    public void CmdChangeUi(int index)
    {
        player.avatartImage.sprite = avatars[index];
        if(NetworkServer.connections.Count>1)
        RpcChangeUI(index);
    }
    [ClientRpc]
    private void RpcChangeUI(int index)
    {
        player.avatartImage.sprite = avatars[index];
    }
}
