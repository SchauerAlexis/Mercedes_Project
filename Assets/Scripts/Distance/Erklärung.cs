using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erklärung : MonoBehaviour
{
    [Header("Erklärung zur Abstandsmessungmessung durch dieses Prefab")]
    public GameObject Distance;
    [Space(10)]

    [Header("Usable Components können auf andere GameObjects gezogen werden")]
    public GameObject UsableComponents;
    [Space(10)]

    [Header("Measuring führt die Messung aus")]
    public GameObject Measuring;
    [Space(10)]

    [Header("Toggle erlaubt an und ausschalten des Tools")]
    public GameObject Toggle;


    void Start()
    {

    }
}
