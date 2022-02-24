using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;
public class GrabbableObject : XRGrabInteractable
{
    [HideInInspector] public bool isGrab;
    public XRBaseInteractor currentInteractor;
    Rigidbody m_Rib;
    private Vector3 startParentPosition;
    private Quaternion startParentRotationQ;
    private Vector3 startChildPosition;
    private Quaternion startChildRotationQ;
    private Matrix4x4 parentMatrix;
    private bool grabedOnce;
   [HideInInspector] public Transform follow;
   [HideInInspector] public Transform followRotation;
   [HideInInspector] public event Action OnHover;
   [HideInInspector] public event Action OnHoverExit;
   [HideInInspector] public event Action OnSelect;
   class SavedTransform
    {
        public Vector3 OriginalPosition;
        public Quaternion OriginalRotation;
    }

    
    Dictionary<XRBaseInteractor, SavedTransform> m_SavedTransforms = new Dictionary<XRBaseInteractor, SavedTransform>();

#pragma warning disable CS0672 // Un membre se substitue au membre obsolète
#pragma warning disable CS0618 // Le type ou le membre est obsolète
    /// <summary>This method was added by ian to enable dropping of objects 
    /// when we need to do it in code (e.g. at level end or when exhausted)
    /// </summary>
    /// <param name="interactor">Usually the player's hand</param>
    public void CustomForceDrop(XRBaseInteractor interactor)
    {
        onSelectExit.Invoke(interactor);
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        base.OnSelectEntering(interactor);
        if (interactor is XRDirectInteractor)
        {
            SavedTransform savedTransform = new SavedTransform();

            savedTransform.OriginalPosition = interactor.attachTransform.position;
            savedTransform.OriginalRotation = interactor.attachTransform.rotation;

            m_SavedTransforms[interactor] = savedTransform;


            bool haveAttach = attachTransform != null;
            interactor.attachTransform.position = haveAttach ? attachTransform.position : m_Rib.worldCenterOfMass;
            interactor.attachTransform.rotation = haveAttach ? attachTransform.rotation : m_Rib.rotation;
        }
        currentInteractor = interactor;
        isGrab = true;
        if(OnSelect !=null)
        OnSelect.Invoke();
    }
 
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        if (interactor is XRDirectInteractor)
        {
            SavedTransform savedTransform = null;
            if (m_SavedTransforms.TryGetValue(interactor, out savedTransform))
            {
                //interactor.attachTransform.localPosition = savedTransform.OriginalPosition;
                //interactor.attachTransform.localRotation = savedTransform.OriginalRotation;

                interactor.attachTransform.localPosition = Vector3.zero;
                interactor.attachTransform.localRotation = Quaternion.identity;

                m_SavedTransforms.Remove(interactor);
            }
        }
        currentInteractor = null;
        isGrab = false;
    }
    protected override void OnHoverEntered(XRBaseInteractor interactor)
    {
        if (Mathf.Abs( Vector3.Distance(interactor.transform.position, transform.position)) > 1f)
            return;
        base.OnHoverEntered(interactor);
        OnHover.Invoke();
    }
    protected override void OnHoverExited(XRBaseInteractor interactor)
    {
        base.OnHoverExited(interactor);
        OnHoverExit.Invoke();
    }
#pragma warning restore CS0618 // Le type ou le membre est obsolète
#pragma warning restore CS0672 // Un membre se substitue au membre obsolète
    private void Start()
    {
        if (!m_Rib)
            m_Rib = GetComponent<Rigidbody>();
        if (follow)
        {
            m_Rib.constraints = RigidbodyConstraints.FreezeAll;

            startParentPosition = follow.position;
            startParentRotationQ = follow.rotation;

            startChildPosition = transform.position;
            startChildRotationQ = transform.rotation;
            //founded by testing
            startChildPosition = DivideVectors(Quaternion.Inverse(follow.rotation) * (startChildPosition - startParentPosition), follow.lossyScale);
        }
    }
    private void Update()
    {
        if (!grabedOnce && follow != null)
        {
            //simulate child effect
            parentMatrix = Matrix4x4.TRS(follow.position, follow.rotation, follow.lossyScale);

            transform.position = parentMatrix.MultiplyPoint3x4(startChildPosition);

            transform.rotation = (follow.rotation * Quaternion.Inverse(startParentRotationQ)) * startChildRotationQ;
        }
    }
    Vector3 DivideVectors(Vector3 num, Vector3 den)
    {

        return new Vector3(num.x / den.x, num.y / den.y, num.z / den.z);

    }


}

