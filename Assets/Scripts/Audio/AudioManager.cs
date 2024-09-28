using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private float _masterVolume = 1f;
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
    private void SoundToPlay(SoundSO sound){
        var clip = sound.Clip;
        var volume = sound.Volume * _masterVolume;
        var pitch = sound.Pitch;
        var loop = sound.Loop;

        if(sound.RandomizePitch){
            pitch += Random.Range(-sound.RanomPitchRangeModifier, sound.RanomPitchRangeModifier);
        }

        PlaySound(clip, volume, pitch, loop);
    }

    private void PlaySound(AudioClip clip, float volume, float pitch, bool loop){
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume * _masterVolume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        audioSource.Play();

        if (!loop){
            Destroy(audioSource, clip.length);
        }
    }

    private void Gun_OnShoot(){
        SoundToPlay(_gunShoot);
    }
    private void PlayerController_OnJump(){
        SoundToPlay(_playerJump);
    }
}
