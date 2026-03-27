using UnityEngine;

public class LookAtEnemy : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private Transform _rotationObject;
    [SerializeField] private UnitType _targetTypeTrigger;

    [Header("Настройки поиска")]
    [SerializeField] private float searchRadius = 10f;
    [SerializeField] private LayerMask enemyLayer; // Слой врагов
    [SerializeField] private float searchInterval = 0.5f; // Интервал поиска (если не каждый кадр)
    
    public GameObject nearestEnemy;
    private float distanceToNearestEnemy;
    private float lastSearchTime;
    
    private void Start()
    {
        lastSearchTime = -searchInterval;
        
        FindNearestEnemyInRadius();
    }
    
    private void Update()
    {
        if (Time.time - lastSearchTime >= searchInterval)
        {
            FindNearestEnemyInRadius();
            lastSearchTime = Time.time;
        }

        if (nearestEnemy == null) return;
        
        InstantRotateY();
    }
    
    
    
    public void FindNearestEnemyInRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius, enemyLayer);
        
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;
        
        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent<Enemy>(out var enemy))
            {
                if (enemy.Types.Contains(_targetTypeTrigger))
                {
                    Debug.Log($"<color=yellow>{enemy.name}</color>");

                    float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);

                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        closestEnemy = collider.gameObject;
                    }
                }
            }
        }
        
        nearestEnemy = closestEnemy;
        distanceToNearestEnemy = closestDistance;
    }

    void InstantRotateY()
    {
        Vector3 worldDirection = nearestEnemy.transform.position - transform.position;
        Vector3 localDirection = transform.InverseTransformDirection(worldDirection);

        float targetAngle = Mathf.Atan2(localDirection.y, localDirection.x) * Mathf.Rad2Deg;

        float curentAngle = _rotationObject.localEulerAngles.z;
        float newAngle = Mathf.LerpAngle(curentAngle, targetAngle, 5f * Time.deltaTime);
        _rotationObject.localRotation = Quaternion.Euler(0, 0, newAngle);  
    }
    
    public Vector3 GetDirectionToNearestEnemy()
    {
        if (nearestEnemy != null)
        {
            return (nearestEnemy.transform.position - transform.position).normalized;
        }
        return Vector3.zero;
    }
    
    public bool IsEnemyInRange()
    {
        return nearestEnemy != null;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
        
        if (nearestEnemy != null && Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, nearestEnemy.transform.position);
        }
    }
}
