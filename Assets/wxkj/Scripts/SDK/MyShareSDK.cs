using UnityEngine;
using System.Collections;
using cn.sharesdk.unity3d;
public class MyShareSDK : MonoBehaviour {


    private static MyShareSDK myshareSDK = null;

    public static MyShareSDK MyshareSDK
    {
        get { return MyShareSDK.myshareSDK; }
        set { MyShareSDK.myshareSDK = value; }
    }
  
    public ShareSDK shareSdk;
	// Use this for initialization
	void Start () {
        myshareSDK = this;
        if (shareSdk != null)
        {
            Debug.Log("MyShareSDK 初始化回调！！！");
  //          shareSdk.showUserHandler = getUserInforCallback;
            shareSdk.authHandler = OnAuthResultHandler;
            //分享回调事件  
            shareSdk.shareHandler += ShareResultHandler;
        }
        else
        {
            Debug.Log("shareSdk为空");
        }
	}
    private string _token;
    private string _opendid;


    //获取微信个人信息成功回调,登录	  
    public void getUserInforCallback(int reqID, ResponseState state, PlatformType type, Hashtable data)
    {
        Debug.Log("ranger getUserInforCallback Start");
        print("ranger " + data.toJson()+MiniJSON.jsonEncode(data));

        //      _opendid = (string)data["openid"];
        //     _token = (string)data["unionid"];

#if UNITY_ANDROID
         _opendid = (string)data["openid"];
  //       _token = (string)data["unionid"];
#elif UNITY_IPHONE
         _opendid = (string)data["openid"];
//         _token = _token;
#endif
        print("ranger " + data["nickname"]+" Token "+_token+" Openid "+_opendid);

        doLogin();
    }

    // 分享结果回调  
    void ShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {   //成功  
        if (state == ResponseState.Success)
        {
 
        }
        //失败  
        else if (state == ResponseState.Fail)
        {
           // message.text = ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
        }
        //关闭  
        else if (state == ResponseState.Cancel)
        {
           // message.text = ("cancel !");
        }
    }  

    public string Username() 
    {
        return _opendid;
    }
    public string Password()
    {
        return _token;
    }

    void doLogin()
    {
        if (Game.SocketHall.SocketNetTools.Connected)
        {
            Game.SocketHall.LoginMsg(_opendid, _token, 3);
            print("doLogin1" + _opendid + "  " + _token);

        }
        else
        {
            Game.SocketHall.SocketNetTools.OnConnect -= OnConnect;
            Game.SocketHall.SocketNetTools.OnConnect += OnConnect;
            Game.InitHallSocket(GlobalConfig.address);
            print("doLogin2" + _opendid + "  " + _token);

        }
    }
    //微信分享房间
    public void Share(string roomId = null)
    {

 

        string room = (roomId == null) ? "" : "房间号是：" + roomId;
 

        string text = "大庆麻将.快来!" + room;
        ShareContent content = new ShareContent();
        content.SetTitle("大庆麻将");
        content.SetText(text);
        content.SetImageUrl("http://daqingmj.youhao88.com:8081/icon.png");
        content.SetUrl("daqingmj.youhao88.com:8081/DaQingMJ.html");
        content.SetShareType(ContentType.Webpage);

        shareSdk.ShareContent(PlatformType.WeChat, content);


    }

    //微信分享宣传
    public void ShareWeChatMoments(string roomId = null)
    {

    

  //      string room = (roomId == null) ? "" : "房间号是：" + roomId;
       
   //     string text = "齐齐哈尔麻将.快来!" + room;
        ShareContent content = new ShareContent();
   //     content.SetTitle("齐齐哈尔麻将");
  //      content.SetText(text);
        content.SetImageUrl("http://daqingmj.youhao88.com:8081/DaQingMJ.jpg");
   //     content.SetUrl("www.daqingmj.youhao88.com:8081/index2.html");
        content.SetShareType(ContentType.Image);

        shareSdk.ShareContent(PlatformType.WeChatMoments, content);


    }


    void OnConnect()
    {
        Game.SocketHall.SocketNetTools.OnConnect -= OnConnect;

        if (Game.SocketHall.SocketNetTools.Connected)
        {
            Game.SocketHall.LoginMsg(_opendid, _token, 3);
        }
        else
        {
            //Game.LoadingPage.Hide();
            Game.DialogMgr.PushDialog(UIDialog.SingleBtnDialog, "连接失败！");
        }
    }

    void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
    {
        if (state == ResponseState.Success)
        {
            print("authorize success !" + "Platform :" + type);

 
            Hashtable authinfo = shareSdk.GetAuthInfo(PlatformType.WeChat);
            print("OnAuthResultHandler-GetAuthInfo" + MiniJSON.jsonEncode(authinfo));
 
            //         _opendid = (string)authinfo["openID"];
            //          _token = (string)authinfo["token"];


#if UNITY_ANDROID
            _opendid = (string)authinfo["openID"];
            _token = (string)authinfo["token"];
#elif UNITY_IPHONE
            _opendid = (string)authinfo["uid"];
            _token = (string)authinfo["token"];
#endif
            print("OnAuthResultHandler-send username and password: " + _opendid + "  " + _token);

            doLogin();
        }
        else if (state == ResponseState.Fail)
        {
#if UNITY_ANDROID
            print("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
        }
        else if (state == ResponseState.Cancel)
        {
            print("cancel !");
        }
    }
}
