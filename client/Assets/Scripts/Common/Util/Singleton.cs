using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : new()
{
	protected static T _instance = default(T);
	private static UnityEngine.Object _objLock = new UnityEngine.Object();

	protected Singleton()
	{
		Debug.Assert(_instance == null);
	}

	public static void Depose()
	{
		_instance = default(T);
	}

	public static T instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_objLock)
				{
					if (_instance == null)
					{
						_instance = new T();
					}
				}
			}
			return _instance;
		}
	}
}
