using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using MatchVS;
using UnityEngine;
using UnityEngine.Experimental.Director;
using UnityEngine.UI;


public class GameRoomContext : BaseContext
{
    public GameRoomContext() : base(UIType.GameRoomBoard)
    {
    }
}

public class PlayerDis
{
    public int userid;
    public int dis;

    public PlayerDis(int userid, int dis)
    {
        this.userid = userid;
        this.dis = dis;
    }
}

public class GameRoomBoard : BaseView
{
    public RoomPlayer[] players;
    public Text title;
    public Text detail;
    public Text roomID;
	public GameObject frameRate;
    private List<int> readyPlayers = new List<int>();
    private List<PlayerDis> playerDis = new List<PlayerDis>();
    private GameObject target;
    private int currentCount;

    private bool specialArea;

    private void OnEnable()
    {
        specialArea = false;
        Message("");
	    detail.text = "";
		frameRate.SetActive(GameManager.Instance.Mode == GameMode.FrameMode);

        GameManager.Instance.RoomPlayers.Sort((a,b)=>a.userid.CompareTo(b.userid));

        for (int i = 0; i < GameManager.Instance.RoomPlayers.Count; i++)
        {
           
            players[i].UpdateInfo(GameManager.Instance.RoomPlayers[i]);
            if (GameManager.Instance.RoomPlayers[i].userid == GameManager.userID)
            {
                target = players[i].Target;
            }
        }

        GameManager.Instance.leaveRoomResponse += OnLeaveRoomResponse;
        GameManager.Instance.sendEventNotify += OnSendEventNotify;
        GameManager.Instance.roomPeerLeaveRsp += LeavePeerNofity;
        GameManager.Instance.sendEventGroupNotify += OnSendEventGroupNotify;
        GameManager.Instance.subscribeEventGroupRsp += SubscribeEventGroupRsp;
	    GameManager.Instance.FrameUpdate += FrameUpdate;

        JsonData data = new JsonData();
        data["action"] = "ready";
        string value = data.ToJson();
        GameManager.SendEvent(value, new int[] { GameManager.userID });

        GameManager.Instance.ClearAll();
        readyPlayers.Clear();
        playerDis.Clear();

        if (GameManager.Instance.RoomOwner)
        {
            for (int i = 0; i < GameManager.Instance.RoomPlayers.Count; i++)
            {
                if (GameManager.Instance.RoomPlayers[i].robot)
                {
                    readyPlayers.Add(GameManager.Instance.RoomPlayers[i].userid);
                }
            }
        }

        if (readyPlayers.Count == 2) {
            JsonData info = new JsonData();
            info["action"] = "start";
            string jsonValue = info.ToJson();
            GameManager.SendEvent(jsonValue, new int[] { GameManager.userID });

            detail.text = "游戏开始";
            GameManager.Instance.Gamestart = true;
        }

        roomID.text = "房间号: " + GameManager.Instance.GameID;

        currentCount = 3;

        if (GameManager.Instance.Mode == GameMode.FrameMode)
        {
            title.text = "房间名称[帧同步模式]";
            if (GameManager.Instance.RoomOwner) {
                GameManager.SetFrameSync(10);
            }
        }
        else
        {
            title.text = "房间名称";
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

    private void OnDisable()
    {
        GameManager.Instance.leaveRoomResponse -= OnLeaveRoomResponse;
        GameManager.Instance.sendEventNotify -= OnSendEventNotify;
        GameManager.Instance.roomPeerLeaveRsp -= LeavePeerNofity;
        GameManager.Instance.sendEventGroupNotify -= OnSendEventGroupNotify;
        GameManager.Instance.subscribeEventGroupRsp -= SubscribeEventGroupRsp;
	    GameManager.Instance.FrameUpdate -= FrameUpdate;
	}

    private void OnLeaveRoomResponse(MsRoomLeaveRsp tRsp)
    {
        SingleTone<ContextManager>.Instance.ShowView(new GameLobbyContext(), false);
	}

    private void SubscribeEventGroupRsp(MsSubscribeEventGroupRsp trsp)
    {
        if (specialArea)
        {
            JsonData data = new JsonData();
            data["action"] = "enter";
            data["id"] = GameManager.userID;
            string value = JsonUtil.toJson(data);
            GameManager.SendEventGroup(value,new string[] { "specialArea" });
        }
    }

    public void GameOver()
    {
        if (!GameManager.Instance.RoomOwner)
        {
            StartCoroutine(GameOver(1));
        }
        else
        {
            int robotCount = 0;
            for (int i = 0; i < GameManager.Instance.RoomPlayers.Count; i++)
            {
                if (GameManager.Instance.RoomPlayers[i].robot)
                {
                    robotCount++;
                }
            }
            if (robotCount == 2)
            {
                for (int i = 0; i < GameManager.Instance.RoomPlayers.Count; i++)
                {
                    playerDis.Add(new PlayerDis(GameManager.Instance.RoomPlayers[i].userid,
                        (int) GetPlayerInfoByID(GameManager.Instance.RoomPlayers[i].userid).mileageValue));
                }
                playerDis.Sort((a, b) => -a.dis.CompareTo(b.dis));

                for (int i = 0; i < playerDis.Count; i++)
                {
                    int userid = playerDis[i].userid;
                    GameManager.Instance.AddUserID(new PlayerResult() {rewardNum = GetPlayerInfoByID(userid).rewardNum,userid = userid});
                }
                SingleTone<ContextManager>.Instance.ShowView(new GameOverContext(), false);
            }
            else
            {
                for (int i = 0; i < GameManager.Instance.RoomPlayers.Count; i++) {
                    if (GameManager.Instance.RoomPlayers[i].robot) {
                        playerDis.Add(new PlayerDis(GameManager.Instance.RoomPlayers[i].userid,
                            (int)GetPlayerInfoByID(GameManager.Instance.RoomPlayers[i].userid).mileageValue));
                    }
                }
	            if (playerDis.Count == currentCount - 1)
	            {
		            playerDis.Add(new PlayerDis(GameManager.userID, (int)GetPlayerInfoByID(GameManager.userID).mileageValue));
		            playerDis.Sort((a, b) => -a.dis.CompareTo(b.dis));

		            for (int i = 0; i < playerDis.Count; i++) {
		                int userid = playerDis[i].userid;
		                GameManager.Instance.AddUserID(new PlayerResult() { rewardNum = GetPlayerInfoByID(userid).rewardNum, userid = userid });
                    }

		            SingleTone<ContextManager>.Instance.ShowView(new GameOverContext(), false);
				}
            }
        }
    }

    private IEnumerator GameOver(float time)
    {
        yield return new WaitForSeconds(time);
        JsonData info = new JsonData();
        info["action"] = "gameover";
        info["dis"] = (int)GetPlayerInfoByID(GameManager.userID).mileageValue;
        string value = info.ToJson();
        GameManager.SendEvent(value, new int[] { GameManager.userID });
    }

    private void OnSendEventNotify(MsMsgNotify tRsp)
    {
        string payload = tRsp.cpProto;
        JsonData data = JsonUtil.toObject(payload);
        string action = (string) data["action"];
        if (action.Equals("ready"))
        {
            if (GameManager.Instance.RoomOwner)
            {
                bool flag = readyPlayers.Contains(tRsp.srcUserID);
                if (!flag)
                {
                    readyPlayers.Add(tRsp.srcUserID);
                }
            
                if (readyPlayers.Count == currentCount - 1)
                {
                    JsonData info = new JsonData();
                    info["action"] = "start";
                    string value = info.ToJson();
                    GameManager.SendEvent(value, new int[] { GameManager.userID });

                   Message("游戏开始");
                    GameManager.Instance.Gamestart = true;
                }
            }
        }

        if (action.Equals("gameover"))
        {
            if (GameManager.Instance.RoomOwner)
            {
                PlayerDis dis = playerDis.Find(t => t.userid == tRsp.srcUserID);
                if (dis == null)
                {
                    int mile = (int)data["dis"];
                    PlayerDis player = new PlayerDis(tRsp.srcUserID,mile);
                    playerDis.Add(player);
                }
                if (playerDis.Count == currentCount - 1)
                {
                    playerDis.Add(new PlayerDis(GameManager.userID, (int)GetPlayerInfoByID(GameManager.userID).mileageValue));
                    playerDis.Sort((a,b)=> -a.dis.CompareTo(b.dis));
                    JsonData info = new JsonData();
                    info["action"] = "result";
                    info["first"] = playerDis[0].userid;
                    info["second"] = playerDis[1].userid;
	                if (currentCount < 3)
	                {
		                info["third"] = 0;
	                }
	                else
	                {
						info["third"] = playerDis[2].userid;
	                }
                    string value = info.ToJson();

                    for (int i = 0; i < playerDis.Count; i++)
                    {
                        int userid = playerDis[i].userid;
                        GameManager.Instance.AddUserID(new PlayerResult() { rewardNum = GetPlayerInfoByID(userid).rewardNum, userid = userid });
                    }

                    GameManager.SendEvent(value, new int[] { GameManager.userID });

                    SingleTone<ContextManager>.Instance.ShowView(new GameOverContext(), false);
                }
            }
        }

        if (action.Equals("start"))
        {
            GameManager.Instance.Gamestart = true;
            Message("游戏开始");
        }

     
        if (action.Equals("result"))
        {
            int first = (int)data["first"];
            int second = (int)data["second"];
            int third = (int)data["third"];

            GameManager.Instance.AddUserID(new PlayerResult() {userid = first});
            GameManager.Instance.AddUserID(new PlayerResult() {userid = second});
            GameManager.Instance.AddUserID(new PlayerResult() {userid = third});

            SingleTone<ContextManager>.Instance.ShowView(new GameOverContext(), false);
        }

        if (action.Equals("roomLeaveRoom"))
        {
            int gameowner = (int) data["RoomOwner"];
            if (GameManager.userID == gameowner)
            {
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

	private void FrameUpdate(MsFrameData frameData)
	{
		MsFrameItem[] items = frameData.frameItems;
		for (int i = 0; i < items.Length; i++)
		{
			MsFrameItem tRsp = items[i];
			string payload = tRsp.cpProto;
			JsonData jsonInfo = JsonUtil.toObject(payload);
			string action = (string)jsonInfo["action"];
			if (action.Equals("AccelerateDown")) {
				AccelerateDown(tRsp.srcUserID);
				Message("[帧同步消息:]" + tRsp.srcUserID + "加速");
			}

			if (action.Equals("AccelerateUp")) {
				AccelerateUp(tRsp.srcUserID);
				Message("[帧同步消息:]" + tRsp.srcUserID + "减速");
			}
			if (action.Equals("RightDown")) {
				RightDown(tRsp.srcUserID);
				Message("[帧同步消息:]" + tRsp.srcUserID + "开始向右");
			}
			if (action.Equals("RightUp")) {
				RightUp(tRsp.srcUserID);
				Message("[帧同步消息:]" + tRsp.srcUserID + "停止向右");
			}
			if (action.Equals("LeftDown")) {
				LeftDown(tRsp.srcUserID);
				Message("[帧同步消息:]" + tRsp.srcUserID + "开始向左");
			}
			if (action.Equals("LeftUp")) {
				LeftUp(tRsp.srcUserID);
				Message("[帧同步消息:]" + tRsp.srcUserID + "停止向左");
			}
		}
	}

	private void OnSendEventGroupNotify(MsSendEventGroupNotify tRsp)
    {
        string payload = tRsp.cpProto;
        JsonData data = JsonUtil.toObject(payload);
        string action = (string) data["action"];
        if (action.Equals("enter"))
        {
            int id = (int) data["id"];
            GameObject target = GetPlayerInfoByID(id).Target;
            target.GetComponent<CharacterMove>().OnEnter();

            data = new JsonData();
            data["action"] = "enterResponse";
            data["id"] = GameManager.userID;
            string value = JsonUtil.toJson(data);
            GameManager.SendEventGroup(value, new string[] {"specialArea"});

            Message("玩家" + id + "进入特殊区域");
        }
        else if (action.Equals("exist"))
        {
            int id = (int) data["id"];
            GameObject target = GetPlayerInfoByID(id).Target;
            target.GetComponent<CharacterMove>().OnExist();

            Message("玩家" + id + "离开特殊区域");
        }
        else if (action.Equals("enterResponse"))
        {
            int id = (int) data["id"];
            GameObject target = GetPlayerInfoByID(id).Target;
            target.GetComponent<CharacterMove>().OnEnter();
        }
    }

    public void AccelerateDown()
    {
        JsonData data = new JsonData();
        data["action"] = "AccelerateDown";
        string value = data.ToJson();
	    if (GameManager.Instance.Mode == GameMode.FrameMode)
	    {
			GameManager.SendFrameEvent(value);
		}
	    else
	    {
		    target.GetComponent<CharacterMove>().moveSpeed += 0.2f;
		    GetPlayerInfoByID(GameManager.userID).UpdateSpeed(80);
			GameManager.SendEvent(value, new int[] { GameManager.userID });
		    Message("加速");
		}
	}

    public void AccelerateUp()
    {
        JsonData data = new JsonData();
        data["action"] = "AccelerateUp";
        string value = data.ToJson();
	    if (GameManager.Instance.Mode == GameMode.FrameMode) {
		    GameManager.SendFrameEvent(value);
	    } else {
		    target.GetComponent<CharacterMove>().moveSpeed -= 0.2f;
		    GetPlayerInfoByID(GameManager.userID).UpdateSpeed(50);
			GameManager.SendEvent(value, new int[] { GameManager.userID });
		    Message("减速");
		}
    }

    public void RightDown()
    {
        JsonData data = new JsonData();
        data["action"] = "RightDown";
        string value = data.ToJson();
	    if (GameManager.Instance.Mode == GameMode.FrameMode) {
		    GameManager.SendFrameEvent(value);
	    } else {
		    target.GetComponent<CharacterMove>().rightMoveSpeed += 0.2f;
			GameManager.SendEvent(value, new int[] { GameManager.userID });
		    Message("开始向右");
		}
    }

    public void RightUp()
    {
        JsonData data = new JsonData();
        data["action"] = "RightUp";
        string value = data.ToJson();
	    if (GameManager.Instance.Mode == GameMode.FrameMode) {
		    GameManager.SendFrameEvent(value);
	    } else {
		    target.GetComponent<CharacterMove>().rightMoveSpeed -= 0.2f;
			GameManager.SendEvent(value, new int[] { GameManager.userID });
		    Message("停止向右");
		}
    }

    public void LeftDown()
    {
        JsonData data = new JsonData();
        data["action"] = "LeftDown";
        string value = data.ToJson();
	    if (GameManager.Instance.Mode == GameMode.FrameMode) {
		    GameManager.SendFrameEvent(value);
	    } else {
		    target.GetComponent<CharacterMove>().leftMoveSpeed += 0.2f;
			GameManager.SendEvent(value, new int[] { GameManager.userID });
		    Message("开始向左");
		}
    }

    public void LeftUp()
    {
        JsonData data = new JsonData();
        data["action"] = "LeftUp";
        string value = data.ToJson();
	    if (GameManager.Instance.Mode == GameMode.FrameMode) {
		    GameManager.SendFrameEvent(value);
	    } else {
		    target.GetComponent<CharacterMove>().leftMoveSpeed -= 0.2f;
			GameManager.SendEvent(value, new int[] { GameManager.userID });
		    Message("停止向左");
		}
		
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

    private void LeavePeerNofity(MsRoomPeerLeaveRsp tRsp)
    {
        currentCount--;
    }

    public void Message(string info)
    {
        string value = detail.text;
        value += "\n" + info;
        detail.text = value;
    }

    public void OnEnterSpecialArea(GameObject go)
    {
        GameObject target = GetPlayerInfoByID(GameManager.userID).Target;
        if (target == go)
        {
            specialArea = true;
            GameManager.SubscribteEventGroup(new []{"specialArea"},new string[]{});
        }
    }

    public void OnExitSpecialArea(GameObject go)
    {
        GameObject target = GetPlayerInfoByID(GameManager.userID).Target;
        if (target == go)
        {
            JsonData data = new JsonData();
            data["action"] = "exist";
            data["id"] = GameManager.userID;
            string value = JsonUtil.toJson(data);
            GameManager.SendEventGroup(value,new string[] { "specialArea" });

            for (int i = 0; i < players.Length; i++) {
                players[i].Target.GetComponent<CharacterMove>().OnExist();
            }

            specialArea = false;

            StartCoroutine(LeaveSpecailArea());
        }
    }

    private IEnumerator LeaveSpecailArea()
    {
        yield return new WaitForSeconds(0.1f);
        GameManager.SubscribteEventGroup(new string[] { }, new string[] { "specialArea" });
    }

    public void OnBack()
    {
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
