using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SingleTone<T> where T : class {
	private static T _instance;
	public static T Instance
	{
		get { return _instance; }
	}

	public static void Creator() {
		_instance = (T)Activator.CreateInstance(typeof(T), false);
	}
}