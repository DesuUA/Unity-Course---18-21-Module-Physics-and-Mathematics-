using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Animations : MonoBehaviour
{
    protected bool IsPlaying { get; set; } = true;
    
    private InteractableItem _interactableItem;
    
    private bool _hasComponent;

    protected virtual void Start()
    {
        _hasComponent = TryGetComponent<InteractableItem>(out _interactableItem);
        if (_hasComponent == false)
            Debug.LogError($"[{nameof(Animations)}] {nameof(InteractableItem)} component not found on {gameObject.name}", this);
    }

    protected virtual void Update()
    {
        if (_hasComponent)
        {
            IsPlaying = !_interactableItem.Grabbed;
        }
    }
}
