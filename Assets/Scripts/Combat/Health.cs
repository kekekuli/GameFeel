 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private int _startingHealth = 3;

    private int _currentHealth;

    private void Start() {
        ResetHealth();
    }

    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            SpawnSplatterPrefab();
            Destroy(gameObject);
        }
    }
    private void SpawnSplatterPrefab() {
        GameObject newSplatterPrefab = Instantiate(_splatterPrefab, transform.position, Quaternion.identity);
        SpriteRenderer splatterSpriteRenderer = newSplatterPrefab.GetComponent<SpriteRenderer>();
        ColorChanger colorChanger = GetComponent<ColorChanger>();
        splatterSpriteRenderer.color = colorChanger.DefaultColor;
    }
}
