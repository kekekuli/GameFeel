using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;

    private Gun _gun;
    private Vector2 _fireDirection;

    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Health health = other.gameObject.GetComponent<Health>();
        health?.TakeDamage(_damageAmount);
        _gun.ReleaseBulletFromBool(this);
    }
    public void Init(Gun gun, Vector2 bulletSpawnPoint, Vector2 mousePos)
    {
        _gun = gun;
        transform.position = bulletSpawnPoint;
        _fireDirection = (mousePos - bulletSpawnPoint).normalized;
    }
}