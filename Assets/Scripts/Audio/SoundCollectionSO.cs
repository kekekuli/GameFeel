using UnityEngine;

[CreateAssetMenu()]
public class SoundCollectionSO : ScriptableObject
{
    [Header("Music")]
    public SoundSO[] FightMusic;
    public SoundSO[] DiscoMusic;

    [Header("SFX")]
    public SoundSO[] Shoot;
    public SoundSO[] Jump;
    public SoundSO[] Splatter;
    public SoundSO[] Jetpack;
    public SoundSO[] GrenadeBeep;
    public SoundSO[] GrenadeExplode;
    public SoundSO[] PlayerHit;
    public SoundSO[] MegaKill;
}
