using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(SphereCollider))]
public class SnapBox : MonoBehaviour
{
    private Collider sCol;
    private Collider lastUsed;
    private PlayerControlManager pcm;
    private MouseInteractionManager mouse;
    [HideInInspector] public GameObject Hologram;
    [HideInInspector] public Vector3 scaler = Vector3.zero;
    private bool isAnimating = false;
    private bool set = false;

    [Header("Are there multiple possible Targets?")]
    public bool isUndefined = false;

    [Header("Target for this Snapdropzone in Single Mode")]
    public GameObject dropTarget;

    [Header("Target for this Snapdropzone in Multiple Mode")]
    public List<GameObject> CorrectTargets;
    public List<GameObject> WrongTargets;

    [Header("Can the Object be taken out of the Zone after use")]
    public bool isTakeable = true;

    [Header("Should the GameObject get disabled after use")]
    public bool disableAfterExtraction = false;
    private GameObject disableObjekt;

    [Header("Allow the Object in the zone to Snap")]
    private bool allowSnap = false;
    private Vector3 distance;
    public bool fixated;
    public float treshold = 0.001f;
    public float magnitude;
    private bool flag = false;

    [Header("Has the correct Object been placed")]
    public bool isCorrect;
    public bool isWrong;

    [Header("Zone Attributes")]
    public bool isFull;
    public Vector3 insertingOffset;
    public Material SnapMat;
    public Material chainMat;
    [Space(5f)]
    public bool showMeshPreview = true;
    public Mesh fallbackMesh;

    [Header("Type of insert animation")]
    public bool screw;
    [Range(0f, 3600f)]
    public int rotationAngle = 360;
    [Space(3f)]
    public bool fly;
    public bool pop;
    [Space(5f)]
    public float animationSpeed = 0.3f;
    public float animationSmoothness = 50;


    void Start()
    {
        Hologram = new GameObject("Hologram");
        Hologram.transform.parent = this.gameObject.transform;
        Hologram.transform.localPosition = Vector3.zero;
        Hologram.transform.localRotation = Quaternion.identity;

        if (SnapMat == null)
            SnapMat = Resources.Load("SnapMat", typeof(Material)) as Material;

        if (fallbackMesh == null)
            fallbackMesh = Resources.Load("LowPolySphere", typeof(Mesh)) as Mesh;

        Hologram.AddComponent<MeshFilter>();
        Hologram.AddComponent<MeshRenderer>();
        Hologram.GetComponent<Renderer>().material = SnapMat;

        pcm = FindObjectOfType<PlayerControlManager>();
        mouse = FindObjectOfType<MouseInteractionManager>();

        sCol = this.GetComponent<Collider>();
        sCol.isTrigger = true;

        if (Convert.ToInt32(screw) + Convert.ToInt32(fly) + Convert.ToInt32(pop) != 1)
        {
            screw = false;
            fly = false;
            pop = true;
        }

        if (!isUndefined)
        {
            scaler = new Vector3(dropTarget.transform.lossyScale.x / transform.lossyScale.x, dropTarget.transform.lossyScale.y / transform.lossyScale.y, dropTarget.transform.lossyScale.z / transform.lossyScale.z);

            if (showMeshPreview)
                Hologram.GetComponent<MeshFilter>().mesh = dropTarget.GetComponent<MeshFilter>().mesh;
            else
                Hologram.GetComponent<MeshFilter>().mesh = fallbackMesh;
        }
        else
        {
            scaler = new Vector3(CorrectTargets[0].transform.lossyScale.x / transform.lossyScale.x, CorrectTargets[0].transform.lossyScale.y / transform.lossyScale.y, CorrectTargets[0].transform.lossyScale.z / transform.lossyScale.z);

            if (showMeshPreview)
                Hologram.GetComponent<MeshFilter>().mesh = CorrectTargets[0].GetComponent<MeshFilter>().mesh;
            else
                Hologram.GetComponent<MeshFilter>().mesh = fallbackMesh;
        }

        Hologram.transform.localScale = scaler;
    }

