using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _whiteFlashMaterial;
    [SerializeField] private float _flashTime = .1f;

    private SpriteRenderer[] _spriteRenderers;
    private Color _defaultColor;
    private ColorChanger _colorChanger;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    public void StartFlash()
    {
        StartCoroutine(FlashRoutine());
    }
    private IEnumerator FlashRoutine()
    {
        foreach (SpriteRenderer sr in _spriteRenderers)
        {
            sr.material = _whiteFlashMaterial;

            _colorChanger?.SetColor(Color.white);
        }
        yield return new WaitForSeconds(_flashTime);

        SetDefaultMaterial();
    }
    private void SetDefaultMaterial()
    {
        foreach (SpriteRenderer sr in _spriteRenderers)
        {
            sr.material = _defaultMaterial;
            _colorChanger?.SetColor(_colorChanger.DefaultColor);
        }
    }
}
