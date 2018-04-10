
using LitJson;
using MatchVS;
using UnityEngine;

public class GsMatchingContext : BaseContext {
	public GsMatchingContext() : base(UIType.GsMatchingBoard) {
	}
}

public class GsMatchingBoard : BaseView {

	public MatchingItem[] items;
	private MsRoomInfo roomInfo;
	private int currentCount = 0;
	private bool joinOver = false;

	private void OnEnable() {
		GameManager.Instance.joinRoomResponse += JoinRoomResponse;
		GameManager.Instance.leaveRoomResponse += LeaveRoomResponse;
		GameManager.Instance.roomPeerJoinRsp += JoinPeerNotify;
		GameManager.Instance.roomPeerLeaveRsp += LeavePeerNofity;
		GameManager.Instance.sendEventNotify += OnSendEventNotify;

		currentCount = 0;
	    timer = 0;
		joinOver = false;

		GameManager.Instance.ClearPlayer();
		GameManager.Instance.Gameover = false;
		GameManager.Instance.Gamestart = false;
		GameManager.Instance.RoomOwner = false;
	}

	public override void OnEnter() {
		gameObject.SetActive(true);
	}

	public override void OnExist() {
		gameObject.SetActive(false);
	}

	public override void OnResume() {
		gameObject.SetActive(true);
	}

	public override void OnPause() {
		gameObject.SetActive(false);
	}

    private float timer = 0;
    private void Update()
    {
        if (GameManager.Instance.RoomOwner && !joinOver)
        {
            if (timer < 9.5f)
            {
                timer += Time.deltaTime;
            }
            else
            {
                GameManager.JoinOver(roomInfo.roomID, "matchvs");
	            joinOver = true;
            }
        }
    }

    private void JoinRoomResponse(MsJoinRandomRsp tRsp) {
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
		for (int i = 0; i < infoList.Length; i++) {
			GameManager.Instance.AddRoomPlayer(new UserInfo(infoList[i].userId, false));

			items[i].UpdateInfo(infoList[i].userId);
		}
		items[currentCount].UpdateInfo(GameManager.userID);
		GameManager.Instance.AddRoomPlayer(new UserInfo(GameManager.userID, false));
		currentCount++;
	}

	private void LeaveRoomResponse(MsRoomLeaveRsp tRsp) {
		SingleTone<ContextManager>.Instance.Back();
	}

	public void LeaveRoom() {
		if (currentCount == 3)
			return;
		GameManager.LeaveRoom();
	}

	private void JoinPeerNotify(MsRoomPeerJoinRsp tRsp) {
		GameManager.Instance.AddRoomPlayer(new UserInfo(tRsp.userID, false));

		items[currentCount].UpdateInfo(tRsp.userID);
		currentCount++;

	    if (currentCount == 3)
	    {
            GameManager.JoinOver(roomInfo.roomID,"matchvs");
	    }
	}

	private void LeavePeerNofity(MsRoomPeerLeaveRsp tRsp) {
		int userID = tRsp.userID;
		for (int i = 0; i < items.Length; i++) {
			items[i].RemoveNotify(userID);
		}

		currentCount--;

		if (!GameManager.Instance.RoomOwner) {
			GameManager.Instance.RoomOwner = true;
			for (int i = 0; i < items.Length; i++) {
				items[i].RemoveNotify(GameManager.userID);
			}
			items[0].UpdateInfo(GameManager.userID);
		}

		GameManager.Instance.RemovePlayer(userID);
	}

	private void OnSendEventNotify(MsMsgNotify tRsp) {
		string payload = tRsp.cpProto;
		JsonData data = JsonUtil.toObject(payload);
		string action = (string)data["action"];

		if (action.Equals("gsRobot")) {
			int userid = (int)data["userid"];
			UserInfo user = new UserInfo(userid, true);
			GameManager.Instance.AddRoomPlayer(user);
			items[currentCount].UpdateInfo(userid);
			currentCount++;

		    if (currentCount == 3)
		    {
		        GameManager.JoinOver(roomInfo.roomID, "matchvs");
            }
		}

		if (action.Equals("gsReady")) {
			SingleTone<ContextManager>.Instance.ShowView(new GsGameRoomContext(), false);
		}
	}


	private void OnDisable() {
		GameManager.Instance.joinRoomResponse -= JoinRoomResponse;
		GameManager.Instance.leaveRoomResponse -= LeaveRoomResponse;
		GameManager.Instance.roomPeerJoinRsp -= JoinPeerNotify;
		GameManager.Instance.roomPeerLeaveRsp -= LeavePeerNofity;
		GameManager.Instance.sendEventNotify -= OnSendEventNotify;
	}
}
