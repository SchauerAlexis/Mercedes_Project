using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// debug abhängig von active index auswählen
// 4 Networkplayer bauen für die verschiedenen Modi

public class ModeManager : MonoBehaviour
{
    [Header("Player Control Manager")]
    public PlayerControlManager playerManager;
    private int activePlayerIndex = 0;

    [Header("Buttons")]
    public GameObject keyboard;
    public GameObject OnButton;
    public GameObject OffButton;
    public GameObject StartButton;
    public GameObject BackButton;

    [Header("Used Texts")]
    public GameObject ConnectionErrorText;
    public Text usedPlayertext;
    public TMPro.TMP_InputField input;
    public string sceneName = "Campus";

    private float ToggleBufferTime = 0.5f;
    private float timer = 0.0f;
    private int isMulti = 0;

    private void Awake()
    {
        PlayerPrefs.DeleteKey("Player");
        PlayerPrefs.DeleteKey("MultiPlayer");
    }
    private void Start()
    {
        isMulti = 0;
        InvokeRepeating("UpdatePlayer", 2f, 1f);
    }

    void Update()
    {

    }

    void UpdatePlayer()
    {
        activePlayerIndex = playerManager.activePlayerIndex;
        usedPlayertext.text = playerManager.activePlayer.name;
    }

    public void OnlineMode()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (!ConnectionErrorText.activeSelf)
                StartCoroutine("ConnectionError");
        }
        else
        {
            PlayerPrefs.SetString("Mode", "Online");
            keyboard.SetActive(true);
            BackButton.SetActive(true);
            OnButton.SetActive(false);
            OffButton.SetActive(false);
        }
    }

    public void SetNicknameAndLoadScene()
    {
        if (input.text == "")
            input.text = "Pseudo Name";

        PhotonNetwork.NickName = input.text;
        Debug.Log("NickName: " + PhotonNetwork.NickName);

        isMulti = 1;
        StartButton.SetActive(true);
        keyboard.SetActive(false);
    }

    public void OfflineMode()
    {
        PlayerPrefs.SetString("Mode", "Offline");
        PhotonNetwork.OfflineMode = true;
        BackButton.SetActive(true);
        OnButton.SetActive(false);
        OffButton.SetActive(false);
        StartButton.SetActive(true);
    }

    IEnumerator ConnectionError()
    {
        yield return new WaitForSeconds(0.1f);
        ConnectionErrorText.SetActive(true);
        yield return new WaitForSeconds(1f);
        ConnectionErrorText.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        ConnectionErrorText.SetActive(true);
        yield return new WaitForSeconds(1f);
        ConnectionErrorText.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        ConnectionErrorText.SetActive(true);
        yield return new WaitForSeconds(1f);
        ConnectionErrorText.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }

    public void BackToMenu()
    {
        OnButton.SetActive(true);
        OffButton.SetActive(true);
        BackButton.SetActive(false);
        keyboard.SetActive(false);
        StartButton.SetActive(false);

        isMulti = 0;
    }

    public void ButtonStart()
    {
        PlayerPrefs.SetInt("Player", activePlayerIndex);
        PlayerPrefs.SetInt("MultiPlayer", isMulti);

        SceneManager.LoadScene(sceneName);
    }
}
