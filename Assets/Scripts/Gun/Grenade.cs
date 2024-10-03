using UnityEngine;

public class Grenade : Projectile
{
    [SerializeField] private float _torque = 20f;

    private void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();

        var selfCollider = GetComponent<Collider2D>();
        var playColliders = PlayerController.Instance.GetComponents<Collider2D>();
        foreach (var collider in playColliders)
            Physics2D.IgnoreCollision(selfCollider, collider, true);
    }

    public override void Init(Gun gun, Vector2 spawnPoint, Vector2 mousePos)
    {
        base.Init(gun, spawnPoint, mousePos);
        _rigidBody.velocity = _fireDirection * _moveSpeed;
        _rigidBody.AddTorque(_torque);
    }
}
