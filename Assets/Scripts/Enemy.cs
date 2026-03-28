using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEditor;

[RequireComponent(typeof(EnemyUpgrade))]
public class Enemy : MonoBehaviour, IDamageble
{
    [SerializeField] private float _health;
    [field: SerializeField] public List<UnitType> Types { get; private set; }

    public Action onTakeDamage;
    public Action onDie;
    public Action<UnitType> onSpawn;

    public void Spawn(UnitType type)
    {
        onSpawn?.Invoke(type);
    }

    public void Die()
    {
        onDie?.Invoke();
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log($"Enemy took {damage} damage!");

        _health -= damage;
        onTakeDamage?.Invoke();

        if (_health <= 0)
            Die();
    }
}
