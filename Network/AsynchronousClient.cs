using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;


public class AsynchronousClient : MonoBehaviour
{
    public bool OkayToTick = false;
    public static AsynchronousClient Instance;
    public float TickFreq;

    public string LobbyRoom;



    public string lastRoomLog = "";
    public Thread t1;
   

    void Start()
    {
        Instance = this;
        msg("Client Started");




    }

  

    public void Tick()
    {
        OkayToTick = false;
          RequestRoomLog();
        Thread.Sleep(Convert.ToInt32(TickFreq * 100));
        OkayToTick = true;
        t1.Abort();
    }

    public void RequestRoomLog()
    {
    string data =   Send("RequestRoomLog:" + RoomLogManager.Instance.sLogList.Count.ToString());
        if (data.Length != 0)
        {
            lastRoomLog = data;

        }
    }

    void Update()
    {

        if (lastRoomLog != "")
        {

            RoomLogManager.Instance.ReadLog(lastRoomLog);
            lastRoomLog = "";
        }
        if (OkayToTick)
        {

            t1 = new Thread(Tick) { Name = "Thread 1" };
         t1.Start();

        }
    }


    public string Send(string message)
    {
        message = NetworkManager.Instance.cPlayer.sName + ">" + NetworkManager.Instance.sRoomName + ">" + message;
      NetworkManager.Instance._ConnectionStatus = NetworkManager.ConnectionStatus.Connected;

        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

        IPAddress ip = IPAddress.Parse("82.36.47.11");

        //   Debug.Log(ip.ToString());


        clientSocket.Connect(ip, 8889);
        Debug.Log("Client Socket Program - Server Connected ...");

        NetworkStream serverStream = clientSocket.GetStream();
        byte[] outStream = System.Text.Encoding.ASCII.GetBytes(message + "<EOF>");
        serverStream.Write(outStream, 0, outStream.Length);
        serverStream.Flush();

        byte[] inStream = new byte[2048];
        //    serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
        serverStream.Read(inStream, 0, 2048);


        
        string returndata = System.Text.Encoding.ASCII.GetString(inStream);

        string[] splitReturnData = returndata.Split("<".ToCharArray());

        returndata = splitReturnData[0];

        msg(returndata);


        return returndata;
    }

    public void msg(string mesg)
    {
        Debug.Log(Environment.NewLine + " >> " + mesg);
    }
}
