using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LitJson;
using MatchVSDemo_WinFrom;
using UnityEngine;

namespace MatchVS {
	public class MatchVSEngine {
		public static Native.JsonRpc_CCallCsharp handler = new Native.JsonRpc_CCallCsharp();
		static MatchVSEngine() {
			Native.JsonRpc_onLoad();
		}
		public static List<MatchVSResponse> listener;

#if UNITY_IPHONE
    [MonoPInvokeCallback(typeof(Native.handleJsonString))]
#endif
		public static int handleJsonStringImp(IntPtr jsonMessage, int jsonMessageLen) {
			byte[] buf = new byte[jsonMessageLen];
			Marshal.Copy(jsonMessage, buf, 0, jsonMessageLen);

			string json = System.Text.Encoding.UTF8.GetString(buf);
			Console.WriteLine("handleJsonStringImp:" + json);
			if (listener != null) {
				foreach (MatchVSResponse item in listener) {
					item.callback(json);
				}
			}
			return 0;
		}

#if UNITY_IPHONE
    [MonoPInvokeCallback(typeof(Native.handleJsonString))]
#endif
		public static int handleByteImp(int action, IntPtr data, int dataLen) {
			Console.WriteLine("handleByteImp: dataLen" + dataLen);
			switch (action) {
				case 0:
					if (listener != null && dataLen > 0) {
						byte[] buffer = new byte[dataLen];
						Marshal.Copy(data, buffer, 0, dataLen);

						string payload = System.Text.Encoding.UTF8.GetString(buffer);
						MsBusiMsgRsp rsp = new MsBusiMsgRsp();
						rsp.payload = payload;
						rsp.payloadSize = payload.Length;
						foreach (MatchVSResponse item in listener) {
							item.busiMsgNotify(buffer, buffer.Length);
						}
					}
					break;
			}
			return 0;
		}

		public int init(MatchVSResponse[] pMatchVSResponse, string channel, string platform, int gameid) {
			try {
				handler.handleString = handleJsonStringImp;
				handler.handleByte = handleByteImp;
				Native.JsonRpc_regitCCallCsharp(ref handler);
				listener = new List<MatchVSResponse>();
				if (pMatchVSResponse != null) {
					for (int i = 0; i < pMatchVSResponse.Length; i++) {
						if (pMatchVSResponse[i] != null) {
							listener.Add(pMatchVSResponse[i]);
						}
					}
				}

				JsonData jo = new JsonData();
				jo["action"] = "init";
				jo["pMatchVSResponse"] = JsonMapper.ToObject(JsonUtil.toJson(pMatchVSResponse));
				jo["sChannel"] = channel;
				jo["sPlatform"] = platform;
				jo["iGameId"] = gameid;
				//			jo["pIsPushQueue"] = 1;

				JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
			} catch (Exception e) {
				Log.w(e);
			}
			return 0;
		}

		public int gerVersion() {
			JsonData jo = new JsonData();
			jo["action"] = "getVersion";
			return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		}

		public int uninit() {
			JsonData jo = new JsonData();
			jo["action"] = "uninit";
			return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		}

		public int registerUser() {
			JsonData jo = new JsonData();
			jo["action"] = "registerUser";
			return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		}

		public int login(int userid, string token, int gameid, int gameVersion, string appkey, string secretkey,
			string deviceid, int gatewayid) {
			JsonData jo = new JsonData();
			jo["action"] = "login";
			jo["iUserId"] = userid;
			jo["sToken"] = token;
			jo["iGameId"] = gameid;
			jo["iGameVersion"] = gameVersion;
			jo["pAppKey"] = appkey;
			jo["pSecretKey"] = secretkey;
			jo["pDeviceId"] = deviceid;
			jo["iGatewayId"] = gatewayid;
			return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		}

		public int logout() {
			JsonData jo = new JsonData();
			jo["action"] = "logout";
			return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		}

		public int createRoom(MsCreateRoomInfo roomInfo, string userProfile)
		{
		    string json = JsonMapper.ToJson(roomInfo);
			JsonData jo = JsonUtil.toObject(json);
		    jo["action"] = "createRoom";
            jo["userProfile"] = userProfile;
			return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		}

