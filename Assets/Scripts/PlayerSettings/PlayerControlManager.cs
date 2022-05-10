using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerControlManager : MonoBehaviour
{
    [Header("Multiplayer Options")]
    public GameObject networkManager;
    public GameObject networkApplication;
    public bool useMultiPlayer = false;
    public bool isIntroScene = false;
    public bool debugStream = false;
    [Space(10f)]
    public bool online = false;
    public bool offline = false;

    [Header("Choose Debug Players. There are also VR/XR Players and Cameras")]
    public bool DebugFPC = false;
    public bool DebugXR = false;
    public bool DebugCam = false;

    [Header("Momentary Active Player")]
    public GameObject activePlayer;
    public int activePlayerIndex;

    [Header("Existing Players")]
    public GameObject FPCPlayer;
    public GameObject XRPlayer;
    public GameObject CamPlayer;

    [Header("Player Interfaces")]
    [SerializeField] private GameObject FPCInterface;
    [SerializeField] private GameObject XRInterface;
    [SerializeField] private GameObject CamInterface;

    [Header("Button Parameters for the size and Position of the On Screen Buttons")]
    [SerializeField] private float Edgewidth = 10f;
    [SerializeField] private float Mainwidth = 9f;
    [SerializeField] private int height = 25;
    [SerializeField] private int topOffset = 10;
    private int top = 0;

    [Header("The hight of the First Person Player")]
    public float playerhight = 3.5f;

    [Header("Where you spawn when changing players")]
    public bool synchronize = false;
    public Transform spawnPoint;
    [Space(2f)]
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;

    [Header("Button Variables")]
    public int playercount = 0;
    public int buttoncount = 0;

    private bool isXRAvailable = false;
    private string loadedDeviceName;
    private GameObject[] Players;
    private GameObject[] Interfaces;
    private Canvas[] cans;
    private Camera[] cams;
    public Camera playCamera;


    private void Awake()
    {
        if (!useMultiPlayer && !PlayerPrefs.HasKey("MultiPlayer") && !isIntroScene)
        {
            if (networkManager != null)
                Destroy(networkManager);

            if (networkApplication != null)
                networkApplication.SetActive(false);
        }

        InvokeRepeating("CheckDebug", 10f, 20f);
    }

    void CheckDebug()
    {
        if (debugStream)
        {
            Debug.Log("Active?  " + XRSettings.isDeviceActive);
            Debug.Log("XRAvail?  " + XRSettings.enabled);

            Debug.Log("Save state: " + PlayerPrefs.HasKey("SaveState"));
            if (PlayerPrefs.HasKey("SaveState"))
                Debug.Log("Save state value: " + PlayerPrefs.GetInt("SaveState"));

            Debug.Log("MultiPlayer: " + PlayerPrefs.HasKey("MultiPlayer"));
            if (PlayerPrefs.HasKey("MultiPlayer"))
                Debug.Log("MultiPlayer value: " + PlayerPrefs.GetInt("MultiPlayer"));
        }
    }

    void Start()
    {
        // makes sure that every player in the scene is placed and instantiated
        if (FPCPlayer == null)
        {
            Debug.Log("DefaultPlayer not in Inspector");
            try
            {
                FPCPlayer = GameObject.FindGameObjectWithTag("DefaultPlayer");
            }
            catch (System.Exception)
            {
                Debug.LogWarning("DefaultPlayer not in Scene");
            }
        }
        else
            FPCPlayer.SetActive(false);

        if (XRPlayer == null)
        {
            Debug.Log("XRPlayer not in Inspector");
            try
            {
                XRPlayer = GameObject.FindGameObjectWithTag("XRPlayer");
            }
            catch (System.Exception)
            {
                Debug.LogWarning("XRPlayer not in Scene");
            }
        }
        else
            XRPlayer.SetActive(false);

        if (CamPlayer == null)
        {
            Debug.Log("Overview not in Inspector");
            CamPlayer = GameObject.FindGameObjectWithTag("OverviewCamera");

            try
            {
                CamPlayer.SetActive(false);
            }
            catch (System.Exception)
            {
                Debug.LogWarning("Overview not in Scene");
            }
        }
        else
            CamPlayer.SetActive(false);


        Players = new GameObject[] { FPCPlayer, XRPlayer, CamPlayer };  // writes the Players in an Array with all Players
        Interfaces = new GameObject[] { FPCInterface, XRInterface, CamInterface }; // writes the Interfaces in an Array with all Players
        cans = Resources.FindObjectsOfTypeAll<Canvas>();  // an Array with all Canvas
        cams = Resources.FindObjectsOfTypeAll<Camera>();  // an Array with all Cameras
        playCamera = cams[0];
        // sets the spwan position and rotation to fixed
        spawnPosition = spawnPoint.position;
        spawnRotation = spawnPoint.rotation;

        //Debug.Log("Enabled?  " + XRSettings.enabled);
        //Debug.Log("List Names  " + XRSettings.loadedDeviceName);
        isXRAvailable = XRSettings.enabled;
        loadedDeviceName = XRSettings.loadedDeviceName;

        if (isXRAvailable)
        {
            Debug.Log("Using VR device: " + XRSettings.loadedDeviceName + " possible. Switch in GUI Settings"); // Displays the used Device in the Console and activates the function
        }
        else
        {
            Debug.Log("No real VR device");
            DebugXR = false;
        }


        if (PlayerPrefs.HasKey("SaveState"))
        {
            bool holdFPC = Convert.ToBoolean(PlayerPrefs.GetInt("FPC"));
            bool holdXR = Convert.ToBoolean(PlayerPrefs.GetInt("XR"));
            bool holdCam = Convert.ToBoolean(PlayerPrefs.GetInt("Cam"));
            int holdActive = PlayerPrefs.GetInt("Player");

            DebugPlayerSetting(holdFPC, holdXR, holdCam, true);

            switch (holdActive)
            {
                case 1:
                    DebugPlayerSetting(false, true, false, false);
                    break;
                case 2:
                    DebugPlayerSetting(false, false, true, false);
                    break;
                default:
                    DebugPlayerSetting(true, false, false, false);
                    break;
            }

            Debug.Log("Used Saved Values");
        }
        else
        {
            DebugPlayerSetting(DebugFPC, DebugXR, DebugCam, true);
            PlayerPrefs.SetInt("SaveState", 1);

            Debug.Log("Created Saved Values");
        }

        if (PlayerPrefs.HasKey("MultiPlayer"))
        {
            if (PlayerPrefs.GetInt("MultiPlayer") == 1)
                online = true;
            else
                offline = true;
        }
        else
        {
            if (networkManager != null)
                networkManager.SetActive(false);
        }
    }

    public void DebugPlayerSetting(bool FPC, bool XR, bool Cam, bool configurate)
    {
        if (!online)
        {
            SynchronizePositions();

            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].SetActive(false);
            }

            // Set Players
            if (XR)
            {
                StartCoroutine(EnableVR());
                Players[1].SetActive(true);
            }
            else if (FPC)
            {
                StartCoroutine(DisableVR());
                Players[0].SetActive(true);
            }
            else if (Cam)
            {
                StartCoroutine(DisableVR());
                Players[2].SetActive(true);
            }
            else
            {
                Debug.Log("No Player choosen");
            }

            if (configurate)
            {
                // Raises Playercount for every activated Debug Player
                playercount = Convert.ToInt32(XR) + Convert.ToInt32(FPC) + Convert.ToInt32(Cam);

                // Synchronize bools in case of a public call
                PlayerPrefs.SetInt("XR", Convert.ToInt32(XR));
                PlayerPrefs.SetInt("FPC", Convert.ToInt32(FPC));
                PlayerPrefs.SetInt("Cam", Convert.ToInt32(Cam));

                DebugXR = XR;
                DebugFPC = FPC;
                DebugCam = Cam;
            }

            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i].activeSelf)
                {
                    Interfaces[i].SetActive(true);
                    activePlayer = Players[i];
                    activePlayerIndex = i;
                }
                else
                {
                    Interfaces[i].SetActive(false);
                }
            }

            PlayerPrefs.SetInt("Player", activePlayerIndex);
            StartCoroutine(SwitchEventCamera());
        }
        else
        {
            Debug.Log("Cant Switch Players when Playing Online!");
        }
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD

    private void OnGUI()
    {
        if (online)
        {
            DrawLineMode("Online");
        }
        else if (offline)
        {
            DrawLineMode("Offline");
            Draw2DDebug();
        }
        else
        {
            Draw2DDebug();
        }
    }

    public void DrawLineMode(string Line)
    {
        top = 2 * height;

        string text = Line;
        float width = (Edgewidth + text.Length * Mainwidth);
        float left = Screen.width * 0.5f - width;

        if (GUI.Button(new Rect(left, top, 2 * width, 2 * height), text))
        {
            Debug.Log("Did Something");
        }
    }

    public void Draw2DDebug()
    {
        if (playercount > 1)
        {
            buttoncount = 0;
            top = Screen.height - height - topOffset;

            if ((DebugFPC) && (!FPCPlayer.activeSelf))
            {
                buttoncount++;
                string text = "First Person Player";
                float width = (Edgewidth + text.Length * Mainwidth);
                float left = Screen.width * ((float)buttoncount / (float)playercount) - width / 2f;

                if (GUI.Button(new Rect(left, top, width, height), text))
                {
                    DebugPlayerSetting(true, false, false, false);
                }
            }

            if ((DebugXR) && (!XRPlayer.activeSelf))
            {
                buttoncount++;
                string text = "XR Rig";
                float width = (Edgewidth + text.Length * Mainwidth);
                float left = Screen.width * ((float)buttoncount / (float)playercount) - width / 2f;

                if (GUI.Button(new Rect(left, top, width, height), text))
                {
                    DebugPlayerSetting(false, true, false, false);
                }
            }

            if ((DebugCam) && (!CamPlayer.activeSelf))

            {
                buttoncount++;
                string text = "DebugCam";
                float width = (Edgewidth + text.Length * Mainwidth);
                float left = Screen.width * ((float)buttoncount / (float)playercount) - width / 2f;

                if (GUI.Button(new Rect(left, top, width, height), text))
                {
                    DebugPlayerSetting(false, false, true, false);
                }
            }
        }

    }

