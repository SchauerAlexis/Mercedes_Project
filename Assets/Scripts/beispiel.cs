using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class beispiel : MonoBehaviour
{

    public Text tutut;

    public GameObject hallo;

    private bool togle = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HalloAus()
    {
        tutut.enabled = togle;
        togle = !togle;
    }
}
