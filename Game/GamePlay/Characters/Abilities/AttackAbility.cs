using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using Random = UnityEngine.Random;
using UnityEngine;


public class AttackAbility : Ability {


    public override IEnumerator Cast(Agent offensiveAgent, Agent targetAgent, bool DidHit, bool DidDie, int Damage)
    {
        StartCoroutine(Attack(offensiveAgent, targetAgent, DidHit, DidDie, Damage));
        yield return null;
    }

    public override void CastRequest(Agent offensiveAgent, Agent targetAgent)
    {
        Debug.Log("Reached CastRequest()");
       CalculateAttackResult(offensiveAgent, targetAgent);
    }


    //Calculate the results of an attack and asks ExchangeGameDataToSendIt
    public void CalculateAttackResult(Agent curAgent, Agent enemyAgent)
    {
        Debug.Log("Reached CalculateAttacResult()");

        string result = "";
        int dammage = 0;

        //First get the ID of the attacker and the victim
        result = result + curAgent.ID.ToString() + ">" + enemyAgent.ID.ToString();

        //Then calculate wether the attack hit or not
        if (Random.Range(0, 100 + enemyAgent.GetDodge()) < curAgent.GetAccuracy() ||curAgent.IsBehindAgent(enemyAgent.gameObject))
        {
            //we hit
            result = result + ">hit";

            int percent = 0;
            switch ( curAgent.DamageType)
            {
                case "Slash":
                    percent = Convert.ToInt32(( curAgent.GetAttack() / 100) * enemyAgent.GetSlashResistance());
                    break;
                case "Piercing":
                    percent = Convert.ToInt32((curAgent.GetAttack() / 100) * enemyAgent.GetPiercingResistance());
                    break;
                case "Bludgeoning":
                    percent = Convert.ToInt32((curAgent.GetAttack() / 100) * enemyAgent.GetBludgeoningResistance());
                    break;

            }
            int baseDamage =curAgent.GetAttack() - percent;
            dammage = baseDamage;

            //check if enemy is flanked
            if (enemyAgent.IsFlanked())
            {
                dammage = dammage + (baseDamage / 2);
            }

            //check for back stab
            if ( curAgent.IsBehindAgent(enemyAgent.gameObject))
            {
                dammage = dammage + (baseDamage / 2);

            }

        }
        else
        {
            // we missed
            result = result + ">miss";
        }
        //Add dammage to result and check for death, add that to result to
        result = result + ">" + dammage.ToString();
        if (enemyAgent.GetHitPoints() <= dammage)
        {
            //death
            result = result + ">true";
        }
        else
        {

            result = result + ">false";

        }
        StartCoroutine(ExchangeGameData.Instance.SendAttackRequest(result));
    }

    //Attack an enemy and deal damage based on results from the RoomLog
    public IEnumerator Attack(Agent curAgent, Agent enemyAgent, bool didWeHit, bool didTheyDie, int damage)
    {
        //Update the room log
        GamePlayLogGUI.Instance.MakeSenseOfAttackData(curAgent, enemyAgent, didWeHit, didTheyDie, damage);

        yield return StartCoroutine(curAgent.LerpLookAtTarget(gameObject.transform.rotation, enemyAgent.gameObject.transform.position));

    curAgent.TriggerRandomAttackAnimation();

        if (didWeHit == true)
        {
            enemyAgent.TriggerRandomGetHitAnimation();
            enemyAgent.CurrentHitpoints = enemyAgent.CurrentHitpoints - damage;
            if (didTheyDie == true)
            {
                //They died to play their death animation and remove them from their team list

                enemyAgent.TriggerRandomDeathAnimation();
                GameManager.Instance.cTeams[GameManager.Instance.FindAgentsTeamID(enemyAgent)].Agents.Remove(enemyAgent);
            }
        }
        else
        {
            enemyAgent.TriggerRandomBlockAnimation();
        }



    }

}
