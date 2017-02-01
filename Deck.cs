using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {

	public string sName;
	public Unit Hero;
	public string sFaction;
	public List<Unit> Units = new List<Unit>();



	public string sDeckDataParse()
	{
		SendRecievePlayerData cSendRecievePlayerData = GameObject.Find ("_NetworkManager").GetComponent<SendRecievePlayerData>();
		string sep1 = "*";
		string sep2 = "(";

		string sParse = sName + sep2 + cSendRecievePlayerData.sPHPReady(Hero.sName) + sep2 + cSendRecievePlayerData.sPHPReady(sFaction) + sep2;

		string sUnitParse = "";

		int iUnitIndex = 0;
		foreach(Unit cUnit in Units)
		{
			sUnitParse = sUnitParse + cUnit.sId;
			if(iUnitIndex != Units.Count)
			{
				sUnitParse = sUnitParse + sep1;
			}
		}

		sParse = sParse + sUnitParse;
		return sParse;
	}

}
