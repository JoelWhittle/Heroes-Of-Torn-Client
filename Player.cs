using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public static Player Instance;
	public string sName;
	public bool bLoggedIn;
	public int iRank;
	public int iGold;
	public int iDust;
	public bool bInGame;

    public int SelectedDeckIndex;

	public UnitCollection cUnitCollection;

	void Start()
	{
	    Instance = this;
		cUnitCollection = gameObject.GetComponent<UnitCollection> ();
	}



}
