using System;
using System.Collections;
using UnityEngine;

namespace cn.sharesdk.unity3d
{
		[Serializable]
		public class ShareSDKConfig
		{
				public string appKey;
				public string appSecret;

				public ShareSDKConfig()
				{
						this.appKey = "wx51a1c386614f8ee5";
                        this.appSecret = "a9bf1e70bff99ced5b2a3a95b992452e";
				}
		}		
				
}


