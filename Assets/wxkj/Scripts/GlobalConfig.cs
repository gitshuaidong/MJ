using UnityEngine;
using System.Collections;

public class GlobalConfig : MonoBehaviour 
{
    

#if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
 
    public static string address = "daqingmj.youhao88.com:5000";//121.40.177.10   103.47.82.46:5000
#else
    public static string address = "daqingmj.youhao88.com:5000";
#endif

#if UNITY_IOS || UNITY_IPHONE
	public const string URL_ROOT = "plaza2.youhao88.com";
 
#else
    public const string URL_ROOT = "plaza2.youhao88.com";
	#endif

    //xxtea解密秘钥
	public const string SECRET = "729d9ea91f7d2c1583eb22336b31ca91";

    //版本号
    public static string GetVersion
	{
        get
        {
            return "1.0.0";
        }
	}
	
	//appkey
	public static string GetAppKey
	{
		get { return "wx17484d0f5db34daf"; }
	}

    //平台ID//设备号  1:ios 2:android 3:winphon 4:other
    public static int GetPlatformId
	{
		get
		{
			#if    UNITY_ANDROID
			return 2;
			#elif  UNITY_IOS || UNITY_IPHONE
			return 1;
			#endif
			return 4;
		}
	}

    public static string DeviceId
    {
        get
        {
            return SystemInfo.deviceUniqueIdentifier;
        }
    }

	public static float HeartBeatTime = 30;
}
