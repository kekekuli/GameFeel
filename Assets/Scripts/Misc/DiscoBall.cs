using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBall : MonoBehaviour, IHitable
{
    private Flash _flash;

    private void Awake()
    {
        _flash = GetComponent<Flash>();  
    }

    public void TakeHit()
    {
        _flash.StartFlash();
        var _discoBallManager = FindObjectOfType<DiscoBallManager>();
        _discoBallManager?.DiscoParty();
    }
}
