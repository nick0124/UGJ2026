using UnityEngine;

public class TurretMusic : MonoBehaviour
{
    private MusicController musicController;
    public MidiMusic midiMusic;
    [SerializeField] private int audioIndex;
    private Shooting shooting;
    private LookAtEnemy lookAtEnemy;

    private void Start()
    {
        FindMusicController();
        
        if (musicController != null)
        {
            musicController.SetActiveAudio(audioIndex);
            midiMusic = musicController.GetMidiMusic(audioIndex);
            midiMusic.AddMidiNoteListener(OnMidiNote);

            shooting = GetComponent<Shooting>();
            lookAtEnemy = GetComponent<LookAtEnemy>();
            
            // Вариант 2: если нужно передавать индекс из другого места
            // musicController.SetActiveAudio(GetComponent<SomeOtherScript>().someIndex);
        }
        else
        {
            Debug.LogWarning("MusicController не найден на сцене!");
        }
    }

    private void OnDestroy()
    {
        if (musicController != null)
        {
            musicController.SetActiveAudio(audioIndex);
            midiMusic.RemoveMidiNoteListener(OnMidiNote);
        }
    }

    private void FindMusicController()
    {
        musicController = FindFirstObjectByType<MusicController>();
        
        if (musicController == null)
        {
            Debug.LogError("MusicController не найден на сцене!");
        }
    }

    void OnMidiNote(int noteIndex, float time)
    {
        Debug.Log($"Нота #{noteIndex} в момент {time} секунд");
        if(lookAtEnemy.IsEnemyInRange())
        {
            shooting.Shoot();
        }
    }
}
