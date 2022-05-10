using Photon.Pun;
using UnityEngine;
using System;


public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject SpawnedPlayerPrefab;
    public int usedPlayer;

    private void Start()
    {
        usedPlayer = PlayerPrefs.GetInt("Player");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if (usedPlayer == 1)
            SpawnedPlayerPrefab = PhotonNetwork.Instantiate("Network XR Player", transform.position, transform.rotation);
        else if (usedPlayer == 2)
            SpawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Cam Player", transform.position, transform.rotation);
        else
            SpawnedPlayerPrefab = PhotonNetwork.Instantiate("Network FPC Player", transform.position, transform.rotation);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(SpawnedPlayerPrefab);
    }
}
