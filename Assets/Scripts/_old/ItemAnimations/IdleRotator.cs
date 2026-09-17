using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class IdleRotator : Animations
{
    [SerializeField] private float _rotateSpeed = 50f;

    protected override void Start()
    {
        base.Start();
        
        float randomStartAngle = Random.Range(0f, 360f);
        transform.Rotate(0f, randomStartAngle, 0f, Space.Self);
    }

    protected override void Update()
    {
        base.Update();
        
        if (IsPlaying)
            transform.Rotate(0f, _rotateSpeed * Time.deltaTime, 0f, Space.Self);
        else
            transform.localRotation = Quaternion.identity;
    }
}
