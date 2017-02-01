using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters;
using JetBrains.Annotations;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Agent : MonoBehaviour
{

    public int CATID;
    public string Owner;
    public string Name;
    public string Race;
    public string UnitType;
    public string Faction;
    public string Rarity;
    public string Collection;
    public int ID;
    public string DamageType;


    public int BaseAttack;
    public int BaseAccuracy;
    public int BaseDodge;
    public int BaseHitPoints;
    public int CurrentHitpoints;
    public int BaseMagic;
    public int BaseMagicResistance;
    public int BaseFireResistance;
    public int BaseSlashResistance;
    public int BasePiercingResistance;
    public int BaseBludgeoningResistance;

    public int BaseMovementSpeed;

    public int BaseMinAttackRange;
    public int BaseMaxAttackRange;

    public int Level;
    public List<Ability> Abilities = new List<Ability>();

    //Pathfinding
    public GameObject ParentHex;
    public bool IsMoving;

    //Animation
    public AnimationCurve MovementRotationCurve;
    public AnimationCurve MovementAnimationCurve;
    public float MovementAnimationLength;
    public float MovementRotationLength;
    public Animation AnimationContainer;
    public AnimationClip WalkingAnimationClip;
    public AnimationClip[] IdleAnimationClips;
    public AnimationClip[] AttackAnimationClips;
    public AnimationClip[] DeathAnimationClips;
    public AnimationClip[] GetHitAnimationClips;
    public AnimationClip[] BlockAnimationClips;

    //Audio
    public AudioSource AudioSourceContainer;
    public AudioClip[] WalkingAudioClips;
    public AudioClip[] AttackAudioClips;
    public AudioClip[] BattlecryAudioClips;
    public AudioClip[] GetHitAudioClips;
    public AudioClip[] BloodiedAudioClips;
    public AudioClip[] DeathAudioClips;
    public AudioClip[] BlockAudioClips;

    public HighlightableObject Highlighter;

    //game
    public List<Buff> CurrentBuffs = new List<Buff>();

    public int GetAttackBuffModifier()
    {
        int n = 0;
        if (CurrentBuffs.Count > 0)
        {
            foreach (Buff buff in CurrentBuffs)
            {
                n = n + buff.AttackBuffModifier;
            }
            return n;
        }
        else
        {
            return 0;
        }
    }
    public int GetAccuracyBuffModifier()
    {
        int n = 0;
        if (CurrentBuffs.Count > 0)
        {
            foreach (Buff buff in CurrentBuffs)
            {
                n = n + buff.AccuracyBuffModifier;
            }
            return n;
        }
        else
        {
            return 0;
        }
    }
    public int GetDodgeBuffModifier()
    {
        int n = 0;
        if (CurrentBuffs.Count > 0)
        {
            foreach (Buff buff in CurrentBuffs)
            {
                n = n + buff.DodgeBuffModifier;
            }
            return n;
        }
        else
        {
            return 0;
        }
    }
    public int GetHitPointsBuffModifier()
    {
        int n = 0;
        if (CurrentBuffs.Count > 0)
        {
            foreach (Buff buff in CurrentBuffs)
            {
                n = n + buff.HitPointsBuffModifier;
            }
            return n;
        }
        else
        {
            return 0;
        }
    }
    public int GetMagicBuffModifier()
    {
        int n = 0;
        if (CurrentBuffs.Count > 0)
        {
            foreach (Buff buff in CurrentBuffs)
            {
                n = n + buff.MagicBuffModifier;
            }
            return n;
        }
        else
        {
            return 0;
        }
    }
    public int GetMagicResistanceBuffModifier()
    {
        int n = 0;
        if (CurrentBuffs.Count > 0)
        {
            foreach (Buff buff in CurrentBuffs)
            {
                n = n + buff.MagicResistanceBuffModifier;
            }
            return n;
        }
        else
        {
            return 0;
        }
    }
    public int GetSlashResistanceBuffModifier()
    {
        int n = 0;
        if (CurrentBuffs.Count > 0)
        {
            foreach (Buff buff in CurrentBuffs)
            {
                n = n + buff.SlashResistanceBuffModifier;
            }
            return n;
        }
        else
        {
            return 0;
        }
    }
    public int GetPiercingResistanceBuffModifier()
    {
        int n = 0;
        if (CurrentBuffs.Count > 0)
        {
            foreach (Buff buff in CurrentBuffs)
            {
                n = n + buff.PiercingResistanceBuffModifier;
            }
            return n;
        }
        else
        {
            return 0;
        }
    }
    public int GetBludgeoningResistanceBuffModifier()
    {
        int n = 0;
        if (CurrentBuffs.Count > 0)
        {
            foreach (Buff buff in CurrentBuffs)
            {
                n = n + buff.BludgeoningResistanceBuffModifier;
            }
            return n;
        }
        else
        {
            return 0;
        }
    }
    public int GetFireResistanceBuffModifier()
    {
        int n = 0;
        if (CurrentBuffs.Count > 0)
        {
            foreach (Buff buff in CurrentBuffs)
            {
                n = n + buff.FireResistanceBuffModifier;
            }
            return n;
        }
        else
        {
            return 0;
        }
    }
    public int GetMovementSpeedBuffModifier()
    {
        int n = 0;
        if (CurrentBuffs.Count > 0)
        {
            foreach (Buff buff in CurrentBuffs)
            {
                n = n + buff.MovementSpeedBuffModifier;
            }
            return n;
        }
        else
        {
            return 0;
        }
    }
    public int GetMinAttackRangeBuffModifier()
    {
        int n = 0;
        if (CurrentBuffs.Count > 0)
        {
            foreach (Buff buff in CurrentBuffs)
            {
                n = n + buff.MinAttackRangeBuffModifier;
            }
            return n;
        }
        else
        {
            return 0;
        }
    }
    public int GetMaxAttackRangeBuffModifier()
    {
        int n = 0;
        if (CurrentBuffs.Count > 0)
        {
            foreach (Buff buff in CurrentBuffs)
            {
                n = n + buff.MaxAttackRangeBuffModifier;
            }
            return n;
        }
        else
        {
            return 0;
        }
    }

    public bool IsConscious()
    {
        if (CurrentHitpoints > 0)
        {
          return true;  
        }
        else
        {
            return false;
        }
    }

    public void Init()
    {
        RegisterAnimationClips();
        RegisterAudioClips();
        Highlighter = gameObject.GetComponent<HighlightableObject>();
        CurrentHitpoints = BaseHitPoints;


    }



    public int GetAttack()
    {
        return BaseAttack  + GetAttackBuffModifier();

    }
    public int GetAccuracy()
    {
        return BaseAccuracy + GetAccuracyBuffModifier();

    }
    public int GetDodge()
    {
        return BaseDodge + GetDodgeBuffModifier();

    }
    public int GetHitPoints()
    {
        return CurrentHitpoints;

    }
    public int GetMagic()
    {
        return BaseMagic + GetMagicBuffModifier();

    }
    public int GetMagicResistance()
    {
        return BaseMagicResistance + GetMagicResistanceBuffModifier();

    }

    public int GetFireResistance()
    {
        return BaseFireResistance + GetFireResistanceBuffModifier();
    }

    public int GetPiercingResistance()
    {
        return BasePiercingResistance + GetPiercingResistanceBuffModifier();
    }

    public int GetSlashResistance()
    {
        return BaseSlashResistance + GetSlashResistanceBuffModifier();
    }

    public int GetBludgeoningResistance()
    {
        return BaseBludgeoningResistance + GetBludgeoningResistanceBuffModifier();
    }


    public int GetMovementSpeed()
    {
        return BaseMovementSpeed + GetMovementSpeedBuffModifier();

    }

    public int GetMinAttackRange()
    {
        return BaseMinAttackRange + GetMinAttackRangeBuffModifier();
    }

    public int GetMaxAttackRange()
    {
        return BaseMaxAttackRange + GetMaxAttackRangeBuffModifier();
    }

    public void RegisterAudioClips()
    {
        AudioSourceContainer = gameObject.GetComponent<AudioSource>();
        AudioSourceContainer.volume = MusicManager.Instance.CharacterAudioVolume;

    }

    public void RegisterAnimationClips()
    {

        AnimationContainer = gameObject.GetComponent<Animation>();

        foreach (AnimationClip acClip in IdleAnimationClips)
        {
            AnimationContainer.AddClip(acClip, acClip.name);
        }
        foreach (AnimationClip acClip in AttackAnimationClips)
        {
            AnimationContainer.AddClip(acClip, acClip.name);
        }
        foreach (AnimationClip acClip in DeathAnimationClips)
        {
            AnimationContainer.AddClip(acClip, acClip.name);
        }
        foreach (AnimationClip acClip in GetHitAnimationClips)
        {
            AnimationContainer.AddClip(acClip, acClip.name);
        }
        foreach (AnimationClip acClip in BlockAnimationClips)
        {
            AnimationContainer.AddClip(acClip, acClip.name);
        }
    }


    public IEnumerator LerpLookAtTarget(Quaternion qStartPosition, Vector3 vTargetPosition)
    {
        Quaternion qDir = Quaternion.LookRotation(vTargetPosition - gameObject.transform.position);
        Vector3 vDir = qDir.eulerAngles;
        float timer = 0f;

        if (Mathf.Abs(gameObject.transform.rotation.eulerAngles.y - vDir.y) < 5)
        {
            Debug.Log("facing right way");
        }
        else
        {


            while (timer <= MovementRotationLength)
            {

                Vector3 vNewRot = Vector3.Lerp(qStartPosition.eulerAngles, vDir,
                    MovementRotationCurve.Evaluate(timer/MovementRotationLength));
                Quaternion qNewRot =
                    Quaternion.Euler(new Vector3(gameObject.transform.rotation.x, vNewRot.y,
                        gameObject.transform.rotation.y));
                gameObject.transform.rotation = qNewRot;
                timer += Time.deltaTime;
                yield return null;
            }
        }

    }

    public IEnumerator LerpToPosition(Vector3 vStartPosition, Vector3 vTargetPosition)
    {

        yield return StartCoroutine(LerpLookAtTarget(gameObject.transform.rotation, vTargetPosition));
        float timer = 0.0f;
        
        while (timer <= MovementAnimationLength)
        {
            transform.position = Vector3.Lerp(vStartPosition, vTargetPosition,
                MovementAnimationCurve.Evaluate(timer/MovementAnimationLength));
            timer += Time.deltaTime;
            yield return null;

        }

    }

    public IEnumerator LerpDownPath(List<GameObject> GoPath)
    {
        List<GameObject> tmp = new List<GameObject>();

        foreach (GameObject go in GoPath)
        {
            if (go != null)
            {
                tmp.Add(go);
            }
        }
        GoPath = tmp;
        Debug.Log("reached lerp down path");
        IsMoving = true;

        Vector3[] vPath = new Vector3[GoPath.Count];

        for (int n = 0; n < GoPath.Count; n++)
        {
            if (GoPath[n]!=null)
            {


                vPath[n] = GoPath[n].gameObject.transform.position;

            }
        }
        TriggerRunningAnimation();
        //Tell parent were moving 
        ParentHex.GetComponent<Hex>().SetOccupier(null);
        //Tell ourselves were moving
        ParentHex = null;

        //Move
        int iCurIndex = 0;
        for (int x = 0; x < vPath.Length ; x++)
        {
            
            yield return StartCoroutine(LerpToPosition(gameObject.transform.position,vPath[iCurIndex]));
            iCurIndex++;

            //Set ParentHex
          ParentHex = Map.GoHex[GoPath[x].GetComponent<Hex>().x, GoPath[x].GetComponent<Hex>().y];
            //Tell ParentHex we occupy it
          ParentHex.GetComponent<Hex>().SetOccupier(this.gameObject);
            if (x != 0)
            {
                //Tell temp parent we are leaving
                Map.GoHex[GoPath[x - 1].GetComponent<Hex>().x, GoPath[x - 1].GetComponent<Hex>().y].GetComponent<Hex>()
                    .SetOccupier(null);
            }
            if (ParentHex != null)
            {
                Debug.Log(ParentHex.name);
            }
        }
        TriggerRandomIdleAnimation();
        IsMoving = false;
        yield return 0;
    }

    public void TriggerRunningAnimation()
    {

        AnimationContainer.clip = WalkingAnimationClip;
        AnimationContainer.Play();
    }

    public void TriggerRandomIdleAnimation()
    {
        AnimationContainer.clip = IdleAnimationClips[Random.Range(0,IdleAnimationClips.Length)];
        AnimationContainer.Play();

    }
    public void TriggerRandomAttackAnimation()
    {

        AnimationContainer.clip = AttackAnimationClips[Random.Range(0, AttackAnimationClips.Length)];
        AnimationContainer.Play();

        if (AttackAudioClips.Length > 0)
        {
            AudioSourceContainer.clip = AttackAudioClips[Random.Range(0, AttackAudioClips.Length)];
            AudioSourceContainer.Play();

        }

    }
    public void TriggerRandomDeathAnimation()
    {
        AnimationContainer.clip = DeathAnimationClips[Random.Range(0, DeathAnimationClips.Length)];
        AnimationContainer.Play();


        if (DeathAudioClips.Length > 0)
        {
            AudioSourceContainer.clip = DeathAudioClips[Random.Range(0, DeathAudioClips.Length)];
            AudioSourceContainer.Play();

        }

    }
    public void TriggerRandomGetHitAnimation()
    {
        AnimationContainer.clip = GetHitAnimationClips[Random.Range(0, GetHitAnimationClips.Length)];
        AnimationContainer.Play();



        if (GetHitAudioClips.Length > 0)
        {
            AudioSourceContainer.clip = GetHitAudioClips[Random.Range(0, GetHitAudioClips.Length)];
            AudioSourceContainer.Play();

        }
    }
    public void TriggerRandomBlockAnimation()
    {
        AnimationContainer.clip = BlockAnimationClips[Random.Range(0, BlockAnimationClips.Length)];
        AnimationContainer.Play();


        if (BlockAudioClips.Length > 0)
        {
            AudioSourceContainer.clip = BlockAudioClips[Random.Range(0, BlockAudioClips.Length)];
            AudioSourceContainer.Play();

        }

    }

    //Bool to see if we are flanked
    public bool IsFlanked()
    {
        int n = 0;
        foreach (GameObject enemy in GetEnemies())
        {
            Agent enemyAgent = enemy.GetComponent<Agent>();
            if (enemyAgent.GetValidAttackableTargets().Contains(gameObject))
            {
                n++;
            }

        }
        if (n > 1)
        {
            return true;
            
        }
        else
        {
            return false;
        }
    }

    //Cycles through all enemies and checks to make sure they are in range and also not obstructed
    public List<GameObject> GetValidAttackableTargets()
    {
        List<GameObject> returnList = new List<GameObject>();

        foreach (GameObject enemy in GetEnemies())
        {
            Debug.Log( enemy.GetComponent<Agent>().ID.ToString() + ":" + GetManhattanDistance(enemy).ToString());
            if (GetManhattanDistance(enemy) >= GetMinAttackRange() && GetManhattanDistance(enemy) <= GetMaxAttackRange())
            {
              returnList.Add(enemy);  
            }
        }
        
        return returnList;
    }

    //Gets the manhattan distance between this agent and an enemy 
    public int GetManhattanDistance(GameObject enemyGameObject)
    {
        return Pathfinding.Instance.GetDistanceAsCrowFlys(ParentHex, enemyGameObject.GetComponent<Agent>().ParentHex);
    }

    //Returns a list of all enemies by cycling through all teams and adding all agents that arent in our team
    public List<GameObject> GetEnemies()
    {
        List<GameObject> returnList = new List<GameObject>();

        foreach (Team team in GameManager.Instance.cTeams)
        {
            if (team.sControllingPlayerName != Player.Instance.sName)
            {
                foreach (Agent agent in team.Agents)
                {
                    returnList.Add(agent.gameObject);
                }  
            }
        }
        return returnList;

    }

    //check to see if this agent is behind another object, usually another agent, ie a backstab attack
    public bool IsBehindAgent(GameObject target)
    {
        Vector3 directionToTarget = target.transform.position - transform.position;
        float angle = Vector3.Angle(target.transform.forward, directionToTarget);
        if (Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270)
        {
            return false;
        }
        else
        {


            if (Mathf.Abs(angle) < 90 || Mathf.Abs(angle) > 270)
            {
                return true;
            }
            else
            {
                return true;

            }
        }

    }
}
