using UnityEngine;
using TMPro;

public class SetIP : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public NetworkManagerRace manager;
    public void SetText(string character)
    {
        if (textMeshProUGUI.text.Contains("local host"))
            textMeshProUGUI.text = string.Empty;
        textMeshProUGUI.text += character;
        manager.networkAddress = textMeshProUGUI.text;
    }
    public void RemoveText()
    {
        textMeshProUGUI.text.Remove(textMeshProUGUI.text.Length-1);
        manager.networkAddress = textMeshProUGUI.text;

    }
}
