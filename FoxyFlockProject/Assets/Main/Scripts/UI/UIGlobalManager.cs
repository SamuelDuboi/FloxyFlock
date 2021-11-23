using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class UIGlobalManager : MonoBehaviour
{
    public static UIGlobalManager instance;

    public TextMeshProUGUI[] stopWatch;
    public TextMeshProUGUI[] gameModeNames;
    public TextMeshProUGUI[] gameModeRules;
    public TextMeshProUGUI[] flockNumbers;
    public Image[] Player1Images;
    public Image[] Player2Image;
    public GameObject[] winPlayer1;
    public GameObject[] winPlayer2;
    public GameObject[] losePlayer1;
    public GameObject[] losePlayer2;
    public bool playeTimer;
    private float timer;
    private int flockNumber;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Update()
    {
        if (playeTimer)
        {
            timer += Time.deltaTime;
            for (int i = 0; i < stopWatch.Length; i++)
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
        for (int i = 0; i < gameModeNames.Length; i++)
        {
            gameModeNames[i].text = name;
        }
        for (int i = 0; i < gameModeRules.Length; i++)
        {
            gameModeRules[i].text = rule;
        }
    }
    public void ChangeFlockNumner(int add)
    {
        flockNumber += add;
        for (int i = 0; i < flockNumbers.Length; i++)
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
        for (int i = 0; i < flockNumbers.Length; i++)
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
            for (int i = 0; i < Player1Images.Length; i++)
            {
                Player1Images[i].sprite = sprite;
            }
        }
        else
        {
            if (Player2Image == null)
                return;
            for (int i = 0; i < Player2Image.Length; i++)
            {
                Player2Image[i].sprite = sprite;
            }
        }
    }
    public void Win(int indexOfWinner)
    {
        if(indexOfWinner == 2)
        {
            for (int i = 0; i < winPlayer2.Length; i++)
            {
                winPlayer2[i].SetActive(true);
            }
            for (int i = 0; i < losePlayer1.Length; i++)
            {
                losePlayer1[i].SetActive(true);
            }
        }
        else if(winPlayer2 != null && winPlayer2.Length > 0)
        {
            for (int i = 0; i < winPlayer1.Length; i++)
            {
                winPlayer1[i].SetActive(true);
            }
            for (int i = 0; i < losePlayer2.Length; i++)
            {
                losePlayer2[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < winPlayer1.Length; i++)
            {
                winPlayer1[i].SetActive(true);
            }
        }

    }
}
