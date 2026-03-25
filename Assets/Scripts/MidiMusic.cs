using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;

public class MidiMusic : MonoBehaviour
{
    [SerializeField] private TextAsset midiFile;
    [SerializeField] private AudioSource audioSource;
    
    private List<float> noteTimes = new List<float>();
    private int currentNoteIndex = 0;
    private double startTime;
    private bool isPlaying = false;
    private bool isLooping = true; // Флаг для зацикливания
    
    void Start()
    {
        if (midiFile == null)
        {
            Debug.LogError("MIDI файл не назначен!");
            return;
        }
        
        LoadAndStartPlayback();
    }
    
    void LoadAndStartPlayback()
    {
        try
        {
            // Загружаем MIDI файл
            using (var memoryStream = new System.IO.MemoryStream(midiFile.bytes))
            {
                var loadedMidiFile = MidiFile.Read(memoryStream);
                
                // Получаем все ноты
                var notes = loadedMidiFile.GetNotes();
                
                // Получаем темп для перевода в реальное время
                var tempoMap = loadedMidiFile.GetTempoMap();
                
                // Сохраняем время каждой ноты в секундах
                noteTimes.Clear();
                foreach (var note in notes)
                {
                    float timeInSeconds = (float)note.TimeAs<MetricTimeSpan>(tempoMap).TotalSeconds;
                    noteTimes.Add(timeInSeconds);
                }
                
                Debug.Log($"Всего нот: {noteTimes.Count}");
                
                // Запускаем музыку и синхронизируем время
                audioSource.Play();
                audioSource.loop = true;
                
                // Используем AudioSettings.dspTime для точной синхронизации
                startTime = AudioSettings.dspTime;
                isPlaying = true;
                
                // Запускаем корутину для воспроизведения нот
                StartCoroutine(PlayNotes());
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка: {e.Message}");
        }
    }
    
    IEnumerator PlayNotes()
    {
        while (isLooping) // Бесконечный цикл
        {
            // Сбрасываем индекс для нового цикла
            currentNoteIndex = 0;
            
            // Ждем начала следующего цикла
            yield return new WaitForSeconds(0.1f);
            
            while (currentNoteIndex < noteTimes.Count)
            {
                // Используем AudioSettings.dspTime для точного времени
                double currentTime = AudioSettings.dspTime - startTime;
                
                // Получаем время следующей ноты
                float nextNoteTime = noteTimes[currentNoteIndex];
                
                if (currentTime >= nextNoteTime - 0.001) // Небольшой допуск
                {
                    Debug.Log($"новая нота - время: {currentTime:F3} сек");
                    Shoot();
                    currentNoteIndex++;
                    
                    // Небольшая задержка чтобы не спамить в одном кадре
                    yield return new WaitForSeconds(0.001f);
                }
                else
                {
                    yield return null;
                }
            }
            
            // Когда все ноты сыграны
            Debug.Log("Все ноты сыграны - начинаем новый цикл");
            
            // Сбрасываем время для синхронизации с музыкой
            // Ждем окончания текущего цикла аудио (если аудио зациклено)
            float audioLength = audioSource.clip.length;
            double timeUntilNextLoop = audioLength - (AudioSettings.dspTime - startTime);
            
            if (timeUntilNextLoop > 0)
            {
                yield return new WaitForSeconds((float)timeUntilNextLoop);
            }
            
            // Сбрасываем время начала для следующего цикла
            startTime = AudioSettings.dspTime;
        }
    }
    
    // Метод для остановки зацикливания
    public void StopLooping()
    {
        isLooping = false;
    }
    
    // Метод для включения/выключения зацикливания
    public void SetLooping(bool loop)
    {
        isLooping = loop;
    }


    [Header("Shooting settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;

    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * bulletSpeed;
        }
        
        Destroy(bullet, 3f);
    }
}