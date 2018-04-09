using System.Collections;
using System.Collections.Generic;
using MatchVS;
using UnityEngine;
using UnityEngine.UI;

public class GameLobbyContext : BaseContext
{
    public GameLobbyContext() : base(UIType.GameLobbyBoard)
    {
    }
}

public class GameLobbyBoard : BaseView
{

    public Text username;
    public Text userid;
    public Text diamond;

    private void OnEnable()
    {
	    GameManager.Instance.logoutResp += LogoutResp;
	    StartCoroutine(InitInfo());
	}

	private IEnumerator InitInfo()
	{
		yield return new WaitForSeconds(0.1f);
		username.text = GameManager.Instance.UserName;
		userid.text = GameManager.userID.ToString();
		diamond.text = GameManager.Instance.Diamond.ToString();
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

    public void OnCreateRoom()
    {
        SingleTone<ContextManager>.Instance.ShowView(new CreateRoomContext(), false);
        MsCreateRoomInfo info = new MsCreateRoomInfo("MatchVS", 3, 0, 0, 0, "matchvs");
        GameManager.CreateRoom(info, "matchvs");

        GameManager.Instance.Mode = GameMode.CreatRoomMode;
    }

    public void OnJoinRoom()
    {
        SingleTone<ContextManager>.Instance.ShowView(new MatchingContext(), false);
        GameManager.JoinRoom();

        GameManager.Instance.Mode = GameMode.MactchMode;
    }

    public void OnJoinSpecifiedRoom()
    {
        SingleTone<ContextManager>.Instance.ShowView(new JoinSpecifiedRoomContext(), false);

        GameManager.Instance.Mode = GameMode.CreatRoomMode;
    }

    public void OnMatchAttrributeRoom()
    {
        SingleTone<ContextManager>.Instance.ShowView(new MatchAttributeContext(), false);

		GameManager.Instance.Mode = GameMode.AttributeMode;
    }

	public void OnFrameButton()
	{
		SingleTone<ContextManager>.Instance.ShowView(new MatchingContext(), false);
	    MsMatchInfoTag tag = new MsMatchInfoTag() { key = "key", value = "Frame" }; ;
        MsMatchInfo info = new MsMatchInfo(3, 0, 0, tag);
	    GameManager.MatchAttributeRoom(info, "matchvs");

        GameManager.Instance.Mode = GameMode.FrameMode;
	}

	public void OnGetRoomList()
    {
        SingleTone<ContextManager>.Instance.ShowView(new RoomListContext(), false);
        MsRoomFilter filter = new MsRoomFilter(3, 0, 0, "");
        GameManager.GetRoomList(filter);
    }

    public void OnGameServer()
    {
        SingleTone<ContextManager>.Instance.ShowView(new GsMatchingContext(), false);
        GameManager.JoinRoom();

        GameManager.Instance.Mode = GameMode.GameServerMode;
    }

    private void LogoutResp(MsLogoutRsp tRsp)
    {
        SingleTone<ContextManager>.Instance.ShowView(new MainMenuContext(), false);
    }


	private void OnDisable()
    {
        GameManager.Instance.logoutResp -= LogoutResp;
    }


    public void OnBack()
    {
        GameManager.Logout();
    }
}
