using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDust;
    [SerializeField] private float _tiltAngle = 20f;
    [SerializeField] private float _tiltSpeed = 5f;
    [SerializeField] private Transform _playerSpriteTransform;
    
    private void Update() {
        DetectMoveDust();
        ApplyTilt();
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

    private void ApplyTilt()
    {
        float targetAngle;
        
        if (PlayerController.Instance.MoveInput.x > 0)
            targetAngle = -_tiltAngle;
        else if (PlayerController.Instance.MoveInput.x < 0)
            targetAngle = _tiltAngle;
        else
            targetAngle = 0;
        
        Quaternion currentRotation = _playerSpriteTransform.rotation;
        Vector3 eulerAngles = currentRotation.eulerAngles;
        Quaternion targetRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, targetAngle);

        _playerSpriteTransform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _tiltSpeed * Time.deltaTime);
    }
    
}
