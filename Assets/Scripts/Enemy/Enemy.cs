using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private float _jumpInterval = 4f;
    [SerializeField] private float _changeDirectionInterval = 3f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockbackThrust = 25f;

    private Rigidbody2D _rigidBody;
    private Movement _movement;
    private ColorChanger _colorChanger;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _movement = GetComponent<Movement>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    private void Start() {
        StartCoroutine(ChangeDirectionRoutine());
        StartCoroutine(RandomJumpRoutine());
    }

    public void Init(Color color){
        _colorChanger.SetDefaultColor(color); 
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            float currentDirection = UnityEngine.Random.Range(0, 2) * 2 - 1; // 1 or -1
            _movement.SetCurrentDirection(currentDirection);
            yield return new WaitForSeconds(_changeDirectionInterval);
        }
    }

    private IEnumerator RandomJumpRoutine() 
    {
        while (true)
        {
            yield return new WaitForSeconds(_jumpInterval);
            float randomDirection = Random.Range(-1, 1);
            Vector2 jumpDirection = new Vector2(randomDirection, 1f).normalized;
            _rigidBody.AddForce(jumpDirection * _jumpForce, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (!player) return;

        var playerMovement = player.GetComponent<Movement>();
        if (!playerMovement.CanMove) return;

        IHitable hitable = other.gameObject.GetComponent<IHitable>();
        hitable?.TakeHit();

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(transform.position, _damageAmount, _knockbackThrust);
    }
}
