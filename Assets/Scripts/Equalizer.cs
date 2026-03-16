using UnityEngine;

public class Equalizer : MonoBehaviour
{
    [Header("Настройки аудио")]
    [SerializeField] private AudioSource audioSource; // Источник музыки
    [SerializeField] private int spectrumSize = 64; // Размер спектра
    [SerializeField] private FFTWindow fftWindow = FFTWindow.Blackman; // Тип FFT
    
    [Header("Настройки порога")]
    [SerializeField] private float threshold = 0.01f; // Порог срабатывания
    [SerializeField] [Range(0, 1)] private float frequencyRange = 0.5f; // Частотный диапазон
    [SerializeField] private float cooldownTime = 0.2f; // Задержка между срабатываниями

    [Header("Выбор частотного диапазона")]
    [SerializeField] private bool useFrequencyRange = false;
    [SerializeField] [Range(0, 1)] private float lowFrequency = 0f; // 0 = низкие частоты
    [SerializeField][Range(0, 1)] private float highFrequency = 1f; // 1 = высокие частоты
    
    [Header("Настройки визуализации")]
    [SerializeField] private Transform objectToScale; // Объект для изменения размера
    [SerializeField] private float scaleMultiplier = 10f; // Множитель масштаба
    [SerializeField] private float smoothSpeed = 5f; // Скорость сглаживания
    [SerializeField] private float minScale = 0.5f; // Минимальный масштаб
    [SerializeField] private float maxScale = 5f; // Максимальный масштаб
    
    [Header("Настройки логирования")]
    [SerializeField] private bool enableLogging = true;
    
    [Header("Целевой объект")]
    [SerializeField] private GameObject targetObject; // Объект с методом OnBeatDetected
    [SerializeField] private string functionName; // Название метода для вызова на целевом объекте

    
    private float[] spectrumData;
    private float lastTriggerTime = 0f;

    private float currentScale = 1f;
    private float targetScale = 1f;
    
    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
            
        spectrumData = new float[spectrumSize];
    }
    
    void Update()
    {
        if (audioSource == null || !audioSource.isPlaying)
            return;

        // Вычисляем среднюю громкость в выбранном диапазоне
        float averageVolume = GetAverageVolume();

        // Вычисляем целевой масштаб
        targetScale = Mathf.Clamp(averageVolume * scaleMultiplier, minScale, maxScale);
        
        // Плавно изменяем масштаб
        currentScale = Mathf.Lerp(currentScale, targetScale, smoothSpeed * Time.deltaTime);
        
        // Применяем масштаб
        objectToScale.localScale = Vector3.one * currentScale;    
            
        // Получаем спектр данных
        audioSource.GetSpectrumData(spectrumData, 0, fftWindow);
        
        // Вычисляем громкость в выбранном диапазоне
        float currentVolume = GetVolumeInRange();
        
        // Проверяем порог
        if (currentVolume >= threshold && Time.time >= lastTriggerTime + cooldownTime)
        {
            lastTriggerTime = Time.time;
            
            // Вызываем метод на целевом объекте
            if (targetObject != null && !string.IsNullOrEmpty(functionName))
                targetObject.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
        }
    }

    private float GetVolumeInRange()
    {
        int index = Mathf.FloorToInt(frequencyRange * (spectrumSize - 1));
        return spectrumData[index];
    }
    
    private float GetAverageVolume()
    {
        if (!useFrequencyRange)
        {
            // Используем весь спектр
            float sum = 0f;
            for (int i = 0; i < spectrumSize; i++)
            {
                sum += spectrumData[i];
            }
            return sum / spectrumSize;
        }
        else
        {
            // Используем только выбранный диапазон частот
            int startIndex = Mathf.FloorToInt(lowFrequency * (spectrumSize - 1));
            int endIndex = Mathf.FloorToInt(highFrequency * (spectrumSize - 1));
            
            float sum = 0f;
            int count = 0;
            
            for (int i = startIndex; i <= endIndex; i++)
            {
                sum += spectrumData[i];
                count++;
            }
            
            return count > 0 ? sum / count : 0f;
        }
    }
}
