using System.Collections;
using System.Collections.Generic;
using LitJson;
using MatchVS;
using UnityEngine;
using UnityEngine.UI;

public class GameOverContext : BaseContext
{
    public GameOverContext() : base(UIType.GameOverBoard)
    {
    }
}

public class GameOverBoard : BaseView
{
    public GameObject tip;
    public Text[] rank;
    public Text[] reward;

    private void OnEnable()
    {
        tip.SetActive(GameManager.Instance.Mode == GameMode.GameServerMode);            

        for (int i = 0; i < GameManager.Instance.LastResult.Count; i++)
        {
            if (GameManager.Instance.LastResult[i].userid == GameManager.userID)
            {
                GameManager.Instance.UpdateDiamond(3-i);
            }
        }

        rank[0].text = GameManager.Instance.LastResult[0].userid.ToString();
        if (GameManager.Instance.Mode == GameMode.GameServerMode)
            reward[0].text = "金币：" + GameManager.Instance.LastResult[0].rewardNum.ToString();
        else
            reward[0].text = "钻石：3";
        if (GameManager.Instance.LastResult.Count < 2)
	    {
			rank[1].text ="离开";
            if(GameManager.Instance.Mode == GameMode.GameServerMode)
	            reward[1].text = "金币：0";
            else
                reward[1].text = "钻石：0";
        }
	    else
	    {
		    rank[1].text = GameManager.Instance.LastResult[1].userid.ToString();
	        if (GameManager.Instance.Mode == GameMode.GameServerMode)
                reward[1].text = "金币：" + GameManager.Instance.LastResult[1].rewardNum.ToString();
	        else
	             reward[1].text = "钻石：2";
        }
	    if (GameManager.Instance.LastResult.Count < 3)
	    {
		    rank[2].text = "离开";
	        if (GameManager.Instance.Mode == GameMode.GameServerMode)
	            reward[2].text = "金币：0";
	        else
	            reward[2].text = "钻石：0";
        }
	    else
	    {
	        if (GameManager.Instance.LastResult[2].userid != 0)
	        {
	            rank[2].text = GameManager.Instance.LastResult[2].userid.ToString();
	            if (GameManager.Instance.Mode == GameMode.GameServerMode)
                    reward[2].text = "金币：" + GameManager.Instance.LastResult[2].rewardNum.ToString();
	            else
	                 reward[2].text = "钻石：1";
            }
	        else
	        {
	            rank[2].text = "离开";
	            if (GameManager.Instance.Mode == GameMode.GameServerMode)
	                reward[2].text = "金币：0";
	            else
	                reward[2].text = "钻石：0";
            }
		}

        GameManager.Instance.leaveRoomResponse += LeaveRoomResponse;
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
        GameManager.Instance.leaveRoomResponse -= LeaveRoomResponse;
    }

    public void OnBack()
    {
        GameManager.LeaveRoom();
    }

    private void LeaveRoomResponse(MsRoomLeaveRsp tRsp)
    {
        SingleTone<ContextManager>.Instance.ShowView(new GameLobbyContext(), false);
    }
}
