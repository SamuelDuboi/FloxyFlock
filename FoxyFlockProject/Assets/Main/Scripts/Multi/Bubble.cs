using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public SphereCollider spherCollider;
    public Transform m_transform;
    public LayerMask layerMask;
    public float radius;
    private GrabablePhysicsHandler _temp;
    public  bool isMalus;
    public bool isFireBall;
    public GrabManager grabManager;
    bool hasFlocks;
    public SoundReader sound;
    public GameObject destroyBubble;
    public GameObject startBubble;
    public GameObject particuleBubble;
    public MeshRenderer meshRenderer;
    public GameObject bubbleCore;
    private MaterialPropertyBlock propBlock;

    private void Start()
    {
        radius = spherCollider.radius *m_transform.lossyScale.x;
        
        spherCollider.enabled = false;
        int rand = Random.Range(1, 5);
        sound.clipName = "OrbPop" + rand.ToString();

        propBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        var collidiers = Physics.OverlapSphere(m_transform.position, radius, layerMask);

       
        for (int i = 0; i < collidiers.Length; i++)
        {
            _temp = collidiers[i].GetComponentInParent<GrabablePhysicsHandler>();
            if (_temp && !hasFlocks)
            {
                if (grabManager == null)
                {
                    if(GetComponentInParent<PlayGround>().GetComponentInChildren<GameModeSolo>().playerMovement!= null)
                    grabManager = GetComponentInParent<PlayGround>().GetComponentInChildren<GameModeSolo>().playerMovement.GetComponentInChildren<GrabManager>();
                    return;
                }
                if(isFireBall)
                    grabManager.AddFireBall(gameObject);
                 else
                    grabManager.AddBubble(isMalus, gameObject);

                UpdateBubbleMat(1);

                hasFlocks = true;
            }
        }
        
        if(collidiers.Length == 0 && grabManager != null && hasFlocks)
        {
            grabManager.RemoveBubble(isMalus, gameObject);

            UpdateBubbleMat(0);

            hasFlocks = false;
        }
    }

    public bool OnDestroyed(bool isFirst)
    {
        if (isFirst)
        {
            destroyBubble.transform.position = m_transform.position;
            destroyBubble.SetActive(true);
            meshRenderer.enabled = false;
            bubbleCore.SetActive(false);
        }
        //sound.Play();
        StartCoroutine(WaitForDestroy (isFirst));
        if (!hasFlocks)
            return false;
        hasFlocks = false;
        return true;
    }
    IEnumerator WaitForDestroy(bool isFirst)
    {
        yield return new WaitForSeconds(1.0f);
        OnSpawn(isFirst);
    }
    private void OnSpawn(bool isFirst)
    {
        if (isFirst)
        {
            destroyBubble.SetActive(false);
            startBubble.SetActive(true);
            startBubble.GetComponent<SoundReader>().Play();

        }
        startBubble.transform.position = transform.position;
        StartCoroutine(WaitForSpawn());

    }

    IEnumerator WaitForSpawn()
    {
        yield return new WaitForSeconds(2.5f);
        startBubble.SetActive(false);
        meshRenderer.enabled = true;
        bubbleCore.SetActive(true);
    }

    private void UpdateBubbleMat(int index)
    {
        //Recup Data
        meshRenderer.GetPropertyBlock(propBlock);
        //EditZone
        propBlock.SetFloat("State", index);
        //Push Data
        meshRenderer.SetPropertyBlock(propBlock);
    }
}
