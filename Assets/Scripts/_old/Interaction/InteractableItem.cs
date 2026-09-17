using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class InteractableItem : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystemPrefab;
    
    public bool Grabbed { get; set; }
    
    public abstract void Interact();
    
    protected virtual void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) 
            return;
        
        if (_particleSystemPrefab != null)
            Instantiate(_particleSystemPrefab, transform.position, transform.rotation);
        
    }
}