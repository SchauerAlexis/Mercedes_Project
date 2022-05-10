using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptButtons : MonoBehaviour
{
    public GameObject playerControlManager;

    private bool DebugXR = false;
    private bool DebugFPC = false;
    private bool DebugCam = false;

    private ChangeActivePlayerSettings[] capsArray;
    void Start()
    {
        capsArray = GetComponentsInChildren<ChangeActivePlayerSettings>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        DebugXR = playerControlManager.GetComponent<PlayerControlManager>().DebugXR;
        DebugFPC = playerControlManager.GetComponent<PlayerControlManager>().DebugFPC;
        DebugCam = playerControlManager.GetComponent<PlayerControlManager>().DebugCam;
        StartCoroutine(ButtonAdapt());
    }

    IEnumerator ButtonAdapt()
    {
        yield return new WaitForSeconds(0.1f);

        foreach (ChangeActivePlayerSettings caps in capsArray)
        {
            if (((caps.DebugXR) && (DebugXR))||((caps.DebugFPC) && (DebugFPC))||((caps.DebugCam) && (DebugCam)))
                caps.gameObject.SetActive(true);
            else
                caps.gameObject.SetActive(false);
        }

        yield return new WaitForEndOfFrame();
    }
}
