using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundSO _gunShoot;
    [SerializeField] private SoundSO _playerJump;

    private void OnEnable()
    {
        Gun.OnShoot += Gun_OnShoot;
        PlayerController.OnJump += PlayerController_OnJump;
    }
    private void OnDisable()
    {
        Gun.OnShoot -= Gun_OnShoot;
        PlayerController.OnJump -= PlayerController_OnJump;
    }
    private void PlaySound(SoundSO sound){
        var soundObj = new GameObject("Temp Audio Source");
        var audioSource = soundObj.AddComponent<AudioSource>(); 
        audioSource.clip = sound.Clip;
        audioSource.Play();
    }

    private void Gun_OnShoot(){
        PlaySound(_gunShoot);
    }
    private void PlayerController_OnJump(){
        PlaySound(_playerJump); 
    }
}
