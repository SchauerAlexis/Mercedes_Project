using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaceNotes : MonoBehaviour
{
    public GameObject Keys;
    public GameObject Input;
    private GameObject NoteHolder;
    private GameObject Note;
    public float distance;
    public float hight;
    public Vector3 noteOffset;

    private PlayerControlManager pcm;
    

    // Start is called before the first frame update
    void Start()
    {
        pcm = FindObjectOfType<PlayerControlManager>();
        NoteHolder = new GameObject("NoteHolder");
        NoteHolder.transform.localPosition = Vector3.zero;
        NoteHolder.transform.localRotation = Quaternion.identity;
    }

    public void CallStand()
    {
        Debug.Log(pcm.playCamera.transform.position);
        Vector3 cameraPosition = pcm.playCamera.transform.position;
        Vector3 hightOffset = new Vector3(0, hight - pcm.playCamera.transform.position.y, 0);
        Vector3 forwardOffset = distance * Vector3.ProjectOnPlane(pcm.playCamera.transform.forward, Vector3.up);
        this.transform.position = cameraPosition + hightOffset + forwardOffset;

        Input.transform.rotation = Quaternion.LookRotation(cameraPosition - this.transform.position);
        Keys.transform.rotation = Quaternion.LookRotation(-forwardOffset);
    }

    public void PlaceText()
    {
        Note = Instantiate(Resources.Load("Note", typeof(GameObject))) as GameObject;

        Note.transform.parent = Keys.transform;
        Note.transform.localPosition = noteOffset;
        Note.transform.localRotation = Quaternion.identity;
        Note.GetComponent<NoteHelper>().noteText.text = Input.GetComponentInChildren<TMP_InputField>().text;
        Note.transform.parent = NoteHolder.transform;
    }
}
