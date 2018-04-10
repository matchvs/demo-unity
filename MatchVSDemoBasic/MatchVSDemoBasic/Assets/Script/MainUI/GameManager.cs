using System;
using System.Collections.Generic;
using LitJson;
using MatchVS;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GameMode
{
    None,
    MactchMode,
    CreatRoomMode,
    AttributeMode,
	FrameMode,
    GameServerMode,
}

public class GameManager : MonoBehaviour
{
	public bool ContainRobot;
    private static int gameid = 200938;
    private static string channel = "MatchVS-Test";
    private static string platform = "alpha";
    private static string appkey = "02933b4f21674f9bbd05e0dab59476a6";
    private static string secret = "3b3ca851f5f74026a9a03f0a72785bdb";
    private static int gameVersion = 1;
    private static string deviceID;
    private static string roomID;
    private static int gatewayid = 0;
    public static int userID;
    public static string token;

    private List<PlayerResult> lastResult = new List<PlayerResult>();

	private TipBoard tip;
    private string username;
    public string UserName
    {
        get { return username; }
        set
        {
            username = value;
            PlayerPrefs.SetString("username", username);
            SaveData();
        }
    }

    public int GameID
    {
        set { gameid = value; }
        get { return gameid; }
    }

    public string Appkey
    {
        set { appkey = value; }
        get { return appkey; }
    }

	public string Secret
	{
		set { secret = value; }
		get { return secret; }
	}

	public string Channel
    {
        set { channel = value; }
        get { return channel; }
    }

    public string RoomID
    {
        set { roomID = value; }
        get { return roomID; }
    }

    public string Platform
    {
        set { platform = value; }
        get { return platform; }
    }

    public void ClearAll()
    {
          lastResult.Clear();
    }

    public void AddUserID(PlayerResult playResult)
    {
        lastResult.Add(playResult);
    }

    public List<PlayerResult> LastResult
    {
        get { return lastResult; }
    }

    private bool roomOwner;
    public bool RoomOwner
    {
        get { return roomOwner; }
        set { roomOwner = value; }
    }

    private bool gamestart;
    public bool Gamestart
    {
        get { return gamestart; }
        set { gamestart = value; }
    }

    private bool gameover;
    public bool Gameover
    {
        get { return gameover; }
        set { gameover = value; }
    }

    private GameMode mode;

    public GameMode Mode
    {
        get { return mode; }
        set { mode = value; }
    }

    private int diamond;

    public int Diamond
    {
        get { return diamond; }
        set
        {
            diamond = value; 
            PlayerPrefs.SetInt("diamod",diamond);
            SaveData();
        }
    }


    public void UpdateDiamond(int count)
    {
        Diamond += count;
    }

    private List<UserInfo> players = new List<UserInfo>();

    public List<UserInfo> RoomPlayers
    {
        get { return players; }
    }

    public void AddRoomPlayer(UserInfo userInfo)
    {
        players.Add(userInfo);
    }

