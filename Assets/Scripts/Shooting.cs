using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Настройки")]
    public GameObject bulletPrefab;  // Префаб пули
    public Transform firePoint;       // Точка выстрела
    public float bulletSpeed = 20f;   // Скорость пули

    [Header("Настройки размера")]
    public float bulletScale = 1f; 

    [Header("Настройки материала")]
    public Material bulletMaterial;
       
    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Renderer renderer = bullet.GetComponent<Renderer>();
        renderer.material = new Material(bulletMaterial);

        bullet.transform.localScale *= bulletScale;
        
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * bulletSpeed;
        }
        
        Destroy(bullet, 3f);
    }
}
