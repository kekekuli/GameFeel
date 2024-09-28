using UnityEngine;

[CreateAssetMenu()]
public class SoundSO : ScriptableObject
{
    public enum AudioTypes{
        Music,
        SFX
    }

    public AudioTypes AudioType;
    public AudioClip Clip;

    public bool Loop = false;
    public bool RandomizePitch = false;
    [Range(0, 1)]
    public float RanomPitchRangeModifier = 0.1f;
    [Range(.1f, 2f)]
    public float Volume = 1f; 
    [Range(.1f, 3f)]
    public float Pitch = 1f;
}
