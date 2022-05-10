using System;
using UnityEngine;

public class ChangeActivePlayerSettings : MonoBehaviour
{
    public PlayerControlManager playerControlManager;

    [Header("Enable Overriding")]
    [Tooltip("Simple switching players without overriding Start Options possible. Only one bool can be activated in this case.")]
    [SerializeField] private bool Override = false;

    [Header("Choose new Player or multiple Players if you want to override Start Option")]
    public bool DebugXR = false;
    public bool DebugFPC = false;
    public bool DebugCam = false;

    // Start is called before the first frame update
    void Start()
    {
        playerControlManager = FindObjectOfType<PlayerControlManager>();

        if ((Convert.ToInt32(DebugXR) + Convert.ToInt32(DebugFPC) + Convert.ToInt32(DebugCam) > 1) && !Override)
        {
            Override = false;
            Debug.LogWarning("You cant switch to more than one Player. Choose Override if you want to reconfigure your settings");
        }
    }

    public void Switch()
    {
        playerControlManager.DebugPlayerSetting(DebugFPC, DebugXR, DebugCam, Override);
    }
}
