using UnityEngine;

public class LookAtEnemy : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private Transform target;
    
    void Update()
    {
        if (target == null) return;
        
        InstantRotateY();
    }
    
    void InstantRotateY()
    {
        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.y = 0;
        
        if (directionToTarget != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(directionToTarget);
        }
    }
}
