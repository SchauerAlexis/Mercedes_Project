using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CSVTest : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Ordnerstruktur")]
    public string foldername = "CSVs";
    public string filename = "xxx";
    private List<string> filenames;
    [SerializeField] private List<string> csvs;
    public string usedCsv = "sprachen";
    public List<string[]> csv;
    public List<string[]> csvdata;

    public TMP_Dropdown drop;

    [Header("Name dieser Scene. Später gegen SceneManager Call ersetzen")]
    public string scene;

    [Header("Textfelder")]
    public List<TextMeshProUGUI> fields;

    [Header("Verfügbare Indexe")]
    [SerializeField] private List<string> fieldnames;

    [Header("Verfügbare Scenen")]
    [SerializeField] private List<string> scenen;

    [Header("Verfügbare Sprachen")]
    [SerializeField] private List<string> sprachen;

    void Start()
    {
        ReadCSV();
        MakeLists();
        MakeFields();
    }


    // Update is called once per frame
    private void ReadCSV()
    {
        filenames = new List<string>(Directory.GetFileSystemEntries(Application.streamingAssetsPath + "/" + foldername));
        csvs = new List<string>();
        csv = new List<string[]>();

        for (int i = 0; i < filenames.Count; i++)
        {
            if (filenames[i].EndsWith(".csv"))
            {
                csvs.Add(filenames[i]);

                if (filenames[i].Contains(usedCsv))
                {
                    string[] lines = File.ReadAllLines(csvs[i]);

                    foreach (string line in lines)
                    {
                        csv.Add(line.Split(';'));
                    }

                    filename = csv[0][0];
                }
            }
        }
    }

    private void MakeLists()
    {
        sprachen.AddRange(csv[1]);
        sprachen.RemoveRange(0, 4);

        drop.AddOptions(sprachen);

        csvdata = csv;
        csvdata.RemoveRange(0, 2);

        foreach (string[] entry in csvdata)
        {
            fieldnames.Add(entry[0]);

            if (!scenen.Contains(entry[1]))
                scenen.Add(entry[1]);
        }
    }

    public void MakeFields()
    {
        for (int i = 0; i < csvdata.Count; i++)
        {
            fields[i].text = csvdata[i][drop.value + 4];
            fields[i].fontSize = Convert.ToInt32(csvdata[i][2]);

            if (csvdata[i][3] == "none")
                fields[i].fontStyle = FontStyles.Normal;
            else if (csvdata[i][3] == "italic")
                fields[i].fontStyle = FontStyles.Italic;
            else if (csvdata[i][3] == "bold")
                fields[i].fontStyle = FontStyles.Bold;
            else if (csvdata[i][3] == "line")
                fields[i].fontStyle = FontStyles.Underline;
        }
    }
}
