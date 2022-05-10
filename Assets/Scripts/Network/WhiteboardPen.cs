using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WhiteboardPen : MonoBehaviour
{
    public Whiteboard whiteboard;

    private XRGrabNetworkInteractable XRInteractable;

    public DrawingColor penColor = DrawingColor.Blue;

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
        whiteboard = GameObject.Find("Whiteboard").GetComponent<Whiteboard>();
        tip = this.transform.Find("Tip");
        attachPoint = this.transform.Find("AttachPoint");

        XRInteractable = GetComponent<XRGrabNetworkInteractable>();
        XRInteractable.onSelectEntered.AddListener(GetController);
    }

    // Update is called once per frame
    void Update()
    {
        if (XRInteractable.isSelected)
        {
            GetTouch();
        }
    }


    private void GetTouch()
    {
        float tipHeight = tip.localScale.y;
        Vector3 tipVector3 = tip.transform.position;

        if (Physics.Raycast(tipVector3, transform.up, out touch, tipHeight))
        {
            if (!(touch.collider.tag == "Whiteboard"))
            {
                return;
            }
            whiteboard = touch.collider.GetComponent<Whiteboard>();

            if (currentController != null)
            {
                SendHaptics();
            }

            whiteboard.SetColor(penColor);
            whiteboard.SetTouchPosition(touch.textureCoord.x, touch.textureCoord.y);
            whiteboard.ToogleTouch(true);

            if (!lastTouch)
            {
                lastTouch = true;
                lastAngle = transform.rotation;
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
        float intensity = Vector3.Distance(currentController.transform.position, attachPoint.position) ;
        
        currentController.SendHapticImpulse(intensity, 0.2f);
    }

    private void GetController(XRBaseInteractor interactor)
    {
        currentController = interactor.GetComponent<XRController>();
    }
}

