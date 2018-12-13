using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using LitJson;

public class Client : MonoBehaviour {

	public static Client instance;      //设置单例

	private Socket client;              //socket客户端
	private string host = "127.0.0.1";  //地址
	private int port = 12080;           //端口
	private Thread thread;              //线程

	private byte[] recvData;            //接收数据流

    public string userName;             //玩家名字

	public delegate void OnReceiveMsg(string msg);      //注册委托
	public OnReceiveMsg onReceiveMsg;                   //委托事件

	void Awake()
	{
		if (instance == null) {                         //初始化单例
			DontDestroyOnLoad (gameObject);
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	void Start () {

		recvData = new byte[1024];                     

        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);       //初始化客户端
        try
        {
            client.Connect(new IPEndPoint(IPAddress.Parse(host), port));                            //连接服务端
			print("连接成功");
			thread = new Thread(Recv);                                                              //初始化线程
			thread.Start();                                                                         //执行线程
		}
		catch(Exception e) {
			print (e.Message);
			return;
		}
	}

	public void SendMessage(string msgToSend)                                                       //发送数据给服务端
	{
		byte[] byteToSend = MsgToBytes (msgToSend);                                                 //转换为byte[]
		client.Send (byteToSend);                                                                   //发送数据
		print ("已发送");
	}

    //发送数据格式为4位长度加上数据内容
	byte[] MsgToBytes(string msgToSend)
	{
		byte[] byteToSend = Encoding.UTF8.GetBytes (msgToSend);
		byte[] msgLength = BitConverter.GetBytes (msgToSend.Length + 4);
		return msgLength.Concat (byteToSend).ToArray<byte> ();
	}

	void Recv()                                                                                     //接收线程
	{
		while (true) {
			int count = client.Receive (recvData);                                                  //接收数据
			if (count != 0) {
				print ("接收成功");
				int byteLength = BitConverter.ToInt32 (recvData, 0);                                //获取数据长度
				byte[] byteContent = recvData.Skip (4).Take (byteLength).ToArray<byte> ();          //获取数据
				string jsonStr = Encoding.UTF8.GetString (byteContent);                             //转换为json字符串
				if (onReceiveMsg != null) {                                                         //发布委托
					onReceiveMsg.Invoke (jsonStr);
				}
				recvData = new byte[1024];                                                          //重置接收数据
			}
		}
	}

    public void SetUserName(string name)                                                            //获得玩家名字
    {
        userName = name;
    }

	void OnApplicationQuit()                                                                        //当游戏结束时，断开与服务端的连接
	{
		print ("连接中断");
		thread.Interrupt ();
		client.Close ();
	}

}