		public int joinRoom(string roomID,string userProfile)
		{
			JsonData jo = new JsonData();
		    jo["action"] = "joinRoom";
			jo["roomID"] = roomID;
			jo["userProfile"] = userProfile;
			return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		}

		public int joinRandomRoom(int iMaxPlayer, string strUserProfile) {
			JsonData jo = new JsonData();
			jo["action"] = "joinRandomRoom";
			jo["iMaxPlayer"] = iMaxPlayer;
			jo["sUserProfile"] = strUserProfile;
			return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		} 

		public int joinRoomWithProperties(MsMatchInfo matchInfo,string userProfile)
		{
		    JsonData jo = JsonUtil.toObject(JsonUtil.toJson(matchInfo));
		    jo["action"] = "joinRoomWithProperties";
            jo["userProfile"] = userProfile;
			return JsonRpc.JsonRpc_callNativeMethod(JsonUtil.toJson(jo));
		} 

		public int joinOver(string cpProto) {
			JsonData jo = new JsonData();
			jo["action"] = "joinOver";
			jo["cpProto"] = cpProto;
			return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		}

		public int getRoomList(MsRoomFilter roomFilter)
		{
		    string data = JsonMapper.ToJson(roomFilter);
		    JsonData jo = JsonUtil.toObject(data);
		    jo["action"] = "getRoomList";
			return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		}

		public int kickPlayer(int userID,string cpProto)
		{
			JsonData jo = new JsonData();
		    jo["action"] = "kickPlayer";
			jo["userID"] = userID;
			jo["cpProto"] = cpProto;
			return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		}

		public int subscribeEventGroup(string[] subGroups, string[] unsubGroups)
		{
			JsonData jo = new JsonData();
		    jo["action"] = "subscribeEventGroup";
			jo["subGroups"] = JsonUtil.toObject(JsonUtil.toJson(subGroups));
		    jo["unsubGroups"] = JsonUtil.toObject(JsonUtil.toJson(unsubGroups));
            return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		}

		public int sendEventGroup(string cpProto, string[] groups)
		{
			JsonData jo = new JsonData();
		    jo["action"] = "sendEventGroup";
			jo["cpProto"] = cpProto;
			jo["groups"] = JsonUtil.toObject(JsonUtil.toJson(groups));
		    Debug.Log("[sendEventGroup]: " + jo.ToJson());
            return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		}

		public int leaveRoom(String payload) {
			JsonData jo = new JsonData();
			jo["action"] = "leaveRoom";
			jo["cpProto"] = payload;
		    return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		}

		public int sendEvent(int iType, string pMsg, int iTargetType, int[] pTargetUserId) {
			JsonData jo = new JsonData();
			jo["action"] = "sendEvent";
			jo["iType"] = iType;
			jo["pMsg"] = pMsg;
			jo["iTargetType"] = iTargetType;
			jo["pTargetUserId"] = JsonUtil.toObject(JsonUtil.toJson(pTargetUserId));
			Debug.Log("[sendevent]: " + jo.ToJson());
		    return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
		}

		public int sendEvent(string pMsg) {
			JsonData jo = new JsonData();
			jo["action"] = "sendEvent";
			jo["pMsg"] = pMsg;
			Debug.Log("[sendevent]: " + jo.ToJson());
		    return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
        }

	    public int setTimestamp(int priority, bool enable)
	    {
            JsonData jo = new JsonData();
		    jo["action"] = "sendFrameEvent";
			jo["priority"] = priority;
	        jo["enable"] = enable;
	        return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
	    }

	    public int setFrameSync(int frameRate)
	    {
	        JsonData jo = new JsonData();
		    jo["action"] = "setFrameSync";
	        jo["frameRate"] = frameRate;
	        return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
	    }

	    public int sendFrameEvent(string cpProto)
	    {
            JsonData jo = new JsonData();
		    jo["action"] = "sendFrameEvent";
	        jo["cpProto"] = cpProto;
	        return JsonRpc.JsonRpc_callNativeMethod(jo.ToJson());
	    }
	}
}
