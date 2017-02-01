using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Attached to the _ClientUnitVault GameObject. Stores the logic for re instantiating units from the database
public class UnitParser : MonoBehaviour {




	//Takes in the Players unit information from the Database and create ingame units
	//from them
	public void CreatePlayersVaultUnits(string sParseData)
	{

		UnitCollection cUnitCollection = GameObject.Find ("Player").GetComponent<UnitCollection> ();
		GameObject goClientUnitVault = GameObject.Find ("_ClientUnitVault");


		string[] sUnits = sParseData.Split ("#".ToCharArray ());

		foreach (string sUnit in sUnits) {
						if (sUnit != "") {
								Unit cUnit = goClientUnitVault.AddComponent<Unit> ();
								cUnitCollection.AllUnits.Add (cUnit);

								string[] sUnitData = sUnit.Split (",".ToCharArray ());

                  
                            Debug.Log(sUnit);
   
								cUnit.sId = sUnitData [0];
                            	cUnit.sCatalogueID = sUnitData [1];
								cUnit.Attack = int.Parse (sUnitData [3]);
								cUnit.Accuracy = int.Parse (sUnitData [4]);
								cUnit.Dodge = int.Parse (sUnitData [5]);
                            //
                                cUnit.HitPoints = int.Parse (sUnitData [6]);
                            	cUnit.MovementSpeed = int.Parse (sUnitData [7]);
								cUnit.Magic = int.Parse (sUnitData [8]);
								cUnit.MagicResistance = int.Parse (sUnitData [9]);
								cUnit.FireResistance = int.Parse (sUnitData [10]);
								cUnit.PiercingResistance = int.Parse (sUnitData [11]);
								cUnit.SlashResistance = int.Parse (sUnitData [12]);
								cUnit.BludgeoningResistance = int.Parse (sUnitData [13]);



								cUnit.iExp = int.Parse (sUnitData [16]);
								cUnit.iLevel = int.Parse (sUnitData [17]);
								cUnit.Abilities = sUnitData [18];
							

				int iCatID = int.Parse(cUnit.sCatalogueID);
				string[] sCatalogueData = cUnitCollection.sUnitCatalogue[iCatID].Split(">".ToCharArray());


        

				cUnit.sName = sCatalogueData[1];
				cUnit.UnitType = sCatalogueData[2];
				cUnit.Faction = sCatalogueData[3];
				cUnit.Rarity = sCatalogueData[4];
				cUnit.Collection = sCatalogueData[5];
				cUnit.Race = sCatalogueData[6];
			 cUnit.DamageType = sCatalogueData[7];
				cUnit.MinRange = int.Parse(sCatalogueData[19]);
				cUnit.MaxRange = int.Parse(sCatalogueData[20]);
				cUnit.sArtist = sCatalogueData[22];
				cUnit.sFlavourText = sCatalogueData[23];





			
						}
				}
		}

	}
