using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bubble : MonoBehaviour
{
    public SphereCollider spherCollider;
    public Transform m_transform;
    public LayerMask layerMask;
    private float radius;
    private GrabablePhysicsHandler _temp;
    public  bool isMalus;
    public bool isFireBall;
    public GrabManager grabManager;
    bool hasFlocks;
    public SoundReader sound;
    private void Start()
    {
        radius = spherCollider.radius *m_transform.lossyScale.x;
        
        spherCollider.enabled = false;
        int rand = Random.Range(1, 5);
        sound.clipName = "OrbPop" + rand.ToString();
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
                    grabManager = GetComponentInParent<PlayGround>().GetComponentInChildren<GameModeSolo>().playerMovement.GetComponentInChildren<GrabManager>();
                if(isFireBall)
                    grabManager.AddFireBall(gameObject);
                 else
                    grabManager.AddBubble(isMalus, gameObject);
                hasFlocks = true;
            }
        }
        
        if(collidiers.Length == 0 && grabManager != null && hasFlocks)
        {
            grabManager.RemoveBubble(isMalus, gameObject);
            hasFlocks = false;
        }
    }

}
