using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDust;
    [SerializeField] private ParticleSystem _proofDust;
    [SerializeField] private float _tiltAngle = 20f;
    [SerializeField] private float _tiltSpeed = 5f;
    [SerializeField] private Transform _playerSpriteTransform;
    [SerializeField] private Transform _cowboyHatTransform;
    [SerializeField] private float _cowboyHatTiltModifer = 2f;
    [SerializeField] private float _yLandVelocityCheck = -10f;
    [SerializeField] private Rigidbody2D _rigidBody;
    
    private Vector2 _velocityBeforePhysicsUpdate;
    private CinemachineImpulseSource _impulseSource;

    private void Awake(){
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update() {
        DetectMoveDust();
        ApplyTilt();
    }

    private void FixedUpdate() {
        _velocityBeforePhysicsUpdate = _rigidBody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (_velocityBeforePhysicsUpdate.y < _yLandVelocityCheck){
            PlayProofDust();
            _impulseSource.GenerateImpulse();
        }
    }

    private void DetectMoveDust(){
        if (PlayerController.Instance.CheckGrounded()){
            if (!_moveDust.isPlaying)
                _moveDust.Play();
        }
        else{
            if (_moveDust.isPlaying)
                _moveDust.Stop();
        }
    }
    private void PlayProofDust(){
        _proofDust.Play();
    }
    private void OnEnable(){
        PlayerController.OnJump += PlayProofDust;
    }
    private void OnDisable(){
        PlayerController.OnJump -= PlayProofDust;
    }

    private void ApplyTilt()
    {
        float targetAngle;
        
        if (PlayerController.Instance.MoveInput.x > 0)
            targetAngle = -_tiltAngle;
        else if (PlayerController.Instance.MoveInput.x < 0)
            targetAngle = _tiltAngle;
        else
            targetAngle = 0;
        
        // Player Sprite Render
        Quaternion currentRotation = _playerSpriteTransform.rotation;
        Vector3 eulerAngles = currentRotation.eulerAngles;
        Quaternion targetRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, targetAngle);
        _playerSpriteTransform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _tiltSpeed * Time.deltaTime);
        
        // Cowboy Hat Render
        Quaternion currentHatRotation = _cowboyHatTransform.rotation;
        Vector3 hatEulerAngles = currentHatRotation.eulerAngles;
        Quaternion targetHatRotation = Quaternion.Euler(hatEulerAngles.x, hatEulerAngles.y, -targetAngle / _cowboyHatTiltModifer);
        _cowboyHatTransform.rotation = Quaternion.Slerp(currentHatRotation, targetHatRotation, _tiltSpeed * +_cowboyHatTiltModifer * Time.deltaTime);
    }
    
}
