using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Knockback : MonoBehaviour
{

    public Action OnKnockbackStart;
    public Action OnKnockbackEnd;

    [SerializeField] private float _knockbackTime = .2f;
    private Vector3 _hitDirection;
    private float _knockbackThrust;

    private Rigidbody2D _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();    
    }

    private void OnEnable() {
        OnKnockbackStart += ApplyKnockbackForce;    
        OnKnockbackEnd += StopKockRoutine;
    }

    private void OnDisable() {
        OnKnockbackStart -= ApplyKnockbackForce;
        OnKnockbackEnd -= StopKockRoutine;
    }

    public void GetKnockedBack(Vector3 hitDirection, float knockbackThrust) {
        Debug.Log("knocked back");
        _hitDirection = hitDirection;
        _knockbackThrust = knockbackThrust;

        OnKnockbackStart?.Invoke();
    }
    public void ApplyKnockbackForce() {
        Vector3 difference = (transform.position - _hitDirection).normalized * _knockbackThrust * _rigidbody.mass;
        _rigidbody.AddForce(difference, ForceMode2D.Impulse);

        StartCoroutine(KnockRoutine());
    }
    public IEnumerator KnockRoutine() {

        yield return new WaitForSeconds(_knockbackTime);

        OnKnockbackEnd?.Invoke();
    }
    private void StopKockRoutine() {
        _rigidbody.velocity = Vector2.zero;
    }
}
