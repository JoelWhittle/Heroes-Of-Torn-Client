using UnityEngine;
using System.Collections;

public class Ability : MonoBehaviour
{


    public string Name;
    public int MinRange;
    public int MaxRange;
    public _TargetType TargetType ;
    public string Description;
    public Sprite Icon;


    public string GetName()
    {
        return Name;

    }

    public virtual IEnumerator Cast(Agent offensiveAgent, Agent targetAgent, int Damage, bool DidDie)
    {
        Debug.Log("NotImplementedYet");
        yield return null;
    }

    public virtual IEnumerator Cast(Agent offensiveAgent, Agent targetAgent, bool DidHit, bool DidDie, int Damage)
    {
        Debug.Log("NotImplementedYet");
        yield return null;
    }

    public virtual void CastRequest(Agent offensiveAgent, Agent targetAgent)
    {

    }


 

    public enum _TargetType
    {
        Hex,
        Agent,
        Self
    }
    

}
