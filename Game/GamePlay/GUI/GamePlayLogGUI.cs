using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePlayLogGUI : MonoBehaviour
{
    public static GamePlayLogGUI Instance;
    
    public List<string> LogList = new List<string>();

    void Start()
    {
        Instance = this;

        AddToLogList("testing..");
        AddToLogList("..testing");
        AddToLogList("..1,2..");
        AddToLogList("3");




    }

    public void AddToLogList(string log)
    {
    LogList.Insert(0,log);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 1000, 20), LogList[2]);
        GUI.Label(new Rect(10, 30, 1000, 20), LogList[1]);
        GUI.Label(new Rect(10, 50, 1000, 20), LogList[0]);


    }


    public void MakeSenseOfAttackData(Agent agent, Agent enemyAgent, bool didWeHit, bool didTheyDie, int damage)
    {
        string message = agent.Owner + "'s " + agent.Name + " " ;

        if (didWeHit == false)
        {
            message = message + "missed " + enemyAgent.Owner + "'s " + enemyAgent.Name;
        }
        else
        {
            message = message + "hit " + enemyAgent.Owner + "'s " + enemyAgent.Name + " for " + damage.ToString() +
                      " damage ";
        }

        if (didTheyDie == false)
        {
            message = message + "they survived ";
            if (agent.IsBehindAgent(enemyAgent.gameObject))
            {
                message = message + "despite being backstabbed ";
                if (enemyAgent.IsFlanked())
                {
                    message = message + "and flanked ";
                }
            }
            else
            {
                if (enemyAgent.IsFlanked())
                {
                    message = message + "despite being flanked ";
                }  
            }
        }
        else
        {
            if (agent.IsBehindAgent(enemyAgent.gameObject))
            {
                message = message + " great backstab ";
                if (enemyAgent.IsFlanked())
                {
                    message = message + "and  flank ";
                }

            }
            if (enemyAgent.IsFlanked())
            {
                message = message + "great flank ";
            }
        }
        Debug.Log(message);
     AddToLogList(message);

       
    }
}
