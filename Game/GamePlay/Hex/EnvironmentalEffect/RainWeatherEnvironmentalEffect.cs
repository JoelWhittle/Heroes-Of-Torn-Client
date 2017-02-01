using UnityEngine;
using System.Collections;

public class RainWeatherEnvironmentalEffect : WeatherEnvironmentalEffect {

    public override void Init(GameObject hexGameObject)
    {
        if (hexGameObject.GetComponent<Hex>().ContainsEnvironmentalEffectType("Fire"))
        {


            DouseFire(hexGameObject);

        }
        else
        {
            base.Init(hexGameObject);
        }
    }

    void DouseFire(GameObject hexGameObject)
    {

        EnvironmentalEffect fireEffect = hexGameObject.GetComponent<Hex>().GetEnvironmentalEffectByTag("Fire");

        fireEffect.Kill();
        EnvironmentalEffectsManager.Instance.SpawnEnvironmentalEffect("Steam", hexGameObject);
    }
}
