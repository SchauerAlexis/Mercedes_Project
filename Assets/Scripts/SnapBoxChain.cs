using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapBoxChain : MonoBehaviour
{
    public bool activate = false;
    public Color markerColor = Color.red;

    public List<SnapBox> snapBoxes;
    private List<GameObject> signs;

    private int index = 0;
    private Material chainMat;

    // Start is called before the first frame update
    private void Start()
    {
        if (chainMat == null)
            chainMat = Resources.Load("chainMat", typeof(Material)) as Material;

        chainMat.color = markerColor;

        StartCoroutine(LaterStart());
    }

    private IEnumerator LaterStart()
    {
        yield return new WaitForSeconds(1f);

        foreach (SnapBox box in snapBoxes)
        {
            GameObject chainSign = new GameObject("chainSign");
            chainSign.transform.parent = box.gameObject.transform;
            chainSign.transform.localPosition = Vector3.zero;
            chainSign.transform.localRotation = Quaternion.identity;
            chainSign.AddComponent<MeshFilter>();
            chainSign.AddComponent<MeshRenderer>();
            chainSign.GetComponent<MeshFilter>().mesh = box.Hologram.GetComponent<MeshFilter>().mesh;
            chainSign.GetComponent<Renderer>().material = chainMat;
            chainSign.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            chainSign.transform.localScale = box.scaler;
            chainSign.SetActive(false);
        }

        InvokeRepeating("LaterUpdate", 1f, 0.05f);
    }

    // Update is called once per frame
    void LaterUpdate()
    {
        if (activate)
        {
            foreach (SnapBox box in snapBoxes)
            {
                if (box == snapBoxes[index])
                {
                    box.transform.Find("chainSign").gameObject.SetActive(!box.isFull);
                }
                else
                    box.transform.Find("chainSign").gameObject.SetActive(false);
            }

            if (snapBoxes[index].fixated)
                index = (index + 1) % snapBoxes.Count;
        }
        else
        {
            foreach (SnapBox box in snapBoxes)
            {
                box.transform.Find("chainSign").gameObject.SetActive(false);
            }
            index = 0;
        }
    }
}
