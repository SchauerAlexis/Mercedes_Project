using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkNoVRHandCorrection : MonoBehaviour
{
    public bool facingLocalXRRig = false;
    public Transform LookAtPoint;
    public Vector3 Orientation;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LookAtPoint.GetComponent<Renderer>().enabled == false)
            LookAtPoint.GetComponent<Renderer>().enabled = true;

        if (facingLocalXRRig)
            this.transform.LookAt(LookAtPoint.position, Orientation);
        else
            this.transform.rotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y * -1, 0);
    }
}
