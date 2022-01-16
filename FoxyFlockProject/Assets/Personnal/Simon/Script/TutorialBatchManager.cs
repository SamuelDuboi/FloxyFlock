using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;
using Mirror.Experimental;

public class TutorialBatchManager : GrabManager
{
    public TutorielManager tutoManager;

    private void Update()
    {
        if(batches.Count == tutoManager.batcheIdMax && tutoManager.batchToFinish)
        {
            tutoManager.tutorialStage++;
        }
    }
    public override void UpdateBatche()
    {
    int previousPool = currentPool;
        #region security test
        if (isFirstBacthPassed)
        {
            if (batches.Count >= tutoManager.batcheIdMax)
            {

                sound.clipName = "FloxMachineNotEmpty";
                sound.Play();
                Debug.LogError("You must follow the indications on the board to continue.");
                return;
            }

            if (!mainPool[currentPool].isEmpty)
            {

                sound.clipName = "FloxMachineNotEmpty";
                sound.Play();
                Debug.LogError("There are still flock on the dispenser");
                return;
            }
            if (!mainPool[currentPool].isEmptyModifier)
            {
                sound.clipName = "FloxMachineNotEmpty";
                sound.Play();
                Debug.LogError("There are still bonus or malus on the dispenser");
                return;
            }

        }
        else
        {
            previousPool = 5000;
            isFirstBacthPassed = true;
        }
        for (int i = 0; i < mainPool[currentPool].floxes.Count; i++)
        {
            if (mainPool[currentPool].floxes[i].GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
            {
                sound.clipName = "FloxMachineBad";
                sound.Play();
                Debug.LogError("Your floxes are still moving");
                return;
            }
            if (mainPool[currentPool].floxes[i].GetComponent<GrabbableObject>().isGrab)
            {
                sound.clipName = "FloxMachineBad";
                sound.Play();
                Debug.Log("grabbed");
                return;
            }
            if (mainPool[currentPool].floxes[i].GetComponent<GrabablePhysicsHandler>().enabled && !mainPool[currentPool].floxes[i].GetComponent<GrabablePhysicsHandler>().isOnPlayground)
            {
                sound.clipName = "FloxMachineBad";
                sound.Play();
                Debug.Log("Flock in stasis");
                return;
            }
        }
        #endregion

        int totalWeight = 0;
        for (int i = 0; i < batches.Count - 1; i++)
        {
            if (batches[i].isEmpty)
                continue;
            totalWeight += batches[i].weight;

        }

        currentPool++;
        if (currentPool == batches.Count)
        {
            sound.PlaySeconde();
            //inputManager.OnSpawn.RemoveListener(SpawnBacth);
            return;
        }
        //in testing
        if (previousPool < 5000)
            Freez(previousPool);

        UpdateMilestone();

        UpdateBoard();

        UpdateInventory();

        UpdateSpecial();

        UpdateBubble();


        sound.ThirdClipName = "FloxMachineGood";
        sound.PlayThird();
    }
}
