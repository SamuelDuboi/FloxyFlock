using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIGlobalManager : MonoBehaviour
{
    public static UIGlobalManager instance;
    public TextMeshProUGUI stopWatch;
    public TextMeshProUGUI gameModeRules;
    public Color firstPositionColor;
    public Color secondPositionColor;
    public float firstPositionScaleUp= 50;
    private float positionSize;

    [Header("Player 1")]
    public Image player1Images;
    public GameObject player1Validation;
    public Image player1Clock;
    public TextMeshProUGUI player1ClockTimer;
    public GameObject player1HandsText;
    public GameObject winPlayer1;
    public GameObject player1fireBallOn;
    public TextMeshProUGUI player1Position;
  [HideInInspector]  public RawImage player1table;
    public GameObject player1FireBallIncoming;

    [Header("Player 2")]

    public Image player2Images;
    public GameObject player2Validation;
    public Image player2Clock;
    public TextMeshProUGUI player2ClockTimer;
    public GameObject player2HandsText;
    public GameObject winPlayer2;
    public GameObject player2fireBallOn;
    public TextMeshProUGUI player2Position;
    [HideInInspector] public RawImage player2table;
    public GameObject player2FireBallIncoming;

    private bool cantPlayTime;
    private float timer;
    public Sprite[] sprites;
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        if (!NetworkManagerRace.instance)
        {
            yield break;
        }
        if (player1Position != null)
            positionSize = player1Position.fontSize;
    }
    private void Update()
    {
        if (!cantPlayTime)
        {
            timer += Time.deltaTime;
            stopWatch.text = ConvertToHourMinSec(timer);
        }
    }

    private string ConvertToHourMinSec(float timer)
    {
        //int hour = Mathf.FloorToInt(timer) / 3600;
        int min = (Mathf.FloorToInt(timer)) / 60;
        int s = Mathf.FloorToInt(timer) - min * 60;
        string secString = s.ToString();
        if (s < 10)
            secString = " 0" + s.ToString();
        string minString = min.ToString();
        
        //return tempString = "Timer\n" + hour.ToString() + " :" + minString + " :" + secString;
        return  $"{minString} : {secString}";
    }

    public void SetRulesMode(string rule)
    {
        gameModeRules.text = rule;

    }

    public void PlayerImage(int index, int indexOfSprite)
    {
        if (index == 0)
        {
            player1Images.sprite = sprites[indexOfSprite];

        }
        else
        {
            if (player2Images == null)
                return;
            player2Images.sprite = sprites[indexOfSprite];

        }
    }
    /// <summary>
    /// index 1 is player 1, index 2 is player 2
    /// </summary>
    /// <param name="indexOfWinner"></param>
    public void Win(int indexOfWinner)
    {
        cantPlayTime = true;
        if (indexOfWinner == 1)
        {
            winPlayer2.SetActive(true);
        }
        else
            winPlayer1.SetActive(true);

    }
    public void Validation(int indexOfWinner, bool isHandInZone, float timer = 0, float maxTimer =0)
    {
        if (indexOfWinner == 1)
        {
            if (!player2Validation.activeSelf)
                player2Validation.SetActive(true);
            if (isHandInZone)
            {
                player2HandsText.SetActive(true);
                if (player2Clock.gameObject.activeSelf)
                    player2Clock.gameObject.SetActive(false);
                
            }
            else
            {
                if (player2HandsText.gameObject.activeSelf)
                    player2HandsText.gameObject.SetActive(false);
                player2Clock.fillAmount = timer / maxTimer;
                player2ClockTimer.text = (maxTimer - timer).ToString();
            }
        }
        else
        {
            if(!player1Validation.activeSelf)
                player1Validation.SetActive(true);
            if (isHandInZone)
            {
                player1HandsText.SetActive(true);
                if (player1Clock.gameObject.activeSelf)
                    player1Clock.gameObject.SetActive(false);

            }
            else
            {
                if (player1HandsText.gameObject.activeSelf)
                    player1HandsText.gameObject.SetActive(false);
                player1Clock.gameObject.SetActive(true);
                player1Clock.fillAmount = timer / maxTimer;
                player1ClockTimer.text = ((int)(maxTimer - timer)).ToString();
            }
        }

    }

    public void IsFirst(int index)
    {
        if(index == 0)
        {
            player1Position.text = "1er";
            player1Position.fontSize+= firstPositionScaleUp;
            player1Position.color = firstPositionColor;
            player2Position.text = "2nd";
            player2Position.fontSize = positionSize;
            player2Position.color = secondPositionColor;
        }
        else
        {
            player2Position.text = "1er";
            player2Position.fontSize += firstPositionScaleUp;
            player2Position.color = firstPositionColor;
            player1Position.text = "2nd";
            player1Position.fontSize = positionSize;
            player1Position.color = secondPositionColor;
        }
    }
    public void IsATie()
    {
        player1Position.text = "2nd";
        player1Position.fontSize = positionSize;
        player1Position.color = secondPositionColor;
        player2Position.text = "2nd";
        player2Position.fontSize = positionSize;
        player2Position.color = secondPositionColor;
        
    }
    public void CanSelectFireBall(int index)
    {
        if (index == 0)
        {
            player1fireBallOn.SetActive(true);
        }
        else
        {
            player2fireBallOn.SetActive(true);
        }
    }
    public void UnSelectFireBall(int index)
    {
        if (index == 0)
        {
            player1fireBallOn.SetActive(false);
        }
        else
        {
            player2fireBallOn.SetActive(false);
        }
    }

    public void FireBallIncoming(int index)
    {
        if (index == 0)
        {
            player2FireBallIncoming.SetActive(true);
        }
        else
        {
            player1FireBallIncoming.SetActive(true);
        }
    }
    public void FireBallNotIncoming(int index)
    {
        if (index == 0)
        {
            player2FireBallIncoming.SetActive(false);
        }
        else
        {
            player1FireBallIncoming.SetActive(false);
        }
    }
}
