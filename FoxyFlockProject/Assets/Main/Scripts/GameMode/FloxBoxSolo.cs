using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloxBoxSolo : GameModeSolo
{
    public Vector3 boxSize;
    public float timeToWin;
    public List<GameObject> flocksToWin;
    [HideInInspector] public float timeInBox;

    public Box box;
    public MeshRenderer boxMesh;
    [HideInInspector] public Vector3 p;
    private int floxesToPlace;


    IEnumerator Start()
    {
        box.transform.localScale = boxSize;
        p = box.transform.position;
        p.y = tableTransform.position.y + boxSize.y/2;
        box.transform.position = p;

        yield return new WaitForEndOfFrame();
        floxesToPlace = ScenesManager.instance.numberOfFlocksInScene;
        //UIGlobalManager.instance.SetGameMode("Flock Box", "0");
        //floxesToPlace = batch.l
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (box.grabbableObjects.Count == floxesToPlace && hands.inPlayground == false)
        {
            tip = "can win";

            boxMesh.material = box.winMat;
            timeInBox += Time.deltaTime;
        }
        else if (box.grabbableObjects.Count != floxesToPlace /*&& hands.inPlayground*/ )
        {
            tip ="Place all you're flocks in the box";

            timeInBox = 0;
            boxMesh.material = box.baseMat;
        }
        else if(box.grabbableObjects.Count == floxesToPlace && hands.inPlayground)
        {
            tip = "hands out";

            timeInBox = 0;
            boxMesh.material = box.defeatMat;
        }

        if (timeInBox >= timeToWin)
        {
            NetworkManagerRace.instance.Win(1);

            playerWin = true;
        }
    }
}
