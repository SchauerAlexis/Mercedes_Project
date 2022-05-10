using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;
using WebSocketSharp;

public class NetworkPlayerUI : MonoBehaviour
{
    private SpriteRenderer speakerIcon;
    private PhotonView photonView;
    private PhotonVoiceView photonVoiceView;
    private TextMesh textMesh;

    // Start is called before the first frame update
    void Start()
    {
        // Get Photon components
        photonView = GetComponentInParent<PhotonView>();
        photonVoiceView = GetComponentInParent<PhotonVoiceView>();
        textMesh = this.GetComponentInChildren<TextMesh>();

        if (photonView.Owner.NickName.IsNullOrEmpty())
            photonView.Owner.NickName = "Pseudo Name";

        textMesh.text = photonView.Owner.NickName;

        speakerIcon = this.GetComponentInChildren<SpriteRenderer>();
        speakerIcon.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        speakerIcon.enabled = photonVoiceView.IsSpeaking;
    }
}
