using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using UnityEngine;
using System.Collections;

public  class TombOfGrhimDhiggin : Map
{

    public   int Width;
   public   int Height;
	// Use this for initialization
	void Start ()
	{

       base.SetWidthAndHeight(Width,Height); 
	base.Init(Width,Height);
        




        base.PushVisuals();

	    InitSpawnPoints();
	}

    public override void InitSpawnPoints()
    {
        NoOfPlayers = 2;
        for (int x = 0 ; x < NoOfPlayers; x++)
        {
            for (int y = 0 ; y < 6 ; y++)
            {
                UnitSpawnPoint sp = gameObject.AddComponent<UnitSpawnPoint>();
                sp.iPlayerTeamId = x;

                if (x == 0)
                {
                    sp.ParentHex = Map.GoHex[y, 0].GetComponent<Hex>();
                }
                if (x == 1)
                {
                    sp.ParentHex = Map.GoHex[y, 6].GetComponent<Hex>();
                }

                SpawnPoints.Add(sp);

            }
        }
    }


    // Update is called once per frame
	void Update () {
     
	}
}
