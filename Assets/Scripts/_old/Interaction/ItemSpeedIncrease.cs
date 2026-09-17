using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemSpeedIncrease : InteractableItem
{
    [SerializeField] private float _speedIncreaseBy = 2f;
    private Mover Mover => GetComponentInParent<Mover>();
    
    public override void Interact()
    {
        if (Mover != null)
        {
            Mover.OverrideBaseSpeed(_speedIncreaseBy + Mover.CurrentSpeed);
            Debug.Log($"Speed increased by {_speedIncreaseBy}, current speed: {Mover.CurrentSpeed}");
            Destroy(gameObject);
        }
        else
            Debug.LogError($"{Mover} not found in {this}");
    }
}
