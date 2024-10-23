using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VirtualVisitVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    
    public void SetSkyboxVideo(VideoClip video)
    {
        videoPlayer.clip = video;
    }
}
