using UnityEngine;
using System.Collections;

public class AxeHurl : OffensiveAbility
{
    public float AnimationTime;
    public AnimationCurve cAnimationCurve;

    public override IEnumerator Cast(Agent offensiveAgent, Agent targetAgent, int Damage , bool DidDie)
    {
        yield return
            StartCoroutine(offensiveAgent.LerpLookAtTarget(offensiveAgent.transform.rotation,
                targetAgent.transform.position));

        offensiveAgent.TriggerRandomAttackAnimation();

      GameObject axePrefab = (GameObject)Instantiate(Prefab, offensiveAgent.transform.position, Quaternion.identity);

   yield return StartCoroutine(Pathfinding.Instance.LerpObjectFromAToB(axePrefab, targetAgent.transform.position, AnimationTime,
            cAnimationCurve));

        Destroy(axePrefab);

        targetAgent.CurrentHitpoints = targetAgent.CurrentHitpoints - Damage;

        targetAgent.TriggerRandomDeathAnimation();
        GameManager.Instance.cTeams[GameManager.Instance.FindAgentsTeamID(targetAgent)].Agents.Remove(targetAgent);
    }

}
