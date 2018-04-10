using System.Collections;
using System.Collections.Generic;
using LitJson;
using MatchVS;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomContext : BaseContext {
	public CreateRoomContext() : base(UIType.CreateRoomBoard) {
	}
}

public class CreateRoomBoard : BaseView {
	public Text roomID;
	public CreateRoomPlayer[] roomPlayer;
	public Button startGame;

	private int currentCount = 0;

	private string roomNum;
	private void OnEnable() {
		currentCount = 0;
		roomNum = "";
		startGame.gameObject.SetActive(false);

		GameManager.Instance.createRoomRsp += CreateRoomRsp;
		GameManager.Instance.leaveRoomResponse += LeaveRoomRsp;
		GameManager.Instance.joinRoomResponse += JoinRoomRsp;
		GameManager.Instance.roomPeerJoinRsp += JoinRoomPeerRsp;
		GameManager.Instance.roomPeerLeaveRsp += LeavePeerNofity;
		GameManager.Instance.kickPlayerRsp += RefreshPlayerInfo;
		GameManager.Instance.kickPlayerNotify += KickPlayerNotify;

		GameManager.Instance.sendEventNotify += OnSendEventNotify;

		GameManager.Instance.ClearPlayer();
		GameManager.Instance.RoomOwner = false;
		GameManager.Instance.Gameover = false;
		GameManager.Instance.Gamestart = false;
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

	private void CreateRoomRsp(MsCreateRoomRsp tRsp) {

		GameManager.Instance.RoomOwner = true;
		roomID.text = tRsp.roomID;
		roomPlayer[currentCount].UpdateInfo(GameManager.userID);

		startGame.gameObject.SetActive(true);

		currentCount++;

		GameManager.Instance.AddRoomPlayer(new UserInfo(GameManager.userID, false));
	}

	private void LeaveRoomRsp(MsRoomLeaveRsp tRsp) {
		SingleTone<ContextManager>.Instance.ShowView(new GameLobbyContext(), false);
	}

	public void JoinRoomRsp(MsJoinRandomRsp tRsp) {
		if (tRsp.status != 200)
		{
			StartCoroutine(JoinError());
			return;
		}
		roomID.text = tRsp.roomInfo.roomID.ToString();
		MsUserInfoList[] infoList = tRsp.userInfoList;
		for (int i = 0; i < infoList.Length; i++) {
			GameManager.Instance.AddRoomPlayer(new UserInfo(infoList[i].userId, false));

			roomPlayer[i].UpdateInfo(infoList[i].userId);
		}
		currentCount = infoList.Length;

		roomPlayer[currentCount].UpdateInfo(GameManager.userID);
		currentCount++;
		GameManager.Instance.AddRoomPlayer(new UserInfo(GameManager.userID, false));
	}

	public void JoinRoomPeerRsp(MsRoomPeerJoinRsp tRsp) {
		roomPlayer[currentCount].UpdateInfo(tRsp.userID);
		currentCount++;
		GameManager.Instance.AddRoomPlayer(new UserInfo(tRsp.userID, false));
	}

	private void LeavePeerNofity(MsRoomPeerLeaveRsp tRsp) {
		int userID = tRsp.userID;
		OnPlayerLeaveRoom(userID);
	}

	private void KickPlayerNotify(MsKickPlayerNotify tRsp) {
		int userID = tRsp.userID;

		if (userID == GameManager.userID) {
			SingleTone<ContextManager>.Instance.ShowView(new GameLobbyContext(), false);
			return;
		}

		OnPlayerLeaveRoom(userID);
	}

	private void OnPlayerLeaveRoom(int userID) {
		GameManager.Instance.RemovePlayer(userID);

		RefreshPlayerInfo();
	}

	private IEnumerator JoinError()
	{
		GameManager.ShowTip("加入房间失败");
		yield return new WaitForSeconds(1.5f);
		SingleTone<ContextManager>.Instance.ShowView(new JoinSpecifiedRoomContext(), false);
	}

	private void RefreshPlayerInfo() {
		currentCount--;
		GameManager.Instance.RoomOwner = false;
		for (int i = 0; i < roomPlayer.Length; i++) {
			roomPlayer[i].ResetInfo();
		}
		for (int i = 0; i < GameManager.Instance.RoomPlayers.Count; i++) {
			if (i == 0) {
				if (GameManager.userID == GameManager.Instance.RoomPlayers[i].userid) {
					GameManager.Instance.RoomOwner = true;
				}
			}
			roomPlayer[i].UpdateInfo(GameManager.Instance.RoomPlayers[i].userid);
		}

		if (GameManager.Instance.RoomOwner)
			startGame.gameObject.SetActive(true);
		else
			startGame.gameObject.SetActive(false);
	}

	private void OnSendEventNotify(MsMsgNotify tRsp) {
		string payload = tRsp.cpProto;
		JsonData data = JsonUtil.toObject(payload);
		string action = (string)data["action"];
		if (action.Equals("toGameBoard")) {
			StartCoroutine(StartGame());
		}
	}

	private IEnumerator StartGame() {
		yield return new WaitForSeconds(0.2f);
		SingleTone<ContextManager>.Instance.ShowView(new GameRoomContext(), false);
	}

	public void OnStart() {
		if (GameManager.Instance.RoomPlayers.Count != 3)
			return;
		JsonData data = new JsonData();
		data["action"] = "toGameBoard";
		string value = data.ToJson();
		GameManager.SendEvent(value, new int[] { GameManager.userID });

		GameManager.JoinOver(roomNum, "matchvs");

		SingleTone<ContextManager>.Instance.ShowView(new GameRoomContext(), false);
	}

	public void Back() {
		GameManager.LeaveRoom();
	}

	private void OnDisable() {
		GameManager.Instance.createRoomRsp -= CreateRoomRsp;
		GameManager.Instance.leaveRoomResponse -= LeaveRoomRsp;
		GameManager.Instance.joinRoomResponse -= JoinRoomRsp;
		GameManager.Instance.roomPeerJoinRsp -= JoinRoomPeerRsp;
		GameManager.Instance.roomPeerLeaveRsp -= LeavePeerNofity;
		GameManager.Instance.kickPlayerRsp -= RefreshPlayerInfo;
		GameManager.Instance.kickPlayerNotify -= KickPlayerNotify;
		GameManager.Instance.sendEventNotify -= OnSendEventNotify;
	}
}

