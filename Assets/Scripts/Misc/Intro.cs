using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class Intro : MonoBehaviour
{
    VideoPlayer videoPlayer;
    bool videoFinished = false;

    float time = 0;
    [SerializeField] private float delayToEnableSkip;
    [SerializeField] private string nextSceneName;

    private void Awake()
    {
        time = 0;
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time < delayToEnableSkip) return;

        //Finished
        if (!videoPlayer.isPlaying && !videoFinished)
        {
            videoFinished = true;
            LoadScene.Load(nextSceneName);
        }

        //Skip
        if (Input.anyKeyDown)
        {
            LoadScene.Load(nextSceneName);
        }
    }
}
