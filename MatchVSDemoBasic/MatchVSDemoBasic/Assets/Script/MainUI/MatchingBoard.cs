using System.Collections;
using System.Collections.Generic;
using System.Threading;
using LitJson;
using MatchVS;
using UnityEngine;

public class MatchingContext : BaseContext
{
    public MatchingContext() : base(UIType.MatchingBoard)
    {
    }
}

public class MatchingBoard : BaseView
{
    public MatchingItem[] items;
    private MsRoomInfo roomInfo;
    private int currentCount = 0;

	private float matchTime;

    private void OnEnable()
    {
        GameManager.Instance.joinRoomResponse += JoinRoomResponse;
        GameManager.Instance.leaveRoomResponse += LeaveRoomResponse;
        GameManager.Instance.roomPeerJoinRsp += JoinPeerNotify;
        GameManager.Instance.roomPeerLeaveRsp += LeavePeerNofity;
        GameManager.Instance.roomJoinOverRsp += JoinOver;
	    GameManager.Instance.sendEventNotify += OnSendEventNotify;
        currentCount = 0;

        GameManager.Instance.ClearPlayer();
        GameManager.Instance.Gameover = false;
        GameManager.Instance.Gamestart = false;
        GameManager.Instance.RoomOwner = false;

	    matchTime = 0;
    }

	private void Update()
	{
		if(!GameManager.Instance.ContainRobot)
			return;
        //匹配时间判断
		if (matchTime >= 3)
		{
            //房主身份判定
			if (!GameManager.Instance.RoomOwner)
				return;
            //当前房间人数判定
			if (currentCount < 3)
			{
                //确定剩余人数
				int cout = 3 - currentCount;
				int userid;
				for (int i = 0; i < cout; i++)
				{   
                    //随机创建用户ID
					userid = Random.Range(10000, 100000000);
					GameManager.Instance.AddRoomPlayer(new UserInfo(userid,true));

                    items[currentCount].UpdateInfo(userid);

                    //创建负载消息（cpProto），
					JsonData data = new JsonData();
					data["action"] = "robot";
					data["userid"] = userid;
					string value = data.ToJson();
                    //向其他玩家发送机器人消息
					GameManager.SendEvent(value,new int[] {GameManager.userID});

					currentCount++;
				}
	
			    StartCoroutine(ShowGameRoom());
			    GameManager.JoinOver(roomInfo.roomID.ToString(), roomInfo.roomProperty);
            }
        }
		else
		{
            //时间统计
		    matchTime += Time.deltaTime;
        }
    }

	public override void OnEnter()
    {
        gameObject.SetActive(true);
    }

    public override void OnExist()
    {
        gameObject.SetActive(false);
    }

    public override void OnResume()
    {
        gameObject.SetActive(true);
    }

    public override void OnPause()
    {
        gameObject.SetActive(false);
    }

    private void JoinRoomResponse(MsJoinRandomRsp tRsp)
    {
	    if (tRsp.status != 200) {
			SingleTone<ContextManager>.Instance.ShowView(new GameLobbyContext(), false);
		    return;
	    }

		if (tRsp.userInfoList.Length == 0)
            GameManager.Instance.RoomOwner = true;

        GameManager.Instance.RoomID = tRsp.roomInfo.roomID.ToString();

        roomInfo = tRsp.roomInfo;
        currentCount += tRsp.userInfoList.Length;
        MsUserInfoList[] infoList = tRsp.userInfoList;
        for (int i = 0; i < infoList.Length; i++)
        {
            GameManager.Instance.AddRoomPlayer(new UserInfo(infoList[i].userId,false));

            items[i].UpdateInfo(infoList[i].userId);
        }
        items[currentCount].UpdateInfo(GameManager.userID);
        GameManager.Instance.AddRoomPlayer(new UserInfo(GameManager.userID,false));
        currentCount++;
        if (currentCount == 3)
        {
			StartCoroutine(ShowGameRoom());
        }
    }

	private void OnSendEventNotify(MsMsgNotify tRsp)
	{
		string payload = tRsp.cpProto;
		JsonData data = JsonUtil.toObject(payload);
		string action = (string) data["action"];
		if (action.Equals("robot"))
		{
			int userid = (int)data["userid"];
			GameManager.Instance.AddRoomPlayer(new UserInfo(userid,true));
			currentCount++;
		    if (currentCount == 3)
		    {
				StartCoroutine(ShowGameRoom());
			}
		}
	}


	public void LeaveRoom()
    {
		if(currentCount==3)
			return;
        GameManager.LeaveRoom();
    }

    private void LeaveRoomResponse(MsRoomLeaveRsp tRsp)
    {
        SingleTone<ContextManager>.Instance.Back();
    }

    private void JoinPeerNotify(MsRoomPeerJoinRsp tRsp)
    {
        GameManager.Instance.AddRoomPlayer(new UserInfo(tRsp.userID,false));

        items[currentCount].UpdateInfo(tRsp.userID);
        currentCount++;
        if (currentCount == 3)
        {
	        if (GameManager.Instance.RoomOwner)
	        {
		        GameManager.JoinOver(roomInfo.roomID.ToString(), roomInfo.roomProperty);
		        SingleTone<ContextManager>.Instance.ShowView(new GameRoomContext(), false);
			}
	        else
	        {
				StartCoroutine(ShowGameRoom());
			}
        }
    }

    private void LeavePeerNofity(MsRoomPeerLeaveRsp tRsp)
    {
        int userID = tRsp.userID;
        for (int i = 0; i < items.Length; i++)
        {
            items[i].RemoveNotify(userID);
        }

        currentCount--;

        if (!GameManager.Instance.RoomOwner)
        {
            GameManager.Instance.RoomOwner = true;
            for (int i = 0; i < items.Length; i++) {
                items[i].RemoveNotify(GameManager.userID);
            }
            items[0].UpdateInfo(GameManager.userID);
        }

        GameManager.Instance.RemovePlayer(userID);
    }

    private void JoinOver(MsRoomJoinOverRsp tRsp)
    {
        //  StartCoroutine(ShowGameRoom());
    }

    private IEnumerator ShowGameRoom()
    {
        yield return new WaitForSeconds(0.2f);
        SingleTone<ContextManager>.Instance.ShowView(new GameRoomContext(), false);
    }

    private void OnDisable()
    {
        GameManager.Instance.joinRoomResponse -= JoinRoomResponse;
        GameManager.Instance.leaveRoomResponse -= LeaveRoomResponse;
        GameManager.Instance.roomPeerJoinRsp -= JoinPeerNotify;
        GameManager.Instance.roomPeerLeaveRsp -= LeavePeerNofity;
        GameManager.Instance.roomJoinOverRsp -= JoinOver;
	    GameManager.Instance.sendEventNotify -= OnSendEventNotify;
	}
}

