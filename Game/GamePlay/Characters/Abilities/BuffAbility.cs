using UnityEngine;
using System.Collections;

public class BuffAbility : Ability
{
    public _EffectableStats[] EffectableStats;
    public int[] Strengths;
    public int Duration;


    public override IEnumerator Cast(Agent offensiveAgent, Agent targetAgent, int Damage, bool DidDie)
    {
        Buff buff = targetAgent.gameObject.AddComponent<Buff>();
        buff.BuffedBy = Name;
        int n = 0;
        foreach (_EffectableStats stat in EffectableStats)
        {
            switch (stat)
            {
                case (_EffectableStats.Attack):

                    buff.AttackBuffModifier = Strengths[n];
                    break;
                case (_EffectableStats.Accuracy):

                    buff.AccuracyBuffModifier = Strengths[n];
                    break;
                case (_EffectableStats.Dodge):

                    buff.DodgeBuffModifier = Strengths[n];
                    break;
                case (_EffectableStats.HitPoints):

                    buff.HitPointsBuffModifier = Strengths[n];
                    break;
                case (_EffectableStats.Magic):

                    buff.MagicBuffModifier = Strengths[n];
                    break;
                case (_EffectableStats.MagicResistance):

                    buff.MagicResistanceBuffModifier = Strengths[n];
                    break;
                case (_EffectableStats.FireResistance):

                    buff.FireResistanceBuffModifier = Strengths[n];
                    break;
                case (_EffectableStats.SlashResistance):

                    buff.SlashResistanceBuffModifier = Strengths[n];
                    break;
                case (_EffectableStats.PiercingResistance):

                    buff.PiercingResistanceBuffModifier = Strengths[n];
                    break;
                case (_EffectableStats.BludgeoningResistance):

                    buff.BludgeoningResistanceBuffModifier = Strengths[n];
                    break;
                case (_EffectableStats.BaseMovementSpeed):

                    buff.MovementSpeedBuffModifier = Strengths[n];
                    break;
                case (_EffectableStats.MinAttackRange):

                    buff.MinAttackRangeBuffModifier = Strengths[n];
                    break;
                case (_EffectableStats.MaxAttackRange):

                    buff.MaxAttackRangeBuffModifier = Strengths[n];
                    break;
           
            }
            n++;
        }
        yield return null;
    }

    public enum _EffectableStats
    {
        
Attack,
  Accuracy,
 Dodge,
HitPoints,
Magic,
MagicResistance,
FireResistance,
SlashResistance,
PiercingResistance,
BludgeoningResistance,
 BaseMovementSpeed,

MinAttackRange,
 MaxAttackRange
        
    }

}