    public void RemovePlayer(int userid)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].userid == userid)
            {
                players.Remove(players[i]);
                return;
            }
        }
    }

    public void ClearPlayer()
    {
        players.Clear();
    }


    [HideInInspector]
    public static MatchVSEngine engine = new MatchVSEngine();

    public static GameManager Instance;

    public static MatchVSResponse[] matchVSResponses = new MatchVSResponse[]
    {
        new MatchVSResponseInner()
    };


    public static void Login(int userID,string token)
    {
        GameManager.userID = userID;
        GameManager.token = token;
        engine.login(userID,token,gameid,gameVersion,appkey,secret,deviceID,gatewayid);
    }

    public static void Logout()
    {
        engine.logout();
    }

    public static void Regist()
    {
        engine.registerUser();
    }

	public static int CreateRoom(MsCreateRoomInfo info, string userProfile)
	{
		return engine.createRoom(info, userProfile);
	}

    public static void KickPlayer(int userid,string cpProto)
    {
        engine.kickPlayer(userid,cpProto);
    }

    public static void GetRoomList(MsRoomFilter filter)
    {
        engine.getRoomList(filter);
    }

    public static int JoinRoom()
    {
        return engine.joinRandomRoom(3, "Matchvs");
    }

    public static int JoinSpecifiedRoom(string roomID,string profile)
    {
        return engine.joinRoom(roomID, profile);
    }

    public static int MatchAttributeRoom(MsMatchInfo info,string userProfile)
    {
        return engine.joinRoomWithProperties(info, userProfile);
    }

    public static void SubscribteEventGroup(string[] subGroups,string[] unsubGroups)
    {
        engine.subscribeEventGroup(subGroups, unsubGroups);
    }

    public static void SendEventGroup( string cpProto, string[] groups)
    {
        engine.sendEventGroup(cpProto, groups);
    }

    public static void LeaveRoom()
    {
        engine.leaveRoom("Matchvs");
    }

    public static void JoinOver(string roomID,string proto)
    {
        engine.joinOver(proto);
    }

    public static void SendEvent(string value,int[] users)
    {
        engine.sendEvent(0, value, 1, users);
    }

	public static void SendEvent(int iType,string value, int[] users )
	{
		engine.sendEvent(iType, value, 1, users);
	}

	public static void SetFrameSync(int rate)
	{
		engine.setFrameSync(rate);
	}

	public static void SendFrameEvent(String cpProto)
	{
		engine.sendFrameEvent(cpProto);
	}

	private  void HashSet(string key ,string value)
    {
        MatchVSHttp.HashSet(this,channel,platform,gameid,userID,key,value,appkey,token, (context,error) => { });
    }

    private void HashGet(string key,MatchVSHttp.OnRsp rsp)
    {
        MatchVSHttp.HashGet(this,channel,platform,gameid,userID,key,appkey,token,rsp);
    }

    private void SaveData()
    {
        JsonData data = new JsonData();
        data["username"] = username;
        data["diamod"] = diamond;

        HashSet("playerInfo",JsonUtil.toJson(data));
    }

    public static void Init()
    {
        Loom.QueueOnMainThread(() => { });
        engine.init(matchVSResponses, channel, platform, gameid);
        Debug.Log("MainmenuBoard->init()");
    }

    private void Awake()
    {
        deviceID = SystemInfo.deviceUniqueIdentifier;

        Instance = this;

	    tip = GameObject.FindObjectOfType<TipBoard>();
    }

	public void InitInfo()
	{
		if (PlayerPrefs.HasKey("userID")) {
			userID = PlayerPrefs.GetInt("userID");
		}

		if (PlayerPrefs.HasKey("token")) {
			token = PlayerPrefs.GetString("token");
		}

		if (!PlayerPrefs.HasKey("username") || !PlayerPrefs.HasKey("diamod") || !PlayerPrefs.HasKey("userID")) {
			UserName =  Random.Range(1000, 10000) + "";
			Diamond = 0;
		} else {
			HashGet("playerInfo", (context, error) =>
			{
				JsonData json = JsonUtil.toObject(context);
				String dataJson = (String)json["data"];
				JsonData data = JsonUtil.toObject(dataJson);
				UserName =(String)data["username"];
				Diamond = (int)data["diamod"];
			});
		}
	}

	public static void ShowTip(string value)
	{
		Instance.tip.SetInfo(value);
	}

	public void Start()
	{
		SingleTone<UIManager>.Creator();
		SingleTone<ContextManager>.Creator();
	}

    private void OnDisable()
    {
        engine.uninit();
    }

    public Action<MsRegisterUserRsp> registResponse;
    public Action<MsLoginRsp> loginResponse;
    public Action<MsJoinRandomRsp> joinRoomResponse;
    public Action<MsRoomLeaveRsp> leaveRoomResponse;
    public Action<MsRoomPeerJoinRsp> roomPeerJoinRsp;
    public Action<MsRoomPeerLeaveRsp> roomPeerLeaveRsp;
    public Action<MsRoomJoinOverRsp> roomJoinOverRsp;
    public Action<MsMsgNotify> sendEventNotify;
    public Action<MsLogoutRsp> logoutResp;
    public Action<MsCreateRoomRsp> createRoomRsp;
    public Action kickPlayerRsp;
    public Action<MsRoomListRsp> roomListRsp;
	public Action<MsKickPlayerNotify> kickPlayerNotify;
    public Action<MsSubscribeEventGroupRsp> subscribeEventGroupRsp;
    public Action<MsSendEventGroupRsp> sendEventGroupRsp;
    public Action<MsSendEventGroupNotify> sendEventGroupNotify;
	public Action<MsSetChannelFrameSyncNotify> setFrameSyncNotify;
	public Action<MsFrameData> FrameUpdate;
}
