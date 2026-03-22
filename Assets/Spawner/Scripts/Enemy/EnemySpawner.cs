using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _timeToSpawn;
    [Space(20f)]
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private List<Enemy> _enemiesPrefab;
    [Space(10f)]
    [SerializeField] private GameManager _gameManager;

    private bool _canSpawn = true;

    private Coroutine _spawnCoroutine;
    private Spawner _spawner;

    private void OnDisable()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
    }

    private void Start()
    {
        _spawner = new Spawner();
        _spawnCoroutine = StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        WaitForSeconds timeToSpawn = new WaitForSeconds(_timeToSpawn);

        while (_canSpawn == true)
        {
            int randomValue = Random.Range(0, _spawnPoints.Count);
            
            Enemy spawnEnemy = _spawner.CreateObject<Enemy>(_enemiesPrefab[0], _spawnPoints[randomValue].position);

            yield return timeToSpawn;
        }

        yield return null;
    }
}
