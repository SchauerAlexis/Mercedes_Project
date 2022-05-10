using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LineRendererWhiteboardPen : MonoBehaviour
{

    public LineRendererWhiteboard whiteboard;

    public DrawingColor penColor = DrawingColor.Blue;

    private XRGrabNetworkInteractableRPC XRInteractable;


    private RaycastHit touch;

    private Transform tip;
    private Transform attachPoint;

    private bool lastTouch;
    private Quaternion lastAngle;
    private XRController currentController;
    private bool isGrabbed;


    // Start is called before the first frame update
    void Start()
    {
        tip = this.transform.Find("Tip");
        attachPoint = this.transform.Find("AttachPoint");

        XRInteractable = GetComponent<XRGrabNetworkInteractableRPC>();
       // Debug.Log("XRGrab: " +XRInteractable);
        XRInteractable.onSelectEntered.AddListener(GetController);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (XRInteractable.isSelected)
        {
            GetTouch();
        }
    }


    private void GetTouch()
    {
        float tipHeight = tip.localScale.y * 0.9f;
        Vector3 tipVector3 = tip.transform.position;

        
        if (Physics.Raycast(tipVector3, transform.up, out touch, tipHeight))
        {
            if (!(touch.collider.tag == "Whiteboard"))
            {
                
                return;
            }
            whiteboard = touch.collider.gameObject.GetComponent<LineRendererWhiteboard>();
            if (currentController != null)
            {
                SendHaptics();
            }

            Vector3 Offset = touch.normal.normalized * 0.001f;

            
            whiteboard.SetTouchPosition(touch.point + Offset);
            

            if (!lastTouch)
            {
                lastTouch = true;
                lastAngle = transform.rotation;
                whiteboard.SetColor(penColor);
                whiteboard.ToogleTouch(true);
            }
        }
        else
        {
            lastTouch = false;
            whiteboard.ToogleTouch(false);
        }

        if (lastTouch)
        {
            transform.rotation = lastAngle;
        }
    }

    private void SendHaptics()
    {
        float intensity = Vector3.Distance(currentController.transform.position, attachPoint.position);

        currentController.SendHapticImpulse(intensity, 0.2f);
    }

    private void GetController(XRBaseInteractor interactor)
    {
        currentController = interactor.GetComponent<XRController>();
    }

    // CollisionsGedöns, nicht zum Laufen gebracht

    //public void OnTipCollisionStay(Collision collision)
    //{
    //    if (!(collision.collider.tag == "Whiteboard"))
    //    {
    //        return;
    //    }

    //    if (currentController != null)
    //    {
    //        SendHaptics();
    //    }

    //    Vector3 Offset = touch.normal.normalized * 0.001f;

    //    whiteboard.SetTouchPosition(collision.GetContact(0).point + Offset);

    //    if (!lastTouch)
    //    {
    //        lastTouch = true;
    //        lastAngle = transform.rotation;
    //        whiteboard.SetColor(penColor);
    //        whiteboard.ToogleTouch(true);
    //    }

    //    transform.rotation = lastAngle;
    //}

    //public void OnTipCollisionExit()
    //{
    //    lastTouch = false;
    //    whiteboard.ToogleTouch(false);
    //}

    //public void OnCollisionStay(Collision collision)
    //{
    //    Debug.Log(collision.collider.tag);

    //    if (!(collision.collider.CompareTag("Whiteboard")))
    //    {
    //        return;
    //    }

    //    if (currentController != null)
    //    {
    //        SendHaptics();
    //    }

    //    Vector3 Offset = touch.normal.normalized * 0.001f;

    //    whiteboard.SetTouchPosition(collision.GetContact(0).point + Offset);

    //    if (!lastTouch)
    //    {
    //        lastTouch = true;
    //        lastAngle = transform.rotation;
    //        whiteboard.SetColor(penColor);
    //        whiteboard.ToogleTouch(true);
    //    }

    //    transform.rotation = lastAngle;
    //}

    //public void OnCollisionExit(Collision collision)
    //{
    //    lastTouch = false;
    //    whiteboard.ToogleTouch(false);
    //}
}
