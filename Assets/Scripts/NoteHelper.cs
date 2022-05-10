using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteHelper : MonoBehaviour
{
    public TMP_Text noteText;
    
    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
