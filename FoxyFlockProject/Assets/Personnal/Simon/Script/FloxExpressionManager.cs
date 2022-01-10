using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloxExpressionManager : MonoBehaviour
{
    public Material floxMat;
    public MeshRenderer flox;
    public Rigidbody rb;
    public GrabbableObject GrabbableFlox;
    public Animator floxanimator;

    public bool isFroze;
    public bool baseFace;
    public bool sleepFace;
    public bool panicFace;
    public List<AudioClip> floxReactions;


    void Start()
    {
        baseFace = true;
    }

    void Update()
    {
        if (floxMat.GetFloat("IsFrozen") == 1)
        {
            isFroze = true;
        }

            if (isFroze )
        {
            baseFace = false;
            floxanimator.SetBool("Base", false);
            panicFace = false;
            floxanimator.SetBool("Panic", false);
            sleepFace = true;
            floxanimator.SetBool("Sleep",true);
        }

        else if (isFroze == false && GrabbableFlox != null && GrabbableFlox.isGrab == false && rb!= null&& rb.velocity != Vector3.zero)
        {
            
            baseFace = false;
            floxanimator.SetBool("Base", false);
            sleepFace = false;
            floxanimator.SetBool("Sleep", false);
            panicFace = true;
            floxanimator.SetBool("Panic", true);
        }
        else if (isFroze == false)
        {
            panicFace = false;
            floxanimator.SetBool("Panic", false);
            sleepFace = false;
            floxanimator.SetBool("Sleep", false);
            baseFace = true;
            floxanimator.SetBool("Base", true);
        }
    }
}
