using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class RestartControlls : MonoBehaviour
{
    public bool restartControls = false;
    private XRDeviceSimulator simulator;

    // Start is called before the first frame update
    void Start()
    {
        simulator = GetComponent<XRDeviceSimulator>();  
    }

    // Update is called once per frame
    void OnEnable()
    {
        if (restartControls)
        {
            StartCoroutine(Restart());
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(0.5f);
        simulator.enabled = false;
        yield return new WaitForSeconds(1);
        simulator.enabled = true;
        yield return null;
    }
}
