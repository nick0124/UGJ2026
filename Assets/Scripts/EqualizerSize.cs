using UnityEngine;

public class EqualizerSize : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public int sampleSize = 1024; // Должно быть степенью двойки (64, 128, 256, 512, 1024, 2048)
    public FFTWindow fftWindow = FFTWindow.Blackman;
    
    [Header("Scale Settings")]
    public Transform objectToScale;
    public Vector3 minScale = Vector3.one;
    public Vector3 maxScale = new Vector3(3f, 3f, 3f);
    
    [Header("Frequency Bands")]
    [Range(0, 10)]
    public float bassMultiplier = 1f;      // Низкие частоты (20-250 Hz)
    [Range(0, 10)]
    public float midMultiplier = 1f;       // Средние частоты (250-4000 Hz)
    [Range(0, 10)]
    public float trebleMultiplier = 1f;     // Высокие частоты (4000-20000 Hz)
    
    [Header("Smoothing")]
    [Range(0, 1)]
    public float smoothTime = 0.5f; // Скорость сглаживания
    
    [Header("Visualization Mode")]
    public EqualizerMode mode = EqualizerMode.AllBands;
    
    private float[] spectrumData;
    private float currentBass;
    private float currentMid;
    private float currentTreble;
    private Vector3 targetScale;
    private Vector3 velocityScale = Vector3.zero;
    
    public enum EqualizerMode
    {
        AllBands,    // Все частоты вместе
        BassOnly,    // Только низкие
        MidOnly,     // Только средние
        TrebleOnly,  // Только высокие
        SeparateAxes // Низкие по X, средние по Y, высокие по Z
    }
    
    void Start()
    {
        // Инициализация
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
            
        if (objectToScale == null)
            objectToScale = transform;
            
        spectrumData = new float[sampleSize];
        targetScale = minScale;
    }
    
    void Update()
    {
        if (audioSource == null || spectrumData == null)
            return;
            
        // Получаем спектр данных
        audioSource.GetSpectrumData(spectrumData, 0, fftWindow);
        
        // Вычисляем разные частотные диапазоны
        CalculateFrequencyBands();
        
        // Определяем целевой масштаб в зависимости от режима
        targetScale = CalculateTargetScale();
        
        // Сглаживаем изменения масштаба
        objectToScale.localScale = Vector3.SmoothDamp(
            objectToScale.localScale, 
            targetScale, 
            ref velocityScale, 
            smoothTime
        );
    }
    
    void CalculateFrequencyBands()
    {
        int bassBands = sampleSize / 8;      // Примерно 1/8 для басов
        int midBands = sampleSize / 4;       // 1/4 для средних
        int trebleBands = sampleSize - bassBands - midBands;
        
        // Bass (низкие частоты)
        float bassSum = 0;
        for (int i = 1; i < bassBands; i++)
        {
            bassSum += spectrumData[i];
        }
        currentBass = Mathf.Clamp01(bassSum / bassBands * bassMultiplier * 100);
        
        // Mid (средние частоты)
        float midSum = 0;
        for (int i = bassBands; i < bassBands + midBands; i++)
        {
            midSum += spectrumData[i];
        }
        currentMid = Mathf.Clamp01(midSum / midBands * midMultiplier * 100);
        
        // Treble (высокие частоты)
        float trebleSum = 0;
        for (int i = bassBands + midBands; i < sampleSize; i++)
        {
            trebleSum += spectrumData[i];
        }
        currentTreble = Mathf.Clamp01(trebleSum / trebleBands * trebleMultiplier * 100);
    }
    
    Vector3 CalculateTargetScale()
    {
        Vector3 scale = minScale;
        
        switch (mode)
        {
            case EqualizerMode.AllBands:
                float average = (currentBass + currentMid + currentTreble) / 3f;
                scale = Vector3.Lerp(minScale, maxScale, average);
                break;
                
            case EqualizerMode.BassOnly:
                scale = Vector3.Lerp(minScale, maxScale, currentBass);
                break;
                
            case EqualizerMode.MidOnly:
                scale = Vector3.Lerp(minScale, maxScale, currentMid);
                break;
                
            case EqualizerMode.TrebleOnly:
                scale = Vector3.Lerp(minScale, maxScale, currentTreble);
                break;
                
            case EqualizerMode.SeparateAxes:
                scale = new Vector3(
                    Mathf.Lerp(minScale.x, maxScale.x, currentBass),
                    Mathf.Lerp(minScale.y, maxScale.y, currentMid),
                    Mathf.Lerp(minScale.z, maxScale.z, currentTreble)
                );
                break;
        }
        
        return scale;
    }
    
    // Для визуализации в инспекторе
    void OnDrawGizmosSelected()
    {
        if (objectToScale != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(objectToScale.position, maxScale);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(objectToScale.position, minScale);
        }
    }
}
