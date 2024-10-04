using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    public bool CanMove => _canMove;
    private bool _canMove = true;
    private Knockback _knockback;

    private float _moveX;
    private Rigidbody2D _rigidBody;
    
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _knockback = GetComponent<Knockback>();
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void OnEnable() {
        _knockback.OnKnockbackStart += CanMoveFalse;
        _knockback.OnKnockbackEnd += CanMoveTrue;
    }
    private void OnDisable() {
        _knockback.OnKnockbackStart -= CanMoveFalse;
        _knockback.OnKnockbackEnd -= CanMoveTrue;
    }

    private void CanMoveTrue(){
        _canMove = true;
    }
    private void CanMoveFalse(){
        _canMove = false;
    }

    public void SetCurrentDirection(float currentDirection)
    {
        _moveX = currentDirection;
    }

    private void Move()
    {
        if (!_canMove)
            return;

        Vector2 movement = new Vector2(_moveX * _moveSpeed, _rigidBody.velocity.y);
        _rigidBody.velocity = movement;
    }
}