#endif

    IEnumerator SwitchEventCamera()
    {
        yield return new WaitForSeconds(1f);

        foreach (GameObject Play in Players)
        {
            if (Play.activeSelf)
            {
                playCamera = Play.GetComponentInChildren<Camera>();

                foreach (Canvas can in cans)
                {
                    can.worldCamera = playCamera;
                }
            }
        }
        yield return null;
    }

    private void SynchronizePositions()
    {
        if (FPCPlayer.activeSelf)
        {
            if (synchronize)
            {
                // Resets the spawn position und view direction to the most recent one
                spawnPosition = FPCPlayer.transform.position;
                spawnRotation.y = FPCPlayer.transform.rotation.y;
            }

            // overview is spawned on headhight of the First Person Player
            CamPlayer.transform.position = new Vector3(spawnPosition.x, spawnPosition.y + playerhight, spawnPosition.z);
            CamPlayer.transform.rotation = spawnRotation;

            XRPlayer.transform.position = spawnPosition;
            XRPlayer.transform.rotation = spawnRotation;
        }

        if (XRPlayer.activeSelf)
        {
            if (synchronize)
            {
                spawnPosition = XRPlayer.transform.position;
                spawnRotation.y = XRPlayer.transform.rotation.y;
            }

            CamPlayer.transform.position = new Vector3(spawnPosition.x, spawnPosition.y + playerhight, spawnPosition.z);
            CamPlayer.transform.rotation = spawnRotation;

            FPCPlayer.transform.position = spawnPosition;
            FPCPlayer.transform.rotation = spawnRotation;
        }

        if (CamPlayer.activeSelf)
        {
            if (synchronize)
            {
                spawnPosition = CamPlayer.transform.position;
                spawnRotation.y = CamPlayer.transform.rotation.y;
            }

            // First Person Player spawns on foot hight or on ground when Overview Camera is under the ground
            float spawnhight = spawnPosition.y - playerhight;
            if (spawnhight < 0.1f)
                spawnhight = 0.1f;

            FPCPlayer.transform.position = new Vector3(spawnPosition.x, spawnhight, spawnPosition.z);
            FPCPlayer.transform.rotation = spawnRotation;

            XRPlayer.transform.position = new Vector3(spawnPosition.x, spawnhight, spawnPosition.z);
            XRPlayer.transform.rotation = spawnRotation;
        }
    }

    IEnumerator EnableVR()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
        /*
        XRGeneralSettings.Instance.Manager.InitializeLoader();
        SubsystemManager.
        yield return null;
        XRSettings.enabled = true;
        */
    }

    IEnumerator DisableVR()
    {
        Debug.Log("Stopping XR...");

        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        yield return null;
        Debug.Log("XR stopped completely.");
        /*
       XRGeneralSettings.Instance.Manager.DeinitializeLoader();
       yield return null;
       XRSettings.enabled = false;
       */
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}


