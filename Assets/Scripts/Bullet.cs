using UnityEngine;

public class Bullet : MonoBehaviour
{
     [Header("Настройки пули")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private bool destroyOnHit = true;
    
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        
        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
            
            Debug.Log($"Bullet hit {other.gameObject.name} and dealt {damage} damage!");
        }
    }
}
