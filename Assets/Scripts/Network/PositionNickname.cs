using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using WebSocketSharp;

public class PositionNickname : MonoBehaviour
{
    public GameObject speakerIcon;

    private PhotonView photonView;
    public PhotonVoiceView photonVoiceView;

    private Transform headRig;

    private TextMesh textMesh;


    // Start is called before the first frame update
    void Start()
    {
        photonView = PhotonView.Get(this);
        XRRig rig = FindObjectOfType<XRRig>();
        headRig = rig.transform.Find("Camera Offset/Main Camera");

        textMesh = GetComponentInChildren<TextMesh>();

        if (photonView.Owner.NickName.IsNullOrEmpty())
            photonView.Owner.NickName = "NickName";

        textMesh.text = photonView.Owner.NickName;

        speakerIcon.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            this.transform.position = headRig.position;
            this.transform.rotation = Quaternion.Euler(0, headRig.rotation.eulerAngles.y, 0);


        }
        speakerIcon.SetActive(photonVoiceView.IsSpeaking);
        Debug.Log("Speaking: " + photonVoiceView.IsSpeaking);
        Debug.Log("Recording: " + photonVoiceView.IsRecording);
    }
}
