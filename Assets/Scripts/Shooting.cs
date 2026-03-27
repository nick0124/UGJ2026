using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Настройки")]
    public GameObject bulletPrefab;  // Префаб пули
    public Transform firePoint;       // Точка выстрела
    public float bulletSpeed = 20f;   // Скорость пули
       
    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * bulletSpeed;
        }
        
        Destroy(bullet, 3f);
    }
}
