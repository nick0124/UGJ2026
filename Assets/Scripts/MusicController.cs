using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private List<MidiMusic> audioSources = new List<MidiMusic>();

    void Start()
    {
        var globalStartTime = AudioSettings.dspTime;

        foreach (var player in audioSources)
        {
            player.StartPlaybackAtTime(globalStartTime);
        }
    }
        
    public void SetActiveAudio(int index)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (i == index)
            {
                if (audioSources[i].IsSoundEnabled)
                {
                    audioSources[i].IsSoundEnabled = false;
                }
                else
                {
                    audioSources[i].IsSoundEnabled = true;
                }
            }
        }
    }

    public MidiMusic GetMidiMusic(int index)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (i == index)
            {
                return audioSources[i];
            }
        }

        return null;
    }
}
