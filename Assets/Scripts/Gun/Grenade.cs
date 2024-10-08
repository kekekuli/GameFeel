using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Grenade : Projectile
{
    public static Action OnBeep;
    public static Action OnExplode;
    [SerializeField] private float _explodeRadius = 5f;
    [SerializeField] private float _torque = 20f;
    [SerializeField] private Light2D _beepLight;
    [SerializeField] private int _beepCount = 3;
    [SerializeField] private float _explodeTime = 1.5f;
    [SerializeField] private float _beepDuration = 0.1f;
    [SerializeField] private LayerMask _enemyLayer;
    private float _beepIntensity;
    private int _beepIndex = 0;
    private CinemachineImpulseSource _impulseSource;

    private void Awake() {
        _beepIntensity = _beepLight.intensity;
        _rigidBody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public override void Init(Gun gun, Vector2 spawnPoint, Vector2 mousePos)
    {
        base.Init(gun, spawnPoint, mousePos);
        _rigidBody.velocity = _fireDirection * _moveSpeed;
        _rigidBody.AddTorque(_torque);
        Utils.RunAfterDelay(this, 0.5f, StartBeep); 
    }

    private void StartBeep(){
        _beepIndex = 0;
        _beepLight.enabled = true;
        _beepLight.intensity = 0;
        StartCoroutine(BeepCoroutine());
    }

    private IEnumerator BeepCoroutine(){
        while(_beepIndex < _beepCount){
            yield return new WaitForSeconds(_explodeTime / _beepCount);
            _beepLight.intensity = _beepIntensity; 
            OnBeep?.Invoke();
            yield return new WaitForSeconds(_beepDuration);
            _beepLight.intensity = 0;
            _beepIndex++;
        }
        _beepLight.enabled = false;
        Explode();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if ((1 << other.gameObject.layer & _enemyLayer) != 0)
            Explode();
    }

    private void Explode(){
        if (!_isFirstCollision) return;
        _isFirstCollision = false;

        Instantiate(_hitVFX, transform.position, Quaternion.identity);
        _gun.ReleaseGrenadeFromBool(this);
        _impulseSource.GenerateImpulse();

        var colliders = Physics2D.OverlapCircleAll(transform.position, _explodeRadius, _enemyLayer);
        foreach (var collider in colliders)
        {
            IHitable hitable = collider.GetComponent<IHitable>();
            hitable?.TakeHit();

            IDamageable damageable = collider.GetComponent<IDamageable>();
            damageable?.TakeDamage(transform.position,_damageAmount, _knockbackThrust);
        }

        OnExplode?.Invoke();
    }
}
