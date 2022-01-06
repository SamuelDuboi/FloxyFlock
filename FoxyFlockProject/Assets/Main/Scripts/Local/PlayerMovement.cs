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
    public Vector3 movementMidle;
    private Vector3 deltaPose;
    private Vector3 initMidle;
    private CharacterStats characterStats;
    private Vector3 forwardOffset;
    private Vector3 rotateOffset ;
    private float upOffset;
    private float yPower;
    private float xPower;
    private float zPower;
    public Transform tempLeftHand;
    public Transform tempRighttHand;
    private bool isntLobby;
    // Start is called before the first frame update
    void Start()
    {
        if(!ScenesManager.instance.IsLobbyScene() && !ScenesManager.instance.IsMenuScene())
        {
            inputManager.OnCanMove.AddListener(Movement);
            inputManager.OnBothTrigger.AddListener(InitMovement);
            isntLobby = true;
        }
        characterStats = inputManager.characterStats;
        forwardOffset = new Vector3(0, characterStats.forwardYOffset, characterStats.forwardZOffset);
        rotateOffset = new Vector3(characterStats.RotateXOffset, characterStats.RotateYOffset, 0);
        upOffset = characterStats.upOffset;
        yPower = characterStats.yPower;
        xPower = characterStats.xPower;
        zPower = characterStats.zPower;


    }

    private void InitMovement(bool seeTable = false)
    {
        tempLeftHand.position = inputManager.leftHand.transform.position;
        tempRighttHand.position = inputManager.leftHand.transform.position;
        
        initMidle = Vector3.Lerp(tempLeftHand.localPosition, tempRighttHand.localPosition, 0.5f);
    }
    public bool SeeTable()
    {
        if (!isntLobby)
            return false;
        Vector3 pointOnScreen = vrHeadSett.GetComponent<Camera>().WorldToScreenPoint(tableRenderer.bounds.center);
        if ((pointOnScreen.x < 0) || (pointOnScreen.x > Screen.width) ||
            (pointOnScreen.y < 0) || (pointOnScreen.y > Screen.height))
        {
            InitMovement();
            return false;
        }
        if (pointOnScreen.z < 0)
        {
            InitMovement();
            return false;
        }
        return true;
    }
    private void Movement()
    {
        //check if table is on screen
        SeeTable();
        if (!tableRenderer)
            return;
        tempLeftHand.position = inputManager.leftHand.transform.position;
        tempRighttHand.position = inputManager.rightHand.transform.position;
        movementMidle = Vector3.Lerp(tempRighttHand.localPosition  , tempLeftHand.localPosition, 0.5f);
        //get delat pos 
        deltaPose = new Vector3((movementMidle.x - initMidle.x), (movementMidle.y - initMidle.y), (movementMidle.z - initMidle.z));
        #region testWithSnapTurn
        /* movementMidle = Vector3.Lerp(tempLeftHand.position  , tempLeftHand.position, 0.5f);
         deltaPose = new Vector3((movementMidle.x - initMidle.x), (movementMidle.y - initMidle.y), (movementMidle.z - initMidle.z));

         var zAngle = Vector3.Dot(vrHeadSett.forward, deltaPose);

         var xAngle = Vector3.Dot(vrHeadSett.right, deltaPose);

         if (Mathf.Abs(xAngle) > 1f || Mathf.Abs(zAngle) > 1)
             return;
           //if the amplutde of the movemnt is mor forward then side
         if (Mathf.Abs(zAngle) > Mathf.Abs(xAngle))
         {

             // if the movemnt is enough to move
             if (forwardOffset.z < Mathf.Abs(zAngle))
             {
                 //if is to close and want to come closer, return
                 if (Vector2.Distance(ToVector2XZ(transform.position), ToVector2XZ(tableTransform.position)) < Vector2.Distance(ToVector2XZ(zClampMin.position), ToVector2XZ(tableTransform.position)) && zAngle*characterStats.zPower*-1 < 0)
                 {
                     return;
                 }
                 //if is to far and want to go farer (is that english ?)
                 else if (Vector2.Distance(ToVector2XZ(transform.position), ToVector2XZ(tableTransform.position)) > Vector2.Distance(ToVector2XZ(zClampMax.position), ToVector2XZ(tableTransform.position)) && zAngle * characterStats.zPower * -1 > 0)
                     return;
                 //move the pos to the table
                 transform.position += new Vector3(tableTransform.position.x - vrHeadSett.transform.position.x, 0, tableTransform.position.z - vrHeadSett.transform.position.z).normalized * zAngle * 50*zPower*Time.deltaTime;
                 // the movement is enough to move forward and/or move upward
                 //MoveUp(forwardOffset);
                 Debug.Log(zAngle);
             }
             //else if can move up
             else
             {
                 MoveUp(upOffset);

             }

         }
         else if (Mathf.Abs(zAngle) < Mathf.Abs(xAngle))
         {
             // if the movemnt is enough to rotate
             if (rotateOffset.x < Mathf.Abs(xAngle))
             {
                 //move the pos to the table
                 transform.RotateAround(tableTransform.position, Vector3.up, xAngle * 180* xPower *Time.deltaTime);
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
         }*/
        #endregion
       
           //if the amplutde of the movemnt is mor forward then side
           if (Mathf.Abs(deltaPose.x) < Mathf.Abs(deltaPose.z))
           {

               // if the movemnt is enough to move
               if (forwardOffset.z < Mathf.Abs(deltaPose.z))
               {
                   //if is to close and want to come closer, return
                   if (Vector2.Distance(ToVector2XZ(transform.position), ToVector2XZ(tableTransform.position)) < Vector2.Distance(ToVector2XZ(zClampMin.position), ToVector2XZ(tableTransform.position)) && deltaPose.z*characterStats.zPower*-1 < 0)
                   {
                       return;
                   }
                   //if is to far and want to go farer (is that english ?)
                   else if (Vector2.Distance(ToVector2XZ(transform.position), ToVector2XZ(tableTransform.position)) > Vector2.Distance(ToVector2XZ(zClampMax.position), ToVector2XZ(tableTransform.position)) && deltaPose.z * characterStats.zPower * -1 > 0)
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
        transform.position += Vector3.up * (deltaPose.y) * yPower ;
    }
    private Vector2 ToVector2XZ(Vector3 a)
    {
        return new Vector2(a.x, a.z);
    }
}
