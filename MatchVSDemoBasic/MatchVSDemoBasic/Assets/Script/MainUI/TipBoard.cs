using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipBoard : MonoBehaviour
{
	public GameObject obj;
	public Text text;
	private float timer;

	private void Update()
	{
		if (timer > 0)
		{
			timer -= Time.deltaTime;
		}
		else
		{
			obj.SetActive(false);
		}
	}

	public void SetInfo(string value)
	{
		timer = 1.5f;
		obj.SetActive(true);
		text.text = value;
	}
}
