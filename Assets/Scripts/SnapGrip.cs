using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapGrip : MonoBehaviour
{
    private MouseInteractionManager mouse;
    private bool allowSnap;

    public GameObject parentPart;

    void Start()
    {
        mouse = FindObjectOfType<MouseInteractionManager>();
    }

    private void Update()
    {
        if (FindObjectOfType<PlayerControlManager>().activePlayerIndex == 1)
        {
            allowSnap = !GetComponent<XRGrabInteractable>().isSelected;
        }
        else
        {
            allowSnap = !mouse.hold;
        }

        if (allowSnap)
        {
            //parentPart.GetComponent<FollowPhysics>().enabled = false;
            transform.position = parentPart.transform.position;
            transform.rotation = parentPart.transform.rotation;
            
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = true;
            }
        }
        else
        {
            //parentPart.GetComponent<FollowPhysics>().enabled = true;
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
    }
}

