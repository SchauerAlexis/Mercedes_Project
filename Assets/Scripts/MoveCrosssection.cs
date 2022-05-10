using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MoveCrosssection : MonoBehaviour
{
    [Header("Inverted Cut Direction")]
    public Toggle invertX;
    public Toggle invertY;
    public Toggle invertZ;

    [Header("Sliders")]
    public Slider sliderX;
    public Slider sliderY;
    public Slider sliderZ;

    [Header("Optical Planes in Children")]
    public GameObject XUp;
    public GameObject XDown;
    [Space(5)]
    public GameObject YUp;
    public GameObject YDown;
    [Space(5)]
    public GameObject ZUp;
    public GameObject ZDown;    
    
    [Header("Hatch Planes in Children for striping")]
    public GameObject XUpHatch;
    public GameObject XDownHatch;
    [Space(5)]
    public GameObject YUpHatch;
    public GameObject YDownHatch;
    [Space(5)]
    public GameObject ZUpHatch;
    public GameObject ZDownHatch;
    
    [Header("Dimensions of the Matrix Calculation for the placement of the planes")]
    public Vector3 xDimension;
    public Vector3 yDimension;
    public Vector3 zDimension;
    [Space(5)]
    public Vector3 startPoint;
    public Vector3 endPoint;
    public Vector3 way;

    [Header("Scale Correction")]
    public Vector3 dimensions = Vector3.one;
    public Vector3 localDimensions = Vector3.one;

    public Vector3 correction;

    [Header("is Inverted?")]
    [SerializeField] private bool inX;
    [SerializeField] private bool inY;
    [SerializeField] private bool inZ;

    private Vector3 xEnd;
    private Vector3 yEnd;
    private Vector3 zEnd;

    void Start()
    {
        dimensions = this.transform.lossyScale;
        localDimensions = this.transform.localScale;

        startPoint = this.transform.position - 0.5f * transform.TransformDirection(dimensions);
        way = transform.TransformDirection(dimensions);

        correction = new Vector3(localDimensions.x / dimensions.x, localDimensions.y / dimensions.y, localDimensions.z / dimensions.z);

        XUp.transform.localScale = Vector3.Scale(XUp.transform.localScale, correction);
        XDown.transform.localScale = XUp.transform.localScale;
        XUpHatch.transform.localScale = XUp.transform.localScale;
        XDownHatch.transform.localScale = XUp.transform.localScale;

        YUp.transform.localScale = Vector3.Scale(YUp.transform.localScale, correction);
        YDown.transform.localScale = YUp.transform.localScale;
        YUpHatch.transform.localScale = YUp.transform.localScale;
        YDownHatch.transform.localScale = YUp.transform.localScale;

        ZUp.transform.localScale = Vector3.Scale(ZUp.transform.localScale, correction);
        ZDown.transform.localScale = ZUp.transform.localScale;
        ZUpHatch.transform.localScale = ZUp.transform.localScale;
        ZDownHatch.transform.localScale = ZUp.transform.localScale;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localScale = new Vector3(dimensions.x * sliderX.value, dimensions.y * sliderY.value, dimensions.z * sliderZ.value);

        xDimension = Vector3.Normalize(transform.TransformDirection(Vector3.right)) * (0.5f * transform.localScale.x);
        yDimension = Vector3.Normalize(transform.TransformDirection(Vector3.up)) * (0.5f * transform.localScale.y);
        zDimension = Vector3.Normalize(transform.TransformDirection(Vector3.forward)) * (0.5f * transform.localScale.z);

        SetPlanes(XUp, XDown, XUpHatch, XDownHatch, xDimension);
        SetPlanes(YUp, YDown, YUpHatch, YDownHatch, yDimension);
        SetPlanes(ZUp, ZDown, ZUpHatch, ZDownHatch, zDimension);



        xEnd = Vector3.Project(way, transform.TransformDirection(Vector3.right));
        yEnd = Vector3.Project(way, transform.TransformDirection(Vector3.up));
        zEnd = Vector3.Project(way, transform.TransformDirection(Vector3.forward));

        inX = invertX.isOn;
        inY = invertY.isOn;
        inZ = invertZ.isOn;

        Vector3 xNow = AdjustCut(inX, xEnd, sliderX.value);
        Vector3 yNow = AdjustCut(inY, yEnd, sliderY.value);
        Vector3 zNow = AdjustCut(inZ, zEnd, sliderZ.value);

        transform.position = startPoint + xNow + yNow + zNow;
    }

    private void SetPlanes(GameObject upplane, GameObject downplane,GameObject upHatch, GameObject downHatch, Vector3 dimens)
    {
        upplane.transform.position = transform.position + dimens;
        downplane.transform.position = transform.position - dimens;

        upHatch.transform.position = upplane.transform.position;
        downHatch.transform.position = downplane.transform.position;
    }

    Vector3 AdjustCut(bool invert, Vector3 end, float sliderValue)
    {
        Vector3 now;

        if (!invert)
        {
            now = end * sliderValue / 2;
        }
        else
        {
            now = end * (1 - sliderValue / 2);
        }
        return now;
    }
}
