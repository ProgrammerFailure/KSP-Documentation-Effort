using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CallbackUtil
{
	public class LazyCoroutineData
	{
		public Coroutine coroutine;

		public readonly Callback cb;

		public int t;

		public int delay;

		public LazyCoroutineData(Callback cb)
		{
			this.cb = cb;
		}
	}

	public static Dictionary<MonoBehaviour, Dictionary<Callback, LazyCoroutineData>> lazyCoroutines;

	public static IEnumerator DelayedCallback(float timeToWait, Callback cb)
	{
		yield return new WaitForSeconds(timeToWait);
		cb();
	}

	public static IEnumerator DelayedCallback(int framesToWait, Callback cb)
	{
		for (int i = 0; i < framesToWait; i++)
		{
			yield return null;
		}
		cb();
	}

	public static IEnumerator DelayedCallback(IEnumerable yieldObj, Callback cb)
	{
		yield return yieldObj;
		cb();
	}

	public static IEnumerator DelayedCallback<T>(float timeToWait, Callback<T> cb, T arg)
	{
		yield return new WaitForSeconds(timeToWait);
		cb(arg);
	}

	public static IEnumerator DelayedCallback<T>(int framesToWait, Callback<T> cb, T arg)
	{
		for (int i = 0; i < framesToWait; i++)
		{
			yield return null;
		}
		cb(arg);
	}

	public static IEnumerator DelayedCallback<T>(IEnumerable yieldObj, Callback<T> cb, T arg)
	{
		yield return yieldObj;
		cb(arg);
	}

	public static IEnumerator DelayedCallback<T, U>(float timeToWait, Callback<T, U> cb, T arg1, U arg2)
	{
		yield return new WaitForSeconds(timeToWait);
		cb(arg1, arg2);
	}

	public static IEnumerator DelayedCallback<T, U>(int framesToWait, Callback<T, U> cb, T arg1, U arg2)
	{
		for (int i = 0; i < framesToWait; i++)
		{
			yield return null;
		}
		cb(arg1, arg2);
	}

	public static IEnumerator DelayedCallback<T, U>(IEnumerable yieldObj, Callback<T, U> cb, T arg1, U arg2)
	{
		yield return yieldObj;
		cb(arg1, arg2);
	}

	public static IEnumerator DelayedCallback<T, U, V>(float timeToWait, Callback<T, U, V> cb, T arg1, U arg2, V arg3)
	{
		yield return new WaitForSeconds(timeToWait);
		cb(arg1, arg2, arg3);
	}

	public static IEnumerator DelayedCallback<T, U, V>(int framesToWait, Callback<T, U, V> cb, T arg1, U arg2, V arg3)
	{
		for (int i = 0; i < framesToWait; i++)
		{
			yield return null;
		}
		cb(arg1, arg2, arg3);
	}

	public static IEnumerator DelayedCallback<T, U, V>(IEnumerable yieldObj, Callback<T, U, V> cb, T arg1, U arg2, V arg3)
	{
		yield return yieldObj;
		cb(arg1, arg2, arg3);
	}

	public static IEnumerator WaitUntil(Func<bool> condition, Callback cb)
	{
		while (!condition())
		{
			yield return null;
		}
		cb();
	}

	public static IEnumerator DoUntil(Callback cb, Func<bool> condition)
	{
		while (!condition())
		{
			cb();
			yield return null;
		}
	}

	public static IEnumerator HoldUntil(Func<bool> condition)
	{
		while (!condition())
		{
			yield return null;
		}
	}

	public static void Clear(this Callback cb)
	{
		cb = delegate
		{
		};
	}

	public static void Clear<T>(this Callback<T> cb)
	{
		cb = delegate
		{
		};
	}

	public static void Clear<T, U>(this Callback<T, U> cb)
	{
		cb = delegate
		{
		};
	}

	public static void Clear<T, U, V>(this Callback<T, U, V> cb)
	{
		cb = delegate
		{
		};
	}

	public static void LazyCallback(Callback cb, MonoBehaviour coroutineHost, int framedelay)
	{
		if (lazyCoroutines == null)
		{
			lazyCoroutines = new Dictionary<MonoBehaviour, Dictionary<Callback, LazyCoroutineData>>();
		}
		Dictionary<Callback, LazyCoroutineData> dictionary;
		if (!lazyCoroutines.ContainsKey(coroutineHost))
		{
			dictionary = new Dictionary<Callback, LazyCoroutineData>();
			lazyCoroutines.Add(coroutineHost, dictionary);
		}
		else
		{
			dictionary = lazyCoroutines[coroutineHost];
		}
		LazyCoroutineData lazyCoroutineData;
		if (!dictionary.ContainsKey(cb))
		{
			lazyCoroutineData = new LazyCoroutineData(cb);
			dictionary.Add(cb, lazyCoroutineData);
		}
		else
		{
			lazyCoroutineData = dictionary[cb];
		}
		lazyCoroutineData.t = Time.frameCount;
		lazyCoroutineData.delay = Mathf.Max(lazyCoroutineData.delay, framedelay);
		if (lazyCoroutineData.coroutine == null)
		{
			lazyCoroutineData.coroutine = coroutineHost.StartCoroutine(lazyCB(lazyCoroutineData, dictionary, coroutineHost));
		}
	}

	public static IEnumerator lazyCB(LazyCoroutineData lzcd, Dictionary<Callback, LazyCoroutineData> lzDict, MonoBehaviour host)
	{
		while (Time.frameCount < lzcd.t + lzcd.delay)
		{
			yield return null;
		}
		lzcd.cb();
		lzcd.coroutine = null;
		lzDict.Remove(lzcd.cb);
		if (lzDict.Keys.Count == 0)
		{
			lazyCoroutines.Remove(host);
		}
	}
}
