using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomPlayer : MonoBehaviour
{
    public List<Sprite> avatars;
    private Sprite thisAvatar;
    private int index;
    public Image avatartImage;
    public int number;
    public NetworkRoomPlayerGamr networkRoom;
    private bool isReady;
    public Image readyButton;
    public ShowUi show;
    public void LeftAvatar()
    {
        if (index == 0)
            index = avatars.Count - 1;
        else
        {
            index--;
        }
        thisAvatar = avatars[index];
        avatartImage.sprite = thisAvatar;
        show.CmdChangeUi(index);
    }

    public void RightAvatar()
    {
        if (index == avatars.Count - 1)
            index = 0;
        else
        {
            index++;
        }
        thisAvatar = avatars[index];
        avatartImage.sprite = thisAvatar;
    }
  

    public void Ready()
    {
        NetworkManagerRace.instance.avatarsSprite[networkRoom.index] = index;
        isReady = !isReady;
        if (isReady)
            readyButton.color = Color.green;
        else
            readyButton.color = Color.white;
        networkRoom.CmdChangeReadyState(isReady);
        networkRoom.CmdSetIndex(networkRoom.index, index);
      //  GetComponentInParent<InputManager>().gameObject.SetActive(false);
    }

    public void Menu()
    {
   
        if (index != 0)
            NetworkManagerRace.instance.StopClient();
        else
            NetworkManagerRace.instance.StopHost();
    }

    public void Host()
    {
        NetworkManagerRace.instance.StartHost();
    }
    public void StartClient()
    {
        NetworkManagerRace.instance.StartClient();
    }
}
