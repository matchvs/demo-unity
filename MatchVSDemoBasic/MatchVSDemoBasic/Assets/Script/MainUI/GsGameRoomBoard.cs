using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using LitJson;
using MatchVS;
using UnityEngine;
using UnityEngine.Experimental.Director;
using UnityEngine.UI;


public class GsGameRoomContext : BaseContext {
	public GsGameRoomContext() : base(UIType.GsGameRoomBoard) {
	}
}

public class GsGameRoomBoard : BaseView {
	public RoomPlayer[] players;
	public Text detail;
	public Text roomID;
	public Transform road;
	private List<int> readyPlayers = new List<int>();
	private List<PlayerDis> playerDis = new List<PlayerDis>();
	private List<GameObject> rewards = new List<GameObject>();
	private GameObject target;
	private bool specialArea;
	private int currentCount = 0;


	private void OnEnable() {
		specialArea = false;
		Message("");
		detail.text = "";

		GameManager.Instance.RoomPlayers.Sort((a, b) => a.userid.CompareTo(b.userid));

		for (int i = 0; i < GameManager.Instance.RoomPlayers.Count; i++) {

			players[i].UpdateInfo(GameManager.Instance.RoomPlayers[i]);
			if (GameManager.Instance.RoomPlayers[i].userid == GameManager.userID) {
				target = players[i].Target;
			}
		}
		Debug.Log("房间人数: " + GameManager.Instance.RoomPlayers.Count);

		GameManager.Instance.leaveRoomResponse += OnLeaveRoomResponse;
		GameManager.Instance.sendEventNotify += OnSendEventNotify;
		GameManager.Instance.roomPeerLeaveRsp += LeavePeerNofity;
		GameManager.Instance.sendEventGroupNotify += OnSendEventGroupNotify;
		GameManager.Instance.subscribeEventGroupRsp += SubscribeEventGroupRsp;

		GameManager.Instance.ClearAll();
		readyPlayers.Clear();
		playerDis.Clear();

		roomID.text = "房间号: " + GameManager.Instance.GameID;

		currentCount = 3;

		//send GsReady info
		JsonData jo = new JsonData();
		jo["action"] = "gsReadyRsp";
		string value = jo.ToJson();
		GameManager.SendEvent(1,value, new int[] { GameManager.userID });
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

	private void OnDisable() {
		GameManager.Instance.leaveRoomResponse -= OnLeaveRoomResponse;
		GameManager.Instance.sendEventNotify -= OnSendEventNotify;
		GameManager.Instance.roomPeerLeaveRsp -= LeavePeerNofity;
		GameManager.Instance.sendEventGroupNotify -= OnSendEventGroupNotify;
		GameManager.Instance.subscribeEventGroupRsp -= SubscribeEventGroupRsp;

		while (rewards.Count > 0)
		{
			GameObject go = rewards[0];
			DestroyImmediate(go);
			rewards.RemoveAt(0);
		}
	}

	private void OnLeaveRoomResponse(MsRoomLeaveRsp tRsp) {
		SingleTone<ContextManager>.Instance.ShowView(new GameLobbyContext(), false);
	}

	private void SubscribeEventGroupRsp(MsSubscribeEventGroupRsp trsp) {
		if (specialArea) {
			JsonData data = new JsonData();
			data["action"] = "enter";
			data["id"] = GameManager.userID;
			string value = JsonUtil.toJson(data);
			GameManager.SendEventGroup(value, new string[] { "specialArea" });
            Message("进入特殊区域");
		}
	}

	private void OnSendEventNotify(MsMsgNotify tRsp) {
		string payload = tRsp.cpProto;
		JsonData data = JsonUtil.toObject(payload);
		string action = (string)data["action"];

		if (action.Equals("gsStart")) {
			JsonData resources = data["rewards"];
			for (int i = 0; i < resources.Count; i++) {
				int x = (int)resources[i]["x"];
				int y = (int)resources[i]["y"];

				//Creat Reward
				GameObject go = Resources.Load<GameObject>("Reward");
				GameObject target = Instantiate(go, road, false);
				target.GetComponent<RectTransform>().localPosition = new Vector3(x,y);
				Reward reward = target.GetComponent<Reward>();
				reward.UpdateInfo(i);
				rewards.Add(target);
			}

			GameManager.Instance.Gamestart = true;
			Message("游戏开始");
		}

		if (action.Equals("gsGameover"))
		{
			if(GameManager.Instance.Gameover)
				return;
			GameManager.Instance.Gameover = true;
			JsonData info = new JsonData();
			info["action"] = "gsScore";
			info["score"] = (int)GetPlayerInfoByID(GameManager.userID).mileageValue;
			info["rewardNum"] = GetPlayerInfoByID(GameManager.userID).rewardNum;
			if (GameManager.Instance.RoomOwner)
			{
				info["roomOwner"] = true;
				JsonData list = new JsonData();
				int robotCount = 0;
				for (int i = 0; i < GameManager.Instance.RoomPlayers.Count; i++)
				{
					if (GameManager.Instance.RoomPlayers[i].robot)
					{
						JsonData robotSocre = new JsonData();
						int robotUserID = GameManager.Instance.RoomPlayers[i].userid;
						robotSocre["userid"] = robotUserID;
						robotSocre["score"] = (int) GetPlayerInfoByID(robotUserID).mileageValue;
						robotSocre["rewardNum"] = GetPlayerInfoByID(robotUserID).rewardNum;

						list.Add(robotSocre);
						robotCount++;
					}
				}

				if (robotCount > 0)
				{
					info["robotScore"] = list;
				}
				else
				{
					info["roomOwner"] = false;
				}
			}
			else
			{
				info["roomOwner"] = false;
			}
			string value = info.ToJson();
			GameManager.SendEvent(1,value,new int[] {GameManager.userID});
		}

		if (action.Equals("gsRewardRsp"))
		{
			int rewardID = (int)data["rewardID"];
			int userid = (int) data["userID"];

			rewards[rewardID].SetActive(false);

			GetPlayerInfoByID(userid).GetReward();

            Message("玩家" + userid + "吃到了资源点,+1金币。");
		}

		if (action.Equals("gsResult")) {
//			int first = (int)data["first"];
//			int second = (int)data["second"];
//			int third = (int)data["third"];

//			GameManager.Instance.AddUserID(first);
//			GameManager.Instance.AddUserID(second);
//			GameManager.Instance.AddUserID(third);

			JsonData list = data["resultList"];
			for (int i = 0; i < list.Count; i++)
			{
				JsonData playerInfo = list[i];
				int userID = (int)playerInfo["userid"];
			    int rewardNum = (int) playerInfo["rewardNum"];
				GameManager.Instance.AddUserID(new PlayerResult() {userid = userID,rewardNum = rewardNum});
			}

			SingleTone<ContextManager>.Instance.ShowView(new GameOverContext(), false);
		}

		if (action.Equals("roomLeaveRoom")) {
			int gameowner = (int)data["RoomOwner"];
			if (GameManager.userID == gameowner) {
				GameManager.Instance.RoomOwner = true;
			}
		}

		if (action.Equals("AccelerateDown")) {
			AccelerateDown(tRsp.srcUserID);
			Message(tRsp.srcUserID + "加速");
		}

		if (action.Equals("AccelerateUp")) {
			AccelerateUp(tRsp.srcUserID);
			Message(tRsp.srcUserID + "减速");
		}
		if (action.Equals("RightDown")) {
			RightDown(tRsp.srcUserID);
			Message(tRsp.srcUserID + "开始向右");
		}
		if (action.Equals("RightUp")) {
			RightUp(tRsp.srcUserID);
			Message(tRsp.srcUserID + "停止向右");
		}
		if (action.Equals("LeftDown")) {
			LeftDown(tRsp.srcUserID);
			Message(tRsp.srcUserID + "开始向左");
		}
		if (action.Equals("LeftUp")) {
			LeftUp(tRsp.srcUserID);
			Message(tRsp.srcUserID + "停止向左");
		}
	}

	private void OnSendEventGroupNotify(MsSendEventGroupNotify tRsp) {
		string payload = tRsp.cpProto;
		JsonData data = JsonUtil.toObject(payload);
		string action = (string)data["action"];
		if (action.Equals("enter")) {
			int id = (int)data["id"];
			GameObject target = GetPlayerInfoByID(id).Target;
			target.GetComponent<CharacterMove>().OnEnter();

			data = new JsonData();
			data["action"] = "enterResponse";
			data["id"] = GameManager.userID;
			string value = JsonUtil.toJson(data);
			GameManager.SendEventGroup(value, new string[] { "specialArea" });

		    Message(id + "进入特殊区域");
        } else if (action.Equals("exist")) {
			int id = (int)data["id"];
			GameObject target = GetPlayerInfoByID(id).Target;
			target.GetComponent<CharacterMove>().OnExist();

            Message(id + "离开特殊区域");
		} else if (action.Equals("enterResponse")) {
			int id = (int)data["id"];
			GameObject target = GetPlayerInfoByID(id).Target;
			target.GetComponent<CharacterMove>().OnEnter();
		}
	}

	public void GameOver()
	{
		if (GameManager.Instance.RoomOwner)
		{
			JsonData data = new JsonData();
			data["action"] = "gsGameover";
			string value = data.ToJson();
			GameManager.SendEvent(value, new int[] { });
		}
//		data = new JsonData();
//		data["action"] = "gsScore";
//		data["dis"] = (int)GetPlayerInfoByID(GameManager.userID).mileageValue;
//		value = data.ToJson();
//		GameManager.SendEvent(1,value, new int[] { GameManager.userID });
	}

	public void OnReward(GameObject target,int index)
	{
	    int id = 0;
	    for (int i = 0; i < players.Length; i++)
	    {
	        if (players[i].Target == target)
	        {
	            id = players[i].playerID;
	        }
	    }
	    JsonData data = new JsonData();
		data["action"] = "gsReward";
		data["rewardID"] = index;
	    data["userID"] = id;
		string value = data.ToJson();
		GameManager.SendEvent(1,value,new int[] {GameManager.userID});
	}

	public void AccelerateDown() {
		target.GetComponent<CharacterMove>().moveSpeed += 0.2f;
		GetPlayerInfoByID(GameManager.userID).UpdateSpeed(80);
		JsonData data = new JsonData();
		data["action"] = "AccelerateDown";
		string value = data.ToJson();
		GameManager.SendEvent(value, new int[] { GameManager.userID });
		Message("加速");
	}

	public void AccelerateUp() {
		target.GetComponent<CharacterMove>().moveSpeed -= 0.2f;
		GetPlayerInfoByID(GameManager.userID).UpdateSpeed(50);
		JsonData data = new JsonData();
		data["action"] = "AccelerateUp";
		string value = data.ToJson();
		GameManager.SendEvent(value, new int[] { GameManager.userID });
		Message("减速");
	}

	public void RightDown() {
		target.GetComponent<CharacterMove>().rightMoveSpeed += 0.2f;
		JsonData data = new JsonData();
		data["action"] = "RightDown";
		string value = data.ToJson();
		GameManager.SendEvent(value, new int[] { GameManager.userID });
		Message("开始向右");
	}

	public void RightUp() {
		target.GetComponent<CharacterMove>().rightMoveSpeed -= 0.2f;
		JsonData data = new JsonData();
		data["action"] = "RightUp";
		string value = data.ToJson();
		GameManager.SendEvent(value, new int[] { GameManager.userID });
		Message("停止向右");
	}

	public void LeftDown() {
		target.GetComponent<CharacterMove>().leftMoveSpeed += 0.2f;
		JsonData data = new JsonData();
		data["action"] = "LeftDown";
		string value = data.ToJson();
		GameManager.SendEvent(value, new int[] { GameManager.userID });
		Message("开始向左");
	}

	public void LeftUp() {
		target.GetComponent<CharacterMove>().leftMoveSpeed -= 0.2f;
		JsonData data = new JsonData();
		data["action"] = "LeftUp";
		string value = data.ToJson();
		GameManager.SendEvent(value, new int[] { GameManager.userID });
		Message("停止向左");
	}

	public void AccelerateDown(int userID) {
		for (int i = 0; i < players.Length; i++) {
			if (players[i].playerID == userID) {
				players[i].Target.GetComponent<CharacterMove>().moveSpeed += 0.2f;
				GetPlayerInfoByID(userID).UpdateSpeed(80);
			}
		}
	}

	public void AccelerateUp(int userID) {
		for (int i = 0; i < players.Length; i++) {
			if (players[i].playerID == userID) {
				players[i].Target.GetComponent<CharacterMove>().moveSpeed -= 0.2f;
				GetPlayerInfoByID(userID).UpdateSpeed(50);
			}
		}
	}

	public void RightDown(int userID) {
		for (int i = 0; i < players.Length; i++) {
			if (players[i].playerID == userID) {
				players[i].Target.GetComponent<CharacterMove>().rightMoveSpeed += 0.2f;
			}
		}
	}

	public void RightUp(int userID) {
		for (int i = 0; i < players.Length; i++) {
			if (players[i].playerID == userID) {
				players[i].Target.GetComponent<CharacterMove>().rightMoveSpeed -= 0.2f;
			}
		}
	}

	public void LeftDown(int userID) {
		for (int i = 0; i < players.Length; i++) {
			if (players[i].playerID == userID) {
				players[i].Target.GetComponent<CharacterMove>().leftMoveSpeed += 0.2f;
			}
		}
	}

	public void LeftUp(int userID) {
		for (int i = 0; i < players.Length; i++) {
			if (players[i].playerID == userID) {
				players[i].Target.GetComponent<CharacterMove>().leftMoveSpeed -= 0.2f;
			}
		}
	}

	private RoomPlayer GetPlayerInfoByID(int id) {
		RoomPlayer info = null;
		for (int i = 0; i < players.Length; i++) {
			if (players[i].playerID == id) {
				info = players[i];
			}
		}
		return info;
	}

	private void LeavePeerNofity(MsRoomPeerLeaveRsp tRsp) {
		currentCount--;
	}

	public void Message(string info) {
		string value = detail.text;
		value += "\n" + info;
		detail.text = value;
	}

	public void OnEnterSpecialArea(GameObject go) {
		GameObject target = GetPlayerInfoByID(GameManager.userID).Target;
		if (target == go) {
			specialArea = true;
			GameManager.SubscribteEventGroup(new[] { "specialArea" }, new string[] { });
		}
	}

	public void OnExitSpecialArea(GameObject go) {
		GameObject target = GetPlayerInfoByID(GameManager.userID).Target;
		if (target == go) {
			JsonData data = new JsonData();
			data["action"] = "exist";
			data["id"] = GameManager.userID;
			string value = JsonUtil.toJson(data);
			GameManager.SendEventGroup(value, new string[] { "specialArea" });

			for (int i = 0; i < players.Length; i++) {
				players[i].Target.GetComponent<CharacterMove>().OnExist();
			}

			specialArea = false;

			StartCoroutine(LeaveSpecailArea());
		}
	}

	private IEnumerator LeaveSpecailArea() {
		yield return new WaitForSeconds(0.1f);
		GameManager.SubscribteEventGroup(new string[] { }, new string[] { "specialArea" });
	}

	public void OnBack() {
		if (GameManager.Instance.RoomOwner) {
			int roomOwner = 0;
			for (int i = 0; i < GameManager.Instance.RoomPlayers.Count; i++) {
				if (GameManager.Instance.RoomPlayers[i].userid != GameManager.userID &&
					!GameManager.Instance.RoomPlayers[i].robot) {
					roomOwner = GameManager.Instance.RoomPlayers[i].userid;
					break;
				}
			}
			JsonData data = new JsonData();
			data["action"] = "roomLeaveRoom";
			data["RoomOwner"] = roomOwner;
			string value = data.ToJson();
			GameManager.SendEvent(value, new int[] { GameManager.userID });
		}
		GameManager.LeaveRoom();
	}

}
