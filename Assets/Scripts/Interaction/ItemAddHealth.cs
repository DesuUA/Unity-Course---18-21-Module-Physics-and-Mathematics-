using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemAddHealth : InteractableItem
{
    [SerializeField] private float _healthToAdd = 10f;
    
    private Health Health => GetComponentInParent<Health>();
    
    public override void Interact()
    {
        if (Health != null)
        {
            Health.AddHealth(_healthToAdd);
            Debug.Log($"Added health: {_healthToAdd}, current health: {Health.CurrentHealth}");
            Destroy(gameObject);
        }
        else
            Debug.LogError($"{Health} not found in {this}");
    }
}
