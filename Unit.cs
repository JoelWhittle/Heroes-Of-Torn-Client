using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	public string sName;
	public string UnitType;
	public string Faction;
	public string Rarity;
	public string Collection;
	public string Race;
    public Race RaceClass;
    public string DamageType;
	public int Attack;
	public int Accuracy;
    public int Dodge;
    public int HitPoints;
    public int Magic;
    public int MagicResistance;
    public int FireResistance;
    public int SlashResistance;
    public int PiercingResistance;
    public int BludgeoningResistance;

	public int MovementSpeed;
	public int MinRange;
	public int MaxRange;
	public string sArtist;
	public string sFlavourText;
	public string sCatalogueID;
	public string  Abilities;

	public string sId;
	public int iExp;
	public int iLevel;


    void Start()
    {
    }

    public Race GetRaceClass()
    {
               RaceClass = Races.Instance.GetRaceByName(Race);
        return RaceClass;
    }
    public int GetBaseAttack()
    {
        return Attack + GetRaceClass().Attack + UnitCollection.Instance.UnitCatalogue[int.Parse(sCatalogueID)].Attack ;
    }
    public int GetBaseAccuracy()
    {
        return Accuracy + GetRaceClass().Accuracy + UnitCollection.Instance.UnitCatalogue[int.Parse(sCatalogueID)].Accuracy;
    }
    public int GetBaseDodge()
    {
        return Dodge + GetRaceClass().Dodge + UnitCollection.Instance.UnitCatalogue[int.Parse(sCatalogueID)].Dodge;
    }

    public int GetBaseHitPoints()
    {
        return HitPoints + GetRaceClass().HitPoints + UnitCollection.Instance.UnitCatalogue[int.Parse(sCatalogueID)].HitPoints;
    }

    public int GetBaseMagic()
    {
        return Magic + GetRaceClass().Magic + UnitCollection.Instance.UnitCatalogue[int.Parse(sCatalogueID)].Magic;
    }
    public int GetBaseMagicResistance()
    {
        return MagicResistance + GetRaceClass().MagicResistance + UnitCollection.Instance.UnitCatalogue[int.Parse(sCatalogueID)].MagicResistance;
    }
    public int GetBaseFireResistance()
    {
        return FireResistance + GetRaceClass().FireResistance + UnitCollection.Instance.UnitCatalogue[int.Parse(sCatalogueID)].FireResistance;
    }
    public int GetBaseSlashResistance()
    {
        return SlashResistance + GetRaceClass().SlashResistance + UnitCollection.Instance.UnitCatalogue[int.Parse(sCatalogueID)].SlashResistance;
    }
    public int GetBasePiercingResistance()
    {
        return PiercingResistance + GetRaceClass().PiercingResistance + UnitCollection.Instance.UnitCatalogue[int.Parse(sCatalogueID)].PiercingResistance;
    }
    public int GetBaseBludgeoningResistance()
    {
        return BludgeoningResistance + GetRaceClass().BludgeoningResistance + UnitCollection.Instance.UnitCatalogue[int.Parse(sCatalogueID)].BludgeoningResistance;
    }
    public int GetBaseMovementSpeed()
    {
        return MovementSpeed + GetRaceClass().MovementSpeed + UnitCollection.Instance.UnitCatalogue[int.Parse(sCatalogueID)].MovementSpeed;
    }


    

}
