using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MultiUIHandler : NetworkBehaviour
{
  [HideInInspector]  public GrabManagerMulti grabManager;


    [Command(requiresAuthority = false)]
    public void CmdGetFireBall()
    {
        UIGlobalManager.instance.CanSelectFireBall(grabManager.playerNumber);
        RpcGetFireBall();

    }
    [ClientRpc]
    public void RpcGetFireBall()
    {
        UIGlobalManager.instance.CanSelectFireBall(grabManager.playerNumber);
    }


    [Command(requiresAuthority = false)]
    public void CmdIsWinning()
    {
        UIGlobalManager.instance.IsFirst(grabManager.playerNumber-1);
        RpcIsWinning();

    }
    [ClientRpc]
    public void RpcIsWinning()
    {
        UIGlobalManager.instance.IsFirst(grabManager.playerNumber-1);
    }

    [Command(requiresAuthority = false)]
    public void CmdIsATie()
    {
        UIGlobalManager.instance.IsATie();
        RpcIsATie();
    }
    [ClientRpc]
    public void RpcIsATie()
    {
        UIGlobalManager.instance.IsATie();
    }


    [Command(requiresAuthority = false)]
    public void CmdUnSelectFireBall()
    {
        UIGlobalManager.instance.UnSelectFireBall(grabManager.playerNumber-1);
        RpcUnSelectFireBall();
    }
    [ClientRpc]
    public void RpcUnSelectFireBall()
    {
        UIGlobalManager.instance.UnSelectFireBall(grabManager.playerNumber-1);
    }

}
