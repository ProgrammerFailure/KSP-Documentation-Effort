using Smooth.Dispose;
using UnityEngine;

public class SmoothDisposer : MonoBehaviour
{
	public static SmoothDisposer _instance;

	public void Awake()
	{
		if ((bool)_instance)
		{
			Debug.LogWarning("Only one " + GetType().Name + " should exist at a time, instantiated by the " + typeof(DisposalQueue).Name + " class.");
			Object.Destroy(this);
		}
		else
		{
			_instance = this;
			Object.DontDestroyOnLoad(this);
		}
	}

	public void LateUpdate()
	{
		DisposalQueue.Pulse();
	}
}
