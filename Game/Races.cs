using UnityEngine;
using System.Collections;
using  System.Collections.Generic;

public class Races : MonoBehaviour {
    public  List<Race> RaceList = new List<Race>();
    public static Races Instance;

	// Use this for initialization
	void Start ()
	{
	    Instance = this;
	}

    public Race GetRaceByName(string raceName)
    {
        Race returnRace = null;
        Debug.Log(Races.Instance.RaceList.Count.ToString());

        foreach (Race race in Races.Instance.RaceList)
        {

            if (race.Name == raceName)
            {
                returnRace = race;
                Debug.Log("found race");
            }
        }

        return returnRace;
    }
}
