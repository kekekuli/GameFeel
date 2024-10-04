using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Grenade : Projectile
{
    public static Action OnBeep;
    public static Action OnExplode;
    [SerializeField] private float _torque = 20f;
    [SerializeField] private Light2D _beepLight;
    [SerializeField] private int _beepCount = 3;
    [SerializeField] private float _beepInterval = 0.5f;
    [SerializeField] private float _beepDuration = 0.1f;
    private float _beepIntensity;
    private int _beepIndex = 0;
    private CinemachineImpulseSource _impulseSource;

    private void Awake() {
        _beepIntensity = _beepLight.intensity;
        _rigidBody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();

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
            yield return new WaitForSeconds(_beepInterval);
            _beepLight.intensity = _beepIntensity; 
            OnBeep?.Invoke();
            yield return new WaitForSeconds(_beepDuration);
            _beepLight.intensity = 0;
            _beepIndex++;
        }
        _beepLight.enabled = false;
        Explode();
    }

    private void Explode(){
        Instantiate(_hitVFX, transform.position, Quaternion.identity);
        _gun.ReleaseGrenadeFromBool(this);
        _impulseSource.GenerateImpulse();
        OnExplode?.Invoke();
    }
}
