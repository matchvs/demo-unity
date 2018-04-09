using System.Collections;
using System.Collections.Generic;
using MatchVS;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    public Text roomID;
    public Text maxPlayer;
    private string roomNum;

    public void UpdateInfo(MsRoomInfoEx info)
    {
        roomNum = info.roomID;
        roomID.text = info.roomID.ToString();
        maxPlayer.text = info.maxPlayer.ToString();
    }

    public void OnJoinRoom()
    {
        SingleTone<ContextManager>.Instance.ShowView(new CreateRoomContext(), false);
        GameManager.JoinSpecifiedRoom(roomNum,"matchvs");
    }
}
