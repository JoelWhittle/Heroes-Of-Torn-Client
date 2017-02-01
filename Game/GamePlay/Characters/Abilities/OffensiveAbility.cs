using UnityEngine;
using System.Collections;

public class OffensiveAbility : Ability
{

    public int Damage;
    public _DamageType DamageType;
    public GameObject Prefab;

    public enum _DamageType

{
    Slashing,
        Piercing,
        Bludgeoning,
        Fire
}
	
}
