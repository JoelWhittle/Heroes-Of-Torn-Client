using UnityEngine;
using System.Collections;

public class CreateAgent : MonoBehaviour
{

    public static CreateAgent Instance;

	// Use this for initialization
	void Start ()
	{
	    Instance = this;
	}

    public void InstantiateUnit(string dataStream)
    {
        Debug.Log("MY DATA STREAM:" + dataStream);
        string[] args =
        dataStream.Split(">".ToCharArray());

        Hex hex = GameObject.Find(args[25]).GetComponent<Hex>();

        GameObject agent = (GameObject) Instantiate(PrefabLibrary.Instance.UnitPrefabs[int.Parse(args[1])], hex.gameObject.transform.position,Quaternion.identity);
        PlayableCharacter pc = agent.GetComponent<PlayableCharacter>();
        hex.Occupier = agent;
        pc.ParentHex = hex.gameObject;
        pc.ID = int.Parse(args[0]);
        pc.CATID = int.Parse(args[1]);
        pc.Name = args[2];
        pc.UnitType = args[3];
        pc.Faction = args[4];
        pc.Rarity = args[5];
        pc.Collection = args[6];
        pc.Race = args[7];
        pc.DamageType = args[8];
        pc.BaseAttack = int.Parse(args[9]);
        pc.BaseAccuracy = int.Parse(args[10]);
        pc.BaseDodge = int.Parse(args[11]);
        pc.BaseHitPoints = int.Parse(args[12]);
        pc.BaseMagic = int.Parse(args[13]);

        pc.BaseMagicResistance = int.Parse(args[14]);
        pc.BaseFireResistance = int.Parse(args[15]);

        pc.BaseSlashResistance = int.Parse(args[16]);
        pc.BasePiercingResistance = int.Parse(args[17]);
        pc.BaseBludgeoningResistance = int.Parse(args[18]);
        pc.BaseMovementSpeed = int.Parse(args[19]);
        pc.BaseMinAttackRange = int.Parse(args[20]);
        pc.BaseMaxAttackRange = int.Parse(args[21]);
        pc.Level = int.Parse(args[22]);
        pc.Owner = args[23];

        //arrange abilities

       
       
        if (args[26] != "")
        {

            string[] abilityParse = args[26].Split("!".ToCharArray());

            foreach (string s in abilityParse)
            {
                if (s != "")
                {
                    pc.Abilities.Add(AbilityManager.Instance.FindAbilityByName(s));
                }
            }
        }

        //Finally add to the lists 
       GameManager.Instance.cTeams[int.Parse(args[24])].Agents.Add(pc); 



        GameManager.Instance.Agents.Add(pc.gameObject);


    }

    public string PrepareCreateAgentString(Unit unit , Hex hex)
    {
        string returnString = "";

        returnString = unit.sId + ">" +
                       unit.sCatalogueID + ">" +
                       unit.sName + ">" +
                       unit.UnitType + ">" +
                       unit.Faction + ">" +
                       unit.Rarity + ">" +
                       unit.Collection + ">" +
                       unit.Race + ">" +
                       unit.DamageType + ">" +
                       unit.GetBaseAttack().ToString() + ">" +
                       unit.GetBaseAccuracy().ToString() + ">" +
                       unit.GetBaseDodge().ToString() + ">" +
                       unit.GetBaseHitPoints().ToString() + ">" +
                       unit.GetBaseMagic().ToString() + ">" +
                       unit.GetBaseMagicResistance().ToString() + ">" +
                       unit.GetBaseFireResistance().ToString() + ">" +
                       unit.GetBaseSlashResistance().ToString() + ">" +
                       unit.GetBasePiercingResistance().ToString() + ">" +
                       unit.GetBaseBludgeoningResistance().ToString() + ">" +
                       unit.GetBaseMovementSpeed().ToString() + ">" +
                       unit.MinRange.ToString() + ">" +
                       unit.MaxRange.ToString() + ">" +
                       unit.iLevel.ToString().ToString() + ">" +
                       NetworkManager.Instance.cPlayer.sName + ">" +
                       GameManager.Instance.GetPlayersTeamID().ToString() + ">" +
                       hex.gameObject.name + ">" +
                       unit.Abilities;
        Debug.Log("ABILITY DEBUG, STREAM = " + unit.Abilities);


                           
        

        return returnString;


    }

    void Update()
    {
  
    }
}
