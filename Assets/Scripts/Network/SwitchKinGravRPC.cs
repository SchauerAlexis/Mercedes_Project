using Photon.Pun;
using UnityEngine;


public class SwitchKinGravRPC : MonoBehaviour
{
    private PhotonView photonView;
    private Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rigidBody = GetComponent<Rigidbody>();
    }


    public void SetKinematic()
    {
        photonView.RPC("SetKinematicOn", RpcTarget.All);
    }

    public void DeSetKinematic()
    {
        photonView.RPC("SetKinematicOff", RpcTarget.All);
    }

    [PunRPC]
    public void SetKinematicOn()
    {
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
        Debug.Log("Kinematic on");

    }

    [PunRPC]
    public void SetKinematicOff()
    {
        Debug.Log(rigidBody.name);
        Debug.Log("Sleeping?" + rigidBody.IsSleeping());
        Debug.Log(rigidBody.isKinematic);
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
        Debug.Log(rigidBody.isKinematic);
        Debug.Log("Kinematic off");
    }
}
