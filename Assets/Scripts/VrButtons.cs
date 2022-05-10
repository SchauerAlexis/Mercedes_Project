using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VrButtons : MonoBehaviour
{
    private Button butt;
    public UnityEvent DoSmt;
    // Start is called before the first frame update
    void Start()
    {
        butt = GetComponentInChildren<Button>();
        butt.onClick.AddListener(Do);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        DoSmt.Invoke();
    }

    private void Do()
    {
        DoSmt.Invoke();
    }
}
