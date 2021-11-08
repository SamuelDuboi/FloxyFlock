using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public InputManager inputManager;
    public Transform vrHeadSett;
    public Renderer tableRenderer;
    public Transform tableTransform;
    public Transform zClampMin;
    public Transform zClampMax;
    public Transform upClamp;
    public Transform downClamp;
    private Vector3 movementMidle;
    private Vector3 deltaPose;
    private Vector3 initMidle;
    private CharacterStats characterStats;
    private Vector3 forwardOffset;
    private Vector3 rotateOffset ;
    private float upOffset;
    private float yPower;
    private float xPower;
    private float zPower;

    // Start is called before the first frame update
    void Start()
    {
        inputManager.OnCanMove.AddListener(Movement);
        inputManager.OnBothTrigger.AddListener(InitMovement);
        characterStats = inputManager.characterStats;
        forwardOffset = new Vector3(0, characterStats.forwardYOffset, characterStats.forwardZOffset);
        rotateOffset = new Vector3(characterStats.RotateXOffset, characterStats.RotateYOffset, 0);
        upOffset = characterStats.upOffset;
        yPower = characterStats.yPower;
        xPower = characterStats.xPower;
        zPower = characterStats.zPower;
    }

    private void InitMovement()
    {
        initMidle = Vector3.Lerp(inputManager.leftHand.transform.localPosition, inputManager.rightHand.transform.localPosition, 0.5f);
    }
    private void Movement()
    {
        //check if table is on screen
        Vector3 pointOnScreen = vrHeadSett.GetComponent<Camera>().WorldToScreenPoint(tableRenderer.bounds.center);
        if ((pointOnScreen.x < 0) || (pointOnScreen.x > Screen.width) ||
            (pointOnScreen.y < 0) || (pointOnScreen.y > Screen.height))
        {
            InitMovement();
            return;
        }
        if (pointOnScreen.z < 0)
        {
            InitMovement();
            return;
        }
        movementMidle = Vector3.Lerp(inputManager.leftHand.transform.localPosition, inputManager.rightHand.transform.localPosition, 0.5f);

        //get delat pos 
        deltaPose = new Vector3((movementMidle.x - initMidle.x), (movementMidle.y - initMidle.y), (movementMidle.z - initMidle.z));

        //if the amplutde of the movemnt is mor forward then side
        if (Mathf.Abs(deltaPose.x) < Mathf.Abs(deltaPose.z))
        {

            // if the movemnt is enough to move
            if (forwardOffset.z < Mathf.Abs(deltaPose.z))
            {
                //if is to close and want to come closer, return
                if (Vector2.Distance(ToVector2XZ(vrHeadSett.position), ToVector2XZ(tableTransform.position)) < Vector2.Distance(ToVector2XZ(zClampMin.position), ToVector2XZ(tableTransform.position)) && deltaPose.z < 0)
                {
                    return;
                }
                //if is to far and want to go farer (is that english ?)
                else if (Vector2.Distance(ToVector2XZ(vrHeadSett.position), ToVector2XZ(tableTransform.position)) > Vector2.Distance(ToVector2XZ(zClampMax.position), ToVector2XZ(tableTransform.position)) && deltaPose.z > 0)
                    return;
                //move the pos to the table
                transform.position += new Vector3(tableTransform.position.x - vrHeadSett.transform.position.x, 0, tableTransform.position.z - vrHeadSett.transform.position.z).normalized * deltaPose.z * zPower;
                // the movement is enough to move forward and/or move upward
                MoveUp(forwardOffset);
            }
            //else if can move up
            else
            {
                MoveUp(upOffset);

            }

        }
        else if (Mathf.Abs(deltaPose.x) > Mathf.Abs(deltaPose.z))
        {
            // if the movemnt is enough to rotate
            if (rotateOffset.x < Mathf.Abs(deltaPose.x))
            {
                //move the pos to the table
                transform.RotateAround(tableTransform.position, Vector3.up, deltaPose.x * 180 * xPower);
                // the movement is enough to move forward and/or move upward
                MoveUp(rotateOffset);
            }
            //else if can move up
            else
            {
                MoveUp(upOffset);
            }
        }
        //in the weird case that the player move the same amount in x and in z
        //if move enough in y axis
        else
        {
            MoveUp(upOffset);
        }

        initMidle = movementMidle;
    }
    private void MoveUp(Vector3 offset)
    {

        if (vrHeadSett.position.y - upClamp.position.y > 0 && deltaPose.y < 0)
            return;

        else if (vrHeadSett.position.y - downClamp.position.y < 0 && deltaPose.y > 0)
            return;

        transform.position += Vector3.up * (deltaPose.y) * yPower;

    }
    private void MoveUp(float offset)
    {

        if (vrHeadSett.position.y - upClamp.position.y > 0 && deltaPose.y < 0)
            return;
        else if (vrHeadSett.position.y - downClamp.position.y < 0 && deltaPose.y > 0)
            return;
        transform.position += Vector3.up * (deltaPose.y) * yPower;
    }
    private Vector2 ToVector2XZ(Vector3 a)
    {
        return new Vector2(a.x, a.z);
    }
}
