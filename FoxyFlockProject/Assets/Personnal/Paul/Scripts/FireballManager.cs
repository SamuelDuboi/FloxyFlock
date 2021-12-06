using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireballManager : MonoBehaviour
{
    [SerializeField] private GameObject outFireball = null;
    [SerializeField] private GameObject inFireball = null;
    [SerializeField] private Transform tableCenter = null;
    [SerializeField] private Transform rig = null;

    [SerializeField] private float exitDistance = 1f;
    [SerializeField] private float enterDistance = 1f;
    [SerializeField] private float frontLimitAngle = 45;
    [SerializeField] private float backLimitAngle = 135;
    [SerializeField] private float upLimitAngle = 50;
    [SerializeField] private float downLimitAngle = 70;

    [SerializeField] private float fireballSpeed = 1f;
    [SerializeField] private float explosionRadius = 0.5f;

    private GameObject _outFireball;
    private GameObject _inFireball;
    private Vector3 targetPosition;
    private SphereCollider fireballCollider;
    private float exitAngleXZ;
    private float exitAngleYZ;
    private int exitIndex = 0;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        FindExitAngle();
        CheckForHandCollision();
    }

    private void Initialize()
    {
        _outFireball = Instantiate(outFireball); // TODO : Instantiate in the right pull / position
        _inFireball = Instantiate(inFireball); // TODO : Instantiate in the right pull / position

        fireballCollider = _inFireball.GetComponent<SphereCollider>();

        _outFireball.SetActive(false);
        _inFireball.SetActive(false);
    }

    private void FindExitAngle()
    {
        if (_outFireball.activeSelf & !_outFireball.GetComponent<XRGrabInteractable>().isSelected)
        {
            float fireballDistanceToCenter = Vector3.Distance(_outFireball.transform.position, rig.position);

            if (fireballDistanceToCenter >= exitDistance)
            {
                Vector3 fireballRigVector = _outFireball.transform.position - rig.position;

                exitAngleXZ = Vector3.SignedAngle(rig.transform.forward, new Vector3(fireballRigVector.x, 0f, fireballRigVector.z), Vector3.up);
                exitAngleYZ = Vector3.SignedAngle(rig.transform.forward, new Vector3(0f, fireballRigVector.y, fireballRigVector.z), Vector3.right);

                if (exitAngleYZ >= downLimitAngle)
                {
                    exitIndex = 5;
                }
                else if (exitAngleYZ <= upLimitAngle)
                {
                    exitIndex = 4;
                }
                else if ((-frontLimitAngle <= exitAngleXZ) && (exitAngleXZ <= frontLimitAngle))
                {
                    exitIndex = 0;
                }
                else if ((frontLimitAngle < exitAngleXZ) && (exitAngleXZ < backLimitAngle))
                {
                    exitIndex = 1;
                }
                else if ((-frontLimitAngle > exitAngleXZ) && (exitAngleXZ > -backLimitAngle))
                {
                    exitIndex = 3;
                }
                else if ((exitAngleXZ >= backLimitAngle) || (exitAngleXZ <= -backLimitAngle))
                {
                    exitIndex = 2;
                }

                ExitEvent();
            }
        }
    }

    private void ExitEvent()
    {
        //TODO : Set outFireball back into the pooler (parent + position)
        _outFireball.SetActive(false);

        EnterEvent(exitIndex); //TODO : Instead, send a signal to server to trigger the other player enter event.
    }

    private void EnterEvent(int _exitIndex)
    {
        _inFireball.SetActive(true);

        targetPosition = tableCenter.position; //TODO : Change to highest freezed flox in the player's tower.

        switch (_exitIndex)
        {
            case 0:
                _inFireball.transform.position = targetPosition + (Vector3.forward * enterDistance);
                break;
            case 1:
                _inFireball.transform.position = targetPosition + (Vector3.right * enterDistance);
                break;
            case 2:
                _inFireball.transform.position = targetPosition + (Vector3.left * enterDistance);
                break;
            case 3:
                _inFireball.transform.position = targetPosition + (Vector3.back * enterDistance);
                break;
            case 4:
                _inFireball.transform.position = targetPosition + (Vector3.up * enterDistance);
                break;
            case 5:
                _inFireball.transform.position = targetPosition + (Vector3.down * enterDistance);
                break;
        }

        Vector3 FireballTableVector = targetPosition - _inFireball.transform.position;

        _inFireball.GetComponent<Rigidbody>().velocity = FireballTableVector * fireballSpeed;
    }

    private void CheckForHandCollision()
    {
        if (_inFireball.activeSelf)
        {
            Collider[] fireballCollisions = Physics.OverlapSphere(_inFireball.transform.position, fireballCollider.radius);

            foreach (Collider collider in fireballCollisions)
            {
                if (collider.gameObject.layer == 11) //Hand layer index
                    Explosion();
                break;
            }
        }
    }

    private void Explosion()
    {
        Collider[] explosionHits = Physics.OverlapSphere(_inFireball.transform.position, explosionRadius);
        List<GameObject> floxesHit = new List<GameObject>();

        _inFireball.SetActive(false); //TODO : Set inFireball back into the pooler (parent + position)

        //Go through each collidesr and add the corresponding flox to a list
        foreach (Collider collider in explosionHits)
        {
            if (collider.tag == "piece")
            {
                GameObject _flox = collider.transform.parent.parent.gameObject;

                if (!floxesHit.Contains(_flox))
                {
                    floxesHit.Add(_flox);
                }
            }
        }

        foreach (GameObject flox in floxesHit)
        {
            flox.GetComponent<FloxBurn>().BurnEvent();
        }
    }
}
