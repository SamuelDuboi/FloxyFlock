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
        NetworkManagerRace.instance.avatarsSprite[number] = index;
        isReady = !isReady;
        if (isReady)
            readyButton.color = Color.green;
        else
            readyButton.color = Color.white;
        networkRoom.CmdChangeReadyState(isReady);
        GetComponentInParent<InputManager>().gameObject.SetActive(false);
    }
}
