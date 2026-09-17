using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private IdleBehavioursType _idleBehavioursType;
    [SerializeField] private AgroBehavioursType _agroBehavioursType;
    
    [SerializeField] private PatrolPoint[] _patrolPoints;

    private Transform[] _patrolPointPosition;
    private IBehaviour _idleBehaviour;
    private IBehaviour _agroBehaviour;
    private Enemy _enemyScript;

    private void Start()
    {
        SpawnPrefab();
    }

    [ContextMenu("Spawn prefab")] private void Spawn()
    {
        SpawnPrefab();
    }

    private void SpawnPrefab()
    {
        _patrolPointPosition = new Transform[_patrolPoints.Length];
        
        for (int i = 0; i < _patrolPoints.Length; i++)
            _patrolPointPosition[i] = _patrolPoints[i] != null ? _patrolPoints[i].transform : null;
        
        GameObject spawnedEnemy = Instantiate(_prefab, transform.position, transform.rotation);
        _enemyScript = spawnedEnemy.GetComponent<Enemy>();
        
        _idleBehaviour = SearchBehaviour(_idleBehavioursType);
        _agroBehaviour = SearchBehaviour(_agroBehavioursType);
        
        if (_enemyScript != null)
        {
            _enemyScript.Init(_idleBehaviour, _agroBehaviour);
        }
    }
    
    private IBehaviour SearchBehaviour(Enum behaviourType)
    {
        switch (behaviourType)
        {
            case IdleBehavioursType.Idle:
                return new Idle(_enemyScript);
            
            case IdleBehavioursType.Patrol:
                return new Patrol(_patrolPointPosition, _enemyScript);
            
            case IdleBehavioursType.RandomMove:
                return new RandomMove(_enemyScript);
            
            case AgroBehavioursType.Chase:
                return new Chase(_enemyScript);
            
            case AgroBehavioursType.RunAway:
                return new RunAway(_enemyScript);
                
            case AgroBehavioursType.Die:
                return new Die(_enemyScript);
            
            default: Debug.LogError("IBehaviour class not found"); break;
        }
        return null;
    }
}
