using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
	private int index;

	public void UpdateInfo(int i)
	{
		this.index = i;
	}

	void OnTriggerEnter2D(Collider2D other) {
		CharacterMove move = other.gameObject.GetComponent<CharacterMove>();
		move.Reward(index);
	}
}
