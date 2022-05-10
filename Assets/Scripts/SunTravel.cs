using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunTravel : MonoBehaviour
{
    private GameObject sun;
    private GameObject root;

    [Header("Aktiviert den Tageszeitenverlauf")]
    public bool useDay;

    [Header("Geschwindigkeit des Tagesverlauf (0 = Realtime)")]
    [Range(0, 2)]
    public int speed;

    [Header("Update Zeit")]
    public int tickTimer = 1;
    public float ticker = 1;
    private float tick = 0;
    private float minDay = 1;

    [Header("Lichter bei Nacht")]
    public Light[] nightLights;

    [Header("Automatischer Sonnenwinkel des Tages")]
    [SerializeField] private float inclination = 23;
    private float dist;

    private DateTime time;

    [Header("Drehachse zum Horizont")]
    public Vector2 eliptik;
    private Vector3 celDir1;
    private Vector3 celDir2;
    private Vector3 celDir3;
    private Vector3 celDir4;


    private Light sunlight;

    private GameObject ball;


    void Start()
    {
        ball = this.GetComponentInChildren<MeshRenderer>().gameObject;
        ball.SetActive(false);

        if (useDay)
        {
            sun = this.gameObject;
            root = this.transform.GetChild(0).gameObject;
            sunlight = this.GetComponentInChildren<Light>();
            sunlight.useColorTemperature = true;

            time = DateTime.Now;

            float doy = Math.Abs(time.DayOfYear - 183);

            inclination = 90 - (66 - (doy / 183 * 47));

            celDir1 = new Vector3(eliptik.x, 0, eliptik.y);
            root.transform.rotation = Quaternion.AngleAxis(inclination, celDir1);

            celDir2 = Vector3.Cross(celDir1, Vector3.up);

            float inclinationRad = inclination * (float)Math.PI / 180f;
            celDir3 = Vector3.RotateTowards(Vector3.up, celDir2, inclinationRad, 1f);
            celDir4 = Vector3.Cross(celDir3, celDir1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (useDay)
        {
            tick += Time.deltaTime;

            if (tick > ticker)
            {
                time = DateTime.Now;

                if (speed == 1)
                    minDay = ((time.Minute * 60) + time.Second) * 0.4f;
                else if (speed == 2)
                    minDay = ((time.Second * 1000) + time.Millisecond) * 0.024f;
                else
                    minDay = (time.Hour * 60) + time.Minute;

                ticker = tickTimer / (Mathf.Pow(35f, speed));  

                float currDeg = (0.25f * minDay) - 180f;

                sun.transform.rotation = Quaternion.AngleAxis(currDeg, celDir4);

                dist = sunlight.transform.position.y;
                sunlight.colorTemperature = 6600f - ((20f - dist) * 250);

                if (dist < -1)
                {
                    sunlight.enabled = false;
                    ball.SetActive(true);

                    foreach (Light li in nightLights)
                    {
                        li.enabled = true;
                    }
                }
                else
                {
                    sunlight.enabled = true;
                    ball.SetActive(false);

                    foreach (Light li in nightLights)
                    {
                        li.enabled = false;
                    }
                }

                tick = 0;
            }
        }
    }
}
