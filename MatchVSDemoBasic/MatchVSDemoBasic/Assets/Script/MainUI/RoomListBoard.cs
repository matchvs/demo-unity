
using System.Collections.Generic;
using MatchVS;
using UnityEngine;

public class RoomListContext : BaseContext
{
    public RoomListContext() : base(UIType.RoomListBoard)
    {
    }
}

public class RoomListBoard : BaseView
{

    public GameObject prefab;
    public Transform parent;

	private List<GameObject> list = new List<GameObject>();

    public void OnEnable()
    {
        GameManager.Instance.roomListRsp += RoomListRsp;
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

    private void RoomListRsp(MsRoomListRsp tRsp)
    {
        MsRoomInfoEx[] info = tRsp.roomInfos;
        for (int i = 0; i < info.Length; i++)
        {
            GameObject go = Instantiate(prefab, parent, false);
            RoomListItem item = go.GetComponent<RoomListItem>();
            item.UpdateInfo(info[i]);
			list.Add(go);
        }
    }

    public void Back()
    {
        SingleTone<ContextManager>.Instance.ShowView(new GameLobbyContext(), false);
    }

	private void Clear()
	{
		while (list.Count > 0)
		{
			GameObject go = list[0];
			list.Remove(go);
			DestroyImmediate(go);
		}
	}

	private void OnDisable()
    {
		Clear();
        GameManager.Instance.roomListRsp -= RoomListRsp;
    }
}
