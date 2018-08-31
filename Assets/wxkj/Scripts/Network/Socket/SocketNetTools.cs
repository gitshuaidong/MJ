using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using packet.msgbase;

public class SocketNetTools : MonoBehaviour
{
    private NetClient client;
    // 连接成功
    public System.Action OnConnect;
    public Queue<PacketBase> pools = new Queue<PacketBase>();
    private Dictionary<int, System.Action<PacketBase>> listeners = new Dictionary<int, System.Action<PacketBase>>();
    private Dictionary<int, System.Action<PacketBase>> onceListeners = new Dictionary<int, System.Action<PacketBase>>();
    private bool connectFinish = false;

    public void StartClient(string address,int port)
    {
        client = new NetClient();
        client.address = address;
        client.port = port;
        client.receiveCallBack -= OnReceive;
        client.receiveCallBack += OnReceive;
        client.connectCallBack -= connectCallBack;
        client.connectCallBack += connectCallBack;
        connectFinish = false;
        client.StartClient();
        //connectFinish = false;
    }

    public void StopClient()
    {
        if (null != client)
        {
            client.receiveCallBack -= OnReceive;
            client.connectCallBack -= connectCallBack;
            client.StopClient();
        }
    }

    public bool Connected
    {
        get
        {
            return null != client && client.Connected;
        }
    }

    void connectCallBack()
    {
        connectFinish = true;
    }

    void Update()
    {
  
 
        //////////////////////////////////////
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
/*            if (client.onlineHallSocket == true)
                Game.DialogMgr.PushDialog(UIDialog.SingleBtnDialog, "没有联网！");
            if (client.onlineMsgSocket == true)
                Game.DialogMgr.PushDialog(UIDialog.SingleBtnDialog, "没有联网！");
            if (client.onlineGameSockett == true)
                Game.DialogMgr.PushDialog(UIDialog.SingleBtnDialog, "没有联网！");
*/

            Debug.LogFormat("没有联网！！！");
            if (Game.SocketGame.SocketNetTools.client != null)
            {
                client.onlineGameSockett = false;
                Game.SocketGame.SocketNetTools.StopClient();

            }
   /*         if (Game.SocketHall.SocketNetTools.client != null)
            {
                client.onlineHallSocket = false;
                Game.SocketHall.SocketNetTools.StopClient();
            }
            if (Game.SocketMsg.SocketNetTools.client != null)
            {
                client.onlineMsgSocket = false;
                Game.SocketMsg.SocketNetTools.StopClient();
            }*/

        }

        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
    /*        if (client != null && client.onlineHallSocket == false)
            {
                StopClient();

                string gameserver = "daqingmj.youhao88.com:5000";
                Game.InitHallSocket(gameserver);
                //               Game.DialogMgr.OnBackPressed();

                client.onlineHallSocket = true;

            }
            if (client != null && client.onlineMsgSocket == false)
            {
                StopClient();

                string gameserver = "daqingmj.youhao88.com:4000";
                Game.InitMsgSocket(gameserver);
                //               Game.DialogMgr.OnBackPressed();

                client.onlineMsgSocket = true;

            }*/
            if (client != null && client.onlineGameSockett == false)
            {
                Game.SocketGame.SocketNetTools.StopClient();

                string gameserver = "daqingmj.youhao88.com:7000";
                Game.InitGameSocket(gameserver);
//                Game.DialogMgr.OnBackPressed();

                client.onlineGameSockett = true;

            }

      
 

//            Debug.LogFormat("使用Wi-Fi！！！");

        }

        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            /*          if (client != null && client.onlineHallSocket == false)
                      {
                          StopClient();

                          string gameserver = "daqingmj.youhao88.com:5000";
                          Game.InitHallSocket(gameserver);
          //                Game.DialogMgr.OnBackPressed();

                          client.onlineHallSocket = true;

                      }
                      if (client != null && client.onlineMsgSocket == false)
                      {
                          StopClient();

                          string gameserver = "daqingmj.youhao88.com:4000";
                          Game.InitMsgSocket(gameserver);
          //                Game.DialogMgr.OnBackPressed();

                          client.onlineMsgSocket = true;

                      }*/

            if (client != null && client.onlineGameSockett == false)
            {
                Game.SocketGame.SocketNetTools.StopClient();

                string gameserver = "daqingmj.youhao88.com:7000";
                Game.InitGameSocket(gameserver);
 //               Game.DialogMgr.OnBackPressed();

                client.onlineGameSockett = true;

            }

 

