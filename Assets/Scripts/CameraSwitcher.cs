using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] _cameras;
    [SerializeField] private KeyCode _switchCameraKey = KeyCode.V;
    
    private Queue<CinemachineVirtualCamera> _cameraQueue;
    private CinemachineVirtualCamera _currentCamera;

    private void Start()
    {
        _cameraQueue = new Queue<CinemachineVirtualCamera>(_cameras);
        SwitchCamera();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(_switchCameraKey))
        {
            SwitchCamera();
        }
    }

    private void SwitchCamera()
    {
        _currentCamera = _cameraQueue.Dequeue();

        foreach (CinemachineVirtualCamera virtualCamera in _cameras)      
        {
            virtualCamera.gameObject.SetActive(false);
        }
        
        _currentCamera.gameObject.SetActive(true);
        _cameraQueue.Enqueue(_currentCamera);
    }
}
