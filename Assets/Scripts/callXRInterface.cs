using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class callXRInterface : MonoBehaviour
{

    [Header("This Script attaches the menu to the left Hand XR Controller")]

    [Header("Used XR Controllers")]
    [SerializeField] private XRBaseController leftXRController;
    [SerializeField] private XRBaseController rightXRController;
    private XRBaseController xCon;

    [Header("Offset")]
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public float scaling = 1;

    private Vector3 scaler;

    [Header("Attach to left Hand")]
    public bool useLeft = false;

    void OnEnable()
    {
        XRBaseController[] controllers = FindObjectsOfType<XRBaseController>();

        foreach (XRBaseController controller in controllers)
        {
            if (controller.gameObject.name.Contains("Left"))
                leftXRController = controller;
            else
                rightXRController = controller;
        }

        scaler = new Vector3(scaling / 100, scaling / 100, scaling / 100);

    }

    // Update is called once per frame
    void Update()
    {
        if (useLeft)
            xCon = leftXRController;
        else
            xCon = rightXRController;

        this.transform.position = xCon.transform.position + Vector3.Scale(positionOffset, scaler);
        this.transform.rotation = xCon.transform.rotation * Quaternion.Euler(rotationOffset);
    }
}
