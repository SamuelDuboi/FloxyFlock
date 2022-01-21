using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;
using Mirror;

public class FireballManager : MonoBehaviour
{
    [Header("Set in hierarchy")]
    public Collider portalCollider;
    
    public Transform portalTransform;
    [SerializeField] private SoundReader portalSoundReader;
    public Transform limite;
    [SerializeField] private Transform explosionTransform;
    [SerializeField] private SoundReader explosionSFX;
    [SerializeField] private SoundReader managerSoundReader;
    [SerializeField] private ParticleSystem explosionVFX;
    [SerializeField] private Transform tableCenter = null;
    [SerializeField] private LayerMask explosionLayer;

    [Header("Set in code")]
    public GameObject outFireball;
    public GameObject inFireball;
    public DetectionHUD detectionHUD;
    public GrabManager grabManager;
    public PlayerMovementMulti playerMovement;

    [Header("Balancing")]
    [SerializeField] private float portalOpeningDuration = 1f;
    [SerializeField] private float portalClosingDuration = 0.5f;
    [SerializeField] private float portalSpawnHeight = 0.2f;
    [SerializeField] private float timeBeforeFireballSpawn = 3f;
    [SerializeField] private float fireballMinSpawnDistance = 0.5f;
    [SerializeField] private float fireballMaxSpawnDistance = 1.5f;
    [SerializeField] private float fireballSpeed = 1f;
    [SerializeField] private float fireballMaxScaleMultiply = 3f;
    [SerializeField] [Range(0f, 1f)] private float fireballMaxLerpDistance = 0.5f;
    [SerializeField] private float explosionRadius = 0.5f;

    [HideInInspector] public bool isPortalOpen = false;
    [HideInInspector] public bool isPortalOpenning = false;
    [HideInInspector] public bool isPortalClosing = false;
    [HideInInspector] public bool isFireballArriving = false;
    [HideInInspector] public Transform rig = null;

    [SerializeField] private int otherPlayerIndex;

    private MultiUIHandler multiUI;
    private bool canLerp = false;
    private bool canDetectTarget = false;
    private Vector3 fireballSpawnPosition = Vector3.zero;
    private Vector3 fireballTargetPosition;
    private Vector3 baseFireballScale;
    private Vector3 startLerpPosition;
    private Vector3 endLerpPosition;
    private Vector3 inFireballInitPos;
    private Vector3 outFireballInitPos;
    private Vector3 initPortalPos;
    public void Initialize()
    {
        inFireball.GetComponent<InFireball>().fireballManager = this;
        outFireball.GetComponent<OutFireball>().fireballManager = this;

        ParticleSystem.MainModule main = explosionVFX.main;
        main.startSize = explosionRadius * 2;
        inFireballInitPos = inFireball.transform.position;
        outFireballInitPos = outFireball.transform.position;
        baseFireballScale = inFireball.transform.localScale;
        initPortalPos = portalTransform.position;
        playerMovement = grabManager.GetComponentInParent<PlayerMovementMulti>();
    }

