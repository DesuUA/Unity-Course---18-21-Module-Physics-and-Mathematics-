using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;

    private void Start()
    {
        Rigidbody bulletBody = GetComponent<Rigidbody>();
        bulletBody.AddForce(transform.forward * _speed, ForceMode.VelocityChange);
  
        Destroy(gameObject, _lifeTime);
    }
}