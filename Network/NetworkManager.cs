using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	public ConnectionStatus _ConnectionStatus;
	public float fTickFreq;
	public bool bLoggedIn;
	public string sRoomName;
	public bool bInGame;
	public Player cPlayer;
	public string serverurl = "http://www.willdevforfood.x10host.com";
	public List<string> CurrentEqualRoomList = new List<string>();
	public LoginMenu cLoginMenu;

	public GameManager cGameManager;
	public GameObject goDisconnectedGUI;
	public ExchangeGameData cExchangeGameData;
	public RoomLogManager cRoomLogManager;


	public string sRequestRoomLogDir;
	public string sMapMetaDataDir;
    public string SRaceDataDir;

	public int iCurBadRequests;

	public string sCurRoomOwner;
    public static NetworkManager Instance;

    public bool bConnected()
	{

		if (_ConnectionStatus == ConnectionStatus.Connected) {
						return true;
				} else {
						return false;
				}
	}



	public void Start()
	{
	    Instance = this;
		bLoggedIn = false;
		cLoginMenu = gameObject.GetComponent<LoginMenu> ();
		cExchangeGameData = gameObject.GetComponent<ExchangeGameData> ();
	    Application.runInBackground = true;

	    AsynchronousClient.Instance.OkayToTick = true;

	}



	IEnumerator HandleRequestRoomLog()
	{

		string requestroomlogURL = this.serverurl + sRequestRoomLogDir  + "?name=" + sRoomName + "&junk=" + Random.Range(0,999).ToString();
				WWW requestroomlogReader = new WWW (requestroomlogURL);
	       	yield return requestroomlogReader;
		
		if (requestroomlogReader.error != null) {
            Debug.Log(requestroomlogReader.error);

			StartCoroutine(BadRoomRequest());
				}
		else {
			iCurBadRequests = 0;
			_ConnectionStatus = ConnectionStatus.Connected;
			string str = requestroomlogReader.text;
			string[] strPart = str.Split("#".ToCharArray());

            
			cRoomLogManager.ReadLog(strPart[1]);
			sCurRoomOwner = strPart[0];
		}
		}
			

	public IEnumerator BadRoomRequest()
	{
		Debug.Log ("shit");
		_ConnectionStatus = ConnectionStatus.Disconnected;
		iCurBadRequests++;
		if(iCurBadRequests == 3)
		{
			goDisconnectedGUI.SetActive(true);
			goDisconnectedGUI.GetComponent<DisconnectedGUI>().RandomizeMessage();
		}
		yield return new WaitForSeconds (0);
		}
			
	public IEnumerator Tick()
	{
		StartCoroutine (NetworkFSM());
		yield return new WaitForSeconds(fTickFreq);
		StartCoroutine (Tick ());
	}

	public IEnumerator NetworkFSM()
	{

		StartCoroutine (HandleRequestRoomLog ());
		yield return new WaitForSeconds(0);
	}



	public bool bShowLoginMenu()
	{
		if (bLoggedIn == false & _ConnectionStatus == ConnectionStatus.Connected) {
						return true;
				} 
		else 
		{

			return false;
		}
	}




	//TO DO CREATE ROOOM
	//ROOM FAIL CALL BACK WORK DOWN LIST






	public IEnumerator LookForRankedGame()
	{

	yield return StartCoroutine(HandleFindRankedGame(GameObject.Find("Player").GetComponent<Player>().iRank));
	}

	IEnumerator HandleFindRankedGame(int Rank)
	{
		
	
			
			
			string str;
	    string room;
            str = AsynchronousClient.Instance.Send("RequestRoomOfRank:" + Rank.ToString());
	    string[] tmp = str.Split("<".ToCharArray());
	    str = tmp[0];
		
			Debug.Log("FINDME:" +str.Length.ToString());
			if(str.Length!=0)
			{
			    room = str;
			
		    
			
					JoinRoom(room);

			}
            else
				{
				yield return StartCoroutine(HandleCreateRankedGame(Rank));

				}
//			else
//			{
//				StartCoroutine(HandleCreateRankedGame(Rank));
//				Debug.Log("made new game as there were NONE avilable");
//			}
	
		

	}

	public void JoinRoom(string sNewRoomName)
	{

      string  str = AsynchronousClient.Instance.Send("PlayerJoinedRoom:" +sNewRoomName + ">l>l>l>" + cPlayer.SelectedDeckIndex.ToString()) ;
if(str == "success")
{ 
		GameObject.Find("_PlayMenu").GetComponent<PlayMenu>().sStatus = "Found Game";
		sRoomName = sNewRoomName;
		cRoomLogManager.sLogList.Clear ();
		cRoomLogManager.sLogStream = "";
		cExchangeGameData.SendRegisterPlayerForGameRequest (cPlayer.sName);
    }
		}


	IEnumerator HandleCreateRankedGame(int Rank)
	{
		string roomName = GameObject.Find("Player").GetComponent<Player>().sName + Random.Range (0, 1000).ToString ();

		cPlayer = GameObject.Find ("Player").GetComponent<Player> ();

        AsynchronousClient.Instance.Send("HandleCreateRankedGame:" + Rank.ToString() + ">" + roomName + ">l>l>" + cPlayer.SelectedDeckIndex.ToString());
        Debug.Log("REACHED AFTER CREATE RANKED SEND");
		cRoomLogManager.sLogList.Clear ();
		cRoomLogManager.sLogStream = "";
		sRoomName = roomName;

	int iMapID = Random.Range(0, GameObject.Find("_MapManager").GetComponent<MapManager>().MapMetaDatas.Count);
		StartCoroutine(cExchangeGameData.InstantiateGame (iMapID));

	   yield  return new WaitForSeconds(0);
	}


	public enum ConnectionStatus 
	{
        Connected,
		Disconnected
	}


}
