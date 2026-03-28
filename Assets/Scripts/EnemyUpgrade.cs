using System.Collections.Generic;
using UnityEngine;

public class EnemyUpgrade : MonoBehaviour
{
    [System.Serializable]
    public struct UpgradeData
    {
        public Transform SpawnPoint;
        public GameObject UpgradeObject;
        public UnitType UpgradeType;
    }

    [Header("Main Settings")]
    [SerializeField] private GameObject _baseObject;
    [SerializeField] private List<UpgradeData> _upgradeData;

    [Space(10f)]
    [Header("Drop Settings")]
    [SerializeField] private float _interactRadius;
    [SerializeField] private LayerMask _interactMask;
    [SerializeField] private Vector3 _dropOffset;

    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();

        _enemy.onSpawn += InitializedUpgrade;
        _enemy.onDie += DropUpgrades;
    }

    private void OnDisable()
    {
        _enemy.onDie -= DropUpgrades;
        _enemy.onSpawn -= InitializedUpgrade;
    }

    public void InitializedUpgrade(UnitType type)
    {
        foreach (var data in _upgradeData)
        {
            GameObject upgradePrefab = data.UpgradeType == type ? data.UpgradeObject : _baseObject;

            GameObject obj = Instantiate(upgradePrefab, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(data.SpawnPoint, false);
            obj.transform.localScale = Vector3.one;
        }

        _enemy.Types.Add(type);        
    }

    public void SetUpgrade(UnitType type)
    {
        if (_enemy.Types.Contains(type) == true) return;

        if (_enemy.Types.Contains(UnitType.Base))
            _enemy.Types.Remove(UnitType.Base);

        UpgradeData upgrade = _upgradeData.Find(i => i.UpgradeType == type);
        
        Destroy(upgrade.SpawnPoint.GetChild(0).gameObject);

        GameObject obj = Instantiate(upgrade.UpgradeObject, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(upgrade.SpawnPoint, false);
        obj.transform.localScale = Vector3.one;

        _enemy.Types.Add(type);
    }

    private void DropUpgrades()
    {
        if (_enemy.Types.Count <= 0) return;
        if(_enemy.Types.Contains(UnitType.Base) == true) return;

        Debug.Log("Create Upgrade");

        foreach (var upgrade in _enemy.Types)
        {
            GameObject upgradePrefab = _upgradeData.Find(u => u.UpgradeType == upgrade).UpgradeObject;

            float randomOffset = Random.Range(0, 2);
            Vector3 spawnPosition = transform.position + _dropOffset * randomOffset;
            GameObject obj = Instantiate(upgradePrefab);
            obj.transform.position = spawnPosition;

            obj.AddComponent<Upgrade>().Initialized(upgrade, _interactRadius, _interactMask);
        }
    }
}
