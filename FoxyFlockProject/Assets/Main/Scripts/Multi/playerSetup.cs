using Mirror;
using UnityEngine;
using System.Collections;

public class playerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentToDisable = new Behaviour[3];
    public bool isRoomPlayer;
    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentToDisable.Length; i++)
            {
                componentToDisable[i].enabled = false;
            }
        }
 
  }
}
