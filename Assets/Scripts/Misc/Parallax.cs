using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _parallaxOffset = .1f;
    private Camera _mainCamera;
    private Vector2 _startPos;
    private Vector2 _travel => (Vector2)_mainCamera.transform.position - _startPos;
    private void Awake() {
        _mainCamera = Camera.main;
    }
    private void Start() {
        _startPos = transform.position;
    }
    private void FixedUpdate() {
        var newPostion = _startPos + new Vector2(_travel.x * _parallaxOffset, 0);
        transform.position = new Vector2(newPostion.x, transform.position.y);
    }
}
