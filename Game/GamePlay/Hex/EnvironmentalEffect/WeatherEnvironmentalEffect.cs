using UnityEngine;
using System.Collections;

public class WeatherEnvironmentalEffect : EnvironmentalEffect {

    public override void Init(GameObject hexGameObject)
    {

        base.Init(hexGameObject);
        gameObject.transform.position = new Vector3(hexGameObject.transform.position.x,
            hexGameObject.transform.position.y + 2, hexGameObject.transform.position.z);
    }
}
