using UnityEngine;
public abstract class Projectile : MonoBehaviour {
    [SerializeField] protected GameObject _hitVFX;
    [SerializeField] protected float _moveSpeed = 10f;
    [SerializeField] protected int _damageAmount = 1;
    [SerializeField] protected float _knockbackThrust = 20f;

    protected bool _isFirstCollision = true;
    protected Gun _gun;
    protected Vector2 _fireDirection;

    protected Rigidbody2D _rigidBody;

    public virtual void Init(Gun gun, Vector2 spawnPoint, Vector2 mousePos)
    {
        _isFirstCollision = true;
        _gun = gun;
        transform.position = spawnPoint;
        _fireDirection = (mousePos - spawnPoint).normalized;
    }
}