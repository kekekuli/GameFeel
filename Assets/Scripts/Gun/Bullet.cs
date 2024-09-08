using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _bulletHitVFX;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockbackThrust = 20f;

    private bool _isReleased = false;

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
        if (_isReleased == true) return;

        _isReleased = true;

        Instantiate(_bulletHitVFX, transform.position, Quaternion.identity);
        
        IHitable hitable = other.gameObject.GetComponent<IHitable>();
        hitable?.TakeHit();

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(_damageAmount, _knockbackThrust);
       
        _gun.ReleaseBulletFromBool(this);
    }
    public void Init(Gun gun, Vector2 bulletSpawnPoint, Vector2 mousePos)
    {
        _isReleased = false;

        _gun = gun;
        transform.position = bulletSpawnPoint;
        _fireDirection = (mousePos - bulletSpawnPoint).normalized;
    }
}