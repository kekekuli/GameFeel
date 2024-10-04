using System;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private TMP_Text scoreTxt;
    private int score = 0;

    private void Awake() {
        scoreTxt = GetComponent<TMP_Text>();
    }

    private void OnEnable() {
        Health.OnDeath += EnemyDeath;        
    }
    private void OnDisable(){
        Health.OnDeath -= EnemyDeath;
    }
    private void EnemyDeath(Health health){
        score ++;
        scoreTxt.text = score.ToString();
    }
}
