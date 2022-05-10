using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class NetworkXRPlayer : MonoBehaviour
{
    public PlayerControlManager pControl;

    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    private PhotonView photonView;

    public Transform headRig;
    public Transform leftHandRig;
    public Transform rightHandRig;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        pControl = FindObjectOfType<PlayerControlManager>();
        player = pControl.XRPlayer;
        Transform offset = player.transform.GetChild(0);

        headRig = offset.transform.Find("Main Camera").transform;
        leftHandRig = offset.transform.Find("LeftHand Controller").transform;
        rightHandRig = offset.transform.Find("RightHand Controller").transform;


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
            MapPosition(head, headRig);
            MapPosition(leftHand, leftHandRig);
            MapPosition(rightHand, rightHandRig);

            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAnimator);
        }

    }

    void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}