using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace VRV
{
    public class VirtualVisitVideo : MonoBehaviour
    {
        public VideoPlayer videoPlayer;

        public void SetSkyboxVideo(VideoClip clip)
        {
            if (clip != null && videoPlayer != null)
            {
                videoPlayer.clip = clip;
                videoPlayer.Play();
            }
        }
    }

}
