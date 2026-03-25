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

    [SerializeField] private List<UpgradeData> _upgradeData;
    [SerializeField] private UnitType _baseType;

    public void SetUpgrade(UnitType type)
    {
        if (_baseType == type) return;

        UpgradeData data = _upgradeData.Find(d => d.UpgradeType == type);

        if(data.SpawnPoint.childCount > 0)
        {
            for(int i = 0; i < data.SpawnPoint.childCount; i++) 
                Destroy(data.SpawnPoint.GetChild(i).gameObject);
        }

        GameObject obj = Instantiate(data.UpgradeObject, Vector3.zero, Quaternion.identity);
       
        obj.transform.SetParent(data.SpawnPoint, false);
        obj.transform.localScale = Vector3.one;
    }
}
