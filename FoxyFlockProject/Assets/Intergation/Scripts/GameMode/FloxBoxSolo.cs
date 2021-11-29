using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloxBoxSolo : GameModeSolo
{
    public Vector3 boxSize;
    public float timeToWin;
    public List<GameObject> FlocksToWin;

    [HideInInspector] public float timeInBox;

    public Box box;
    [HideInInspector] public Vector3 p;

    [HideInInspector] public int floxesToPlace;
    void Start()
    {
        box.transform.localScale = boxSize;
        p = box.transform.position;
        p.y = Tables[0].transform.GetChild(0).transform.position.y + boxSize.y/2;
        box.transform.position = p;

        //floxesToPlace = batch.l
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (box.objectsInBox.Count == floxesToPlace && hands.inPlayground == false)
        {
            tip = "can win";
            box.GetComponent<MeshRenderer>().material = box.winMat;
            timeInBox += Time.deltaTime;
        }
        else if (box.objectsInBox.Count != floxesToPlace && hands.inPlayground )
        {
            tip ="Place all you're flocks in the box";
            timeInBox = 0;
            box.GetComponent<MeshRenderer>().material = box.baseMat;
        }
        else if(box.objectsInBox.Count == floxesToPlace && hands.inPlayground)
        {
            tip = "hands out";
            timeInBox = 0;
            box.GetComponent<MeshRenderer>().material = box.defeatMat;
        }
        if (timeInBox >= timeToWin)
        {
            playerWin = true;
        }
    }
}
