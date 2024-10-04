using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 MoveInput => _frameInput.Move;
    public static Action OnJump, OnJetpack, OnPlayerHit;
    public static PlayerController Instance;

    private Coroutine _jetpackCoroutine;
    [SerializeField] private Gun _gun;
    [SerializeField] private TrailRenderer _jetpackTrail;
    [SerializeField] private float _jumpStrength = 11f;
    [SerializeField] private Transform _feetTransform;
    [SerializeField] private Vector2 _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _extraGravity = 700f;
    [SerializeField] private float _gravityDelay = .2f;
    [SerializeField] private float _coyoteTime = .5f;
    [SerializeField] private float _jetpackTime = .8f;

    private float _timeInAir;
    private float _coyoteTimer;
    private bool _doubleJumpAvailable = true;
    private Health _health;

    private PlayerInput _playerInput;
    private FrameInput _frameInput;
    private Rigidbody2D _rigidBody;
    private Movement _movement;

    public void Awake() {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
        _health = GetComponent<Health>();
    }
    private void OnEnable()
    {
        OnJump += ApplyJumpForce;
        OnJetpack += StartJetpack;
        _health.OnTakeDamage += BordcastHit;
    }
    private void OnDisable()
    {
        OnJump -= ApplyJumpForce;
        OnJetpack -= StartJetpack;
        _health.OnTakeDamage -= BordcastHit;
    }   

    private void BordcastHit(){
        OnPlayerHit?.Invoke();
    }

    private void Update()
    {
        GatherInput();
        Movement();
        CoyoteTimer();
        HandleJump();
        HandleSpriteFlip();
        GravityDelay();
        Jetpack();
        Shoot();
        Grenade();
    }
    private void FixedUpdate() {
        ExtraGravity();     
    }

    public bool CheckGrounded()
    {
        Collider2D isGrounded = Physics2D.OverlapBox(_feetTransform.position,
            _groundCheck, 0f, _groundLayer);
        return isGrounded;
    }

    private void Movement()
    {
        _movement.SetCurrentDirection(_frameInput.Move.x);
    }
    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }

    private void GatherInput()
    {
        // float moveX = Input.GetAxis("Horizontal");
        // _movement = new Vector2(moveX * _moveSpeed, _rigidBody.velocity.y);

        _frameInput = _playerInput.FrameInput;
    }
    private void GravityDelay(){
        if (!CheckGrounded()){
            _timeInAir += Time.deltaTime;
        }
        else{
            _timeInAir = 0;
        }
    }
    private void ExtraGravity(){
        if (_timeInAir > _gravityDelay){
            _rigidBody.AddForce(Vector2.down * _extraGravity * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_feetTransform.position, _groundCheck); 
    }

    private void HandleJump()
    {
        if (!_frameInput.Jump)
            return;
        
        if (CheckGrounded()){
            OnJump?.Invoke();
        }else if (_coyoteTimer > 0){
            OnJump?.Invoke();
        }else if (_doubleJumpAvailable) {
            _doubleJumpAvailable = false;
            OnJump?.Invoke();
        }
    }
    private void CoyoteTimer(){
        if (CheckGrounded()){
            _doubleJumpAvailable = true;
            _coyoteTimer = _coyoteTime;
        }else{
            _coyoteTimer -= Time.deltaTime;
        }
    }

    private void ApplyJumpForce(){
        _rigidBody.velocity = Vector2.zero;
        _timeInAir = 0;
        _coyoteTimer = 0;
        _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
    }

    private void HandleSpriteFlip()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    } 

    private void Jetpack(){
        if (!_frameInput.Jetpack || _jetpackCoroutine != null)
            return;
        OnJetpack?.Invoke();
    }
    private void StartJetpack(){
        _jetpackTrail.emitting = true;
        _jetpackCoroutine = StartCoroutine(JetpackCoroutine());
    }

    private void Shoot(){
        if (!_frameInput.Shoot)
            return;
        _gun.FireShoot();
    }
    private void Grenade(){
        if (!_frameInput.Grenade)
            return;
        _gun.FireGrenade();
    }

    private IEnumerator JetpackCoroutine(){
        float timer = 0;
        while (timer < _jetpackTime){
            _rigidBody.velocity = Vector2.up * _jumpStrength;
            timer += Time.deltaTime;
            yield return null;
        }
        _jetpackCoroutine = null;
        _jetpackTrail.emitting = false;
    }
}
