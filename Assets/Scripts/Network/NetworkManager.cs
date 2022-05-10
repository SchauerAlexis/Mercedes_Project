using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks //Ueberschreibt einige Punkte in Photon
{
    public string Mode;
    public bool toggle = true;


    void Start()
    {
        Mode = PlayerPrefs.GetString("Mode");

        Debug.Log("Try Mode <" + Mode + ">");

        if (Mode == "Online")
        {          
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Versuche zu verbinden...");

            if (PhotonNetwork.IsConnected)
                Debug.Log("Starting Online Mode");
            else
                Debug.Log("Online Mode not available!");
            // Turns Off all GameObjects with MultiplayerOwnership Script
        }

        else
        {
            Debug.Log("Starting Offline Mode");
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 10;
            roomOptions.IsVisible = true;
            roomOptions.IsOpen = true;
            PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Verbunden mit dem Server");
        base.OnConnectedToMaster();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Trete einen Raum bei...");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Ein neuer Spieler tritten den Raum bei");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
