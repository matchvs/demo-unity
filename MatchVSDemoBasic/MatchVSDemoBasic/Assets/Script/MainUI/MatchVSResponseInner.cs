using System.Collections;
using System.Collections.Generic;
using LitJson;
using MatchVS;
using UnityEngine;

public class MatchVSResponseInner : MatchVSResponse {
    public override int busiMsgNotify(byte[] data, int dataLen)
    {
        return 0;
    }

    public override int loginResponse(MsLoginRsp tRsp) {
       
        Loom.QueueOnMainThread(() =>
        {
            Debug.Log("LoginResponse: " + JsonUtil.toJson(tRsp));
            if (GameManager.Instance.loginResponse != null)
                GameManager.Instance.loginResponse(tRsp);
        });
		return 0;
	}

	public override int logoutResponse(MsLogoutRsp tRsp)
	{
	    Loom.QueueOnMainThread(() =>
	    {
	        if (GameManager.Instance.logoutResp != null)
	            GameManager.Instance.logoutResp(tRsp);
	    });
		return 0;
	}

	public override int createRoomResponse(MsCreateRoomRsp tRsp)
	{
        Debug.Log("creatRoomResponse: " + JsonUtil.toJson(tRsp));
        Loom.QueueOnMainThread(() =>
        {
            if (GameManager.Instance.createRoomRsp != null)
                GameManager.Instance.createRoomRsp(tRsp);
        });
		return 0;
	}

	public override int joinRoomResponse(MsJoinRandomRsp tRsp)
    {
        Loom.QueueOnMainThread(() => {
            if (GameManager.Instance.joinRoomResponse != null)
                GameManager.Instance.joinRoomResponse(tRsp);
        });
        return 0;
    }

    public override int joinOverResponse(MsRoomJoinOverRsp tRsp) {
        Debug.Log("joinOverResponse: " + JsonUtil.toJson(tRsp));
        Loom.QueueOnMainThread(() =>
        {
            if (GameManager.Instance.roomJoinOverRsp != null)
                GameManager.Instance.roomJoinOverRsp(tRsp);
        });
		return 0;
	}

	public override int getRoomListResponse(MsRoomListRsp tRsp)
	{
        Debug.Log("getRoomListResponse: " + JsonUtil.toJson(tRsp));
        Loom.QueueOnMainThread(() =>
        {
            if (GameManager.Instance.roomListRsp != null)
                GameManager.Instance.roomListRsp(tRsp);
        });
		return 0;
	}

	public override int leaveRoomResponse(MsRoomLeaveRsp tRsp) {
        Debug.Log("leaveRoomResponse: " + JsonUtil.toJson(tRsp));
        Loom.QueueOnMainThread(() =>
        {
            if (GameManager.Instance.leaveRoomResponse != null)
                GameManager.Instance.leaveRoomResponse(tRsp);
        });
		return 0;
	}

	public override int joinRoomNotify(MsRoomPeerJoinRsp tRsp) {
        Debug.Log("MsRoomPeerJoinRsp: " + JsonUtil.toJson(tRsp));
        Loom.QueueOnMainThread(() =>
        {
            if (GameManager.Instance.roomPeerJoinRsp != null)
                GameManager.Instance.roomPeerJoinRsp(tRsp);
        });
		return 0;
	}

	public override int kickPlayerResponse(int status)
	{
        Debug.Log("kickPlayerResponse: " + status);
        Loom.QueueOnMainThread(() =>
        {
            if (GameManager.Instance.kickPlayerRsp != null)
                GameManager.Instance.kickPlayerRsp();
        });
		return 0;
	}

	public override int kickPlayerNotify(MsKickPlayerNotify tRsp)
	{
		Debug.Log("kickPlayerNotify: " + JsonUtil.toJson(tRsp));
		Loom.QueueOnMainThread(() =>
		{
			if (GameManager.Instance.kickPlayerNotify != null)
				GameManager.Instance.kickPlayerNotify(tRsp);
		});
		return 0;
	}

	public override int subscribeEventGroupRsp(MsSubscribeEventGroupRsp tRsp)
	{
        Debug.Log("subscribeEventGroupRsp: " + JsonUtil.toJson(tRsp));
	    Loom.QueueOnMainThread(() => {
	        if (GameManager.Instance.subscribeEventGroupRsp != null)
	            GameManager.Instance.subscribeEventGroupRsp(tRsp);
	    });
        return 0;
	}

	public override int sendEventGroupRsp(MsSendEventGroupRsp tRsp)
	{
        Debug.Log("SendEventGroupRsp：" + JsonUtil.toJson(tRsp));
        Loom.QueueOnMainThread(() =>
        {
            if (GameManager.Instance.sendEventGroupRsp != null)
                GameManager.Instance.sendEventGroupRsp(tRsp);
        });
		return 0;
	}

	public override int sendEventGroupNotify(MsSendEventGroupNotify tRsp)
	{
        Debug.Log("sendEventGroupNotify: " + JsonUtil.toJson(tRsp));
        Loom.QueueOnMainThread(() =>
        {
            if (GameManager.Instance.sendEventGroupNotify != null)
                GameManager.Instance.sendEventGroupNotify(tRsp);
        });
		return 0;
	}

	public override int leaveRoomNotify(MsRoomPeerLeaveRsp tRsp) {
        Debug.Log("MsRoomPeerLeaveRsp: " + JsonUtil.toJson(tRsp));
        Loom.QueueOnMainThread(() =>
        {
            if (GameManager.Instance.roomPeerLeaveRsp != null)
                GameManager.Instance.roomPeerLeaveRsp(tRsp);
        });
		return 0;
	}

    public override int registerUserResponse(MsRegisterUserRsp tRsp)
    {
        Debug.Log("registerUserResponse: " +JsonUtil.toJson(tRsp));
        Loom.QueueOnMainThread(() =>
        {
            if (GameManager.Instance.registResponse != null)
                GameManager.Instance.registResponse(tRsp);
        });
        return 0;
    }

    public override int sendEventResponse(int status)
    {
        return 0;
    }

    public override int sendEventNotify(MsMsgNotify tRsp)
    {
        Loom.QueueOnMainThread(() =>
        {
            if (GameManager.Instance.sendEventNotify != null)
            {
                GameManager.Instance.sendEventNotify(tRsp);
            }
        });
        return 0;
    }

    public override int setTimestampResponse(MsSetTimestampRsp tRsp)
    {
        return 0;
    }

    public override int setFrameSyncResponse(int status)
    {
        return 0;
    }

    public override int setFrameSyncNotify(MsSetChannelFrameSyncNotify tRsp)
    {
        return 0;
    }

    public override int sendFrameEventResponse(int status)
    {
        return 0;
    }

    public override int frameUpdate(MsFrameData tRsp)
    {
	    Loom.QueueOnMainThread(() => {
		    if (GameManager.Instance.FrameUpdate != null) {
				GameManager.Instance.FrameUpdate(tRsp);
		    }
	    });
		return 0;
    }

    public override int errorResponse(string error)
    {
        Debug.Log("异常内容为: " + error);
        return 0;
    }
}

