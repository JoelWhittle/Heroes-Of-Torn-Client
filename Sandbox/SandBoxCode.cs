using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;


public  class SandBoxCode : MonoBehaviour
    {
    public bool OkayToTick = true;

    void Start()
    {
            msg("Client Started");

       StartCoroutine(Tick());

   

    }


    public IEnumerator Tick()
    {
        OkayToTick = false;
        Send("pop>TestRoom>RequestRoomLog> ");
        yield return new WaitForSeconds(.1f);
        Debug.Log("Tick");
        OkayToTick = true;
    }

    void Update()
    {
        if (OkayToTick)
        {
            StartCoroutine(Tick());
        }
    }


        private void Send(string message)
        {
            System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

            IPAddress ip = IPAddress.Parse("86.12.183.129");

         //   Debug.Log(ip.ToString());
            
         
            clientSocket.Connect(ip, 8888);
            Debug.Log("Client Socket Program - Server Connected ...");

            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(message + "<EOF>");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[1024];
        //    serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
            serverStream.Read(inStream, 0,1024);

            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            msg(returndata);
            
           
        }

        public void msg(string mesg)
        {
            Debug.Log(Environment.NewLine + " >> " + mesg);
        }
    }
