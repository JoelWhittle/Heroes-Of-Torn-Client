using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitCollection : MonoBehaviour {

	public List<string> sUnitCatalogue = new List<string> ();
    public List<CatalogueUnit> UnitCatalogue = new List<CatalogueUnit>();
	public List<Deck> Decks = new List<Deck>();
	public List<Unit> AllUnits = new List<Unit>();
    public static UnitCollection Instance;

    void Start()
    {
        Instance = this;
    }

}
