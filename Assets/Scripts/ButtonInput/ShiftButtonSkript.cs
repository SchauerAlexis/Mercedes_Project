using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShiftButtonSkript : MonoBehaviour
{

    public float ToggleBufferTime;
    float timer = 0.0f;
    public bool Shift;

    public Text ShiftButtonText;

    // Start is called before the first frame update
    void Start()
    {
        Shift = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (Shift)
        {
            ShiftButtonText.color = Color.red;
        }
        else
        {
            ShiftButtonText.color = Color.black;
        }
    }

    public void ShiftKey()
    {

        if (timer > ToggleBufferTime)
        {
            Shift = !Shift;
            
            timer = 0;
        }


    }
}
