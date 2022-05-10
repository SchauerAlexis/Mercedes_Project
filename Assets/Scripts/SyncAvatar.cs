using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class SyncAvatar : MonoBehaviour, IPunObservable
{

    public string tracking;

    public GameObject[] hairs;
    public GameObject[] skins;
    public GameObject[] glases;

    private int hairIndex = 0;
    private int skinIndex = 0;
    private int glasIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        hairIndex = PlayerPrefs.GetInt("hair");
        skinIndex = PlayerPrefs.GetInt("skin");
        glasIndex = PlayerPrefs.GetInt("glas");

        tracking = hairIndex + ";" + skinIndex + ";" + glasIndex;

        InvokeRepeating("ChangeNetAvatar", 3f, 5f);
    }

    private void ChangeNetAvatar()
    {
        string[] val = tracking.Split(';');

        hairIndex = int.Parse(val[0]);
        skinIndex = int.Parse(val[1]);
        glasIndex = int.Parse(val[2]);

        foreach (GameObject hair in hairs)
            hair.SetActive(false);
        hairs[hairIndex].SetActive(true);

        foreach (GameObject skin in skins)
            skin.SetActive(false);
        skins[skinIndex].SetActive(true);

        foreach (GameObject glas in glases)
            glas.SetActive(false);
        glases[glasIndex].SetActive(true);
    }

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(tracking);
        }
        else
        {
            // Network player, receive data
            tracking = (string)stream.ReceiveNext();
        }
    }
    #endregion
}