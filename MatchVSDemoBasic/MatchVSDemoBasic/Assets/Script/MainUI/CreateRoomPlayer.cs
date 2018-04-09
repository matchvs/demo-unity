using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPlayer:MonoBehaviour
{
    private int userid;
    public GameObject kick;
    public Text playerID;

    public void OnEnable()
    {
	   ResetInfo();
    }

    public void UpdateInfo(int userid)
    {
        this.userid = userid;
        this.playerID.text = userid.ToString();
	    if (kick != null)
	    {
			if(GameManager.Instance.RoomOwner)
				kick.SetActive(true);
			else
				kick.SetActive(false);
		}
    }

	public void ResetInfo()
	{
		playerID.text = "用户ID";
		if (kick != null) kick.SetActive(false);
	}

	public void Kick()
    {
        GameManager.KickPlayer(userid,"matchvs");
		GameManager.Instance.RemovePlayer(userid);
    }

	public void RemoveNotify(int userid)
	{
		if (userid != this.userid)
			return;
		this.playerID.text = "用户ID";
	}
}
