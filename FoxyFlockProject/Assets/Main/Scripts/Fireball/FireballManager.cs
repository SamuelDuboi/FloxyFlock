using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireballManager : MonoBehaviour
{
    //Set in hierarchy
    public GameObject outFireball;
    public GameObject inFireball;
    public GameObject portal;

    [SerializeField] private Transform tableCenter = null;
    [SerializeField] private DetectionHUD detectionHUD;
    [SerializeField] private GrabManager grabManager;

    [SerializeField] private float portalOpeningDuration = 1f;
    [SerializeField] private float portalClosingDuration = 0.5f;
    [SerializeField] private float portalSpawnHeight = 1f;

    [SerializeField] private float timeBeforeFireballSpawn = 3f;
    [SerializeField] private float fireballMinSpawnDistance = 0.5f;
    [SerializeField] private float fireballMaxSpawnDistance = 1.5f;
    [SerializeField] private float fireballSpeed = 1f;
    [SerializeField] private float fireballMaxScaleMultiply = 3f;
    [SerializeField] [Range(0f, 1f)] private float fireballMaxLerpDistance = 0.5f;
    [SerializeField] private float explosionRadius = 0.5f;
    [SerializeField] private LayerMask explosionLayer;

    [HideInInspector] public bool isPortalOpen = false;
    [HideInInspector] public bool isPortalOpenning = false;
    [HideInInspector] public bool isPortalClosing = false;
    [HideInInspector] public bool isFireballArriving = false;
    [HideInInspector] public Transform rig = null;

    [SerializeField] private int otherPlayerIndex;

    private bool canLerp = false;
    private bool canDetectTarget = false;
    private MeshRenderer portalRenderer;
    private Collider portalCollider;
    private Vector3 fireballSpawnPosition = Vector3.zero;
    private Vector3 fireballTargetPosition;
    private Vector3 baseFireballScale;
    private Vector3 startLerpPosition;
    private Vector3 endLerpPosition;

    public void Initialize()
    {
        portalRenderer = portal.GetComponent<MeshRenderer>();
        portalCollider = portal.GetComponent<Collider>();

        inFireball.GetComponent<InFireball>().fireballManager = this;
        outFireball.GetComponent<OutFireball>().fireballManager = this;

        baseFireballScale = inFireball.transform.localScale;
    }

    private void Update()
    {
        if (canDetectTarget)
        {
            if (inFireball.activeSelf)
                detectionHUD.CheckAndDisplayTargetDirection(inFireball.transform.position);
            else if (isFireballArriving)
                detectionHUD.CheckAndDisplayTargetDirection(fireballSpawnPosition);
        }

        if (canLerp)
        {
            LerpFireballScale();
        }
    }

    public IEnumerator TryOpenPortal()
    {
        if (isPortalClosing)
        {
            StopCoroutine(TryClosePortal());
            isPortalClosing = false;
        }

        if (!isPortalOpen && !isPortalOpenning)
        {
            isPortalOpenning = true;

            portal.transform.position = new Vector3(tableCenter.position.x, grabManager.positionOfMilestoneIntersection.y + portalSpawnHeight, tableCenter.position.z);
            portalRenderer.enabled = true;

            //TODO : Add portal openning effect here

            yield return new WaitForSeconds(portalOpeningDuration);

            portalCollider.enabled = true;

            isPortalOpenning = false;
            isPortalOpen = true;
        }
    }

    public IEnumerator TryClosePortal()
    {
        if (isPortalOpenning && !isFireballArriving && !outFireball.GetComponent<GrababbleFireball>().isSelected)
        {
            StopCoroutine(TryOpenPortal());
            isPortalOpenning = false;
        }

        if (isPortalOpen && !isPortalOpenning && !isPortalClosing && !isFireballArriving && !outFireball.GetComponent<GrabbableObject>().isSelected)
        {
            isPortalClosing = true;

            portalCollider.enabled = false;

            //TODO : Add portal closing effect here

            yield return new WaitForSeconds(portalClosingDuration);

            portalRenderer.enabled = false;

            isPortalClosing = false;
            isPortalOpen = false;
        }
    }
    public void FireballHitPortal()
    {
        outFireball.SetActive(false);
        outFireball.transform.position = Vector3.zero; //To avoid triggering the fireball again when regrabbing

        NetworkManagerRace.instance.playerController.CmdSpawnInFireBall(NetworkManagerRace.instance.players[otherPlayerIndex]);

        StartCoroutine(TryClosePortal());
    }

    public IEnumerator FireballIncoming()
    {
        isFireballArriving = true;
        //CHANGE LIGHTING + DISPLAY BOARD INFO + PLAY ALERT SOUND TO PLAYER
        StartCoroutine(TryOpenPortal());

        yield return new WaitForSeconds(portalOpeningDuration);

        fireballSpawnPosition = FireballSpawnPosition();
        canDetectTarget = true;

        yield return new WaitForSeconds(timeBeforeFireballSpawn);

        inFireball.SetActive(true);
        inFireball.transform.position = fireballSpawnPosition;
        //ADD FIREBALL SPAWN FEEDBACK HERE

        fireballTargetPosition = grabManager.positionOfMilestoneIntersection;
        Vector3 fireballToTarget = fireballTargetPosition - inFireball.transform.position;
        inFireball.GetComponent<Rigidbody>().AddForce(fireballToTarget * fireballSpeed, ForceMode.Impulse);

        StartFireballLerp();
    }

    private void StartFireballLerp()
    {
        Vector3 startPosition = fireballSpawnPosition;
        Vector3 endPosition = fireballMaxLerpDistance * (fireballSpawnPosition + fireballTargetPosition);
        canLerp = true;
    }

    private void LerpFireballScale()
    {
        Vector3 startToEnd = endLerpPosition - startLerpPosition;
        Vector3 startToFireball = inFireball.transform.position - startLerpPosition;
        float fireballProgression = startToFireball.magnitude / startToEnd.magnitude;

        inFireball.transform.localScale = Vector3.Lerp(baseFireballScale * fireballMaxScaleMultiply, baseFireballScale, fireballProgression);

        if (startToFireball.magnitude >= startToEnd.magnitude)
        {
            inFireball.transform.localScale = baseFireballScale;
            canLerp = false;
        }
            
    }

    private Vector3 FireballSpawnPosition()
    {
        //Randomize distance and angle
        float fireballSpawnDistance = Random.Range(fireballMinSpawnDistance, fireballMaxSpawnDistance);
        float fireballSpawnAngle = Random.Range(0f, 2 * Mathf.PI);

        Vector3 fireballSpawnPoint = tableCenter.transform.right; 
        fireballSpawnPoint = new Vector3((fireballSpawnPoint.x * Mathf.Cos(fireballSpawnAngle) - fireballSpawnPoint.z * Mathf.Sin(fireballSpawnAngle)), tableCenter.transform.position.y + portalSpawnHeight, (fireballSpawnPoint.x * Mathf.Cos(fireballSpawnAngle) - fireballSpawnPoint.z * Mathf.Sin(fireballSpawnAngle)));
        fireballSpawnPoint *= fireballSpawnDistance;

        return fireballSpawnPoint;
    }

    public void Explosion()
    {
        Collider[] explosionHits = Physics.OverlapSphere(inFireball.transform.position, explosionRadius, explosionLayer);
        List<GameObject> floxesHit = new List<GameObject>();

        inFireball.GetComponent<SoundReader>().Play();
        inFireball.SetActive(false);

        //Go through each colliders and add the corresponding flox to a list
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

        //Destroy every floxes in the list
        foreach (GameObject flox in floxesHit)
        {
            GrabbableObject floxInteractable = flox.GetComponent<GrabbableObject>();

            if (floxInteractable.isSelected)
            {
                InteractionManager.instance.SelectExit(floxInteractable.currentInteractor, floxInteractable);
            }

            flox.GetComponent<FloxBurn>().BurnEvent();
        }
    }
}
