using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireballManager : MonoBehaviour
{
    public GameObject outFireball;
    public GameObject inFireball;
    [SerializeField] private Transform tableCenter = null;
    [SerializeField] public Transform rig = null;

    [SerializeField] private float exitDistance = 1f;
    [SerializeField] private float enterDistance = 1f;
    [SerializeField] private float frontLimitAngle = 45;
    [SerializeField] private float backLimitAngle = 135;
    [SerializeField] private float upLimitAngle = 50;
    [SerializeField] private float downLimitAngle = 70;

    [SerializeField] private float fireballSpeed = 1f;
    [SerializeField] private float explosionRadius = 0.5f;

    private Vector3 targetPosition;
    private SphereCollider fireballCollider;
    private float exitAngleXZ;
    private float exitAngleYZ;
    private int exitIndex = 0;

    private void Update()
    {
        if (outFireball == null || rig == null||fireballCollider == null)
            return;
        FindExitAngle();
        CheckForCollision();
    }

    public void Initialize()
    {
        fireballCollider = inFireball.GetComponent<SphereCollider>();
        
    }

    private void FindExitAngle()
    {
        if ( outFireball.activeSelf & !outFireball.GetComponent<XRGrabInteractable>().isSelected)
        {
            float fireballDistanceToCenter = Vector3.Distance(outFireball.transform.position, rig.position);

            if (fireballDistanceToCenter >= exitDistance)
            {
                Vector3 fireballRigVector = outFireball.transform.position - rig.position;

                exitAngleXZ = Vector3.SignedAngle(rig.transform.forward, new Vector3(fireballRigVector.x, 0f, fireballRigVector.z), Vector3.up);
                exitAngleYZ = Vector3.SignedAngle(rig.transform.forward, new Vector3(0f, fireballRigVector.y, fireballRigVector.z), Vector3.left);

                if ((downLimitAngle <= exitAngleYZ) && (exitAngleYZ <= 180 - downLimitAngle))
                {
                    exitIndex = 5; 
                    outFireball.SetActive(false); //TODO : Set inFireball back into the pooler (parent + position)
                    Debug.Log("Exit : Down");
                }
                else if ((-upLimitAngle >= exitAngleYZ) && (exitAngleYZ >= -180 + upLimitAngle))
                {
                    exitIndex = 4;
                    Debug.Log("Exit : Up");
                }
                else if ((-frontLimitAngle <= exitAngleXZ) && (exitAngleXZ <= frontLimitAngle))
                {
                    exitIndex = 0;
                    Debug.Log("Exit : Front");
                }
                else if ((frontLimitAngle < exitAngleXZ) && (exitAngleXZ < backLimitAngle))
                {
                    exitIndex = 1;
                    Debug.Log("Exit : Right");
                }
                else if ((-frontLimitAngle > exitAngleXZ) && (exitAngleXZ > -backLimitAngle))
                {
                    exitIndex = 3;
                    Debug.Log("Exit : Left");
                }
                else if ((exitAngleXZ >= backLimitAngle) || (exitAngleXZ <= -backLimitAngle))
                {
                    exitIndex = 2;
                    Debug.Log("Exit : Back");
                }

                if (exitIndex != 5)
                {
                    ExitEvent();
                }
                
            }
        }
    }

    private void ExitEvent()
    {
        //TODO : Set outFireball back into the pooler (parent + position)
        outFireball.SetActive(false);

        if (exitIndex != 5)
        {
            EnterEvent(exitIndex); //TODO : Instead, send a signal to server to trigger the other player enter event.
        }
    }

    private void EnterEvent(int exitIndex)
    {
        inFireball.SetActive(true);

        targetPosition = tableCenter.position; //TODO : Change to highest freezed flox in the player's tower.

        switch (exitIndex)
        {
            case 0:
                inFireball.transform.position = targetPosition + (Vector3.forward * enterDistance);
                break;
            case 1:
                inFireball.transform.position = targetPosition + (Vector3.right * enterDistance);
                break;
            case 2:
                inFireball.transform.position = targetPosition + (Vector3.back * enterDistance);
                break;
            case 3:
                inFireball.transform.position = targetPosition + (Vector3.left * enterDistance);
                break;
            case 4:
                inFireball.transform.position = targetPosition + (Vector3.up * enterDistance);
                break;
            case 5:
                inFireball.transform.position = targetPosition + (Vector3.down * enterDistance);
                break;
        }

        Vector3 FireballTableVector = targetPosition - inFireball.transform.position;

        inFireball.GetComponent<Rigidbody>().velocity = FireballTableVector * fireballSpeed;
    }

    private void CheckForCollision()
    {
        if (inFireball.activeSelf)
        {
            Collider[] fireballCollisions = Physics.OverlapSphere(inFireball.transform.position, fireballCollider.radius * inFireball.transform.localScale.x); //TODO : Remove reference to local scale once we have the right fireball mesh (not a scaled one).

            foreach (Collider collider in fireballCollisions)
            {
                if (collider.gameObject.layer == 11 || collider.tag == "Piece") //Hand layer index
                {
                    print("Fireball collided with : " + collider);
                    Explosion();
                    break;
                }
            }
        }
    }

    private void Explosion()
    {
        Collider[] explosionHits = Physics.OverlapSphere(inFireball.transform.position, explosionRadius);
        List<GameObject> floxesHit = new List<GameObject>();

        inFireball.SetActive(false); //TODO : Set inFireball back into the pooler (parent + position)

        //Go through each collidesr and add the corresponding flox to a list
        foreach (Collider collider in explosionHits)
        {
            if (collider.tag == "Piece")
            {
                GameObject flox = collider.transform.parent.parent.gameObject;

                if (!floxesHit.Contains(flox))
                {
                    floxesHit.Add(flox);
                }
            }
        }

        foreach (GameObject flox in floxesHit)
        {
            flox.GetComponent<FloxBurn>().BurnEvent();
        }
    }
}
