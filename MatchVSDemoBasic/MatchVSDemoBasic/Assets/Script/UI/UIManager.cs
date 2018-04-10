using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager {
	private Dictionary<UIType, GameObject> _uiDic = new Dictionary<UIType, GameObject>();
	private Transform _canvas;

	public UIManager() {
		_canvas = GameObject.Find("Canvas").transform;
		foreach (Transform item in _canvas) {
			GameObject.Destroy(item.gameObject);
		}
	}

	public GameObject GetSingleUI(UIType type) {
		if (!_uiDic.ContainsKey(type) || _uiDic[type] == null) {
			GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(type.Path));
			go.transform.SetParent(_canvas,false);
			_uiDic.Add(type, go);
		}
		return _uiDic[type];
	}
}