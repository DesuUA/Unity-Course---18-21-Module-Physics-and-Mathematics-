using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraControl : MonoBehaviour
{
    private const string ScrollingAxis = "Mouse ScrollWheel";
    
    [SerializeField] private Transform _sourcePosition;
    [SerializeField] private float _smoothMovingTime = 0.3f;
    
    [FormerlySerializedAs("zoomSpeed")]
    [Header("Zoom Settings")]
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private float _minDistance = 2f;
    [SerializeField] private float _maxDistance = 15f;

    private Vector3 _moveDirection;
    private Vector3 _tempMoveDirection;
    private Vector3 _cameraVelocity = Vector3.zero;
    private Vector3 _offset;

    private void Start()
    {
        _offset = _sourcePosition.position - transform.position;
    }

    private void LateUpdate()
    {
        ScrollingDistance();

        Vector3 targetPosition = _sourcePosition.position - _offset;
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _cameraVelocity, _smoothMovingTime);
    }

    private void ScrollingDistance()
    {
        float currentDistanceToCamera = _offset.magnitude;
        float scrollInput = Input.GetAxis(ScrollingAxis);

        if (Mathf.Abs(scrollInput) > 0.001f)
        {
            float newDistance = Mathf.Clamp(currentDistanceToCamera - scrollInput * _zoomSpeed, _minDistance, _maxDistance);
            
            _offset = _offset.normalized * newDistance;
        }
    }
}