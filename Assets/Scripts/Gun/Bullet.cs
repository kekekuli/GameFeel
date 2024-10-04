using UnityEngine;

public class Bullet : Projectile
{
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (_isFirstCollision == false) return;
        _isFirstCollision = false; 

        Instantiate(_hitVFX, transform.position, Quaternion.identity);
        
        IHitable hitable = other.gameObject.GetComponent<IHitable>();
        hitable?.TakeHit();

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(PlayerController.Instance.transform.position, _damageAmount, _knockbackThrust);
       
        _gun.ReleaseBulletFromBool(this);
    }

}