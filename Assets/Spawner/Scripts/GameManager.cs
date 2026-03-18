using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public const int MIN_TURRET_IN_SCENE = 1;

    [Header("Main Options")]
    [SerializeField] private int _maxTowerInPlace;

    [Header("Tower Options")]
    [SerializeField] private List<Turret> _turretPrefab;

    private Dictionary<int, Turret> _spawnedTurret = new();

    private int _currentTurretInScene;

    private Spawner _spawner;

    public Action<int, GameManager> onSpawnTurret;
    public Action<int, GameManager> onCreateAllTurretCard;
    public Action<int> onDestroyTurret;


    private void Start()
    {
        _spawner = new Spawner();

        InitializedAllTurretCard();
        CreateStartTurret();
    }

    public void SpawnTurret(int turretID)
    {
        Turret selectedTurret = _turretPrefab.Find(t => t.ID == turretID);

        if (_maxTowerInPlace < _currentTurretInScene + 1) return;
        if (_spawnedTurret.ContainsKey(turretID) == true) return;

        Turret spawnTurret = _spawner.CreateObject<Turret>(selectedTurret, Vector3.zero);
        
        _spawnedTurret.Add(turretID, spawnTurret);
        _currentTurretInScene++;

        onSpawnTurret?.Invoke(turretID, this);
    }

    public void DeleteTurret(int turretID)
    {
        Debug.Log("</color=red>Try destroy turret</color>");

        if (MIN_TURRET_IN_SCENE >= _currentTurretInScene) return;
        if (_spawnedTurret.ContainsKey(turretID) == false) return;

        Debug.Log("</color=red>Destroy turret</color>");

        Turret turret = _spawnedTurret[turretID];

        Destroy(turret.gameObject);

        _currentTurretInScene--;
        _spawnedTurret.Remove(turretID);

        onDestroyTurret?.Invoke(turretID);
    }

    private void CreateStartTurret()
    {
        int randomIndex = UnityEngine.Random.Range(0, _turretPrefab.Count);

        SpawnTurret(_turretPrefab[randomIndex].ID);
    }

    private void InitializedAllTurretCard()
    {
        foreach (var turret in _turretPrefab)
        {
            onCreateAllTurretCard?.Invoke(turret.ID, this);
        }
    }
}