//            Debug.LogFormat("使用移动网络！！！");
 
        }
        /////////////////////////////////////////
        /*      if (Game.SocketHall.SocketNetTools.Connected == false)
              {
                  if (Game.SocketHall.SocketNetTools.client != null)
                  {
                      string gameserver = "daqingmj.youhao88.com:5000";
                      Game.InitHallSocket(gameserver);
                  }
              }
              if (Game.SocketMsg.SocketNetTools.Connected == false)
              {
                  if (Game.SocketMsg.SocketNetTools.client != null)
                  {

                      string gameserver = "daqingmj.youhao88.com:4000";
                      Game.InitMsgSocket(gameserver);
                  }
              }*/
  /*      if (Game.SocketGame.SocketNetTools.Connected == false)
        {
            if (Game.SocketGame.SocketNetTools.client != null)
            {
                Game.SocketGame.SocketNetTools.StopClient();
                string gameserver = "daqingmj.youhao88.com:7000";
                Game.InitGameSocket(gameserver);
             
            }
        }else
        {
 
 //           PacketBase testmsg = new PacketBase() { packetType = PacketType.HEARTBEAT };
 //           Game.SocketGame.SocketNetTools.client.SendMsg(testmsg);
        }
*/
        /////////////////////////////////////
        if (connectFinish)
        {
            connectFinish = false;
            if (null != OnConnect)
            {
 
               OnConnect();

            }
        }
        if (pools.Count > 0)
        {
            PacketBase msg = pools.Dequeue();
            if (null != msg)
            {
                DispatchEvent((int)msg.packetType, msg);
                DispatchOnceEvent((int)msg.packetType, msg);
            }
        }
    }

    public void SendMsg(PacketBase msg)
    {
        client.SendMsg(msg);
    }

    public void SendMsg(PacketBase msg, PacketType cmd, System.Action<PacketBase> callback)
    {
        AddEventOnceListener((int)cmd, callback);
        client.SendMsg(msg);
    }

    void OnDestroy()
    {
        StopClient();
    }

    void OnReceive(PacketBase msg)
    {
        Debug.Log("<=receivecallback msg;;;;;");
        pools.Enqueue(msg);
    }

#region Event
    public void AddEventListener(int type, System.Action<PacketBase> handler)
    {
        if (handler == null)
            return;

        if (listeners.ContainsKey(type))
        {
            //这里涉及到Dispath过程中反注册问题，必须使用listeners[type]+=..
            listeners[type] += handler;
        }
        else
        {
            listeners.Add(type, handler);
        }
    }

    private void AddEventOnceListener(int type, System.Action<PacketBase> handler)
    {
        if (handler == null)
            return;

        if (onceListeners.ContainsKey(type))
        {
            //这里涉及到Dispath过程中反注册问题，必须使用listeners[type]+=..
            onceListeners[type] += handler;
        }
        else
        {
            onceListeners.Add(type, handler);
        }
    }

    public void RemoveEventListener(int type, System.Action<PacketBase> handler)
    {
        if (handler == null)
            return;

        if (listeners.ContainsKey(type))
        {
            //这里涉及到Dispath过程中反注册问题，必须使用listeners[type]-=..
            listeners[type] -= handler;
            if (listeners[type] == null)
            {
                //已经没有监听者了，移除.
                listeners.Remove(type);
            }
        }
    }

    private void RemoveEventOnceListener(int type, System.Action<PacketBase> handler)
    {
        if (handler == null)
            return;

        if (onceListeners.ContainsKey(type))
        {
            //这里涉及到Dispath过程中反注册问题，必须使用listeners[type]-=..
            onceListeners[type] -= handler;
            if (onceListeners[type] == null)
            {
                //已经没有监听者了，移除.
                onceListeners.Remove(type);
            }
        }
    }

    private static readonly string szErrorMessage = "NetworkManager Error, Event:{0}, Error:{1}, {2}";

    public void DispatchEvent(int evt, PacketBase msg)
    {
        try
        {
            if (listeners.ContainsKey(evt))
            {
                System.Action<PacketBase> handler = listeners[evt];
                if (handler != null)
                    handler(msg);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format(szErrorMessage, evt, ex.Message, ex.StackTrace));
        }
    }

    private void DispatchOnceEvent(int evt, PacketBase msg)
    {
        try
        {
            if (onceListeners.ContainsKey(evt))
            {
                System.Action<PacketBase> handler = onceListeners[evt];
                if (handler != null)
                {
                    handler(msg);
                    RemoveEventOnceListener(evt, handler);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format(szErrorMessage, evt, ex.Message, ex.StackTrace));
        }
    }

    public void ClearAll()
    {
        listeners.Clear();
    }

    public void ClearEvents(int key)
    {
        if (listeners.ContainsKey(key))
        {
            listeners.Remove(key);
        }
    }
#endregion
}
