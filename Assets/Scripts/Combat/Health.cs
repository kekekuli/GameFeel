using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public static Action<Health> OnDeath;

    public Action OnTakeDamage;

    public GameObject SplatterPrefab => _splatterPrefab;
    public GameObject DeathVFX => _deathVFX;
    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private GameObject _deathVFX;
    [SerializeField] private int _startingHealth = 3;
    private Knockback _knockback;
    private Flash _flash;
    private Health _health;
    private int _currentHealth;

    private void Awake()
    {
        _knockback = GetComponent<Knockback>();
        _flash = GetComponent<Flash>();
        _health = GetComponent<Health>();

    }
    private void Start() {
        ResetHealth();
    }

    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;
        OnTakeDamage?.Invoke();

        if (_currentHealth <= 0) {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
    public void TakeDamage(Vector2 damageSourceDir, int damageAmount, float knockbackThrust)
    {
        _health.TakeDamage(damageAmount);
        _knockback.GetKnockedBack(damageSourceDir, knockbackThrust);
    }
    public void TakeHit()
    {
        _flash.StartFlash();
    }
}
