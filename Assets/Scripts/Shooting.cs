using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Настройки")]
    public GameObject bulletPrefab;  // Префаб пули
    public Transform firePoint;       // Точка выстрела
    public float bulletSpeed = 20f;   // Скорость пули
    public float fireInterval = 1f;    // Интервал между выстрелами (в секундах)
    
    private float timer;
    
    void Start()
    {
        timer = 0f;
    }
    
    void Update()
    {
        // Увеличиваем таймер
        timer += Time.deltaTime;
        
        // Проверяем, прошло ли достаточно времени
        if (timer >= fireInterval)
        {
            Shoot();
            timer = 0f; // Сбрасываем таймер
        }
    }
    
    public void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;
        
        // Создаем пулю
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        // Задаем скорость
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }
        
        // Уничтожаем через 3 секунды
        Destroy(bullet, 3f);
    }
}
