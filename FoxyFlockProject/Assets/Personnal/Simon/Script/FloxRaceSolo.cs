using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloxRaceSolo : GameModeSolo
{
    public float limitHeight;
    private float limitDiameter;
    private float timeToWin;

    private float timeAboveHeight;

    public Limits winLimit;
    public Vector3 p;
    void Start()
    {
        p = winLimit.transform.position;
        p.y = Tables[0].transform.GetChild(0).transform.position.y + limitHeight;
        winLimit.transform.position = p;

        //winLimit.gameObject.diameter = limitDiameter;
        //winLimit.gameObject.diameter = limitDiameter;

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (winLimit.triggered && hands.inPlayground == false)
        {
            winLimit.GetComponent<MeshRenderer>().material = winLimit.winMat;
            timeAboveHeight = Time.deltaTime;
        } else if (winLimit.triggered && hands.inPlayground == true)
        {
            timeAboveHeight = 0;
            winLimit.GetComponent<MeshRenderer>().material = winLimit.defeatMat;
        }
        else
        {
            timeAboveHeight = 0;
            winLimit.GetComponent<MeshRenderer>().material = winLimit.baseMat;
        }
        if (timeAboveHeight >= timeToWin)
        {
            playerWin = true;
        }
    }
}
