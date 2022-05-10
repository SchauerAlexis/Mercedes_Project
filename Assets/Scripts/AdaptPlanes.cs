using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AdaptPlanes : MonoBehaviour
{
    [Header("Cross Section Box")]
    public GameObject box;

    [Header("Optical Planes in Children")]
    public GameObject XUp;
    public GameObject XDown;
    [Space(5)]
    public GameObject YUp;
    public GameObject YDown;
    [Space(5)]
    public GameObject ZUp;
    public GameObject ZDown;

    public Vector3 xDimension;
    public Vector3 yDimension;
    public Vector3 zDimension;

    private Vector3 dimension = Vector3.one;
    public Vector3 correction = Vector3.one;

    void Start()
    {
        correction = box.GetComponent<MoveCrosssection>().correction;

        XUp.transform.localScale = Vector3.Scale(XUp.transform.localScale, correction);
        XDown.transform.localScale = XUp.transform.localScale;
        YUp.transform.localScale = Vector3.Scale(YUp.transform.localScale, correction);
        YDown.transform.localScale = YUp.transform.localScale;
        ZUp.transform.localScale = Vector3.Scale(ZUp.transform.localScale, correction);
        ZDown.transform.localScale = ZUp.transform.localScale;
    }

    void Update()
    {
        this.transform.localScale = box.transform.localScale;
        this.transform.position = box.transform.position;

        dimension = transform.localScale;

        xDimension = Vector3.Scale(dimension, Vector3.right);
        yDimension = Vector3.Scale(dimension, Vector3.up);
        zDimension = Vector3.Scale(dimension, Vector3.forward);

        SetPlanes(XUp, XDown, xDimension);
        SetPlanes(YUp, YDown, yDimension);
        SetPlanes(ZUp, ZDown, zDimension);
    }

    private void SetPlanes(GameObject upplane, GameObject downplane, Vector3 dimens)
    {
        upplane.transform.position = transform.position + 0.5f * dimens;
        downplane.transform.position = transform.position - 0.5f * dimens;
    }
}
