using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

public class ExchangeGameData : MonoBehaviour {

	public GameManager cGameManager;
	public NetworkManager cNetworkManager;
	public Player cPlayer;
	public string 	sCloseRoomURL;
    public static ExchangeGameData Instance ;


    void Start()
    {
        Instance = this;
    }
    	public IEnumerator InstantiateGame(int iMapID)
	{
		Debug.Log ("styarted a new game");
		cGameManager.iCurMapID = iMapID;
		SendRegisterPlayerForGameRequest (cPlayer.sName);

		yield return new WaitForSeconds (0);
	


	}

	public void SendRegisterPlayerForGameRequest(string sName)
	{
	//	StartCoroutine(cNetworkManager.cRoomLogManager.HandleWriteToLog ("RegisterPlayer:" + sName));
	}
	public void RegisterPlayerForGame(string sPlayerName)
	{

		Team cTeam =  cGameManager.gameObject.AddComponent<Team> ();
		cTeam.sControllingPlayerName = sPlayerName;
		cGameManager.cTeams.Add (cTeam);
	
	}


	public void InitialiseGame(string sMapName)
	{

	
	}

	public void SendLoadMapRequest(int iMapID)
	{
		StartCoroutine(cNetworkManager.cRoomLogManager.HandleWriteToLog ("LoadMap:" + iMapID.ToString()));

	}

	public IEnumerator CloseCurrentRoom()
	{
		Debug.Log("wtf4");

		string sRoomName = cNetworkManager.sRoomName;
		
	
		string closeRoomURL = cNetworkManager.serverurl + sCloseRoomURL +"?room=" + sRoomName + "&junk=" + Random.Range(0,1000).ToString();
		Debug.Log (closeRoomURL);
		WWW closeRoomReader = new WWW (closeRoomURL);
		yield return closeRoomReader;
		
		if (closeRoomReader.error != null) {
			Debug.Log("broke in closeroom.php");
			yield return StartCoroutine(CloseCurrentRoom());
		} 
		else 
		{
			//worked
			
		}
	}

	public void LoadMap(int iMapID)
	{


		cGameManager.iCurMapID = iMapID;
		cNetworkManager.bInGame = true;
	    GameObject map =
	        (GameObject) Instantiate(MapManager.Instance.MapPrefab[iMapID], new Vector3(0, 0, 0), Quaternion.identity);
		Instantiate(GameObject.Find("_Manager").GetComponent<PrefabLibrary>().GameCamera, new Vector3(0,0,0), Quaternion.identity);
		Destroy(GameObject.Find ("Camera"));
        GameObject.Find("_MainMenu").SetActive(false);

	    StartCoroutine(DelayBeforeCreatingUnits());
	}

    public IEnumerator DelayBeforeCreatingUnits()
    {
        //start state machine behaviour, init game
        yield return new WaitForSeconds(1);

        GameManager.Instance.GamePlayStateMachine.ChangeState("InitGamePlayState");

    }

    public IEnumerator SendSpawnAgentRequest(string unitstream)
    {
  AsynchronousClient.Instance.Send("SpawnUnit:" + unitstream);

        Debug.Log("SpawnUnit:"  + unitstream);
        yield return new WaitForSeconds(0);
    }

    public void SpawnAgent(string input)
    {
        CreateAgent.Instance.InstantiateUnit(input);
    }


    public void SendAgentMoveRequest(int p, List<GameObject> pathList )
    {
        StartCoroutine(HandleSendAgentMoveRequest(p, pathList
        ))
        ;
    }

    public IEnumerator HandleSendAgentMoveRequest(int AgentID, List<GameObject>  pathList)
    {
        string message = "MoveUnit:" + AgentID.ToString() +">" ;
        string loop = "";
        foreach (GameObject go in pathList)
        {
            loop = loop + go.name;
            loop = loop + ">";
        }
        message = message + loop;


        yield return StartCoroutine(cNetworkManager.cRoomLogManager.HandleWriteToLog(message));

        
    }

    public void ReceiveMoveUnitRequest(string dataStream)
    {
        Debug.Log("reached move unit method");
       string[] args = dataStream.Split(">".ToCharArray());
        List<GameObject> pathList = new List<GameObject>();
        GameObject AgentToMove = null;

        for (int i = 0; i < args.Length; i++)
        {
            if (i == 0)
            {
           AgentToMove = GameManager.Instance.FindAgentObhectByID(int.Parse(args[0]));
                Debug.Log(AgentToMove.name);
            }
            else
            {
                if (args[i]!=null)
                {
                    pathList.Add(GameObject.Find(args[i]));
                }
            }
        }
       StartCoroutine( AgentToMove.GetComponent<Agent>().LerpDownPath(pathList));

    }

    public IEnumerator SendEndTurnRequest(string s)
    {
        yield return StartCoroutine(cNetworkManager.cRoomLogManager.HandleWriteToLog("EndTurn:" + s));

    }

    public void ReceiveEndTurnRequest(string s)
    {
        int id = int.Parse(s);
        GameObject newCurrentAgent = GameManager.Instance.FindAgentObhectByID(id);
  

        GameObject tmpCurAgent = GameManager.Instance.CurrentAgent.gameObject;
        GameManager.Instance.cCurrentTeam.Agents.Remove(tmpCurAgent.GetComponent<Agent>());
        GameManager.Instance.cCurrentTeam.Agents.Add(tmpCurAgent.GetComponent<Agent>());


        GameManager.Instance.CurrentAgent = newCurrentAgent.GetComponent<Agent>();
        GameManager.Instance.cCurrentTeam =
            GameManager.Instance.cTeams[GameManager.Instance.FindAgentsTeamID(newCurrentAgent.GetComponent<Agent>())];
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothFollow>().target = newCurrentAgent.transform;

        GameManager.Instance.GamePlayStateMachine.ChangeState("NewMoveState");
    }

    //Send a request to the room log manager based on information we recieved from Agent.CalculateAttack()
    public IEnumerator SendAttackRequest(string s)
    {
        yield return StartCoroutine(cNetworkManager.cRoomLogManager.HandleWriteToLog("Attack:" + s));

    }

    //Read in the results of an Attack and parse it of to Agent.Attack();
    public void ReceiveAttackRequest(string s)
    {
        string[] args = s.Split(">".ToCharArray());

        Agent curAgent = GameManager.Instance.FindAgentObhectByID(int.Parse(args[0])).GetComponent<Agent>();
        Agent victimAgent = GameManager.Instance.FindAgentObhectByID(int.Parse(args[1])).GetComponent<Agent>();

        bool didWeHit = false;
        bool didTheyDie = false;
        int damage = 0;

        if (args[2] == "hit")
        {
            didWeHit = true;
        }
        damage = int.Parse(args[3]);
        
        if (args[4] == "true")
        {
            didTheyDie = true;
        }
      StartCoroutine( AbilityManager.Instance.FindAbilityByName("Attack").Cast(curAgent,victimAgent,didWeHit,didTheyDie,damage));
        

    }
}

