using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

public class Hex : MonoBehaviour {

	// Our coordinates in the map array
	public int x;
	public int y;
    public HighlightableObject h;
    public Map CMap;
    public GameObject Occupier;

    public GameObject PhysicalModel;

    public int ITileType;
    public bool BBurned = false;

    public List<EnvironmentalEffect>  EnvironmentalEffectsList = new List<EnvironmentalEffect>();

    void Start()
    {

        h = gameObject.GetComponent<HighlightableObject>();
        CMap = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
    }

    public float GetMovementCost()
    {

        return GameObject.FindGameObjectWithTag("Map").GetComponent<Map>().TileType[ITileType].FMovementCost;
    }
    public bool IsWalkable()
    {
        if (Occupier == null)
        {
            return GameObject.FindGameObjectWithTag("Map").GetComponent<Map>().TileType[ITileType].BIsWalkable;
        }
        else
        {
            return false;
        }
    }
    public GameObject GetLeftNeighbour()
    {

        if (x > 0)
        {
            return Map.GoHex[x - 1,y];
        }
        else
        {
            return null;
        }

    }
    public GameObject GetRightNeighbour()
    {

        if (x < Map.width -1)
        {
            return Map.GoHex[x + 1, y];
        }
        else
        {
            return null;
        }

    }

    public GameObject GetUpperLeftNeighbour()
    {
        if (y < Map.height -1)
        {
            if (y%2 == 0)
            {
                if (x > 0)
                {
                    return Map.GoHex[x - 1, y + 1];
                }
                else
                {
                    return null;
                }

            }
            else
            {

                return Map.GoHex[x, y + 1];



            }
        }
        else
        {
            return null;
        }


    }

    public GameObject GetUpperRightNeighbour()
    {
        if (y < Map.height - 1)
        {
            if (y%2 == 0)
            {
                
                    return Map.GoHex[x, y + 1];
                
             

            }
            else
            {
                if ( x < Map.width -1 )
                {
                    return Map.GoHex[x + 1, y + 1];
                }
            else
                {
                    return null;
                    
                }



            }
        }
        else
        {
            return null;
        }
        

    }

    public GameObject GetLowerLeftNeighbour()
    {
        if (y > 0)
        {
            if (y % 2 == 0)
            {
                if (x > 0)
                {
                    return Map.GoHex[x - 1, y - 1];
                }
                else
                {
                    return null;
                }

            }
            else
            {

                return Map.GoHex[x, y - 1];



            }
        }
        else
        {
            return null;
        }


    }

    public GameObject GetLowerRightNeighbour()
    {
        if (y > 0)
        {
            if (y % 2 == 0)
            {
              
                    return Map.GoHex[x, y - 1];
            

            }
            else
            {
                if (x < Map.width -1)
                {
                    return Map.GoHex[x + 1, y - 1];
                }
                else
                {
                    return null;
                }


            }
        }
        else
        {
            return null;
        }


    }
    public List<GameObject> GetNeighbours()
    {
        	List<GameObject> goNeighbours  = new List<GameObject>();

        if (GetLeftNeighbour() != null)
        {
            GameObject left = GetLeftNeighbour();
       goNeighbours.Add(left);
            
        }
        // Right neighbour is also easy to find.
        if (GetRightNeighbour() != null)
        {
            GameObject right = GetRightNeighbour();
            goNeighbours.Add(right);
        }

        if (GetUpperLeftNeighbour() != null)
        {
            GameObject upperleft = GetUpperLeftNeighbour();
            goNeighbours.Add(upperleft);

        }
        if (GetUpperRightNeighbour() != null)
        {
            GameObject upperright = GetUpperRightNeighbour();
            goNeighbours.Add(upperright);
        }
        if (GetLowerLeftNeighbour() != null)
        {
            GameObject lowerleft = GetLowerLeftNeighbour();
            goNeighbours.Add(lowerleft);
        }
        if (GetLowerRightNeighbour() != null)
        {
            GameObject lowerright = GetLowerRightNeighbour();
            goNeighbours.Add(GetLowerRightNeighbour());
        }

        return goNeighbours;

    }

    public int[] GetGraphPosition()
    {
        int[] tmp = new int[2];
        tmp[0]= x;
        tmp[1] = y;
        return tmp;


    }



    public void SetOccupier(GameObject occupier)
    {
        Occupier = occupier;
    }

    public void PushVisuals()
    {
        CMap = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();

        TileType tt = CMap.TileType[ITileType];

        for (int i = 0; i < PhysicalModel.GetComponent<MeshRenderer>().materials.Length; i++)
        {
            PhysicalModel.GetComponent<MeshRenderer>().materials[i].SetTexture("_MainTex", tt.Texture);
          //    PhysicalModel.GetComponent<MeshRenderer>().materials[i].mainTe
        }
    }

    public void RemoveFromEnvironmentalEffectsList(EnvironmentalEffect environmentalEffect)
    {
        EnvironmentalEffectsList.Remove(environmentalEffect);
    }

    public bool ContainsEnvironmentalEffectType(string sType)
    {
        bool bFound = false;
        foreach (EnvironmentalEffect environmentalEffect in EnvironmentalEffectsList)
        {
            if (environmentalEffect != null)
            {
                if (environmentalEffect.STag == sType)
                {
                    bFound = true;


                }
            }
        }

            return bFound;

        
    }

    public EnvironmentalEffect GetEnvironmentalEffectByTag(string sTag)
    {
        EnvironmentalEffect returnEffect= null;


        foreach (EnvironmentalEffect environmentalEffect in EnvironmentalEffectsList)
        {
            if (environmentalEffect != null)
            {
                if (environmentalEffect.STag == sTag)
                {
                    returnEffect = environmentalEffect;
                    


                }
            }
        }

        return returnEffect;

    }
   
}
