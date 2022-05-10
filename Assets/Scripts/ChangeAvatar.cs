using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAvatar : MonoBehaviour
{
    public GameObject[] hairs;
    public GameObject[] skins;
    public GameObject[] glases;

    private int hairIndex = 0;
    private int skinIndex = 0;
    private int glasIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("hair", hairIndex);
        foreach (GameObject hair in hairs)
            hair.SetActive(false);
        hairs[hairIndex].SetActive(true);

        PlayerPrefs.SetInt("skin", skinIndex);
        foreach (GameObject skin in skins)
            skin.SetActive(false);
        skins[skinIndex].SetActive(true);

        PlayerPrefs.SetInt("glas", glasIndex);
        foreach (GameObject glas in glases)
            glas.SetActive(false);
        glases[glasIndex].SetActive(true);
    }

    // Update is called once per frame
    public void NextHair()
    {
        hairIndex = (hairIndex + 1) % hairs.Length;

        foreach (GameObject hair in hairs)
            hair.SetActive(false);

        hairs[hairIndex].SetActive(true);

        PlayerPrefs.SetInt("hair", hairIndex);
    }

    public void NextSkin()
    {
        skinIndex = (skinIndex + 1) % skins.Length;

        foreach (GameObject skin in skins)
            skin.SetActive(false);

        skins[skinIndex].SetActive(true);

        PlayerPrefs.SetInt("skin", skinIndex);
    }

    public void NextGlas()
    {
        glasIndex = (glasIndex + 1) % glases.Length;

        foreach (GameObject glas in glases)
            glas.SetActive(false);

        glases[glasIndex].SetActive(true);

        PlayerPrefs.SetInt("glas", glasIndex);
    }
}
