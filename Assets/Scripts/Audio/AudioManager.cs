using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private float _masterVolume = 1f;
    [SerializeField] private SoundCollectionSO _soundCollection;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;

    private AudioSource _currentMusic;

    private void Start() {
        FightMusic();
    }

    private void OnEnable()
    {
        Gun.OnShoot += Gun_OnShoot;
        PlayerController.OnJump += PlayerController_OnJump;
        Health.OnDeath += Health_OnDeath;
        DiscoBallManager.OnDiscoBallHit += DiscoBallManager_OnDiscoBallHit;
        PlayerController.OnJetpack += PlayerController_OnJetpack;
        Grenade.OnBeep += Grenade_OnBeep;
        Grenade.OnExplode += Grenade_OnExplode;
        PlayerController.OnPlayerHit += Enemy_OnPlayerHit;
    }
    private void OnDisable()
    {
        Gun.OnShoot -= Gun_OnShoot;
        PlayerController.OnJump -= PlayerController_OnJump;
        Health.OnDeath -= Health_OnDeath;
        DiscoBallManager.OnDiscoBallHit -= DiscoBallManager_OnDiscoBallHit;
        PlayerController.OnJetpack -= PlayerController_OnJetpack;
        Grenade.OnBeep -= Grenade_OnBeep;
        Grenade.OnExplode -= Grenade_OnExplode;
        PlayerController.OnPlayerHit -= Enemy_OnPlayerHit;
    }
    private void Grenade_OnExplode()
    {
        PlayRandomSound(_soundCollection.GrenadeExplode);
    }
    private void Grenade_OnBeep()
    {
        PlayRandomSound(_soundCollection.GrenadeBeep); 
    }
    private void DiscoBallManager_OnDiscoBallHit()
    {
        DiscoMusic();
    }

    private void Health_OnDeath(Health health)
    {
        PlayRandomSound(_soundCollection.Splatter);
    }
    private void Enemy_OnPlayerHit(){
        PlayRandomSound(_soundCollection.PlayerHit);
    }

    private void SoundToPlay(SoundSO sound){
        var clip = sound.Clip;
        var volume = sound.Volume * _masterVolume;
        var pitch = sound.Pitch;
        var loop = sound.Loop;
        AudioMixerGroup mixerGroup;

        if(sound.RandomizePitch){
            pitch += Random.Range(-sound.RanomPitchRangeModifier, sound.RanomPitchRangeModifier);
        }

        switch(sound.AudioType){
            case SoundSO.AudioTypes.Music:
                mixerGroup = _musicMixerGroup;
                break;
            case SoundSO.AudioTypes.SFX:
                mixerGroup = _sfxMixerGroup;
                break;
            default:
                mixerGroup = null;
                break;
        }

        PlaySound(clip, volume, pitch, loop, mixerGroup);
    }
    public void PlayRandomSound(SoundSO[] soundSOs){
        if (soundSOs == null || soundSOs.Length == 0) return;
        
        var randomIndex = Random.Range(0, soundSOs.Length);
        SoundToPlay(soundSOs[randomIndex]);
    }

    private void PlaySound(AudioClip clip, float volume, float pitch, bool loop, AudioMixerGroup mixerGroup = null){
        var audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume * _masterVolume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.Play();

        if (!loop){
            Destroy(audioSource, clip.length);
        }

        if (mixerGroup == _musicMixerGroup){
            if (_currentMusic != null)
                _currentMusic.Stop();
            _currentMusic = audioSource; 
        }
    }

    private void Gun_OnShoot(){
        PlayRandomSound(_soundCollection.Shoot);
    }
    private void PlayerController_OnJump(){
        PlayRandomSound(_soundCollection.Jump);
    }
    private void PlayerController_OnJetpack(){
        PlayRandomSound(_soundCollection.Jetpack);
    }
    private void FightMusic()
    {
        PlayRandomSound(_soundCollection.FightMusic);
    }
    private void DiscoMusic()
    {
        PlayRandomSound(_soundCollection.DiscoMusic);
        var soundLength = _soundCollection.DiscoMusic[0].Clip.length;
        Utils.RunAfterDelay(this, soundLength, FightMusic);
    }
}
