using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action OnJump;

    public static PlayerController Instance;
    [SerializeField] private float _jumpStrength = 7f;
    [SerializeField] private Transform _feetTransform;
    [SerializeField] private Vector2 _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _extraGravity = 700f;
    [SerializeField] private float _gravityDelay = .2f;

    private float _timeInAir;
    private bool _doubleJumpAvailable = true;
    private PlayerInput _playerInput;
    private FrameInput _frameInput;
    private Rigidbody2D _rigidBody;
    private Movement _movement;

    public void Awake() {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
    }
    private void OnEnable()
    {
        OnJump += ApplyJumpForce;
    }
    private void OnDisable()
    {
        OnJump -= ApplyJumpForce;
    }   

    private void Update()
    {
        GatherInput();
        Movement();
        HandleJump();
        HandleSpriteFlip();
        GravityDelay();
    }
    private void FixedUpdate() {
        ExtraGravity();     
    }

    private bool CheckGrounded()
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
        
        if (_doubleJumpAvailable){
            _doubleJumpAvailable = false;
            OnJump?.Invoke();
        }else if (CheckGrounded()) {
            _doubleJumpAvailable = true;
            OnJump?.Invoke();
        }
    }

    private void ApplyJumpForce(){
        _rigidBody.velocity = Vector2.zero;
        _timeInAir = 0;
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
}
