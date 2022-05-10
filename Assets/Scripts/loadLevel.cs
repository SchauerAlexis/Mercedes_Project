using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadLevel : MonoBehaviour
{
    public string sceneName = "Intro";

    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
