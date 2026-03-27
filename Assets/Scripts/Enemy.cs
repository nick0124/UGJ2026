using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[RequireComponent(typeof(EnemyUpgrade))]
public class Enemy : MonoBehaviour, IDamageble
{
    [SerializeField] private float _health;

    [field: SerializeField] public List<UnitType> Types { get; private set; }

    private EnemyUpgrade _upgrade;


    public Action onTakeDamage;

    private void Awake()
    {
        _upgrade = GetComponent<EnemyUpgrade>();
    }

    public void Spawn(UnitType type)
    {
        _upgrade.SetUpgrade(type);
        Types.Add(type);
    }

    public void Die()
    {
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
