using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class AbilityManager : MonoBehaviour {

public List<Ability> Abilities = new List<Ability>();
    public static AbilityManager Instance;

    void Start()
    {
        Instance = this;
    }


    //Finds an ability by name. Mostly used to help marry Agent up to their abilities
    public Ability FindAbilityByName(string name)
    {
        Ability returnAbility = null;

        foreach (Ability ability in Abilities)
        {
            if (ability.GetName() == name)
            {
                returnAbility = ability;
            }
        }
        //Ability not found
        if (returnAbility == null)
        {
            Debug.Log("Ability not found: " + name);
        }


        return returnAbility;
    }
}
