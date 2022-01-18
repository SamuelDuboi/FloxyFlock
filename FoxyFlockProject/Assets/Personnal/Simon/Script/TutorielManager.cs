using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorielManager : MonoBehaviour
{
    public TextMeshProUGUI sentance;
    public int tutorialStage;
    public string currentAdvice1;
    public GameObject dispenserButton;
    public List<GameObject> dispenser;
    public CapsuleCollider tableRange;
    public LayerMask playerLayer;

    public bool batchToFinish;
    public int batcheIdMax;
    public GameObject orbs;
    public TutorialBatchManager batchManage;
    public GameObject ennemiTable;

    [HideInInspector] public bool bubbleToFinish;
    [HideInInspector] public bool canGoNextBatch;

    private void Start()
    {
        //tutorialStage = 1;

    }
    private void Update()
    {

        switch (tutorialStage)
        {
            #region 1Point
            case 1:
                currentAdvice1 = "Press grab button to point with your finger and use it to Press the Button in front of you !";
                sentance.text = currentAdvice1;
                ActivateDispenser(); //to delete
                break;
            #endregion
            #region 2MoveAround
            case 2:
                currentAdvice1 = "Press both of the Triggers at the same time and move your hands around you and in front of you to Move Around the Table.";
                sentance.text = currentAdvice1;
                break;
            #endregion
            #region 3Grab
            case 3:
                currentAdvice1 = "You can grab objects if your hand is close enough. Hold the grab button to manipulate objects. \n\n Pull the lever to get your first batch of Floxes.";
                batcheIdMax = 1;
                sentance.text = currentAdvice1;
                batchToFinish =true;
                break;
            #endregion
            #region 4FloxManip
            case 4:
                currentAdvice1 = "Grab the flox and place them on the table. \n\n Once all the flox are placed as you want in the playground, pull the lever to access the next batch of floxes.";
                batcheIdMax = 2;
                sentance.text = currentAdvice1; 
                batchToFinish = true;
                break;
            #endregion
            #region 5Freeze
            case 5 : 
                currentAdvice1 = "Once you pulled the lever, all the flox you placed on the table are Freezed and can't be moved again. \n\n You can continu building on the flox you just placed.";
                sentance.text = currentAdvice1;
                batcheIdMax = 3;
                batchToFinish = true;
                break;
            #endregion
            #region 6Reset
            case 6:
                currentAdvice1 = "If you are not pleased with the way you build your tower you can always erase it and start anew. \n Press the button on the top of the dispenser to reset your tower.";
                sentance.text = currentAdvice1;
                if (batchManage.batches.Count == 1)
                    tutorialStage++;
                break;
            #endregion
            #region 7Bubbles
            case 7:
                currentAdvice1 = "Each time you move to another batch new bubbles appears around your tower. Green bubbles gives bonus flox and the red ones gives malus flox. \n\n Try to freeze flox in the bubbles to earn special flox for your next batch.";
                sentance.text = currentAdvice1;
                orbs.SetActive(true);
                batcheIdMax = 2;
                batchToFinish = true;
                if (bubbleToFinish)
                    canGoNextBatch = true;

                break;
            #endregion
            #region 8Adversity
            case 8:
                currentAdvice1 = "You can play against your friends in a race to the limit. Look left to see the ennemi tower. \n You can consult this Board Info at any time to get informations about the current state of the game.";
                sentance.text = currentAdvice1;
                ennemiTable.SetActive(true); // regarder tour autre joueur
                break;
            #endregion
            #region 9MoveAroundUp
            case 9:
                currentAdvice1 = "Now let's even the play. Here is your new Tower. \n\n Use the Move Around Mode to climb up and build on the top.";
                sentance.text = currentAdvice1; //atteindre certaine jhauteur
                break;
            #endregion
            #region 10FireBallBubble
            case 10:
                currentAdvice1 = "Sometimes a purpple bubble appears. \n Get it and you'll get access to a new power, the Fireball !";
                sentance.text = currentAdvice1; //get une certaine bubble
                break;
            #endregion
            #region 11FireBallLaunch
            case 11:
                currentAdvice1 = "Grab the Fireball in the Dispenser. \n Throw it into the portal that appears and watch the ennemi tower to enjoy the results.";
                sentance.text = currentAdvice1; //lancer la FireBall dans le portail et attendre les effets
                break;
            #endregion
            #region 12FireBallDefense
            case 12:
                currentAdvice1 = "But you're ennemy can also access to a Fireball and use it against you. \n When the alerte for an incoming Fireball appears, get ready to parry it with your own hands !";
                sentance.text = currentAdvice1; // passer à la suite une fois la Fireball reçu peu importe le resulatat git gud lol
                break;
            #endregion
            #region 13FinishTheTuto
            case 13 :
                currentAdvice1 = "You know now everything you need to know to play Foxy Flox."; //attendre un peu
                sentance.text = currentAdvice1;
                break;
            #endregion
            #region 14End
            case 14:
                currentAdvice1 = "This is the end of this Tutorial. \n Good luck and Have fun playing Foxy Flox !"; // ouvrir le menu pause et forcer
                sentance.text = currentAdvice1;
                break;
                #endregion
        }

    }

    public void ActivateDispenser()
    {
        for (int i = 0; i < dispenser.Count; i++)
        {
            dispenser[i].SetActive(true);
        }
        dispenserButton.SetActive(false);
        tutorialStage = 2;
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.layer == playerLayer)
        {
            tutorialStage = 3;
        }
    }
}
