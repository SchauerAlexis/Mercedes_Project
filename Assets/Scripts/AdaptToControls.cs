using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptToControls : MonoBehaviour
{
    public PlayerControlManager playerControlManager;

    private int internIndex = 100;

    public GameObject[] FPCObjects;
    public GameObject[] XrObjects;
    public GameObject[] CamObjects;

    // Update is called once per frame
    void Start()
    {
        if (playerControlManager == null)
            playerControlManager = FindObjectOfType<PlayerControlManager>();

        InvokeRepeating("CheckControls", 1f, 1f);
    }

    private void CheckControls()
    {
        if (internIndex != playerControlManager.activePlayerIndex)
        {
            internIndex = playerControlManager.activePlayerIndex;

            if (internIndex == 0)
            {
                foreach (GameObject obj in FPCObjects)
                {
                    if (obj != null)
                        if (!obj.activeInHierarchy)
                            obj.SetActive(true);
                }

                foreach (GameObject obj in XrObjects)
                {
                    if (obj != null)
                        if (obj.activeInHierarchy)
                            obj.SetActive(false);
                }

                foreach (GameObject obj in CamObjects)
                {
                    if (obj != null)
                        if (obj.activeInHierarchy)
                            obj.SetActive(false);
                }
            }
            else if (internIndex == 1)
            {
                foreach (GameObject obj in FPCObjects)
                {
                    if (obj != null)
                        if (obj.activeInHierarchy)
                            obj.SetActive(false);
                }

                foreach (GameObject obj in XrObjects)
                {
                    if (obj != null)
                        if (!obj.activeInHierarchy)
                            obj.SetActive(true);
                }

                foreach (GameObject obj in CamObjects)
                {
                    if (obj != null)
                        if (obj.activeInHierarchy)
                            obj.SetActive(false);
                }
            }
            else if (internIndex == 2)
            {
                foreach (GameObject obj in FPCObjects)
                {
                    if (obj != null)
                        if (obj.activeInHierarchy)
                            obj.SetActive(false);
                }

                foreach (GameObject obj in XrObjects)
                {
                    if (obj != null)
                        if (obj.activeInHierarchy)
                            obj.SetActive(false);
                }

                foreach (GameObject obj in CamObjects)
                {
                    if (obj != null)
                        if (!obj.activeInHierarchy)
                            obj.SetActive(true);
                }
            }
            else
                Debug.Log("Check Controlls Error");
        }
    }
}
