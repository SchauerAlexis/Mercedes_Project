using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

/// <summary>
/// Small modification of the classic XRGrabInteractable that will keep the position and rotation offset between the
/// grabbed object and the controller instead of snapping the object to the controller. Better for UX and the illusion
/// of holding the thing (see Tomato Presence : https://owlchemylabs.com/tomatopresence/)
/// </summary>
public class XRGrabNetworkInteractableRPCPosRot : XRGrabInteractable
{

    class SavedTransform
    {
        public Vector3 OriginalPosition;
        public Quaternion OriginalRotation;
    }

    private PhotonView photonView;
    private SwitchKinGravRPC SwitchKinGrav;

    Dictionary<XRBaseInteractor, SavedTransform> m_SavedTransforms = new Dictionary<XRBaseInteractor, SavedTransform>();

    Rigidbody m_Rb;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        SwitchKinGrav = GetComponent<SwitchKinGravRPC>();
    }

    protected override void Awake()
    {
        base.Awake();

        //the base class already grab it but don't expose it so have to grab it again
        m_Rb = GetComponent<Rigidbody>();
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        photonView.RequestOwnership();

        if (interactor is XRDirectInteractor)
        {

            SavedTransform savedTransform = new SavedTransform();

            savedTransform.OriginalPosition = interactor.attachTransform.localPosition;
            savedTransform.OriginalRotation = interactor.attachTransform.localRotation;

            m_SavedTransforms[interactor] = savedTransform;


            bool haveAttach = attachTransform != null;

            interactor.attachTransform.position = haveAttach ? attachTransform.position : m_Rb.worldCenterOfMass;
            interactor.attachTransform.rotation = haveAttach ? attachTransform.rotation : m_Rb.rotation;



        }
        SwitchKinGrav.SetKinematic();
        base.OnSelectEntered(interactor);
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        if (interactor is XRDirectInteractor)
        {
            SavedTransform savedTransform = null;
            if (m_SavedTransforms.TryGetValue(interactor, out savedTransform))
            {
                interactor.attachTransform.localPosition = savedTransform.OriginalPosition;
                interactor.attachTransform.localRotation = savedTransform.OriginalRotation;

                m_SavedTransforms.Remove(interactor);
            }
            Transform grabbedObjectTransform = interactor.gameObject.transform;
            grabbedObjectTransform.localRotation = Quaternion.Euler(0, 0, 0);
            grabbedObjectTransform.localPosition = Vector3.zero;
        }

        base.OnSelectExited(interactor);
        SwitchKinGrav.DeSetKinematic();
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        int interactorLayerMask = 1 << interactor.gameObject.layer;
        return base.IsSelectableBy(interactor) && (interactionLayerMask.value & interactorLayerMask) != 0;
    }
}
