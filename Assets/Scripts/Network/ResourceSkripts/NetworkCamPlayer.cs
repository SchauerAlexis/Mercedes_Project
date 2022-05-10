using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class NetworkCamPlayer : MonoBehaviour
{
    public PlayerControlManager pControl;

    public Transform head;
    private PhotonView photonView;
    public Transform headRig;
    private GameObject player;


    void Start()
    {
        photonView = GetComponent<PhotonView>();

        pControl = FindObjectOfType<PlayerControlManager>();
        player = pControl.CamPlayer;

        headRig = player.transform;

        if (photonView.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }

            foreach (var item in GetComponentsInChildren<Collider>())
            {
                item.enabled = false;
            }
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            head.position = headRig.position;
            head.rotation = headRig.rotation;
        }
    }
}