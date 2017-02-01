using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GroundEnvironmentalEffectFire : GroundEnvironmentalEffect

{

    public override void Init(GameObject hexGameObject)
    {
    
        
        base.Init(hexGameObject);
        if (hexGameObject.GetComponent<Hex>().BBurned)
        {
            Kill();
        }

       if (hexGameObject.GetComponent<Hex>().ContainsEnvironmentalEffectType("Rain"))
       {
            hexGameObject.GetComponent<Hex>().GetEnvironmentalEffectByTag("Rain").Kill();
           EnvironmentalEffectsManager.Instance.SpawnEnvironmentalEffect("Steam",hexGameObject);
            Kill();
        }
        Lifespan = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>().TileType[hexGameObject.GetComponent<Hex>().ITileType].IFuel;

    }

    public override void Tick()
    {
        if (Lifespan > -1)
        {
            base.Tick();
            if (Lifespan > -1)

            {

                Propogate();
            }
        }
        else
        {
            Debug.Log("didnt tick");
        }
      
    
    }


    public override void Kill()
    {
       HexGameObject.GetComponent<Hex>().BBurned = true;
        base.Kill();
    }

    public void Propogate()
    {
        foreach (GameObject gameObjectNeighbour in HexGameObject.GetComponent<Hex>().GetNeighbours())
        {
            if (gameObjectNeighbour.GetComponent<Hex>().ContainsEnvironmentalEffectType("Fire"))
            {
                Debug.Log("gasdf");
            }
            else
            {
               if (Random.Range(0,100) <  GameObject.FindGameObjectWithTag("Map").GetComponent<Map>().TileType[gameObjectNeighbour.GetComponent<Hex>().ITileType].IBurnChance )

                {
                    EnvironmentalEffectsManager.Instance
                        .SpawnEnvironmentalEffect("Fire", gameObjectNeighbour);
                }
            }
        }

    }


  

}
