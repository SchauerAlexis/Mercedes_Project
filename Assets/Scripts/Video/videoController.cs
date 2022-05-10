//using System.IO;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Video;

public class videoController : MonoBehaviour
{
    public bool useClip = false;

    public VideoClip[] clips;
    private VideoPlayer video;

    private float videospeed = 1;
    [Space(20f)]
    public int index = 0;
    public GameObject pauseScreen;
    public string ordnername = "Videos";

    public List<string> filenames;
    public List<string> validNames;


    // Start is called before the first frame update
    void Start()
    {
        video = GetComponent<VideoPlayer>();

        if (useClip)
        {
            if (clips[index] != null)
                video.clip = clips[index];
        }
        else
        {
            filenames = new List<string>(Directory.GetFileSystemEntries(Application.streamingAssetsPath + "/" + ordnername));
            validNames = new List<string>();

            for (int i = 0; i < filenames.Count; i++)
            {
                if (filenames[i].EndsWith(".mp4"))
                {
                    validNames.Add(filenames[i]);
                }
            }

            video.url = validNames[index];
        }
    }


    public void StopStartVideo()
    {
        if (video.playbackSpeed != 0)
        {
            videospeed = video.playbackSpeed;
            video.playbackSpeed = 0;
            pauseScreen.SetActive(true);
        }
        else
        {
            video.playbackSpeed = videospeed;
            pauseScreen.SetActive(false);
        }
    }

    public void FastVideo()
    {
        if (video.playbackSpeed == 1)
        {
            video.playbackSpeed = 3;
        }
        else if (video.playbackSpeed == 3)
        {
            video.playbackSpeed = 1;
        }
        else
        {
            // nothing
        }
    }

    public void MuteVideo()
    {
        video.SetDirectAudioMute(0, !video.GetDirectAudioMute(0));
    }

    public void NextVideo()
    {
        StartCoroutine(Next());
    }

    private IEnumerator Next()
    {
        bool temp = video.GetDirectAudioMute(0);
        video.SetDirectAudioMute(0, true);

        if (!useClip)
        {
            index = (index + 1) % validNames.Count;
            yield return new WaitForSeconds(0.3f);
            video.url = validNames[index];
        }
        else
        {
            index = (index + 1) % clips.Length;
            yield return new WaitForSeconds(0.3f);
            if (clips[index] != null)
                video.clip = clips[index];
        }

        yield return new WaitForSeconds(0.3f);

        video.playbackSpeed = 0;

        yield return new WaitForSeconds(0.5f);

        video.SetDirectAudioMute(0, temp);
    }
}
