using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayer : MonoBehaviour
{
    public Text userid;
    public Text mileage;
    public Text speed;
	public Text coinNum;
    public int playerID;
    public GameObject Target;
    private Vector3 originalPosition;
	[HideInInspector]
    public float mileageValue;
	[HideInInspector]
	public int rewardNum;

    private void Awake() {
        originalPosition = Target.transform.localPosition;
    }

    private void OnEnable() {
        Target.transform.localPosition = originalPosition;
        mileageValue = 0;
	    rewardNum = 0;
	}

    private void Update() {
        if (GameManager.Instance.Gamestart&& !GameManager.Instance.Gameover) {
            UpdateMileage();
        }
    }

    public void UpdateMileage() {
        mileageValue += Time.deltaTime * Target.GetComponent<CharacterMove>().moveSpeed * 100;
        this.mileage.text = (int)mileageValue + "km";
    }

    public void UpdateInfo(UserInfo userInfo)
    {
        this.userid.text = userInfo.userid.ToString();
        playerID = userInfo.userid;
        mileage.text = "0km";
        speed.text = "50迈";
        if(coinNum != null)
	        coinNum.text = "0";
    }

    public void UpdateSpeed(float value) {
        this.speed.text = value + "迈";
    }

	public void GetReward()
	{
		rewardNum++;
		if (coinNum != null)
			coinNum.text = rewardNum.ToString();
	}
}
