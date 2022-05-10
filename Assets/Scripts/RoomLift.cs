using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLift : MonoBehaviour
{
    public GameObject[] rooms;
    public GameObject[] pics;
    public GameObject[] dependencies;
    public int index = 0;
    private bool allow = true;

    [Header("Movement Speed")]
    public float speed;

    [Header("Hight of the Rooms")]
    public Vector3 roomHubDistance;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject room in rooms)
        {
            if (room != rooms[index])
            {
                room.SetActive(false);
                StartCoroutine(ResetRoom(room));
            }
            else
                room.SetActive(true);
        }

        if (pics[index] != null)
        {
            foreach (GameObject pic in pics)
            {
                if (pic != pics[index])
                    pic.SetActive(false);
                else
                    pic.SetActive(true);
            }
        }

        if (dependencies[index] != null)
        {
            foreach (GameObject dependence in dependencies)
            {
                if (dependence != pics[index])
                    dependence.SetActive(false);
                else
                    dependence.SetActive(true);
            }
        }
    }

    public void ChangeLift()
    {
        if (allow)
        {
            allow = false;

            StartCoroutine(ResetRoom(rooms[index]));

            if (pics[index] != null)
                pics[index].SetActive(false);

            if (dependencies[index] != null)
                dependencies[index].SetActive(false);

            index = (index + 1) % rooms.Length;

            StartCoroutine(SetRoom(rooms[index]));

            if (pics[index] != null)
                pics[index].SetActive(true);

            if (dependencies[index] != null)
                dependencies[index].SetActive(true);
        }
    }

    public void SetLift(int roomIndex)
    {
        if (allow)
        {
            if ((roomIndex < rooms.Length) && (roomIndex != index))
            {
                allow = false;

                StartCoroutine(ResetRoom(rooms[index]));

                if (pics[index] != null)
                    pics[index].SetActive(false);

                if (dependencies[index] != null)
                    dependencies[index].SetActive(false);

                index = roomIndex;

                StartCoroutine(SetRoom(rooms[index]));

                if (pics[index] != null)
                    pics[index].SetActive(true);

                if (dependencies[index] != null)
                    dependencies[index].SetActive(true);
            }
        }
    }

    IEnumerator SetRoom(GameObject room)
    {
        Vector3 startPosition = room.transform.localPosition;
        Vector3 endPosition = startPosition + roomHubDistance;

        float hubspeed = 0.1f / speed;

        room.SetActive(true);

        for (int i = 0; i <= 100; i++)
        {
            room.transform.localPosition = Vector3.Lerp(startPosition, endPosition, i / 100f);
            yield return new WaitForSeconds(hubspeed);
        }

        allow = true;

        yield return null;
    }

    IEnumerator ResetRoom(GameObject room)
    {
        Vector3 startPosition = room.transform.localPosition;
        Vector3 endPosition = startPosition - roomHubDistance;

        float hubspeed = 0.1f / speed;

        /*
        foreach (Joint j in room.GetComponentsInChildren<Joint>())
        {
            j.gameObject.SetActive(false);
        }
        */

        for (int i = 0; i <= 100; i++)
        {
            room.transform.localPosition = Vector3.Lerp(startPosition, endPosition, i / 100f);
            yield return new WaitForSeconds(hubspeed);
        }

        room.SetActive(false);

        yield return null;
    }
}
