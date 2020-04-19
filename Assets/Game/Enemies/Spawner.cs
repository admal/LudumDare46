using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    public GameObject _objectToSpawn;
    [SerializeField]
    public GameObject[] _objectsToSpawn;

    [SerializeField]
    [Tooltip("Spawn every X seconds")]
    private float _spawnRate = 20f;

    [SerializeField]
    private float _minSpawnRate = 5f;

    private SpawnPoint[] _spawnPoints;

    [SerializeField]
    private float _increaseDifficultyRate = 60f;
    [SerializeField]
    [Range(-1, 1)]
    [Tooltip("By what percent more frequently object should be spawned")]
    private float _spawnMoreOftenBy = 0.2f;

    [SerializeField]
    private bool _spawnOnStart = false;

    public bool IsSpawning = true;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Spawner started");
        _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        StartCoroutine(UpdateSpawn());
        StartCoroutine(UpdateSpawnRate());

        if (_spawnOnStart)
        {
            SpawnInRandomPlace();   
        }
    }

    private IEnumerator UpdateSpawn()
    {
        while (IsSpawning)
        {
            yield return new WaitForSeconds(_spawnRate);

            SpawnInRandomPlace();
        }
    }

    private IEnumerator UpdateSpawnRate()
    {
        while (true)
        {
            yield return new WaitForSeconds(_increaseDifficultyRate);
            _spawnRate -= _spawnMoreOftenBy * _spawnRate;

            if (_spawnRate < _minSpawnRate)
            {
                _spawnRate = _minSpawnRate;
                break;
            }
        }
    }

    private void SpawnInRandomPlace()
    {
        var toSpawn = _objectToSpawn;
        if (toSpawn == null)
        {
            toSpawn = _objectsToSpawn[Random.Range(0, _objectsToSpawn.Length)];
        }

        var spawnerIdx = Random.Range(0, _spawnPoints.Length);
        var spawner = _spawnPoints[spawnerIdx];

        Instantiate(toSpawn, spawner.transform.position, Quaternion.identity);
    }

    public void StopSpawning()
    {
        IsSpawning = false;
    }

    public void StartSpawning()
    {
        IsSpawning = true;
        StartCoroutine(UpdateSpawn());
    }
}
