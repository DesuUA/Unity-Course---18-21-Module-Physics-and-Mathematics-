using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    public float CurrentHealth { get; private set; }

    private void Start()
    {
        CurrentHealth = _health;
    }
    
    public void AddHealth(float healthToAdd)
    {
        CurrentHealth += healthToAdd;
    }
}
