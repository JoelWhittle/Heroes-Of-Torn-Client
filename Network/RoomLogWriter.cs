using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//This class recieves messages from the RoomLogManager and does its best to make sure they
//reach the server
public class RoomLogWriter : MonoBehaviour
{


    public static RoomLogWriter Instance;
    public bool IsBusy = false;

    public List<string> Messages = new List<string>(); 



	// Use this for initialization
	void Start ()
	{

	    Instance = this;
	}
	
    //
    public IEnumerator HandleWriteToLog(string sMessage)
    {
        IsBusy = true;
        sMessage = SendRecievePlayerData.Instance.sPHPReady(sMessage);
        //string sRoomName = NetworkManager.Instance.sRoomName;

        //string addtologURL = NetworkManager.Instance.serverurl +RoomLogManager.Instance.sAddToLogURL + "?message=" + sMessage + "&name=" + sRoomName;
        //Debug.Log(addtologURL);
        //WWW addtologReader = new WWW(addtologURL);
        //yield return addtologReader;

        AsynchronousClient.Instance.Send(sMessage);

        //if (addtologReader.error != null)
        //{
        //    //problem
        //    Debug.Log(addtologReader.error.ToString());
        //    SendFirstMessage();

        //}
        //else
        //{
            //worked

            RemoveFirstMessage();

            if (Messages.Count > 0)
            {
                SendFirstMessage();
            }
            else
            {
                IsBusy = false;
            }
        return null;
        
        //}
    }



    public void SendFirstMessage()
    {
        if (Messages.Count  > 0)
        {
        StartCoroutine(HandleWriteToLog(Messages[0]));
  
        }
    }

    public void RemoveFirstMessage()
    {
        Messages.RemoveAt(0);
    }

    public void AddMessageToList(string message)
    {
        Messages.Add(message);

        if (IsBusy == false)
        {
            SendFirstMessage();
        }
    }
}
