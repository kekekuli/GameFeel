using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private float _masterVolume = 1f;
    [SerializeField] private SoundCollectionSO _soundCollection;

    private void OnEnable()
    {
        Gun.OnShoot += Gun_OnShoot;
        PlayerController.OnJump += PlayerController_OnJump;
        Health.OnDeath += Health_OnDeath;
    }
    private void OnDisable()
    {
        Gun.OnShoot -= Gun_OnShoot;
        PlayerController.OnJump -= PlayerController_OnJump;
        Health.OnDeath -= Health_OnDeath;
    }

    private void Health_OnDeath(Health health)
    {
        PlayRandomSound(_soundCollection.Splatter);
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
    public void PlayRandomSound(SoundSO[] soundSOs){
        if (soundSOs == null || soundSOs.Length == 0) return;
        
        var randomIndex = Random.Range(0, soundSOs.Length);
        SoundToPlay(soundSOs[randomIndex]);
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
        PlayRandomSound(_soundCollection.Shoot);
    }
    private void PlayerController_OnJump(){
        PlayRandomSound(_soundCollection.Jump);
    }
}
