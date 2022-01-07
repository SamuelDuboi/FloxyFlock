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

    [HideInInspector] private int index;
    [HideInInspector] private GrabManager manager;
    public MeshFilter mesh;
    public MeshRenderer meshMat;
    [HideInInspector] private bool isMalus;
    public bool isModifier;
    public bool isFireBall;
     public int indexInList;
    private MaterialPropertyBlock propBlock;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponentInChildren<MeshFilter>();
        propBlock = new MaterialPropertyBlock();
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

    public void ApplyVisual(int _index, GrabManager grabManager)
    {
        index = _index;
        manager = grabManager;

    }
    public void ApplyVisual(GrabManager grabManager, Mesh _mesh, Material _mat)
    {
        manager = grabManager;
        mesh.mesh = _mesh;
        meshMat.material = _mat;
    }
    public void ApplyVisual(int _index, GrabManager grabManager, Mesh _mesh, Material _mat)
    {
        index = _index;
        manager = grabManager;
        mesh.mesh = _mesh;
        meshMat.material = _mat;
        if (propBlock == null)
            propBlock = new MaterialPropertyBlock();
        meshMat.GetPropertyBlock(propBlock);
        propBlock.SetInt("SelectedColorTint", 3);
        meshMat.SetPropertyBlock(propBlock);
    }
    public void ApplyVisual(int _index, GrabManager grabManager,int _indexInList, bool _isMalus)
    {
        index = _index;
        manager = grabManager;
        indexInList = _indexInList;
        isMalus = _isMalus;
        if (propBlock == null)
            propBlock = new MaterialPropertyBlock();
        meshMat.GetPropertyBlock(propBlock);
        if (isMalus)
        {
            propBlock.SetInt("SelectedColorTint", 2);
        }
        else
            propBlock.SetInt("SelectedColorTint", 1);
        meshMat.SetPropertyBlock(propBlock);
    }
    public void ApplyVisual(Mesh _mesh, Material _mat)
    {

        mesh.mesh = _mesh;
        meshMat.material = _mat;
    }
    public void ApplyVisual(int _indexInList)
    {
        indexInList = _indexInList;
    }
    public void ApplyVisual(int _indexInList, bool _malus)
    {
        indexInList = _indexInList;
        isMalus = _malus;
        if (propBlock == null)
            propBlock = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(propBlock);
        if (isMalus)
        {
            propBlock.SetInt("SelectedColorTint", 2);
        }
        else
            propBlock.SetInt("SelectedColorTint", 1);
        meshRenderer.SetPropertyBlock(propBlock);

    }
    public void ApplyVisual(int _indexInList, GrabManager grabManager, bool _malus)
    {
        indexInList= _indexInList;
        isMalus = _malus;
        manager = grabManager;
        if (propBlock == null)
            propBlock = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(propBlock);
        if (isMalus)
        {
            propBlock.SetInt("SelectedColorTint", 2);
        }
        else
            propBlock.SetInt("SelectedColorTint", 1);
        meshRenderer.SetPropertyBlock(propBlock);

    }
}
