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

    [Header("Smoothing")]
    [Range(0, 1)]
    public float smoothTime = 0.5f; // Скорость сглаживания


    [Header("Logging Settings")]
    public GameObject bulletPrefab;
    public float shootThreshold = 0.7f; // Порог срабатывания лога (0-1)
    public float shootCooldown = 0f; // Задержка между логами в секундах

    private float[] spectrumData;
    private float currentBass;
    private Vector3 targetScale;
    private Vector3 velocityScale = Vector3.zero;

    private float lastLogTime;

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

        MusicShoot();

        int bassBands = sampleSize / 8;

        float bassSum = 0;
        for (int i = 1; i < bassBands; i++)
        {
            bassSum += spectrumData[i];
        }
        currentBass = Mathf.Clamp01(bassSum / bassBands * bassMultiplier * 100);

        targetScale = Vector3.Lerp(minScale, maxScale, currentBass);

        objectToScale.localScale = Vector3.SmoothDamp(
            objectToScale.localScale,
            targetScale,
            ref velocityScale,
            smoothTime
        );
    }

    void MusicShoot()
    {
        if (Time.time - lastLogTime < shootCooldown)
            return;

        if (currentBass > shootThreshold)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0, 1), transform.rotation);
            bullet.transform.localScale = Vector3.SmoothDamp(
                objectToScale.localScale,
                targetScale,
                ref velocityScale,
                smoothTime
            );

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = transform.forward * 20;
            }

            Destroy(bullet, 3f);
            lastLogTime = Time.time;
        }
    }
}
