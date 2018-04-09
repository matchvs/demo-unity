using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseContext : MonoBehaviour {
	public UIType Type { get; private set; }

	public BaseContext(UIType type) {
		Type = type;
	}
}