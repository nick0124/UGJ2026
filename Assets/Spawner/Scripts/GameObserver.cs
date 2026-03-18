using UnityEngine;

[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(GameView))]
public class GameObserver : MonoBehaviour
{
    private GameManager _manager;
    private GameView _view;

    private void Awake()
    {
        _manager = GetComponent<GameManager>();
        _view = GetComponent<GameView>();

        _manager.onCreateAllTurretCard += _view.CreateAllTurretCards;
        _manager.onSpawnTurret += _view.CreateSpawnedTurretCard;
        _manager.onDestroyTurret += _view.DestroyTurret;
    }

    private void OnDisable()
    {
        _manager.onCreateAllTurretCard -= _view.CreateAllTurretCards;
        _manager.onSpawnTurret -= _view.CreateSpawnedTurretCard;
        _manager.onDestroyTurret -= _view.DestroyTurret;
    }
}
