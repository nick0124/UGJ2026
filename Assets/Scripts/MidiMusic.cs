using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class MidiMusic : MonoBehaviour
{
    [SerializeField] private TextAsset midiFile;
    [SerializeField] private AudioSource audioSource;
    
    private List<float> noteTimes = new List<float>();
    private int currentNoteIndex = 0;
    private double startTime;
    private bool isLooping = true; // Флаг для зацикливания

    private bool isSoundEnabled = false;

    private bool isPlaying = false;


    [Header("MIDI Events")]
    public MidiNoteEvent OnMidiNote;
    
    [System.Serializable]
    public class MidiNoteEvent : UnityEvent<int, float> { }

    public bool IsSoundEnabled 
    { 
        get { return isSoundEnabled; }
        set 
        { 
            isSoundEnabled = value;
            if (audioSource != null)
            {
                audioSource.volume = isSoundEnabled ? 1 : 0;
            }
        }
    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }
    
    void Start()
    {            
        if (OnMidiNote == null)
            OnMidiNote = new MidiNoteEvent();

        audioSource.volume = 0;

        LoadMidiFile();
    }

    void LoadMidiFile()
    {
        try
        {
            using (var memoryStream = new System.IO.MemoryStream(midiFile.bytes))
            {
                var loadedMidiFile = MidiFile.Read(memoryStream);
                var notes = loadedMidiFile.GetNotes();
                var tempoMap = loadedMidiFile.GetTempoMap();
                
                noteTimes.Clear();
                foreach (var note in notes)
                {
                    float timeInSeconds = (float)note.TimeAs<MetricTimeSpan>(tempoMap).TotalSeconds;
                    noteTimes.Add(timeInSeconds);
                }
                
                Debug.Log($"{gameObject.name}: Загружено нот: {noteTimes.Count}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка загрузки MIDI: {e.Message}");
        }
    }

    public void StartPlaybackAtTime(double globalStartTime)
    {
        if (isPlaying) return;
        
        startTime = globalStartTime;
        isPlaying = true;
        currentNoteIndex = 0;
        
        audioSource.Play();
        audioSource.loop = true;
        
        StartCoroutine(PlayNotes());
    }

    IEnumerator PlayNotes()
    {
        while (isLooping && isPlaying)
        {
            while (currentNoteIndex < noteTimes.Count && isPlaying)
            {
                double currentTime = AudioSettings.dspTime - startTime;
                float nextNoteTime = noteTimes[currentNoteIndex];
                
                if (currentTime >= nextNoteTime - 0.001)
                {
                    OnMidiNote?.Invoke(currentNoteIndex, (float)currentTime);
                    currentNoteIndex++;
                    yield return new WaitForSeconds(0.001f);
                }
                else
                {
                    yield return null;
                }
            }
            
            if (isPlaying)
            {
                // Цикл - сброс времени начала для следующего прохода
                float audioLength = audioSource.clip.length;
                double timeUntilNextLoop = audioLength - (AudioSettings.dspTime - startTime);
                
                if (timeUntilNextLoop > 0)
                {
                    yield return new WaitForSeconds((float)timeUntilNextLoop);
                }
                
                startTime = AudioSettings.dspTime;
                currentNoteIndex = 0;
            }
        }
    }

    public void AddMidiNoteListener(UnityAction<int, float> listener)
    {
        OnMidiNote.AddListener(listener);
    }
    
    public void RemoveMidiNoteListener(UnityAction<int, float> listener)
    {
        OnMidiNote.RemoveListener(listener);
    }
}