using System.Collections;
using UnityEngine;

public class CoroutineHost : MonoBehaviour
{
	public bool disposable;

	public static CoroutineHost Create(string name, bool persistThroughSceneChanges, bool disposable)
	{
		CoroutineHost coroutineHost = new GameObject(name).AddComponent<CoroutineHost>();
		coroutineHost.disposable = disposable;
		if (persistThroughSceneChanges)
		{
			Object.DontDestroyOnLoad(coroutineHost);
			Object.DontDestroyOnLoad(coroutineHost.gameObject);
		}
		return coroutineHost;
	}

	public new Coroutine StartCoroutine(IEnumerator coroutine)
	{
		if (!disposable)
		{
			return base.StartCoroutine(coroutine);
		}
		return base.StartCoroutine(RunAndDispose(coroutine));
	}

	public IEnumerator RunAndDispose(IEnumerator coroutine)
	{
		yield return base.StartCoroutine(coroutine);
		Terminate();
	}

	public void Terminate()
	{
		Object.DestroyImmediate(base.gameObject);
	}
}
