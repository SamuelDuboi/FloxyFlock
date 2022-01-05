using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Representation : MonoBehaviour
{
    [SerializeField] private float modelRotationSpeed;
    [SerializeField] private Transform modelTransform;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    [HideInInspector] public int index;
    [HideInInspector] public GrabManager manager;
    [HideInInspector] public RawImage image;
    [HideInInspector] public bool isMalus;
    public bool isModifier;
    public bool isFireBall;
    [HideInInspector] public int indexInList;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponentInChildren<MeshFilter>();
    }

    private void Update()
    {
        RotateModel();
    }

    private void RotateModel()
    {
        if (meshFilter != null)
        {
            modelTransform.Rotate(new Vector3(0f, 0f, modelRotationSpeed) * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!manager.isOnCollision)
            manager.isOnCollision = true;
        if (isFireBall)
        {
            ((GrabManagerMulti)manager).GetPieceFireball(other.GetComponentInParent<XRDirectInteractor>());
            return;
        }
        if (!isModifier)
            manager.GetPiece(other.GetComponentInParent<XRDirectInteractor>(), index);
        else
            manager.GetPieceModifier(other.GetComponentInParent<XRDirectInteractor>(), index,isMalus,indexInList);

    }
    private void OnCollisionStay(Collision collision)
    {
        if (!manager.isOnCollision)
            manager.isOnCollision = true;
        if (isFireBall)
        {
            ((GrabManagerMulti)manager).GetPieceFireball(collision.gameObject.GetComponentInParent<XRDirectInteractor>());
            return;
        }
        if (!isModifier)
            manager.GetPiece(collision.gameObject.GetComponentInParent<XRDirectInteractor>(), index);
        else
            manager.GetPieceModifier(collision.gameObject.GetComponentInParent<XRDirectInteractor>(), index, isMalus, indexInList);
    }
    private void OnTriggerExit(Collider other)
    {
        if (manager.isOnCollision)
            manager.isOnCollision = false;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (manager.isOnCollision)
            manager.isOnCollision = false;
    }
}
