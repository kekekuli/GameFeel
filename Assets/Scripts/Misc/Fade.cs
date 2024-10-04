using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;

public class Fade : MonoBehaviour
{
    [SerializeField] private float _fadeTime;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _respawnPoint;

    private Image _image;
    private CinemachineVirtualCamera _virtualCam;

    private void Awake() {
        _image = GetComponent<Image>();
        _virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
    }    

    public void FadeInAndOut(){
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn(){
        yield return StartCoroutine(FadeCoroutine(1f));

        RespawnPlayer();
        FadeOut();
    }
    public void FadeOut(){
        StartCoroutine(FadeCoroutine(0f));
    }

    public IEnumerator FadeCoroutine(float targetAlpha){
        float elapsedTime = 0f;
        float startAlpha = _image.color.a;

        while(elapsedTime < _fadeTime){
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / _fadeTime);
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, newAlpha);
            yield return null;
        }
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, targetAlpha);
    }

    private void RespawnPlayer(){
        var player = Instantiate(_playerPrefab, _respawnPoint.position, Quaternion.identity);
        _virtualCam.Follow =  player.transform;
    }
}
