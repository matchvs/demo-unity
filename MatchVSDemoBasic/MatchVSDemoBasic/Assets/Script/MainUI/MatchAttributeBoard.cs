using System.Collections;
using System.Collections.Generic;
using LitJson;
using MatchVS;
using UnityEngine;
using UnityEngine.UI;

public class MatchAttributeContext : BaseContext
{
    public MatchAttributeContext() : base(UIType.MatchAttributeBoard)
    {
    }
}

public class MatchAttributeBoard : BaseView
{

    public Toggle SchemeA;
    public Toggle SchemeB;

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

    public void OnStartGame()
    {
        MsMatchInfoTag tag = null;

        if (!SchemeA.isOn && !SchemeB.isOn)
        {
            return;
        }

        if (SchemeA.isOn && SchemeB.isOn)
        {
            tag = new MsMatchInfoTag() {key = "key",value = "AB"};
        }

        if (SchemeA.isOn)
        {
            tag = new MsMatchInfoTag() { key = "key", value = "A" };
        }

        if (SchemeB.isOn) {
            tag = new MsMatchInfoTag() { key = "key", value = "B" };
        }
        SingleTone<ContextManager>.Instance.ShowView(new MatchingContext(), false);
        MsMatchInfo info = new MsMatchInfo(3, 0, 0, tag);
        GameManager.MatchAttributeRoom(info, "matchvs");
    }

    public void OnBack()
    {
        SingleTone<ContextManager>.Instance.ShowView(new GameLobbyContext(), false);
    }
}
