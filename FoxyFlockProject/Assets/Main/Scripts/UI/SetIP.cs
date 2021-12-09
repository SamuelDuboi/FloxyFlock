using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetIP : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public NetworkManagerRace manager;
    public void SetText(string character)
    {
        textMeshProUGUI.text += character;
        manager.networkAddress = textMeshProUGUI.text;
    }
    public void RemoveText()
    {
        textMeshProUGUI.text.Remove(textMeshProUGUI.text.Length-1);
        manager.networkAddress = textMeshProUGUI.text;

    }
}
