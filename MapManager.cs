using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

	public List<MapMetaData>  MapMetaDatas = new List<MapMetaData>();
    public static MapManager Instance;

    void Start()
    {
        Instance = this;
    }

//tmp
    public GameObject[] MapPrefab;
}
