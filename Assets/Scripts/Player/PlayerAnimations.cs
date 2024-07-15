using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDust;
    
    private void Update() {
        DetectMoveDust();
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
}
