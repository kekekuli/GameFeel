using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private Action OnDeath;
    
    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private GameObject _deathVFX;
    [SerializeField] private int _startingHealth = 3;

    private int _currentHealth;

    private void Start() {
        ResetHealth();
    }
    private void OnEnable() {
        OnDeath += SpawnSplatterPrefab;
        OnDeath += SpawnDeathVFX;
    }
    private void OnDisable(){
        OnDeath -= SpawnSplatterPrefab;
        OnDeath -= SpawnDeathVFX;
    }

    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
    private void SpawnSplatterPrefab() {
        GameObject newSplatterPrefab = Instantiate(_splatterPrefab, transform.position, Quaternion.identity);
        SpriteRenderer splatterSpriteRenderer = newSplatterPrefab.GetComponent<SpriteRenderer>();
        ColorChanger colorChanger = GetComponent<ColorChanger>();
        splatterSpriteRenderer.color = colorChanger.DefaultColor;
    }
    private void SpawnDeathVFX() {
        GameObject newDeathVFX = Instantiate(_deathVFX, transform.position, Quaternion.identity);   
        ParticleSystem.MainModule ps = newDeathVFX.GetComponent<ParticleSystem>().main;
        ColorChanger colorChanger = GetComponent<ColorChanger>();
        ps.startColor = colorChanger.DefaultColor;
    }
}
