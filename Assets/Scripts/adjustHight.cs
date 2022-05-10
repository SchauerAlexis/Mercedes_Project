using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class adjustHight : MonoBehaviour
{
    public Slider slide;
    public CharacterController character;

    private Vector3 scale;
    public float multiply;

    // Start is called before the first frame update
    void Start()
    {
        scale = character.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newScale = new Vector3(1, 0.7f + (slide.value * multiply), 1);

        character.transform.localScale = Vector3.Scale(scale, newScale);
    }
}
