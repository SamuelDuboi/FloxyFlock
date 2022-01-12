using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MultiUIHandler : NetworkBehaviour
{
  [HideInInspector]  public GrabManagerMulti grabManager;

    [Command(requiresAuthority =false)]
    public void CmdInitManagers(GameObject[] grabManagers)
    {
        NetworkManagerRace.instance.grabManagers = new GrabManagerMulti[2];
        NetworkManagerRace.instance.grabManagers[0] = grabManagers[0].GetComponentInChildren<GrabManagerMulti>();
        NetworkManagerRace.instance.grabManagers[1] = grabManagers[1].GetComponentInChildren<GrabManagerMulti>();
        RpcInitManagers(grabManagers);
    }


    [ClientRpc]
    public void RpcInitManagers(GameObject[] grabManagers)
    {
        NetworkManagerRace.instance.grabManagers = new GrabManagerMulti[2];
        NetworkManagerRace.instance.grabManagers[0] = grabManagers[0].GetComponentInChildren<GrabManagerMulti>();
        NetworkManagerRace.instance.grabManagers[1] = grabManagers[1].GetComponentInChildren<GrabManagerMulti>();
    }

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
        RpcIsWinning(grabManager.playerNumber - 1);

    }
    [ClientRpc]
    public void RpcIsWinning(int index)
    {
        UIGlobalManager.instance.IsFirst(index);
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
    public void CmdSelectFireBall()
    {
        UIGlobalManager.instance.CanSelectFireBall(grabManager.playerNumber-1);
        RpcUnSelectFireBall();
    }
    [ClientRpc]
    public void RpcSelectFireBall()
    {
        if (grabManager.playerNumber - 1 == 0)
            UIGlobalManager.instance.CanSelectFireBall(1);
        else
            UIGlobalManager.instance.CanSelectFireBall(0);
    }

    [Command(requiresAuthority = false)]
    public void CmdUnSelectFireBall()
    {
        UIGlobalManager.instance.UnSelectFireBall(grabManager.playerNumber - 1);
        RpcUnSelectFireBall();
    }
    [ClientRpc]
    public void RpcUnSelectFireBall()
    {
        if (grabManager.playerNumber - 1 == 0)
            UIGlobalManager.instance.UnSelectFireBall(1);
        else
            UIGlobalManager.instance.UnSelectFireBall(0);
    }

    [Command(requiresAuthority = false)]
    public void CmdValidate(int indexOfWinner, bool isHandInZone, float timer, float maxTimer)
    {
        UIGlobalManager.instance.Validation(indexOfWinner,isHandInZone,timer,maxTimer);
        RpcValidate(indexOfWinner,isHandInZone,timer,maxTimer);
    }
    [ClientRpc]
    public void RpcValidate(int indexOfWinner, bool isHandInZone, float timer , float maxTimer )
    {
            UIGlobalManager.instance.Validation(indexOfWinner, isHandInZone, timer, maxTimer);
    }


}
