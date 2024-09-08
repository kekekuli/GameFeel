using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class DiscoBallManager : MonoBehaviour
{
    [SerializeField] private float _discoBallPartyTime = 2f;
    [SerializeField] private float _discoGlobalLightIntensity = 0.35f;
    [SerializeField] private Light2D _globalLight;

    private float _defaultGlobalLightIntensity;
    private Coroutine _discoCoroutine;
    private ColorSpotlight[] _allSpotLights;

    public static Action OnDiscoBallHit;

    private void Awake() {
        _defaultGlobalLightIntensity = _globalLight.intensity;
    }

    private void Start() {
        _allSpotLights = FindObjectsByType<ColorSpotlight>(FindObjectsSortMode.None);
    }

    private void OnEnable() {
        OnDiscoBallHit += DimTheLights;     
    }
    private void OnDisable() {
        OnDiscoBallHit -= DimTheLights;      
    }
    private void DimTheLights(){
        foreach (var spotlight in _allSpotLights)
        {
            StartCoroutine(spotlight.SpotLightDiscoParty(_discoBallPartyTime));
        }

        _discoCoroutine = StartCoroutine(GlobalListhResetRoutine(_discoBallPartyTime));
    }
    public void DiscoParty(){
        if (_discoCoroutine != null) return;

        OnDiscoBallHit?.Invoke();
    }
    private IEnumerator GlobalListhResetRoutine(float time){
        _globalLight.intensity = _discoGlobalLightIntensity;
        yield return new WaitForSeconds(time);
        _globalLight.intensity = _defaultGlobalLightIntensity;

        _discoCoroutine = null;
    }
}
