using System.Runtime.Remoting;
using JetBrains.Annotations;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
	public string sMapName;
	public int iCurMapID;
	public List<string> sPlayers = new List<string>();
	public List<Team> cTeams = new List<Team>();
    public List<GameObject> Agents = new List<GameObject>(); 
	public Team cCurrentTeam;
    public Agent CurrentAgent;
    

    public StateMachine GamePlayStateMachine;


    public Agent CalculateNextAgent()
    {
        Agent returnAgent = null;
        bool foundAgent = false;


        int newIndex = FindTeamIndex(cCurrentTeam) + 1;
        if (newIndex == cTeams.Count )
        {
            newIndex = 0;
        }
        
        returnAgent = cTeams[newIndex].Agents[0];
  
        Debug.Log("next agent  " + returnAgent.ID.ToString());
        return returnAgent;

    }

    public int FindTeamIndex(Team team)
    {
        int index = 0;
        for (int i = 0; i < cTeams.Count; i++)
        {
            if (cTeams[i] == team)
            {
                index = i;
            }
        }
        return index;
    }

	public NetworkManager cNetworkManager;

	void Start()
	{
	    Instance = this;
		cNetworkManager = GameObject.Find ("_NetworkManager").GetComponent<NetworkManager> ();
	    GamePlayStateMachine = gameObject.GetComponent<StateMachine>();
        
	}



	public bool bIsMasterClient()
	{
		Debug.Log("MASTER   " + "  " + cNetworkManager.sCurRoomOwner);
		if (cNetworkManager.sCurRoomOwner == GameObject.Find ("Player").GetComponent<Player> ().sName) {
						return true;
				} else {
						return false;
				}
	}

	public int iFullPlayerCount()
	{
		return GameObject.Find("_MapManager").GetComponent<MapManager>().MapMetaDatas[iCurMapID].iNoOfPlayers  ;
	}

    public Team GetPlayersTeam()
    {
        Team returnTeam = null;
        foreach (Team team in cTeams)
        {
            if (team.sControllingPlayerName == Player.Instance.sName)
            {
                returnTeam = team;
            }
        }
        return returnTeam;
    }

    public int GetPlayersTeamID()
    {
        int i = 0;
        bool found = false;
        foreach (Team team in cTeams)
        {
            if (team.sControllingPlayerName == Player.Instance.sName)
            {
                found = true;
            }
            if (found == false)
            {
                i++;
            }
        }
        return i;
    }

    public GameObject FindAgentObhectByID(int ID)
    {
        GameObject returnGameObject = null;

        foreach (GameObject agent in Agents)
        {
            if (agent.GetComponent<Agent>().ID == ID)
            {
                returnGameObject = agent;
            }
        }
        return returnGameObject;
    
    }

    public int FindAgentsTeamID(Agent agent)
    {
        int returnInt = 0;

        for (int i = 0; i < cTeams.Count ; i++)
        {
            foreach (Agent a in cTeams[i].Agents)
            {
                if (agent == a)
                {
                    returnInt = i;
                }
            }  
        }

        return returnInt;
    }
    public  void SpawnMyAgents()
    {
        StartCoroutine(HandleSpawnAgents());
    }

    public IEnumerator HandleSpawnAgents()
    {
             //    UnitCollection.Instance.Decks[Player.Instance.SelectedDeckIndex].Units.Count

        List<UnitSpawnPoint> mySpawnPoints = new List<UnitSpawnPoint>();
        Map cMap = (GameObject.FindGameObjectWithTag("Map")).GetComponent<Map>();

        foreach (UnitSpawnPoint sp in cMap.SpawnPoints)
        {
            if (sp.iPlayerTeamId == GameManager.Instance.GetPlayersTeamID())
            {
             mySpawnPoints.Add(sp);   
            }
        }
        for (int i = 0; i < UnitCollection.Instance.Decks[Player.Instance.SelectedDeckIndex].Units.Count; i++)
        {
            Unit unit = UnitCollection.Instance.Decks[Player.Instance.SelectedDeckIndex].Units[i];
            Hex hex = mySpawnPoints[i].ParentHex;

        yield return   StartCoroutine(ExchangeGameData.Instance.SendSpawnAgentRequest(CreateAgent.Instance.PrepareCreateAgentString(unit,hex)));
        }
       
    }

    public void EndTurn()
    {
        GamePlayGUI.Instance.EnableEndTurnButton(false);
        GamePlayGUI.Instance.EnableAbilitySelectButton(false);

        GamePlayStateMachine.ChangeState("IdleState");
        string s = CalculateNextAgent().ID.ToString();
       StartCoroutine( ExchangeGameData.Instance.SendEndTurnRequest(s));
    }


}
