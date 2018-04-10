using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinSpecifiedRoomContext : BaseContext
{
    public JoinSpecifiedRoomContext() : base(UIType.JoinSpecifiedRoomBoard)
    {
    }
}

public class JoinSpecifiedRoomBoard : BaseView
{
    public InputField roomID;

    private void OnEnable()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        roomID.text = "";
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
        gameObject.SetActive(false);
    }

    public override void OnPause()
    {
        gameObject.SetActive(false);
    }

    public void OnJoinRoom()
    {
        string roomID = this.roomID.text;
		if(roomID.Equals(""))
			return;
	    SingleTone<ContextManager>.Instance.ShowView(new CreateRoomContext(), false);
		int status = GameManager.JoinSpecifiedRoom(roomID, "matchvs");
        if (status != 0)
        {
            GameManager.ShowTip("加入房间失败");
            SingleTone<ContextManager>.Instance.ShowView(new JoinSpecifiedRoomContext(), false);
        }
    }

  

    public void OnBack()
    {
        SingleTone<ContextManager>.Instance.ShowView(new GameLobbyContext(), false);
    }
}
