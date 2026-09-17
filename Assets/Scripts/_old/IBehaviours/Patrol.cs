using UnityEngine;

public class Patrol : IBehaviour
{
    private readonly Enemy _me;
    private readonly Transform[] _patrolPoints;
    private readonly float _distanceThreshold = 0.2f;
    
    private int _currentPointIndex = 0;
    private Vector3 _currentTargetPoint;
    private Vector3 MyPosition => _me.transform.position;

    public Patrol(Transform[] points, Enemy enemy)
    {
        _patrolPoints = points;
        _me = enemy;

        if (_patrolPoints == null || _patrolPoints.Length == 0)
            return;
        
        if (_patrolPoints[_currentPointIndex] == null)
        {
            Debug.LogError($"{_me.name}: Patrol point {_currentPointIndex} is null");
            _currentTargetPoint = _me.transform.position;
            return;
        }

        _currentTargetPoint = _patrolPoints[_currentPointIndex].position;
        
        _distanceThreshold *= _distanceThreshold;
    }

    public void Behave()
    {
        if (_patrolPoints == null || _patrolPoints.Length == 0)
        {
            Debug.LogError($"No patrol points set on Patrol Behaviour");
            _me.MoveDirection = Vector3.zero;
            return;
        }
        
        float distance = (_currentTargetPoint - MyPosition).sqrMagnitude;
        
        if (distance < _distanceThreshold)
            _currentTargetPoint = NextPoint();
        
        _me.MoveDirection = (_currentTargetPoint - MyPosition).normalized;
    }
    
    private Vector3 NextPoint()
    {
        if (_patrolPoints.Length <= 1) return _currentTargetPoint;
        
        if (_currentPointIndex == _patrolPoints.Length - 1)
            _currentPointIndex = 0;
        else
            _currentPointIndex += 1;

        if (_patrolPoints[_currentPointIndex] == null)
        {
            Debug.LogError($"{_me.name}: Patrol point {_currentPointIndex} is null");
            return _currentTargetPoint;
        }
        else 
            return _patrolPoints[_currentPointIndex].position;
    }
}