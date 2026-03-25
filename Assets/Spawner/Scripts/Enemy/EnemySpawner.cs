using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Collections;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public struct WaveSettig
    {
        public float WaveTime;
        public UnitType WaveType;
    }

    [Header("Spawn Settigns")]
    [SerializeField] private List<WaveSettig> _waveSettigList;   
    [SerializeField] private float _timeToSpawn;
    [Space(20f)]
    [Header("Enemy Settings")]
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private Enemy _enemyPrefab;
    [Space(10f)]
    [Header("Other Links")]
    [SerializeField] private GameManager _gameManager;

    private bool _canSpawn = true;

    private int _curentWaveSettingIndex = 0;

    private Spawner _spawner;

    private Coroutine _waveCoroutine;
    private Coroutine _spawnCoroutine;
    

    private void OnDisable()
    {
        if (_waveCoroutine != null) StopCoroutine(_waveCoroutine);
        if (_spawnCoroutine != null) StopCoroutine(_spawnCoroutine);
    }

    private void Start()
    {
        _spawner = new Spawner();

        SetWave();
    }

    private void SetWave()
    {
        _curentWaveSettingIndex = Mathf.Clamp(_curentWaveSettingIndex, 0, _waveSettigList.Count - 1);

        _waveCoroutine = StartCoroutine(PlayWave(_curentWaveSettingIndex));

        _curentWaveSettingIndex++;
    }

    private IEnumerator PlayWave(int waveIndex)
    {
        WaveSettig waveSettigns = _waveSettigList[waveIndex];

        WaitForSeconds waitingTime = new WaitForSeconds(waveSettigns.WaveTime);

        if (_spawnCoroutine != null) StopCoroutine(_spawnCoroutine);

        _spawnCoroutine = StartCoroutine(SpawnEnemy(waveSettigns.WaveType));

        yield return waitingTime;

        _waveCoroutine = null;
        SetWave();
    }

    private IEnumerator SpawnEnemy(UnitType type)
    {
        WaitForSeconds timeToSpawn = new WaitForSeconds(_timeToSpawn);

        while (_canSpawn == true)
        {
            int randomValue = Random.Range(0, _spawnPoints.Count);
            
            Enemy spawnEnemy = _spawner.CreateObject<Enemy>(_enemyPrefab, _spawnPoints[randomValue].position);
            spawnEnemy.Spawn(type);

            yield return timeToSpawn;
        }

        yield return null;
    }
}
