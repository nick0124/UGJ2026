using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public const int MIN_TURRET_IN_SCENE = 1;

    [Header("Main Options")]
    [SerializeField] private LayerMask _spawnTurretMask;
    [SerializeField] private int _maxTowerInPlace;

    [Header("Tower Options")]
    [SerializeField] private List<Turret> _turretPrefab;

    private Dictionary<int, Turret> _spawnedTurret = new();

    private bool _isSelectTurret = false;
    private int _selectedTurretID;
    private int _currentTurretInScene;

    private Spawner _spawner;
    private PlayerAction _playerAction;

    public Action<int, GameManager> onSpawnTurret;
    public Action<int, GameManager> onCreateAllTurretCard;
    public Action<int> onDestroyTurret;


    private void Start()
    {
        _spawner = new Spawner();
        
        _playerAction = new PlayerAction();
        _playerAction.Enable();

        _playerAction.Player.Click.performed += ctx => ClickInSpace();

        InitializedAllTurretCard();
        CreateStartTurret();
    }

    public void SelectedTurret(int turretID)
    {
        if(_selectedTurretID == turretID)
        {
            _isSelectTurret = false;
            _selectedTurretID = int.MaxValue;
            return;
        }

        _isSelectTurret = true;
        _selectedTurretID = turretID;
    }

    public void SpawnTurret(int turretID, Vector3 position)
    {
        Turret selectedTurret = _turretPrefab.Find(t => t.ID == turretID);

        if (_maxTowerInPlace < _currentTurretInScene + 1) return;
        if (_spawnedTurret.ContainsKey(turretID) == true) return;

        Turret spawnTurret = _spawner.CreateObject<Turret>(selectedTurret, position);
        
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
        //int randomIndex = UnityEngine.Random.Range(0, _turretPrefab.Count);

        SpawnTurret(_turretPrefab[0].ID, Vector3.zero);
    }

    private void InitializedAllTurretCard()
    {
        foreach (var turret in _turretPrefab)
        {
            onCreateAllTurretCard?.Invoke(turret.ID, this);
        }
    }

    private void ClickInSpace()
    {
        if(_isSelectTurret == false) return;

        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit, 100, _spawnTurretMask))
        {
            if (IsPointerOverUI() == true) return;

            if(hit.collider.tag == "SpawnArea")
            {
                SpawnTurret(_selectedTurretID, hit.point);
                Debug.Log($"<color=red>Turret spawn Position</color> {hit.point}");
            }    
        }

        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow, 1f);
    }

    private bool IsPointerOverUI()
    {
        if(EventSystem.current == null) return false;

        return EventSystem.current.IsPointerOverGameObject(-1);
    }
}
