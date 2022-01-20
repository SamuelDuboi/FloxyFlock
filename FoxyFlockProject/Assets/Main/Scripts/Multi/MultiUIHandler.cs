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
        if(NetworkManagerRace.instance.grabManagers[1])
            NetworkManagerRace.instance.grabManagers[1] = grabManagers[1].GetComponentInChildren<GrabManagerMulti>();
        RpcInitManagers(grabManagers);
    }


    [ClientRpc]
    public void RpcInitManagers(GameObject[] grabManagers)
    {
        NetworkManagerRace.instance.grabManagers = new GrabManagerMulti[2];
        NetworkManagerRace.instance.grabManagers[0] = grabManagers[0].GetComponentInChildren<GrabManagerMulti>();
        if (NetworkManagerRace.instance.grabManagers[1])
            NetworkManagerRace.instance.grabManagers[1] = grabManagers[1].GetComponentInChildren<GrabManagerMulti>();
    }

    [Command(requiresAuthority = true)]
    public void CmdGetFireBall()
    {
       // UIGlobalManager.instance.CanSelectFireBall(grabManager.playerNumber);
        RpcGetFireBall();

    }
    [ClientRpc]
    public void RpcGetFireBall()
    {
        UIGlobalManager.instance.CanSelectFireBall(grabManager.playerNumber);
    }


    [Command(requiresAuthority = true)]
    public void CmdIsWinning()
    {
        //UIGlobalManager.instance.IsFirst(grabManager.playerNumber-1);
        RpcIsWinning(grabManager.playerNumber - 1);

    }
    [ClientRpc]
    public void RpcIsWinning(int index)
    {
        UIGlobalManager.instance.IsFirst(index);
    }

    [Command(requiresAuthority = true)]
    public void CmdIsATie()
    {
        //UIGlobalManager.instance.IsATie();
        RpcIsATie();
    }
    [ClientRpc]
    public void RpcIsATie()
    {
        UIGlobalManager.instance.IsATie();
    }


    [Command(requiresAuthority = true)]
    public void CmdSelectFireBall()
    {
       // UIGlobalManager.instance.CanSelectFireBall(grabManager.playerNumber-1);
        RpcSelectFireBall(grabManager.playerNumber - 1);
    }
    [ClientRpc]
    public void RpcSelectFireBall(int index)
    {
            UIGlobalManager.instance.CanSelectFireBall(index);
    }

    [Command(requiresAuthority = true)]
    public void CmdUnSelectFireBall()
    {
        //UIGlobalManager.instance.UnSelectFireBall(grabManager.playerNumber - 1);
        RpcUnSelectFireBall(grabManager.playerNumber - 1);
    }
    [ClientRpc]
    public void RpcUnSelectFireBall(int index)
    {
       
            UIGlobalManager.instance.UnSelectFireBall(index);
    }

    [Command(requiresAuthority = true)]
    public void CmdFireBallIncoming()
    {
        //UIGlobalManager.instance.FireBallIncoming(grabManager.playerNumber - 1);
        RpcFireBallIncoming(grabManager.playerNumber - 1);
    }
    [ClientRpc]
    public void RpcFireBallIncoming(int index)
    {
            UIGlobalManager.instance.FireBallIncoming(index);
    }

    [Command(requiresAuthority = true)]
    public void CmdFireBallNotIncoming()
    {
        //UIGlobalManager.instance.FireBallNotIncoming(grabManager.playerNumber - 1);
        RpcFireBallNotIncoming(grabManager.playerNumber - 1);
    }
    [ClientRpc]
    public void RpcFireBallNotIncoming(int index)
    {
            UIGlobalManager.instance.FireBallNotIncoming(index);
    }

    [Command(requiresAuthority = true)]
    public void CmdValidate(int indexOfWinner, bool isHandInZone, float timer, float maxTimer)
    {
        //UIGlobalManager.instance.Validation(indexOfWinner,isHandInZone,timer,maxTimer);
        RpcValidate(indexOfWinner,isHandInZone,timer,maxTimer);
    }
    [ClientRpc]
    public void RpcValidate(int indexOfWinner, bool isHandInZone, float timer , float maxTimer )
    {
            UIGlobalManager.instance.Validation(indexOfWinner, isHandInZone, timer, maxTimer);
    }
    [Command(requiresAuthority = true)]
    public void CmdCloseValidate(int indexOfWinner)
    {
        RpcCloseValidate(indexOfWinner);
    }
    [ClientRpc]
    public void RpcCloseValidate(int indexOfWinner)
    {
        UIGlobalManager.instance.CloseValidation(indexOfWinner);
    }


}
