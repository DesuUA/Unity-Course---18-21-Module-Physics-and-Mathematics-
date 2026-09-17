using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public abstract class Mover : MonoBehaviour
{
    [field: Header("Movement")] 
    [SerializeField] private float _baseSpeed = 1.5f;
    [SerializeField] protected float boostMultiplier = 1.5f;
    [SerializeField] protected float jumpForce = 1f;
    [SerializeField] protected float rotationSpeed = 500f;

    protected bool Jump;
    
    public float CurrentSpeed { get; private set; }
    private bool _isBoosted;
    
    public abstract void SetDirection(Vector3 direction);

    protected virtual void Start()
    {
        CurrentSpeed = _baseSpeed;
    }

    public void SetSpeedBoost(bool enableBoost)
    {
        if (_isBoosted == enableBoost) return; 

        _isBoosted = enableBoost;
        CurrentSpeed = enableBoost ? _baseSpeed * boostMultiplier : _baseSpeed;
    }
    
    public virtual void SetJump(bool jump)
    {
        Jump = jump;
    }
    
    public void OverrideBaseSpeed(float newSpeed)
    {
        CurrentSpeed = newSpeed;
        _baseSpeed = newSpeed;
    }
}