using UnityEngine;
using Photon.Pun;

public class MultiplayerOwnership : MonoBehaviour
{

    private PhotonView photonView;
    public int MasterID;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    public void Ownership()
    {
        if (photonView.IsMine == false)
            photonView.RequestOwnership();
    }

    private void OnDisable()
    {
        if ((PlayerPrefs.GetString("Mode") == "Online")&&(!PhotonNetwork.IsMasterClient))
        {
            photonView.TransferOwnership(PhotonNetwork.MasterClient.ActorNumber);
        }
    }
}