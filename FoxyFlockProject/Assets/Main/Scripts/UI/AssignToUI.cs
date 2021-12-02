using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class AssignToUI : MonoBehaviour
{
    public TextMeshProUGUI stopWatch;
    public TextMeshProUGUI gameModeName;
    public TextMeshProUGUI gameModeRule;
    public TextMeshProUGUI flockNumber;
    public Image player1Image;
    public GameObject winPlayer1;
    public GameObject losePlayer1;
    // Start is called before the first frame update
    void Start()
    {
        var instance = UIGlobalManager.instance;
        instance.stopWatch.Add(stopWatch);
        instance.gameModeNames.Add(gameModeName);
        instance.gameModeRules.Add(gameModeRule);
        instance.flockNumbers.Add(flockNumber);
        instance.Player1Images.Add(player1Image);
        instance.winPlayer1.Add(winPlayer1);
        instance.losePlayer1.Add(losePlayer1);
    }

}
