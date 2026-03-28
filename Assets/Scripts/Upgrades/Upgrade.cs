using UnityEngine;

[RequireComponent (typeof(DropAnimation))]
public class Upgrade : MonoBehaviour
{
    [Header("<color=green><b>Main Settings</b></color>")]
    private LayerMask _interactLayer;
    private float _interactRadius;

    private float _timer = 0f;

    private UnitType _upgradeType;

    public void Initialized(UnitType type, float interactDistance, LayerMask mask)
    {
        _upgradeType = type;
        _interactRadius = interactDistance;
        _interactLayer = mask;
    }

    private void FixedUpdate()
    {
        CheckInteraction();
    }

    private void CheckInteraction()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _interactRadius, _interactLayer);
    
        if(hits.Length > 0)
        {
            if (hits[0].TryGetComponent<EnemyUpgrade>(out EnemyUpgrade enemyUpgrade))
            {
                enemyUpgrade.SetUpgrade(_upgradeType);
                Destroy(gameObject);
            }
        }
    }
}
