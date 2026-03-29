using System;
using UnityEngine;

public class Turret : MonoBehaviour, IDamageble
{
    [field: SerializeField] public Texture Icon { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public float Health { get; private set; }

    public Action<int, bool> onDie;

    public void Die()
    {
        Debug.Log($"{gameObject.name} turret is destroyed.");
        onDie?.Invoke(ID, true);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
    }
}
