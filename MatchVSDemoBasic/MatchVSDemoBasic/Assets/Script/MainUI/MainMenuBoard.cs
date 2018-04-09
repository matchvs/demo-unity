using System.Collections;
using System.Collections.Generic;
using MatchVS;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuContext : BaseContext {
	public MainMenuContext() : base(UIType.MainMenuBoard) {
	}
}


public class MainMenuBoard : BaseView {
	public InputField gameidInput;
	public InputField appkeyInput;
	public InputField secretInput;
	public Dropdown choosecChannel;
	public Toggle toggle_platform;
	private int userID = 04;
	private string token = "";

	private string[] channels = { "MatchVS", "MatchVS-Test", "MatchVS-Test1", "Migu", "Migu-Test" };

	private void OnEnable() {
		GameManager.Instance.registResponse += RegistResponse;
		GameManager.Instance.loginResponse += LoginResponse;

		string pGameid = PlayerPrefs.GetString("gameid");
		string pAppkey = PlayerPrefs.GetString("appkey");
		string secret = PlayerPrefs.GetString("secret");

		if (!string.IsNullOrEmpty(pGameid)
			&& !string.IsNullOrEmpty(pAppkey) && !string.IsNullOrEmpty(secret)) {
			gameidInput.text = pGameid;
			appkeyInput.text = pAppkey;
			secretInput.text = secret;
		}
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

	public void OnLogin() {
		PlayerPrefs.SetString("gameid", gameidInput.text);
		PlayerPrefs.SetString("appkey", appkeyInput.text);
		PlayerPrefs.SetString("secret", secretInput.text);

		GameManager.Instance.Channel = channels[choosecChannel.value];
		GameManager.Instance.Platform = toggle_platform.isOn ? "alpha" : "release";
		GameManager.Instance.GameID = int.Parse(gameidInput.text);
		GameManager.Instance.Appkey = appkeyInput.text;
		GameManager.Instance.Secret = secretInput.text;

		GameManager.Init();

	    if (!PlayerPrefs.HasKey("userID") || !PlayerPrefs.HasKey("token"))
	    {
	        GameManager.Regist();
	    }
	    else
	    {
		    GameManager.Instance.InitInfo();
			GameManager.Login(GameManager.userID,GameManager.token);
	    }
	}

	public void ClearAllInfo() {
		PlayerPrefs.DeleteAll();
	}

	private void RegistResponse(MsRegisterUserRsp tRsp) {
		userID = tRsp.userID;
	    GameManager.userID = userID;
        PlayerPrefs.SetInt("userID",userID);
		token = tRsp.token;
	    GameManager.token = token;
        PlayerPrefs.SetString("token",token);

		GameManager.Instance.InitInfo();

		GameManager.Login(userID, token);
	}

	private void LoginResponse(MsLoginRsp tRsp) {
		SingleTone<ContextManager>.Instance.ShowView(new GameLobbyContext(), false);
	}

	private void OnDisable() {
		GameManager.Instance.registResponse -= RegistResponse;
		GameManager.Instance.loginResponse -= LoginResponse;
	}
}

