using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIGlobalManager : MonoBehaviour
{
    public static UIGlobalManager instance;

    public List< TextMeshProUGUI> stopWatch = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> gameModeNames = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> gameModeRules = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> flockNumbers = new List<TextMeshProUGUI>();
    public List<Image> player1Images = new List<Image>();
    public List<Image> player2Images = new List<Image>();
    public List<GameObject> winPlayer1 = new List<GameObject>();
    public List<GameObject> winPlayer2 = new List<GameObject>();
    public List<GameObject> losePlayer1 = new List<GameObject>();
    public List<GameObject> losePlayer2 = new List<GameObject>();
    public bool playeTimer;
    private float timer;
    private int flockNumber;
    public GameObject canvasPlayer2;

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
        yield return new WaitForSeconds(2);
        if(NetworkManagerRace.instance.numberOfPlayer == 1)
        {

        }
    }
    private void Update()
    {
        if (playeTimer)
        {
            timer += Time.deltaTime;
            for (int i = 0; i < stopWatch.Count; i++)
            {
                stopWatch[i].text =ConvertToHourMinSec(timer);
            }
            Win(1);
        }
    }

    public void StartTimer()
    {
        playeTimer = true;

    }
    private string ConvertToHourMinSec(float timer)
    {
        string tempString = "";
        int hour = Mathf.FloorToInt(timer) /3600;
        int min = (Mathf.FloorToInt(timer) - hour * 3600) / 60;
        int s = Mathf.FloorToInt(timer) - hour * 3600 - min * 60;
        string secString = s.ToString();
        if (s < 10)
            secString = " 0" + s.ToString();
        string minString = min.ToString();
        if (min < 10)
            minString = " 0" + min.ToString();
            return tempString ="Timer\n"+ hour.ToString() + " :" + minString + " :" + secString;
    }
    public void SetGameMode(string name, string rule)
    {
        for (int i = 0; i < gameModeNames.Count; i++)
        {
            gameModeNames[i].text = name;
        }
        for (int i = 0; i < gameModeRules.Count; i++)
        {
            gameModeRules[i].text = rule;
        }
    }
    public void SetRulesMode(string rule)
    {
        
        for (int i = 0; i < gameModeRules.Count; i++)
        {
            gameModeRules[i].text = rule;
        }
    }
    public void ChangeFlockNumner(int add)
    {
        flockNumber += add;
        for (int i = 0; i < flockNumbers.Count; i++)
        {
            if (flockNumber > 1)
                flockNumbers[i].text = "Floxes :" + flockNumber.ToString();
            else
                flockNumbers[i].text = "Flox :" + flockNumber.ToString();

        }
    }
    public void ResetFlockNumber()
    {
        flockNumber = 0;
        for (int i = 0; i < flockNumbers.Count; i++)
        {
            if (flockNumber > 1)
                flockNumbers[i].text = "Floxes :" + flockNumber.ToString();
            else
                flockNumbers[i].text = "Flox :" + flockNumber.ToString();

        }
    }
    public void PlayerImage(int index, Sprite sprite)
    {
        if(index == 1)
        {
            for (int i = 0; i < player1Images.Count; i++)
            {
                player1Images[i].sprite = sprite;
            }
        }
        else
        {
            if (player2Images == null)
                return;
            for (int i = 0; i < player2Images.Count; i++)
            {
                player2Images[i].sprite = sprite;
            }
        }
    }
    public void Win(int indexOfWinner)
    {
        if(indexOfWinner == 1)
        {
            for (int i = 0; i < winPlayer2.Count; i++)
            {
                winPlayer2[i].SetActive(true);
            }
            for (int i = 0; i < losePlayer1.Count; i++)
            {
                losePlayer1[i].SetActive(true);
            }
        }
        else if(winPlayer2 != null && winPlayer2.Count > 0)
        {
            for (int i = 0; i < winPlayer1.Count; i++)
            {
                winPlayer1[i].SetActive(true);
            }
            for (int i = 0; i < losePlayer2.Count; i++)
            {
                losePlayer2[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < winPlayer1.Count; i++)
            {
                winPlayer1[i].SetActive(true);
            }
        }

    }

    public void AddPlayer(int index, TextMeshProUGUI _stopwatch, TextMeshProUGUI _gameModeNames, TextMeshProUGUI _gameModeRules, TextMeshProUGUI _flockNumbers, Image _playerImages,GameObject _winPlayer, GameObject _losePlayer)
    {
        stopWatch.Add(_stopwatch);
        gameModeNames.Add(_gameModeNames);
        gameModeRules.Add(_gameModeRules);
        flockNumbers.Add(_flockNumbers);
        if(index == 0)
        {
            player1Images.Add(_playerImages);
            winPlayer1.Add(_winPlayer);
            losePlayer1.Add(_losePlayer);
        }
        else
        {
            player2Images.Add(_playerImages);
            winPlayer2.Add(_winPlayer);
            losePlayer2.Add(_losePlayer);
        }
    }
}
