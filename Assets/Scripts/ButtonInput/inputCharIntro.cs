using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class inputCharIntro : MonoBehaviour
{
    private float ToggleBufferTime = 0.1f;
    float timer = 0.0f;

    public TMP_InputField Keypad;
    public GameObject ShiftButton;
    public string AddChar;
    public string AddCapital;

    bool Shift;

    void Update()
    {
        timer += Time.deltaTime;

        Shift = ShiftButton.GetComponent<ShiftButtonSkript>().Shift;
    }

    public void InputChars()
    {
        if (timer > ToggleBufferTime)
        {
            if (Shift)
            {
                Keypad.text += AddCapital;
            }
            else
            {
                Keypad.text += AddChar;
            }


            timer = 0;
        }
    }

    public void DeleteChars()
    {
        if (timer > ToggleBufferTime)
        {
            Keypad.text = "";

            timer = 0;
        }
    } 
    
    public void Space()
    {
        if (timer > ToggleBufferTime)
        {
            Keypad.text += " ";

            timer = 0;
        }
    }

    public void Backspace()
    {
        if (timer > ToggleBufferTime)
        {
            int HLenght = Keypad.text.Length;

            Keypad.text = Keypad.text.Remove(HLenght - 1);

            timer = 0;
        }
    }


}
