using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Map : MonoBehaviour {

	public GameObject hexPrefab;


    // Size of the map in terms of number of hex tiles
    // This is NOT representative of the amount of 
    // world space that we're going to take up.
    // (i.e. our tiles might be more or less than 1 Unity World Unit)
    public static int width = 10;
    public static int height = 10;
	float xOffset = 0.85f;
	float zOffset = 0.75f;
    public static GameObject[,] GoHex = new GameObject[width,height];
    public TileType[] TileType;
public List<UnitSpawnPoint> SpawnPoints = new List<UnitSpawnPoint>();
    public int NoOfPlayers = 2;
    public Texture2D MapData;
    public static Map Instance;


	// Use this for initialization
	void Start () {
	    {
	        Instance = this;
	    }
  

	}
    
private void ResizeArray(ref GameObject[,] original, int cols, int rows)

{

    //create a new 2 dimensional array with

    //the size we want
    GameObject[,] newArray = new GameObject[cols, rows];
    //copy the contents of the old array to the new one

    Array.Copy(original, newArray, original.Length);

    //set the original to the new array

    original = newArray;

}

    public void SetWidthAndHeight(int w, int h)
    {
        width = w;
        height = h;
        ResizeArray(ref GoHex,w,h);
        Debug.Log("width= "+ width.ToString() + " + height= " + height.ToString());
    }

    public void Init(int w, int h)
    {
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {

                float xPos = x * xOffset;

                // Are we on an odd row?
                if (y % 2 == 1)
                {
                    xPos += xOffset / 2f;
                }


                GameObject hex_go = (GameObject)Instantiate(hexPrefab, new Vector3(xPos * 2, 0, y * zOffset * 2), Quaternion.identity);
                GoHex[x, y] = hex_go;
                // Name the gameobject something sensible.
                hex_go.name = "Hex_" + x + "_" + y;

                // Make sure the hex is aware of its place on the map
                hex_go.GetComponent<Hex>().x = x;
                hex_go.GetComponent<Hex>().y = y;

                // For a cleaner hierachy, parent this hex to the map
                hex_go.transform.SetParent(this.transform);

             //   hex_go.isStatic = true;


            }
        }

      
        GameObject table = (GameObject)Instantiate(PrefabLibrary.Instance.TablePrefab, new Vector3(5, -1f, 10), Quaternion.identity);
      table.transform.localScale = new Vector3(30,1,30);
        
        
        SnapTileTypesToMapData(w,h);
        gameObject.GetComponent<Pathfinding>().Init(w,h);  

    }

    public void SnapTileTypesToMapData(int w, int h)
    {
        Color ColourType0 = new Color(1,0,0);
        Color ColourType1 = new Color(0, 1, 0);
        Color ColourType2 = new Color(0, 0, 1);


        for (int x = 0 ; x < w ; x++ )
        {
            for (int y = 0; y < h ; y++)
            {

                if (MapData.GetPixel(x,y) == ColourType0)
                {
                    GoHex[x, y].GetComponent<Hex>().ITileType = 0;
                }
                else
              
                if (MapData.GetPixel(x, y) == ColourType1)
                {
                    GoHex[x, y].GetComponent<Hex>().ITileType = 1;
                }
                else
               
                if (MapData.GetPixel(x, y) == ColourType2)
                {
                    GoHex[x, y].GetComponent<Hex>().ITileType = 2;
                }
                else
                {
                }
            }
        }
    }
    public void PushVisuals()
    {
        for (int n = 0; n < width; n++)
        {
            for (int i = 0; i < height ; i++)
            {
                GoHex[n, i].GetComponent<Hex>().PushVisuals();
            }
        }
    }



    public virtual void InitSpawnPoints()
    {
        
    }
	// Update is called once per frame
	void Update () {

	
	
	}


}
