using System.Collections;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    [SerializeField] private float _disableTime = 1f;
    private Collider2D _collider;
    private bool _playerOnPlatform;

    private void Update() {
        DetectInput();
    }

    private void Awake() {
        _collider = GetComponent<Collider2D>();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<PlayerController>()) {
            _playerOnPlatform = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.GetComponent<PlayerController>()) {
            _playerOnPlatform = false;
        }
    }

    private void DetectInput(){
        if (!_playerOnPlatform)
            return;
        
        if (Input.GetKeyDown(KeyCode.S)) {
            StartCoroutine(DisableCollisition());
        }
    }

    private IEnumerator DisableCollisition() {
        var Collider2Ds = PlayerController.Instance.gameObject.GetComponents<Collider2D>();        

        foreach (var collider in Collider2Ds) {
            Physics2D.IgnoreCollision(collider, _collider, true);
        }

        yield return new WaitForSeconds(_disableTime);

        foreach (var collider in Collider2Ds) {
            Physics2D.IgnoreCollision(collider, _collider, false);
        }
    }
}
