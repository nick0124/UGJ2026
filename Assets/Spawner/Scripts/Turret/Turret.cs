using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class Turret : MonoBehaviour, IDamageble
{
    [Header("Main Settings")]
    [field: SerializeField] public Texture Icon { get; private set; }
    [field: SerializeField] public int ID { get; private set; }

    [Header("Character")]
    [SerializeField] private float _health;

    public void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_health < 0)
            Die();
    }
}
