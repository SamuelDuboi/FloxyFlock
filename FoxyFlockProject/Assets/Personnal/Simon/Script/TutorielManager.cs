using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorielManager : MonoBehaviour
{
    public int tutorialStage;
    public GameObject dispenserButton;
    public List<GameObject> dispenser;
    public string currentAdvice1;
    public string currentAdvice2;


    private void Update()
    {
        #region 1Point
        currentAdvice1 = "Press grab to point and Press the Button in front of you !";
        currentAdvice2 = null;
        #endregion
        #region 2MoveAround
        currentAdvice1 = "Foxy Flox is a stacking game in wich you stack flox on one another to reach a height limit as fast as possible. First come close to the table !";
        currentAdvice2 = "Press both of the Triggers at the same time and move your hands around you and in front of you to Move Around the Table.";
        #endregion
        #region 3Grab
        currentAdvice1 = "You can grab objects if your hand is close enough. Hold the grab button to manipulate objects.";
        currentAdvice2 = "Pull the lever to get your first batch of Floxes.";
        #endregion
        #region 4FloxManip
        currentAdvice1 = "Grab the flox and place them on the table.";
        currentAdvice2 = "Once all the flox are placed as you want in the playground, pull the lever to access the next batch of floxes";
        #endregion
        #region 5Freeze
        currentAdvice1 = "Once you pulled the lever, all the flox you placed on the table are Freezed and can't be moved again.";
        currentAdvice2 = "You can continu building on the flox you just placed.";
        #endregion
        #region 6Reset
        currentAdvice1 = "If you are not pleased with the way you build your tower can always erase it and start anew.";
        currentAdvice2 = "Press the button on the top of the dispenser to reset your tower.";
        #endregion
        #region 7Bubbles
        currentAdvice1 = "Each time you move to another batch new bubbles appears around your tower. Green bubbles gives bonus flox and the red ones gives malus flox.";
        currentAdvice2 = "Try to freeze floxe in the bubbles to earn special flox for your next batch.";
        #endregion
        #region 8Adversity
        currentAdvice1 = "You can play against your friends in a race to the limit. Look left to see the ennemi tower.";
        currentAdvice2 = "This Info Board gives you informations about the current state of the game. You can consult it at any time to see your progress and your ennemy's.";
        #endregion
        #region 9MoveAroundUp
        currentAdvice1 = "Now let's even the play. Here is your new Tower.";
        currentAdvice2 = "Use the Move Around Mode to climb up and build on the top.";
        #endregion
        #region 10FireBallBubble
        currentAdvice1 = "Sometimes a purpple bubble appears.";
        currentAdvice2 = "Get it and you'll get access to a new power, the Fireball !";
        #endregion
        #region 11FireBallLaunch
        currentAdvice1 = "Grab the Fireball in the Dispenser.";
        currentAdvice2 = "Throw it into the portal that appears and watch the ennemi tower to enjoy the results.";
        #endregion
        #region 12FireBallDefense
        currentAdvice1 = "But you're ennemy can also access to a Fireball and use it against you.";
        currentAdvice2 = "When the alerte for an incoming Fireball appears, get ready to parry it with your own hands !";
        #endregion
        #region 13FinishTheTuto
        currentAdvice1 = "You know now everything you need to know to play Foxy Flox.";
        currentAdvice2 = null; 
        #endregion
        #region 14End
        currentAdvice1 = "This is the end of this Tutorial";
        currentAdvice2 = "Good luck and Have fun playing Foxy Flox !";
        #endregion
    }




    public void ActivateDispenser()
    {
        for(int i =0; i <=dispenser.Count; i++)
        {
            dispenser[i].SetActive(true);
        }
        dispenserButton.SetActive(false);
    }
}
