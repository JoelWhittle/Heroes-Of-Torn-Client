using UnityEngine;
using System.Collections;

public class Buff : MonoBehaviour
{

    public int Duration;
    public string BuffedBy;
    public int AttackBuffModifier;
    public int AccuracyBuffModifier;
    public int DodgeBuffModifier;
    public int HitPointsBuffModifier;
    public int MagicBuffModifier;
    public int MagicResistanceBuffModifier;
    public int FireResistanceBuffModifier;
    public int SlashResistanceBuffModifier;
    public int PiercingResistanceBuffModifier;
    public int BludgeoningResistanceBuffModifier;

    public int MovementSpeedBuffModifier;

    public int MinAttackRangeBuffModifier;
    public int MaxAttackRangeBuffModifier;

    void Start()
    {
        gameObject.GetComponent<Agent>().CurrentBuffs.Add(this);
    }

    public void Remove()
    {
        gameObject.GetComponent<Agent>().CurrentBuffs.Remove(this);
        Destroy(this);
    }

    public void Tick()
    {
        Duration--;
        if (Duration == 0)
        {
            Remove();
            
        }
    }
}
