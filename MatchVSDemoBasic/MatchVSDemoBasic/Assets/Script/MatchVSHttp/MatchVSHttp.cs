using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class MatchVSHttp : MonoBehaviour {
	public delegate void OnRsp(String context, String err);

	public static String Url = "http://test79open.matchvs.com";
	public static String UrlHashSet = "/wc5/hashSet.do?gameID={0}&userID={1}&key={2}&value={3}&sign={4}";
	public static String UrlHashGet = "/wc5/hashGet.do?gameID={0}&userID={1}&key={2}&sign={3}";

	public static void HashSet(MonoBehaviour context, string channel,string platform,int gameid, int userid, string key, string value, String appkey, String token, OnRsp callback) {
		var signString = String.Format("{0}&gameID={1:d}&key={2}&userID={3:d}&value={4}&{5}", appkey, gameid, key, userid,
			value, token);
		var toMd5 = Md5(signString);
		var strUrl = GetUrl(channel,platform);
		strUrl += UrlHashSet;
		strUrl = String.Format(strUrl, gameid, userid, key, value, toMd5);
		doGet(context, strUrl, "", callback);
	}

	public static void HashGet(MonoBehaviour context,string channel,string platform, int gameid, int userid, string key, String appkey, String token, OnRsp callback) {
		var signString = String.Format("{0}&gameID={1:d}&key={2}&userID={3}&{4}", appkey, gameid,
			key, userid, token);
		var toMd5 = Md5(signString);
		var strUrl = GetUrl(channel,platform);
		strUrl += UrlHashGet;
		strUrl = String.Format(strUrl, gameid, userid, key, toMd5);
		doGet(context, strUrl, "", callback);
	}

	private static void doGet(MonoBehaviour context, String url, String pars, OnRsp cb) {
		context.StartCoroutine(getHttp(url, pars, cb));
	}

	private static IEnumerator getHttp(String url, String pars, OnRsp cb) {
		var www = new WWW(url + pars);
		yield return www;
		log("www on request ->" + url);
		if (!IsEmpty(www.error)) {
			cb("", www.error);
			loge("www on response->" + " err:" + www.error);
		}
		if (www.isDone) {
			cb(www.text, "");
			log("www on response->" + www.text);
		}
	}

	private static void loge(String v) {
		Debug.LogError(v);
	}

	private static void log(String v) {
		Debug.Log(v);
	}

	private static bool IsEmpty(String error) {
		return error == null || error.Length <= 0;
	}

	public static String Md5(String input) {
		var md5 = MD5.Create();
		var inputBytes = Encoding.ASCII.GetBytes(input);
		var hash = md5.ComputeHash(inputBytes);
		var sb = new StringBuilder();
		for (var i = 0; i < hash.Length; i++) sb.Append(hash[i].ToString("X2"));
		return sb.ToString().ToLower();
	}

	private static String GetUrl(string channel,string platform)
	{
		string url = "http://";
		if (channel.Equals("MatchVS"))
		{
			if (platform.Equals("alpha"))
				url += "alphavsopen.matchvs.com";
			else if (platform.Equals("release"))
				url += "vsopen.matchvs.com";
		}
		if (channel.Equals("MatchVS-Test"))
		{
				url += "test79open.matchvs.com";
		}
		if (channel.Equals("MatchVS-Test1")) {
			if (platform.Equals("alpha"))
				url += "alphazwopen.matchvs.com";
			else if (platform.Equals("release"))
				url += "zwopen.matchvs.com";
		}
		return url;
	}
}