    private void Update()
    {/*
        if (canDetectTarget)
        {
            if (inFireball.activeSelf)
                detectionHUD.CheckAndDisplayTargetDirection(inFireball.transform.position);
            else if (isFireballArriving)
                detectionHUD.CheckAndDisplayTargetDirection(fireballSpawnPosition);
        }
*/
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

            playerMovement.CmdMoveObject(portalTransform.gameObject, new Vector3(tableCenter.position.x, limite.position.y + portalSpawnHeight, tableCenter.position.z));
            portalSoundReader.PlaySeconde();

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
            playerMovement.CmdMoveObject(portalTransform.gameObject, initPortalPos);
            isPortalClosing = false;
            isPortalOpen = false;
        }
    }
    public void FireballHitPortal()
    {
        outFireball.transform.position = outFireballInitPos;
        if (multiUI == null)
            multiUI = GetComponentInParent<PlayGround>().GetComponentInChildren<GameModeSolo>().playerMovement.grabManager.GetComponent<GrabManagerMulti>().multiUI;

        multiUI.CmdFireBallIncoming();
        multiUI.GetComponentInParent<PlayerMovementMulti>().CmdSpawnInFireBall(NetworkManagerRace.instance.players[otherPlayerIndex]);

        StartCoroutine(TryClosePortal());
    }

    public IEnumerator FireballIncoming()
    {
        isFireballArriving = true;
        //CHANGE LIGHTING + DISPLAY BOARD INFO + PLAY ALERT SOUND TO PLAYER
        managerSoundReader.Play();

        yield return new WaitForSeconds(5f);

        StartCoroutine(TryOpenPortal());

        yield return new WaitForSeconds(portalOpeningDuration);

        fireballSpawnPosition = FireballSpawnPosition();
        canDetectTarget = true;

        yield return new WaitForSeconds(timeBeforeFireballSpawn);

        inFireball.transform.position = fireballSpawnPosition;
        inFireball.SetActive(true);
        portalSoundReader.Play();
        if (multiUI == null)
            multiUI = GetComponentInParent<PlayGround>().GetComponentInChildren<GameModeSolo>().playerMovement.grabManager.GetComponent<GrabManagerMulti>().multiUI;
       multiUI.CmdUnSelectFireBall();
        if (grabManager.positionOfMilestoneIntersection != Vector3.zero)
            fireballTargetPosition = grabManager.positionOfMilestoneIntersection;
        else
            fireballTargetPosition = tableCenter.position;

        Vector3 fireballToTarget = fireballTargetPosition - inFireball.transform.position;
        inFireball.GetComponent<Rigidbody>().AddForce(fireballToTarget * fireballSpeed, ForceMode.Impulse);
        //StartFireballLerp();
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

        Vector3 fireballSpawnPoint = new Vector3(tableCenter.transform.position.x + Mathf.Cos(fireballSpawnAngle) * fireballSpawnDistance, portalTransform.position.y, tableCenter.transform.position.z + Mathf.Sin(fireballSpawnDistance) * fireballSpawnDistance);

        return fireballSpawnPoint;
    }

    public void LunchExplosion()
    {
        playerMovement.CmdExplosion(inFireball.transform.position);
    }

    public void Explosion(Vector3 position)
    {
        Collider[] explosionHits = Physics.OverlapSphere(inFireball.transform.position, explosionRadius, explosionLayer);
        List<GameObject> floxesHit = new List<GameObject>();
        if (multiUI == null)
            multiUI = GetComponentInParent<PlayGround>().GetComponentInChildren<GameModeSolo>().playerMovement.grabManager.GetComponent<GrabManagerMulti>().multiUI;

        multiUI.CmdFireBallNotIncoming();
        explosionTransform.position =position;
        explosionVFX.Play();
        explosionSFX.Play();

        inFireball.transform.position = inFireballInitPos;

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
            if (flox.GetComponent<GrabbableObject>())
            {
                GrabbableObject floxInteractable = flox.GetComponent<GrabbableObject>();
                if (floxInteractable.isSelected)
                {
                    InteractionManager.instance.SelectExit(floxInteractable.currentInteractor, floxInteractable);
                }
            }
            flox.GetComponent<FloxBurn>().BurnEvent();
        }
        isFireballArriving = false;
       StartCoroutine( TryClosePortal());
        canDetectTarget = false;

    }

    [ContextMenu("Call Fireball To Self")]
    public void CallFireballInEditor()
    {
        StartCoroutine(FireballIncoming());
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;

        Vector3 wireDiscPosition = Vector3.zero;

        if (tableCenter != null)
        {
            wireDiscPosition = tableCenter.position + Vector3.up * portalSpawnHeight;
        }
        else
        {
            wireDiscPosition = transform.position + Vector3.up * portalSpawnHeight;
        }

        Handles.DrawWireDisc(wireDiscPosition, Vector3.up, fireballMinSpawnDistance);
        Handles.DrawWireDisc(wireDiscPosition, Vector3.up, fireballMaxSpawnDistance);
    }
#endif
}
