using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiUseButton : MonoBehaviour
{
    public UnityEvent callEvent;
    // Update is called once per frame
    public void ButtonListener()
    {
        callEvent.Invoke();
    }

    public void OnTriggerEnter(Collider other)
    {
        callEvent.Invoke();
    }
}
