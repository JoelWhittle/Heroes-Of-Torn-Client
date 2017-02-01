using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RoomLogManager : MonoBehaviour {

	public string sAddToLogURL;
	public string sLogStream;
	public List<string> sLogList;
	public string sLogLineSeperator;

	public ExchangeGameData cExchangeGameData;
	public NetworkManager cNetworkManager;

    public static RoomLogManager Instance;


    void Start()
    {
        Instance = this;
    }
	public void ActOnLog(string sLogLine)
	{
		string[] str = sLogLine.Split (":".ToCharArray ());
		string sFunction = str [0];
		 

		switch(sFunction)
		{
		case "LoadMap":

			Debug.Log("WANT TO LOAD A MAP");

			cExchangeGameData.LoadMap(0);
			break;

		case "RegisterPlayer":

			cExchangeGameData.RegisterPlayerForGame(str[1]);
			break ; 

            case "SpawnUnit":

            cExchangeGameData.SpawnAgent(str[1]);

                break;
            case "MoveUnit":

                cExchangeGameData.ReceiveMoveUnitRequest(str[1]);
                Debug.Log("reached read log state machine");
                break;

            case "EndTurn":

                cExchangeGameData.ReceiveEndTurnRequest(str[1]);
                Debug.Log("reached end turn read log state machine");
                break;
            case "Attack":

                cExchangeGameData.ReceiveAttackRequest(str[1]);
                Debug.Log("reached attack read log state machine");
                break;
	 default:
			Debug.Log("Function not implemented yet: " + sLogLine);

			break;

		}

	}



	public void ReadLog (string sNewLog)
	{
		//if the string is empty, do nothing, if not, seperate it of, command by command and add to the loglist

	    if (sNewLog.Length==0)
	    {
	        
	    }
	    else
	    {
	        
			Debug.Log("LOG CHANGED:" + sNewLog);

			string[] sSplitNewLog = sNewLog.Split(sLogLineSeperator.ToCharArray());

			for(int i = 0; i < sSplitNewLog.Length  ; i++)
			{
			    if (sSplitNewLog[i].Length > 0)
			    {

			        Debug.Log("WORKING WITH THIS LINE: " + sSplitNewLog[i]);
			        sLogList.Add(sSplitNewLog[i]);
			        ActOnLog(sSplitNewLog[i]);
			    }
			}

			sLogStream = sNewLog;
		}



	}



    public IEnumerator HandleWriteToLog(string sMessage)
    {

        yield return new WaitForSeconds(0);

        RoomLogWriter.Instance.AddMessageToList(sMessage);
    }

}
