using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class NetworkFPCPlayer : MonoBehaviour
{
    public PlayerControlManager pControl;

    public Transform head;
    private PhotonView photonView;
    public Transform headRig;
    public GameObject playerBody;
    public Transform playerBodyPlace;
    private GameObject player;


    void Start()
    {
        photonView = GetComponent<PhotonView>();

        pControl = FindObjectOfType<PlayerControlManager>();
        player = pControl.FPCPlayer;

        headRig = player.GetComponentInChildren<Camera>().transform;
        playerBodyPlace = player.transform.Find("Torso").transform;

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

            playerBody.transform.position = playerBodyPlace.position;
            playerBody.transform.rotation = playerBodyPlace.rotation;
        }
    }
}