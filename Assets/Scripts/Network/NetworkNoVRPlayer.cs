using Photon.Pun;
using UnityEngine;

public class NetworkNoVRPlayer : MonoBehaviour
{
    public Transform head;

    private PhotonView photonView;

    public Transform headRig;

    private GameObject player;
    // private GameObject Debugplayer;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        player = FindObjectOfType<CharacterController>().gameObject;

        headRig = player.GetComponentInChildren<Camera>().transform;

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
