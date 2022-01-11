using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIGlobalManager : MonoBehaviour
{
    public static UIGlobalManager instance;
    public TextMeshProUGUI stopWatch;
    public TextMeshProUGUI gameModeRules;



    public Image player1Images;
    public GameObject player1Validation;
    public Image player1Clock;
    public TextMeshProUGUI player1ClockTimer;
    public GameObject player1HandsText;
    public GameObject winPlayer1;




    public Image player2Images;
    public GameObject player2Validation;
    public Image player2Clock;
    public TextMeshProUGUI player2ClockTimer;
    public GameObject player2HandsText;
    public GameObject winPlayer2;




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

    }
    private void Update()
    {

        timer += Time.deltaTime;
        stopWatch.text = ConvertToHourMinSec(timer);
    }

    private string ConvertToHourMinSec(float timer)
    {
        string tempString = "";
        int hour = Mathf.FloorToInt(timer) / 3600;
        int min = (Mathf.FloorToInt(timer) - hour * 3600) / 60;
        int s = Mathf.FloorToInt(timer) - hour * 3600 - min * 60;
        string secString = s.ToString();
        if (s < 10)
            secString = " 0" + s.ToString();
        string minString = min.ToString();
        if (min < 10)
            minString = " 0" + min.ToString();
        return tempString = "Timer\n" + hour.ToString() + " :" + minString + " :" + secString;
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
            if (!player1Validation.activeSelf)
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

}
