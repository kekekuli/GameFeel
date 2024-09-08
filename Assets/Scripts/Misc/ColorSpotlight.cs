using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpotlight : MonoBehaviour
{
    [SerializeField] private GameObject _spotlightHead;
    [SerializeField] private float _rotationSpeed = 12f;
    [SerializeField] private float _maxRotation = 45f;
    
    private float _currentRotation = 0f;

    private void Start()
    {
        RandomStartingRotation(); 
    }

    private void Update()
    {
        RotateHead(); 
    }

    private void RotateHead()
    {
        _currentRotation += Time.deltaTime * _rotationSpeed;
        float z = Mathf.PingPong(_currentRotation, _maxRotation);
        _spotlightHead.transform.localRotation = Quaternion.Euler(0, 0, z); 
    }

    private void RandomStartingRotation()
    {
        _currentRotation = UnityEngine.Random.Range(-_maxRotation, _maxRotation); 
    }
}
