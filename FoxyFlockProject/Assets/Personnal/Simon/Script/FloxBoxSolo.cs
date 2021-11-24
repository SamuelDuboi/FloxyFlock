using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloxBoxSolo : GameModeSolo
{
    public Vector3 boxSize;
    public float timeToWin;

    [HideInInspector] public float timeInBox;

    public Limits box;
    [HideInInspector] public Vector3 p;

    public int floxesToPlace;
    void Start()
    {
        gameObject.transform.GetChild(0).transform.localScale = boxSize;
        p = box.transform.position;
        p.y = Tables[0].transform.GetChild(0).transform.position.y + boxSize.y/2;
        box.transform.position = p;

        //floxesToPlace = batch.l
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (box.triggered && hands.inPlayground == false)
        {
            Debug.Log("can win");
            winLimit.GetComponent<MeshRenderer>().material = winLimit.winMat;
            timeInBox += Time.deltaTime;
        }
        else if (winLimit.triggered && hands.inPlayground == true)
        {
            Debug.Log("hands out");
            timeInBox = 0;
            winLimit.GetComponent<MeshRenderer>().material = winLimit.defeatMat;
        }
        else
        {
            Debug.Log("try too reach height");
            timeInBox = 0;
            winLimit.GetComponent<MeshRenderer>().material = winLimit.baseMat;
        }
        if (timeInBox >= timeToWin)
        {
            playerWin = true;
        }
    }
}
