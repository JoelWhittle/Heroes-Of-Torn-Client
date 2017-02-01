using System;
using UnityEngine;
using System.Collections;

public class EnvironmentalEffect : MonoBehaviour
{
    

    public string STag;
    public GameObject HexGameObject;
    public int Lifespan;




    public virtual void Init(GameObject hexGameObject)
    {
        HexGameObject = hexGameObject;
        HexGameObject.GetComponent<Hex>().EnvironmentalEffectsList.Add(this);
        SubscribeToManager();

    }
 

    public virtual  void Tick()
    {
        Lifespan--;
        if (Lifespan < 0)
        {
            Kill();
        }
    }

    public virtual  void Kill()
    {
        HexGameObject.GetComponent<Hex>().RemoveFromEnvironmentalEffectsList(this);
      EnvironmentalEffectsManager.Instance.Tick -= new EnvironmentalEffectsManager.TickHandler(Tick);


//        gameObject.SetActive(false);

DestroyObject(gameObject);
    }

    void SubscribeToManager()
    {

           EnvironmentalEffectsManager.Instance.Tick += new EnvironmentalEffectsManager.TickHandler(Tick);

    }

}
