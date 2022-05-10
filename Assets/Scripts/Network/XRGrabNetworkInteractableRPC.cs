using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class XRGrabNetworkInteractableRPC : XRGrabInteractable
{
    private PhotonView photonView;
    private SwitchKinGravRPC SwitchKinGrav;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        SwitchKinGrav = GetComponent<SwitchKinGravRPC>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        photonView.RequestOwnership();
        SwitchKinGrav.SetKinematic();
        base.OnSelectEntered(interactor);
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        SwitchKinGrav.DeSetKinematic();
    }
}