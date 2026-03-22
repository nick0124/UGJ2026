using UnityEngine;

public class Enemy : MonoBehaviour, IDamageble
{
    [SerializeField] private float _health;

    public void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log($"Enemy took {damage} damage!");

        _health -= damage;

        if (_health <= 0)
            Die();
    }
}
