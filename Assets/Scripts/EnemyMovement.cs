using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private Transform target; // Цель, к которой нужно двигаться
    [SerializeField] private float moveSpeed = 5f; // Скорость движения
    [SerializeField] private float rotationSpeed = 360f; // Скорость поворота (градусов в секунду)
    [SerializeField] private float stoppingDistance = 0.5f; // Дистанция остановки
    
    [Header("Дополнительные настройки")]
    [SerializeField] private bool moveOnStart = true; // Начать движение при старте
    [SerializeField] private bool rotateTowardsTarget = true; // Поворачиваться к цели
    [SerializeField] private bool smoothRotation = true; // Плавный поворот
    
    private bool isMoving = false;
    
    void Start()
    {
        if (moveOnStart && target != null)
        {
            StartMoving();
        }
    }
    
    void Update()
    {
        if (!isMoving || target == null) return;
        
        // Двигаемся к цели
        //MoveTowardsTarget();
        
        // Поворачиваемся к цели (если нужно)
        if (rotateTowardsTarget)
        {
            RotateTowardsTarget();
        }
    }
    
    public void MoveTowardsTarget()
    {
        // Получаем позиции, игнорируя разницу по Y
        Vector3 currentPos = transform.position;
        Vector3 targetPos = target.position;
        
        // Оставляем текущую Y координату
        targetPos.y = currentPos.y;
        
        // Проверяем дистанцию
        float distance = Vector3.Distance(currentPos, targetPos);
        
        if (distance > stoppingDistance)
        {
            // Вычисляем направление движения (только XZ)
            Vector3 direction = (targetPos - currentPos).normalized;
            
            // Двигаем объект
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Достигли цели
            OnTargetReached();
        }
    }
    
    private void RotateTowardsTarget()
    {
        // Получаем направление к цели (игнорируем Y)
        Vector3 targetPosition = target.position;
        targetPosition.y = transform.position.y;
        
        Vector3 directionToTarget = targetPosition - transform.position;
        
        if (directionToTarget != Vector3.zero)
        {
            if (smoothRotation)
            {
                // Плавный поворот
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
            else
            {
                // Мгновенный поворот
                transform.LookAt(targetPosition);
            }
        }
    }
    
    private void OnTargetReached()
    {
        Debug.Log($"Цель достигнута: {target.name}");
        isMoving = false;
    }
    
    // Публичные методы для управления
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    public void StartMoving()
    {
        if (target != null)
        {
            isMoving = true;
        }
        else
        {
            Debug.LogWarning("Цель не назначена!");
        }
    }
    
    public void StopMoving()
    {
        isMoving = false;
    }
    
    // Визуализация в редакторе
    private void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);
            
            // Рисуем сферу на дистанции остановки
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(target.position, stoppingDistance);
        }
    }
}
