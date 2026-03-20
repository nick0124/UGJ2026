using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();
    
    void Start()
    {

    }
    
    public void SetActiveAudio(int index)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (i == index)
            {
                if (audioSources[i].volume == 0f)
                {
                    audioSources[i].volume = 1f;
                }
                else
                {
                    audioSources[i].volume = 0f;
                }
            }
        }
    }
}
