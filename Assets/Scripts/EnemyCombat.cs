using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _delay;

    private IDamageble _target;
    private float _lastAttackTime;
    private bool _canAttack;

    private EnemyMovement _movement;

    private void OnDisable()
    {
        _movement.onSetTarget -= CanAttack;
    }

    private void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
        _movement.onSetTarget += CanAttack;
    }

    private void Update()
    {
        if (_target == null)
            _canAttack = false;

        if (_canAttack == true && _target != null)
            Attack();
    }

    private void Attack()
    {
        if(Time.time - _lastAttackTime >= _delay)
        {
            _lastAttackTime = Time.time;
            _target.TakeDamage(_damage);
        }
    }

    public void CanAttack(IDamageble target) 
    {
        _canAttack = true;
        _target = target;
    }
}
