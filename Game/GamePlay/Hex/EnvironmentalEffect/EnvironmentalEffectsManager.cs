using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentalEffectsManager : MonoBehaviour
{
   

    public GameObject GroundFirePrefab;
    public GameObject WeatherRainPrefab;
    public GameObject SteamPrefab;

    public static EnvironmentalEffectsManager Instance;

    public event TickHandler Tick;
    public delegate void TickHandler();

    void Start()
    {
        Instance = this;
    }



    public void SpawnEnvironmentalEffect(string Tag, GameObject goHex)
{
        if (goHex.GetComponent<Hex>().ContainsEnvironmentalEffectType(Tag))
        {

        }
        else
        {


            switch (Tag)
            {
                case "Fire":

                    GameObject goFire =
                        (GameObject) Instantiate(GroundFirePrefab, goHex.transform.position, Quaternion.identity);

                    goFire.GetComponent<GroundEnvironmentalEffectFire>().Init(goHex);
                    break;

                case "Steam":



                    GameObject goSteam =
                        (GameObject) Instantiate(SteamPrefab, goHex.transform.position, Quaternion.identity);

                    goSteam.GetComponent<SteamCloudEnvironmentalEffect>().Init(goHex);
                    break;

            }

        }

}

    void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (Tick!=null)
            {
                Tick();
            }
        }
    }
}
