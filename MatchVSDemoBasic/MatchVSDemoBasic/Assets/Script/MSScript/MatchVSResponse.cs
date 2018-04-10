using System;
using LitJson;
using UnityEngine;


namespace MatchVS
{
    public abstract class MatchVSResponse
    {
        private string TAG = "post-MatchVSResponse-c#";
        public abstract int busiMsgNotify(byte[] data, int dataLen);
        public abstract int loginResponse(MsLoginRsp tRsp);
        public abstract int logoutResponse(MsLogoutRsp tRsp);
	    public abstract int createRoomResponse(MsCreateRoomRsp tRsp);
        public abstract int joinRoomResponse(MsJoinRandomRsp tRsp);
        public abstract int joinOverResponse(MsRoomJoinOverRsp tRsp);
	    public abstract int getRoomListResponse(MsRoomListRsp tRsp);
        public abstract int leaveRoomResponse(MsRoomLeaveRsp tRsp);
        public abstract int joinRoomNotify(MsRoomPeerJoinRsp tRsp);
	    public abstract int kickPlayerResponse(int status);
	    public abstract int kickPlayerNotify(MsKickPlayerNotify tRsp);
	    public abstract int subscribeEventGroupRsp(MsSubscribeEventGroupRsp tRsp);
	    public abstract int sendEventGroupRsp(MsSendEventGroupRsp tRsp);
	    public abstract int sendEventGroupNotify(MsSendEventGroupNotify tRsp);
        public abstract int leaveRoomNotify(MsRoomPeerLeaveRsp tRsp);
        public abstract int registerUserResponse(MsRegisterUserRsp tRsp);
        public abstract int sendEventResponse(int status);
        public abstract int sendEventNotify(MsMsgNotify tRsp);
        public abstract int setTimestampResponse(MsSetTimestampRsp tRsp);
        public abstract int setFrameSyncResponse(int status);
        public abstract int setFrameSyncNotify(MsSetChannelFrameSyncNotify tRsp);
        public abstract int sendFrameEventResponse(int status);
        public abstract int frameUpdate(MsFrameData tRsp);
        public abstract int errorResponse(string error);
        public int callback(string source)
        {
            Log.i(TAG, "callback: " + source);
            Debug.Log("[source]: " +source);
            try
            {
                JsonData jo = JsonUtil.toObject(source);
                string methodName = (string)jo["action"];

                if (methodName.Equals("loginResponse"))
                {
                    MsLoginRsp tRsp = JsonMapper.ToObject<MsLoginRsp>(jo["rsp"].ToJson());
                    if (tRsp.status == 200)
                    {
                        loginResponse(tRsp);
                    }
                    else
                    {
                        errorResponse(tRsp.status.ToString());
                    }
                    return 0;
                }

                if (methodName.Equals("logoutResponse")) 
                {
                    MsLogoutRsp tRsp = JsonMapper.ToObject<MsLogoutRsp>(jo["rsp"].ToJson());
                    if (tRsp.status == 200)
                    {
                        logoutResponse(tRsp);
                    }
                    return 0;
                }

	            if (methodName.Equals("createRoomResponse"))
	            {
		            MsCreateRoomRsp tRsp = JsonMapper.ToObject<MsCreateRoomRsp>(jo["rsp"].ToJson());
	                if (tRsp.status == 200)
	                {
	                    createRoomResponse(tRsp);
	                }
	                return 0;
	            }

	            if (methodName.Equals("roomJoinResponse"))
                {
                    MsJoinRandomRsp tRsp = JsonMapper.ToObject<MsJoinRandomRsp>(jo["rsp"].ToJson());
                    joinRoomResponse(tRsp);
                    return 0;
                }

                if (methodName.Equals("roomJoinOverResponse")) {
                    MsRoomJoinOverRsp tRsp = JsonMapper.ToObject<MsRoomJoinOverRsp>(jo["rsp"].ToJson());
                    if (tRsp.status == 200)
                    {
                        joinOverResponse(tRsp);
                    }
                    return 0;
                }

	            if (methodName.Equals("getRoomListResponse"))
	            {
		            MsRoomListRsp tRsp = JsonMapper.ToObject<MsRoomListRsp>(jo["rsp"].ToJson());
		            getRoomListResponse(tRsp);
		            return 0;
	            }

	            if (methodName.Equals("kickPlayerRsp"))
	            {
		            JsonData tRsp = jo["rsp"];
		            int status = (int) tRsp["status"];
		            kickPlayerResponse(status);
		            return 0;
	            }

	            if (methodName.Equals("kickPlayerNotify"))
	            {
		            MsKickPlayerNotify tRsp = JsonMapper.ToObject<MsKickPlayerNotify>(jo["rsp"].ToJson());
		            kickPlayerNotify(tRsp);
		            return 0;
	            }

	            if (methodName.Equals("subscribeEventGroupResponse"))
	            {
		            MsSubscribeEventGroupRsp tRsp = JsonMapper.ToObject<MsSubscribeEventGroupRsp>(jo["rsp"].ToJson());
		            subscribeEventGroupRsp(tRsp);
		            return 0;
	            }

	            if (methodName.Equals("sendEventGroupRsp"))
	            {
		            MsSendEventGroupRsp tRsp = JsonMapper.ToObject<MsSendEventGroupRsp>(jo["rsp"].ToJson());
		            sendEventGroupRsp(tRsp);
		            return 0;
	            }

	            if (methodName.Equals("sendEventGroupNotify"))
	            {
		            MsSendEventGroupNotify tRsp = JsonMapper.ToObject<MsSendEventGroupNotify>(jo["rsp"].ToJson());
		            sendEventGroupNotify(tRsp);
		            return 0;
	            }

	            if (methodName.Equals("roomLeaveResponse")) {
                    MsRoomLeaveRsp tRsp = JsonMapper.ToObject<MsRoomLeaveRsp>(jo["rsp"].ToJson());
                    if (tRsp.status == 200)
                    {
                        leaveRoomResponse(tRsp);
                    }
                    return 0;
                }

                if (methodName.Equals("joinRoomNotify")) {
                    MsRoomPeerJoinRsp tRsp = JsonMapper.ToObject<MsRoomPeerJoinRsp>(jo["rsp"].ToJson());
                    joinRoomNotify(tRsp);
                    return 0;
                }

                if (methodName.Equals("leaveRoomNotify"))
                {
                    MsRoomPeerLeaveRsp tRsp = JsonMapper.ToObject<MsRoomPeerLeaveRsp>(jo["rsp"].ToJson());
                    leaveRoomNotify(tRsp);
                    return 0;
                }

                if (methodName.Equals("registerUserResponse"))
                {
                    MsRegisterUserRsp tRsp = JsonMapper.ToObject<MsRegisterUserRsp>(jo["rsp"].ToJson());
                    registerUserResponse(tRsp);
                    return 0;
                }

                if (methodName.Equals("sendEventNotify"))
                {
                    MsMsgNotify tRsp = JsonMapper.ToObject<MsMsgNotify>(jo["rsp"].ToJson());
                    sendEventNotify(tRsp);
                    return 0;
                }

                if (methodName.Equals("sendEventRsp"))
                {
                    sendEventResponse((int) jo["rsp"]["status"]);
                    return 0;
                }

                if (methodName.Equals("setTimestampResponse"))
                {
                    MsSetTimestampRsp tRsp = JsonMapper.ToObject<MsSetTimestampRsp>(jo["rsp"].ToJson());
                    setTimestampResponse(tRsp);
                    return 0;
                }

                if (methodName.Equals("setFrameSyncResponse"))
                {
                    setFrameSyncResponse((int) jo["rsp"]["status"]);
                    return 0;
                }

                if (methodName.Equals("setFrameSyncNotify"))
                {
                    MsSetChannelFrameSyncNotify tRsp =
                        JsonMapper.ToObject<MsSetChannelFrameSyncNotify>(jo["rsp"].ToJson());
                    setFrameSyncNotify(tRsp);
                    return 0;
                }

                if (methodName.Equals("sendFrameEventResponse"))
                {
                    sendFrameEventResponse((int) jo["rsp"]["status"]);
                    return 0;
                }

                if (methodName.Equals("frameUpdate"))
                {
                    MsFrameData tRsp = JsonMapper.ToObject<MsFrameData>(jo["rsp"].ToJson());
					frameUpdate(tRsp);
                    return 0;
                }

                if (methodName.Equals("errorResponse"))
                {
                    String error = (String)jo["sError"];
                    errorResponse(error);
                    return 0;
                }

            }
            catch (Exception e)
            {
               Log.w(e);
            }
            return 0;
        }
    }
}

