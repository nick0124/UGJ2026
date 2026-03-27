using UnityEngine;

public class EqualizerSize : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public int sampleSize = 1024; // Должно быть степенью двойки (64, 128, 256, 512, 1024, 2048)
    public FFTWindow fftWindow = FFTWindow.Blackman;

    [Header("Scale Settings")]
    public Transform objectToScale;
    [Range(0f, 5f)]
    public float minMultiplier = 0.5f;      // Минимальный множитель размера
    [Range(0f, 5f)]
    public float maxMultiplier = 1.5f;      

    [Header("Frequency Bands")]
    [Range(0, 10)]
    public float bassMultiplier = 1f;      // Низкие частоты (20-250 Hz)

    [Header("Smoothing")]
    [Range(0, 1)]
    public float smoothTime = 0.1f; // Скорость сглаживания

    private float[] spectrumData;
    private float currentBass;
    private Vector3 targetScale;
    private Vector3 velocityScale = Vector3.zero;
    private Vector3 originalScale;

    private float lastLogTime;

    void Start()
    {
        // Инициализация
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (objectToScale == null)
            objectToScale = transform;

        originalScale = objectToScale.localScale;

        spectrumData = new float[sampleSize];
        targetScale = originalScale * minMultiplier;
    }

    void Update()
    {
        if (audioSource == null || spectrumData == null)
            return;

        audioSource.GetSpectrumData(spectrumData, 0, fftWindow);

        int bassBands = sampleSize / 8;

        float bassSum = 0;
        for (int i = 1; i < bassBands; i++)
        {
            bassSum += spectrumData[i];
        }
        currentBass = Mathf.Clamp01(bassSum / bassBands * bassMultiplier * 100);

        float currentMultiplier = Mathf.Lerp(minMultiplier, maxMultiplier, currentBass);

        targetScale = originalScale * currentMultiplier;

        objectToScale.localScale = Vector3.SmoothDamp(
            objectToScale.localScale,
            targetScale,
            ref velocityScale,
            smoothTime
        );
    }

    public void SetAudioSource(AudioSource newAudioSource)
    {
        audioSource = newAudioSource;
    }
}
