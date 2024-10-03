using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;

    public enum ProjectileType
    {
        Bullet,
        Grenade
    };

    private ProjectileType _projectileType = ProjectileType.Bullet;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Grenade _grenadePrefab;
    [SerializeField] private float gunFireCD = 0.5f;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private float _muzzleFlashTime = 0.5f;

    private ObjectPool<Bullet> _bulletPool;
    private ObjectPool<Grenade> _grenadePool;
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Vector2 _mousePos;
    private float _lastFireTime = 0f;
    private Coroutine _muzzleFlashRoutine;

    private Animator _animator;
    private CinemachineImpulseSource _impulseSource;

    public void Start()
    {
        CreatePools();
    }

    private void CreatePools()
    {
        _bulletPool = new ObjectPool<Bullet>(() => { return Instantiate(_bulletPrefab); },
            bullet => { bullet.gameObject.SetActive(true); }, bullet => { bullet.gameObject.SetActive(false); },
            bullet => { Destroy(bullet); }
            );
        _grenadePool = new ObjectPool<Grenade>(() => {return Instantiate(_grenadePrefab);},
            grenade => {grenade.gameObject.SetActive(true);}, grenade => {grenade.gameObject.SetActive(false);},
            grenade => {Destroy(grenade);}
            );
    }

    public void ReleaseBulletFromBool(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }
    public void ReleaseGrenadeFromBool(Grenade grenade){
        _grenadePool.Release(grenade);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        RotateGun();
    }

    public void FireShoot()
    {
        
        if (Time.time >= _lastFireTime){
            _projectileType = ProjectileType.Bullet;
            OnShoot?.Invoke();
        }
    }
    public void FireGrenade(){
        if (Time.time >= _lastFireTime){
            _projectileType = ProjectileType.Grenade; 
            OnShoot?.Invoke();
        }
    }

    private void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += ResetLastFireTime;
        OnShoot += FireAnimation;
        OnShoot += GunScreenShake;
        OnShoot += MuzzleFlash;
    }

    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
        OnShoot -= FireAnimation;
        OnShoot -= GunScreenShake;
        OnShoot -= MuzzleFlash;
    }

    private void GunScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

private void ResetLastFireTime()
    {
        _lastFireTime = Time.time + gunFireCD;
    }

    private void ShootProjectile()
    {
        Projectile projectile ;
        if (_projectileType == ProjectileType.Bullet)
            projectile = _bulletPool.Get();
        else
            projectile = _grenadePool.Get();

        projectile.Init(this, _spawnPoint.position, _mousePos);
    }
    
    private void FireAnimation()
    {
        _animator.Play(FIRE_HASH, 0, 0f);
    }

    private void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
    private void MuzzleFlash()
    {
        if (_muzzleFlashRoutine != null)
            StopCoroutine(_muzzleFlashRoutine);
        _muzzleFlashRoutine = StartCoroutine(MuzzleFlashRoutine());
    }

    private IEnumerator MuzzleFlashRoutine()
    {
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(_muzzleFlashTime);
        _muzzleFlash.SetActive(false);
    }
}
