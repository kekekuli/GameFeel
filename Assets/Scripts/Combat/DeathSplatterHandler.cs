using UnityEngine;
using UnityEditor;

public class DeathSplatterHandler : MonoBehaviour{
    private void SpawnSplatterPrefab(Health sender) {
        GameObject newSplatterPrefab = Instantiate(sender.SplatterPrefab, sender.transform.position, Quaternion.identity);
        SpriteRenderer splatterSpriteRenderer = newSplatterPrefab.GetComponent<SpriteRenderer>();
        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();
        if (colorChanger != null){
            splatterSpriteRenderer.color = colorChanger.DefaultColor;
            newSplatterPrefab.transform.SetParent(transform);
        }
    }
    private void SpawnDeathVFX(Health sender) {
        GameObject newDeathVFX = Instantiate(sender.DeathVFX, sender.transform.position, Quaternion.identity);   
        ParticleSystem.MainModule ps = newDeathVFX.GetComponent<ParticleSystem>().main;
        ColorChanger colorChanger = sender.GetComponent<ColorChanger>();
        if (colorChanger != null){
            ps.startColor = colorChanger.DefaultColor;
            newDeathVFX.transform.SetParent(transform);
        }
    }
   

    private void OnEnable() {
        Health.OnDeath += SpawnSplatterPrefab;
        Health.OnDeath += SpawnDeathVFX;
    }
    private void OnDisable(){
        Health.OnDeath -= SpawnSplatterPrefab;
        Health.OnDeath -= SpawnDeathVFX;
    }
}

