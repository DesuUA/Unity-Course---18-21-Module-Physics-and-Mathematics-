using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class IdleSway : Animations
{
    [Tooltip("Amplitude frequency per second")]
    [SerializeField] private float _frequency = 4f;
    
    [Tooltip("Max sway up/down")]
    [SerializeField] private float _amplitude = 0.1f;
    
    [SerializeField] private bool _useLocalSpace = true;
    
    private float _startPosition;
    private float _randomTimeDrift;
        
    protected override void Start()
    {
        base.Start();
        
        _startPosition = _useLocalSpace ? transform.localPosition.y : transform.position.y;
        _randomTimeDrift = Random.Range(0f, Mathf.PI * 2f);
    }

    protected override void Update()
    {
        base.Update();
        
        if (IsPlaying)
        {
            float newPositionY = _startPosition + Mathf.Sin((Time.time * _frequency) + _randomTimeDrift) * _amplitude;

            if (_useLocalSpace)
            {
                Vector3 pos = transform.localPosition;
                pos.y = newPositionY;
                transform.localPosition = pos;
            }
            else
            {
                Vector3 pos = transform.position;
                pos.y = newPositionY;
                transform.position = pos;
            }
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }
}
