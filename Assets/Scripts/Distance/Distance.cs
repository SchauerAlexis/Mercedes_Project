using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Mathf;

public class Distance : MonoBehaviour
{
    [Header("Ansatzpunkte für das Maßband")]
    public GameObject leftHandle;
    public GameObject rightHandle;
        
    [Header("Snapppoints um die Ansatzpunkte zum Controller zu teleportieren")]
    public GameObject leftSnap;
    public GameObject rightSnap;

    [Header("Textfeld zur Ausgabe der Messung")]
    public GameObject textFeld;

    [Header("Scrollbar für die Linienbreite")]
    public Scrollbar scroll;

    [Header("Durchschalten der Messungsvarianten")]
    public bool TryIt = false;

    [Header("Bestimmt von welchem Pin die Linie abgeht")]
    public bool leftStart = true;

    [Header("Korrekturfaktor für die Liniengröße")]
    public float lineSizeCorrection = 1;

    private GameObject leftPoint;
    private GameObject rightPoint;

    private LineRenderer line;
    private Text ausgabeText;
    private RectTransform ausgabeForm;
    private Vector2 starRect;

    private Vector3 nullPosition;
    private Vector3 xyzPosition;
    private Vector3 distance;

    private string[] ausgaben;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        ausgaben = new string[5];
        line = GetComponent<LineRenderer>();

        leftPoint = leftHandle.transform.GetChild(0).gameObject;
        rightPoint = rightHandle.transform.GetChild(0).gameObject;

        ausgabeText = textFeld.GetComponentInChildren<Text>();
        ausgabeForm = textFeld.GetComponent<RectTransform>();
        starRect = ausgabeForm.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        Measure();
    }

    public void Measure()
    {
        if (leftStart)
        {
            nullPosition = leftPoint.transform.position;
            xyzPosition = rightPoint.transform.position;
        }
        else
        {
            xyzPosition = leftPoint.transform.position;
            nullPosition = rightPoint.transform.position;
        }

        distance = nullPosition - xyzPosition;

        Vector3 xPosition = nullPosition - new Vector3(distance.x, 0, 0);
        Vector3 yPosition = nullPosition - new Vector3(0, distance.y, 0);
        Vector3 zPosition = nullPosition - new Vector3(0, 0, distance.z);
        Vector3 xyPosition = xPosition - new Vector3(0, distance.y, 0);

        ausgaben[0] = "<- distance -> : " + Round(distance.magnitude * 100) / 100;
        ausgaben[1] = "<- x -> : " + Round(Abs(distance.x) * 100) / 100;
        ausgaben[2] = "<- y -> : " + Round(Abs(distance.y) * 100) / 100;
        ausgaben[3] = "<- z -> : " + Round(Abs(distance.z) * 100) / 100;
        ausgaben[4] = "<- distance -> : " + Round(distance.magnitude * 100) / 100 + "\n" + "<- x -> : " + Round(Abs(distance.x) * 100) / 100 + "\n" + "<- y -> : " + Round(Abs(distance.y) * 100) / 100 + "\n" + "<- z -> : " + Round(Abs(distance.z) * 100) / 100;

        line.enabled = true;
        line.startWidth = scroll.value * 0.02f * lineSizeCorrection;
        line.endWidth = line.startWidth;

        switch (index)
        {
            case 1:
                ausgabeForm.sizeDelta = new Vector2(starRect.x, starRect.y);
                line.startColor = Color.red;
                line.endColor = line.startColor;
                line.positionCount = 2;
                line.SetPosition(0, nullPosition);
                line.SetPosition(1, xPosition);
                line.loop = false;
                break;

            case 2:
                ausgabeForm.sizeDelta = new Vector2(starRect.x, starRect.y);
                line.startColor = Color.green;
                line.endColor = line.startColor;
                line.positionCount = 2;
                line.SetPosition(0, nullPosition);
                line.SetPosition(1, yPosition);
                line.loop = false;
                break;

            case 3:
                ausgabeForm.sizeDelta = new Vector2(starRect.x, starRect.y);
                line.startColor = Color.blue;
                line.endColor = line.startColor;
                line.positionCount = 2;
                line.SetPosition(0, nullPosition);
                line.SetPosition(1, zPosition);
                line.loop = false;
                break;

            case 4:
                ausgabeForm.sizeDelta = new Vector2(starRect.x, starRect.y * 3);
                line.startColor = Color.white;
                line.endColor = line.startColor;
                line.positionCount = 4;
                line.SetPosition(0, nullPosition);
                line.SetPosition(1, xPosition);
                line.SetPosition(2, xyPosition);
                line.SetPosition(3, xyzPosition);
                line.loop = true;
                break;

            default:
                ausgabeForm.sizeDelta = new Vector2(starRect.x, starRect.y);
                line.startColor = Color.yellow;
                line.endColor = line.startColor;
                line.positionCount = 2;
                line.SetPosition(0, nullPosition);
                line.SetPosition(1, xyzPosition);
                line.loop = false;
                break;
        }

        ausgabeText.text = ausgaben[index];

        if (TryIt)
        {
            ChangeLine();
            TryIt = false;
        }
    }

    public void Snap(string controller)
    {
        if (controller == "left" || controller == "Left")
        {
            leftHandle.transform.position = leftSnap.transform.position;
        }
        else if (controller == "right" || controller == "Right")
        {
            rightHandle.transform.position = rightSnap.transform.position;
        }
        else
            Debug.Log("Controller " + controller + " not found. Use <left> or <right>");
    }

    public void ChangeLine()
    {
        index++;
        index %= ausgaben.Length;
    }

    public void ChangeController()
    {
        leftStart = !leftStart;
    }
}
