using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking.NetworkSystem;
using UnityStandardAssets.Utility;

public class InitGamePlayState : IState
{

    private bool bTriggeredSpawnUnit = false;

    public string Name()
    {
        return "InitGamePlayState";

    }

    public void Enter()
    {
        //Find my Team Id and my selected deck and instantiate agents accordingly
        Debug.Log("entering init gameplay");

        //change music to battle music !!!!

        MusicManager.Instance.ChangeAudioClip(MusicManager.Instance.Battle);
    }

    public void Exit()
    {

        GameManager.Instance.cCurrentTeam = GameManager.Instance.cTeams[0];
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothFollow>().target =
            GameManager.Instance.cCurrentTeam.Agents[0].transform;
        GameManager.Instance.CurrentAgent = GameManager.Instance.cCurrentTeam.Agents[0];
    }

    public void UpdateState()
    {
                    bool ready = true;

        if (GameManager.Instance.GetPlayersTeamID() == 0 && bTriggeredSpawnUnit == false)
        {
            // Debug.Log("chicken dinner");
            bTriggeredSpawnUnit = true;
            SpawnAgents();

        }
        else
        {
            int id = GameManager.Instance.GetPlayersTeamID();
            for (int i = 0; i < id; i++)

            {
                if (GameManager.Instance.cTeams[i].Agents.Count != 6)
                {
                    ready = false;
                }
            }
            if (ready == true && bTriggeredSpawnUnit == false)
            {
                bTriggeredSpawnUnit = true;
                SpawnAgents();

            }
        }
        if (GameManager.Instance.cTeams[GameManager.Instance.cTeams.Count -1].Agents.Count ==6)
        {
            Debug.Log("ak=ll units loaded, break to new state");
            GameManager.Instance.GamePlayStateMachine.ChangeState("NewMoveState");
        }
   

}

	

    public void SpawnAgents()
    {
        GameManager.Instance.SpawnMyAgents();
    }
}
