using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WaypointSelectorMovement : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private List<Transform> _points = new List<Transform>();
    
    [Header("Movement Settings")]
    [Tooltip("Distance to target to switch the target point")]
    [SerializeField] private float _distanceThreshold = 0.1f;
    [Tooltip("Time to stay at each point in seconds")]
    [SerializeField] private float _waitTime = 2f;
    
    
    private Vector3[] _pointsPosition;
    private int _currentPointIndex = -1;
    private int _searchDirection = 1;
    
    private Vector3 _currentTargetPoint;
    
    private bool _isStationed;
    private float _currentWaitTimer;
    
    private Vector3 MoverPosition => _mover.transform.position;

    private void Start()
    {
        if (_points == null || _points.Count == 0)
        {
            Debug.LogWarning($"[{nameof(WaypointSelectorMovement)}] Empty/Null {nameof(_points)}.", this);
            enabled = false;
            return;
        }
        
        _distanceThreshold *= _distanceThreshold;
        
        _pointsPosition = new Vector3[_points.Count];
        
        for (int i = 0; i < _points.Count; i++)
        {
            _pointsPosition[i] = _points[i].position;
        }
        
        SetClosestPoint();
    }

    private void Update()
    {
        if (_isStationed)
        {
            _mover.SetDirection(MoverPosition);
            
            _currentWaitTimer += Time.deltaTime;
            
            if (_currentWaitTimer >= _waitTime)
            {
                _isStationed = false;
                _currentWaitTimer = 0f;
                NextPoint();
            }
            
            return;
        }
        
        float distance = (_currentTargetPoint - MoverPosition).sqrMagnitude;
        
        if (distance < _distanceThreshold)
        {
            _isStationed = true;
            return;
        }
        
        _mover.SetDirection(_currentTargetPoint);
    }

    private void NextPoint()
    {
        if (_pointsPosition.Length <= 1) return;
        
        if (_currentPointIndex == _pointsPosition.Length - 1)
            _searchDirection = -1;
        
        else if (_currentPointIndex == 0)
            _searchDirection = 1;
        
        _currentPointIndex += _searchDirection;
        
        _currentTargetPoint = _pointsPosition[_currentPointIndex];
    }
    
    private void SetClosestPoint()
    {
        _currentPointIndex = 0;
        
        float distance = (_pointsPosition[0] - MoverPosition).sqrMagnitude;

        for (int i = 1; i < _pointsPosition.Length; i++)
        {
            float newDistance = (_pointsPosition[i] - MoverPosition).sqrMagnitude;
            
            if (newDistance < distance)
            {
                distance = newDistance;
                _currentPointIndex = i;
            }
        }
        
        _currentTargetPoint = _pointsPosition[_currentPointIndex];
    }
}