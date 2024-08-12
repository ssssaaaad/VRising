using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public GameObject menu;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Play();
        if (menu == null)
            return;
        videoPlayer.loopPointReached += OnVideoEnd;
    }
    void OnDestroy()
    {
        if (menu == null)
            return;
        videoPlayer.loopPointReached -= OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        menu.SetActive(true);
    }

}
