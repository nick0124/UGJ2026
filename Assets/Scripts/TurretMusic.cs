using UnityEngine;

public class TurretMusic : MonoBehaviour
{
    private MusicController musicController;
    [SerializeField] private int audioIndex;

    private void Start()
    {
        // Находим MusicController на сцене
        FindMusicController();
        
        // Вызываем метод при создании объекта
        if (musicController != null)
        {
            // Здесь можно задать нужный индекс. 
            // Вариант 1: задать индекс в инспекторе
            musicController.SetActiveAudio(audioIndex);
            
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
        // Вызываем метод при удалении объекта
        if (musicController != null)
        {
            musicController.SetActiveAudio(audioIndex);
        }
    }

    private void FindMusicController()
    {
        // Ищем объект со скриптом MusicController
        musicController = FindFirstObjectByType<MusicController>();
        
        if (musicController == null)
        {
            Debug.LogError("MusicController не найден на сцене!");
        }
    }
    
    // Публичный метод для установки индекса, если нужно изменить его динамически
    public void SetAudioIndex(int index)
    {
        audioIndex = index;
    }
}
