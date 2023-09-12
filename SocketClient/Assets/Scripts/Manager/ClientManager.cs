using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using SocketGameProtocol;
using System.Net;
using System.Threading;

public class ClientManager:BaseManager
{
    private Socket socket;
    private Message message;
    public ClientManager(GameFace face):base(face)
    {

    }
    public override void OnInit()
    {
        base.OnInit();
        message = new Message();
        InitSocket();
        InitUDP();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        message = null;
        CloseSocket();
    }

    //��ʼ��socket
    
    private void InitSocket()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect("192.168.17.190",6666);
            //GameFace.Instance.text.text = "成功连接服务器";
            UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
            {
                panel.ShowMessage("成功连接到服务器");
            });
            StartReceive();
        }
        catch(Exception e)
        {
            UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
            {
                panel.ShowMessage("连接服务器失败");
            });
            Debug.LogWarning(e);
        }
    }

    //�ر�socket
    private void CloseSocket()
    {
        if(socket!=null&&socket.Connected)
        {
            socket.Close();
        }
    }

    private void StartReceive()
    {
        socket.BeginReceive(message.Buffer, message.StartIndex, message.Remsize, SocketFlags.None, ReceiveCallBack, null);
    }

    private void ReceiveCallBack(IAsyncResult iar)
    {
        //try
        {
            if (socket == null || socket.Connected == false) return;
            int len = socket.EndReceive(iar);

            if(len==0)
            {
                CloseSocket();
                return;
            }

            message.ReadBuffer(len, HandleResponse);
            StartReceive();
        }
        //catch
        //{
        //    Debug.Log("接受消息出问题啦");
        //}


    }

    private void HandleResponse(MainPack pack)
    {
        face.HandleResponse(pack);
    }

    public void Send(MainPack pack)
    {
        socket.Send(Message.PackData(pack));
    }


    //UDP代码
    private Socket udpSocket;
    private IPEndPoint IEP;
    private EndPoint remoteIP;
    private Byte[] buffer = new Byte[1024];
    private Thread receiveThread;
    private void InitUDP()
    {
        udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IEP = new IPEndPoint(IPAddress.Parse("192.168.17.190"), 6667);
        remoteIP = IEP;
        try
        {
            udpSocket.Connect(IEP);
        }
        catch
        {
            Debug.Log("UDP连接失败");
            return;
        }
        receiveThread = new Thread(ReceiveMsg);
        receiveThread.Start();
    }

    private void ReceiveMsg()
    {
        Debug.Log("UDP开始接收");
        while(true)
        {
            int len = udpSocket.ReceiveFrom(buffer, ref remoteIP);
            MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 0, len);
            HandleResponse(pack);
        }
    }

    public void SendTo(MainPack pack)
    {
        Byte[] sendBuff = Message.PackDataUDP(pack);
        udpSocket.SendTo(sendBuff, sendBuff.Length, SocketFlags.None, remoteIP);
    }
}
