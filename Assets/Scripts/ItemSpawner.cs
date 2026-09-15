using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectPrefabs;
    [SerializeField] private SpawnPoint[] _spawnPoints;
    
    private List<GameObject> _remainingObjectsToSpawn;
    private List<SpawnPoint> _freeSpawnPoints;
    
    private void Awake()
    {
        _remainingObjectsToSpawn = new List<GameObject>(_objectPrefabs.Length);
        _freeSpawnPoints = new List<SpawnPoint>(_spawnPoints.Length);
        
        _remainingObjectsToSpawn.AddRange(_objectPrefabs);
        _freeSpawnPoints.AddRange(_spawnPoints);
    }

    private void Start()
    {
        int spawnCount = Mathf.Min(_remainingObjectsToSpawn.Count, _freeSpawnPoints.Count);
        
        for (int i = spawnCount-1; i >= 0; i--)
        {
            Instantiate(_remainingObjectsToSpawn[i], _freeSpawnPoints[i].transform.position, _freeSpawnPoints[i].transform.rotation);
            
            _remainingObjectsToSpawn.RemoveAt(i);
            _freeSpawnPoints.RemoveAt(i);
        }
    }
}
