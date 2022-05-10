using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class MouseInteractionManager : MonoBehaviour
{
    public float GripDistance = 2f;

    public Color colorSelect = Color.green;
    public LayerMask layerMask;
    private Color bufferColor;
    private PlayerControlManager control;

    public bool allow = false;
    public bool turn = false;
    public bool isHit = false;
    public bool hold = false;

    private Camera usedCam;

    public GameObject currentHit;
    public GameObject lastHit;

    private RaycastHit hit;
    private Ray ray;
    public float distance;

    void Start()
    {
        control = FindObjectOfType<PlayerControlManager>();
        usedCam = control.playCamera;
        currentHit = gameObject;
        lastHit = gameObject;
    }

    void Update()
    {
        if (control.activePlayerIndex != 1)
        {
            usedCam = control.playCamera;
            CheckHit();

            if (isHit)
                DoNewHit();
        }
    }

    void CheckHit()
    {
        ray = usedCam.ScreenPointToRay(Input.mousePosition);

        if (!hold)
        {
            if (Physics.Raycast(ray, out hit, GripDistance, layerMask))
            {
                currentHit = hit.transform.gameObject;

                if (currentHit != lastHit)
                {
                    if (lastHit.GetComponent<XRGrabInteractable>() != null)
                        UndoLastHit();

                    if (currentHit.GetComponent<XRGrabInteractable>() != null)
                    {
                        distance = hit.distance;
                        bufferColor = currentHit.GetComponentInChildren<Renderer>().material.color;
                        currentHit.GetComponentInChildren<Renderer>().material.color = colorSelect;
                        isHit = true;
                    }
                    lastHit = currentHit;
                }
            }
            else
            {
                if (lastHit.GetComponent<XRGrabInteractable>() != null)
                    UndoLastHit();

                lastHit = gameObject;
            }
        }
    }

    private void DoNewHit()
    {
        // halten programmieren
        if (allow)
        {
            hold = true;
            currentHit.GetComponent<Rigidbody>().isKinematic = true;
            currentHit.transform.position = ray.GetPoint(distance);

            if (turn)
                currentHit.transform.LookAt(usedCam.transform.position);
        }
        else
        {
            if(hold)
                currentHit.GetComponent<Rigidbody>().isKinematic = false;
            hold = false;
        }
    }

    private void UndoLastHit()
    {
        lastHit.GetComponentInChildren<Renderer>().material.color = bufferColor;
        isHit = false;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            allow = true;

        if (context.canceled)
            allow = false;
    }
    public void OnTurn(InputAction.CallbackContext context)
    {
        if (context.performed)
            turn = true;

        if (context.canceled)
            turn = false;
    }
}