    private void Update()
    {
        if (isFull)
        {
            if (!isAnimating)
            {
                if (pcm.activePlayerIndex == 1)
                {
                    if (!lastUsed.GetComponent<XRGrabInteractable>().isSelected)
                        if (!flag)
                        {
                            allowSnap = true;
                            flag = true;
                        }
                        else
                            flag = false;
                }
                else
                {
                    if (!mouse.hold)
                        if (!flag)
                        {
                            allowSnap = true;
                            flag = true;
                        }
                        else
                            flag = false;
                }
            }

            if (allowSnap)
            {
                allowSnap = false;
                Place(lastUsed);
            }

            distance = lastUsed.transform.position - transform.position;
            magnitude = distance.magnitude;
            fixated = (distance.magnitude <= treshold);

            Hologram.SetActive(!fixated);
            sCol.enabled = !fixated;

            lastUsed.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isFull && !isAnimating)
        {
            if (other.GetComponent<XRGrabInteractable>() != null)
            {
                if (isUndefined)
                {
                    if (CorrectTargets.Contains(other.gameObject))
                    {
                        set = true;
                        isCorrect = true;
                        isWrong = false;
                    }
                    else if (WrongTargets.Contains(other.gameObject))
                    {
                        set = true;
                        isCorrect = false;
                        isWrong = true;
                    }
                }
                else
                {
                    if (other.gameObject == dropTarget)
                    {
                        set = true;
                        isCorrect = true;
                        isWrong = false;
                    }
                }
            }
        }

        if (set)
        {
            set = false;
            isFull = true;
            lastUsed = other;

            Hologram.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isFull && isTakeable && !isAnimating)
        {
            if (other == lastUsed)
            {
                Hologram.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

                //onHold = false;
                isFull = false;
                isCorrect = false;
                isWrong = false;
                flag = false;

                if (other.gameObject.GetComponent<Rigidbody>() != null)
                {
                    other.GetComponent<Rigidbody>().isKinematic = false;
                }

                if (disableAfterExtraction)
                {
                    disableObjekt = other.gameObject;
                    InvokeRepeating("Disable", 0f, 0.5f);
                }
            }
        }
    }

    private void Place(Collider other)
    {
        if (isCorrect)
        {
            isAnimating = true;
            if (screw)
                StartCoroutine(Screwin(other));
            else if (pop)
                StartCoroutine(Popin(other));
            else if (fly)
                StartCoroutine(Flyin(other));
        }
        else
            other.gameObject.transform.position = transform.position;
    }

    private void Disable()
    {
        if (pcm.activePlayerIndex == 1)
        {
            if (!lastUsed.GetComponent<XRGrabInteractable>().isSelected)
            {
                disableObjekt.SetActive(false);
                CancelInvoke("Disable");
            }
        }
        else
        {
            if (!mouse.hold)
            {
                disableObjekt.SetActive(false);
                CancelInvoke("Disable");
            }
        }
    }

    private IEnumerator Screwin(Collider other)
    {
        Vector3 startPosition = other.gameObject.transform.position;
        Vector3 endPosition = transform.position + insertingOffset;

        Quaternion startRotation = other.gameObject.transform.rotation;
        Quaternion endRotation = transform.rotation;

        float speed = animationSpeed / animationSmoothness;

        for (float i = 0; i <= animationSmoothness; i++)
        {
            other.gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, i / animationSmoothness);
            other.gameObject.transform.rotation = Quaternion.Lerp(startRotation, endRotation, i / animationSmoothness);
            yield return new WaitForSeconds(speed);
        }

        startPosition = endPosition;
        endPosition = transform.position;

        for (float i = 0; i <= animationSmoothness; i++)
        {
            other.gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, i / animationSmoothness);
            other.gameObject.transform.rotation = Quaternion.AngleAxis(rotationAngle * (i / animationSmoothness), insertingOffset);
            yield return new WaitForSeconds(speed);
        }

        isAnimating = false;
        yield return null;
    }

    private IEnumerator Flyin(Collider other)
    {
        Vector3 startPosition = other.gameObject.transform.position;
        Vector3 endPosition = transform.position + insertingOffset;

        Quaternion startRotation = other.gameObject.transform.rotation;
        Quaternion endRotation = transform.rotation;

        float speed = animationSpeed / animationSmoothness;

        for (int i = 0; i <= animationSmoothness; i++)
        {
            other.gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, i / animationSmoothness);
            other.gameObject.transform.rotation = Quaternion.Lerp(startRotation, endRotation, i / animationSmoothness);
            yield return new WaitForSeconds(speed);
        }

        startPosition = endPosition;
        endPosition = transform.position;

        for (int i = 0; i <= animationSmoothness; i++)
        {
            other.gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, i / animationSmoothness);
            yield return new WaitForSeconds(speed);
        }

        isAnimating = false;
        yield return null;
    }

    private IEnumerator Popin(Collider other)
    {
        other.gameObject.transform.position = transform.position;
        other.gameObject.transform.rotation = transform.rotation;

        isAnimating = false;
        yield return null;
    }
}
