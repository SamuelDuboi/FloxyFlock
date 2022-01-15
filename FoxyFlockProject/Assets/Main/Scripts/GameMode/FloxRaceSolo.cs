using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloxRaceSolo : GameModeSolo
{
    public float limitHeight;
    private float limitDiameter;
    public float timeToWin;
    private float timeAboveHeight;
    public Limits winLimit;
    [HideInInspector] public Vector3 p;
    private MaterialPropertyBlock propBlock;
    [SerializeField] private MeshRenderer meshRenderer;

    void Start()
    {
        p = winLimit.transform.position;
        p.y = tableTransform.position.y + limitHeight;
        winLimit.transform.position = p;
        //UIGlobalManager.instance.SetGameMode("Flock Race","0");
        //winLimit.gameObject.diameter = limitDiameter;
        //winLimit.gameObject.diameter = limitDiameter;

        propBlock = new MaterialPropertyBlock();

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (winLimit.triggered && hands.inPlayground == false)
        {
            tip = "can win";

            UpdateLimitMat(2);

            timeAboveHeight += Time.deltaTime;
            if (playerMovement == null)
                UIGlobalManager.instance.Validation(number, false, timeAboveHeight, timeToWin);
            else
                playerMovement.grabManager.GetComponent<GrabManagerMulti>().multiUI.CmdValidate(number, false, timeAboveHeight, timeToWin);
        } 
        else if (winLimit.triggered && hands.inPlayground == true)
        {
            tip = "hands out";
            if (playerMovement == null)
                UIGlobalManager.instance.Validation(number, true);
            else
                playerMovement.grabManager.GetComponent<GrabManagerMulti>().multiUI.CmdValidate(number, true,0,0);
            timeAboveHeight = 0;

            UpdateLimitMat(1);
        }
        else
        {
            tip = "try too reach height";
            timeAboveHeight = 0;

            UpdateLimitMat(0);
        }
        if (timeAboveHeight >= timeToWin)
        {
            if (playerMovement)
            {
                if (number == 0)
                    playerMovement.CmdWin1();
                else
                    playerMovement.CmdWin2();
            }
            else
            {
                UIGlobalManager.instance.Win(0);
            }
           
            Destroy(this);
        }
    }

    private void UpdateLimitMat(int index)
    {
        //Recup Data
        meshRenderer.GetPropertyBlock(propBlock);
        //EditZone
        propBlock.SetFloat("SelectedTint", index);
        //Push Data
        meshRenderer.GetComponent<MeshRenderer>().SetPropertyBlock(propBlock);
    }
}
