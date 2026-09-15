using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShootRound : InteractableItem
{
    [SerializeField] private Bullet _bulletPrefab;
    
    private BulletSpawnPoint _bulletSpawnPoint;
    private bool _hasSpawnPoint;
    
    private void Start()
    {
        _bulletSpawnPoint = GetComponentInChildren<BulletSpawnPoint>();
        if (_bulletSpawnPoint == null)
            Debug.LogError($"{this}: BulletSpawnPoint not found.");
        else
            _hasSpawnPoint = true;
    }
    
    public override void Interact()
    {
        if (_hasSpawnPoint)
        {
            Instantiate(_bulletPrefab, _bulletSpawnPoint.transform.position, _bulletSpawnPoint.transform.rotation);
            Destroy(gameObject);
        }
    }
}