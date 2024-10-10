using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Smooth.Dispose;

public static class DisposalQueue
{
	public static readonly object queueLock;

	public static Queue<IDisposable> enqueue;

	public static Queue<IDisposable> dispose;

	static DisposalQueue()
	{
		queueLock = new object();
		enqueue = new Queue<IDisposable>();
		dispose = new Queue<IDisposable>();
		new Thread(Dispose).Start();
		new GameObject(typeof(SmoothDisposer).Name).AddComponent<SmoothDisposer>();
	}

	public static void Enqueue(IDisposable item)
	{
		lock (queueLock)
		{
			enqueue.Enqueue(item);
		}
	}

	public static void Pulse()
	{
		lock (queueLock)
		{
			Monitor.Pulse(queueLock);
		}
	}

	public static void Dispose()
	{
		while (true)
		{
			lock (queueLock)
			{
				while (enqueue.Count == 0)
				{
					Monitor.Wait(queueLock);
				}
				Queue<IDisposable> queue = enqueue;
				enqueue = dispose;
				dispose = queue;
			}
			while (dispose.Count > 0)
			{
				try
				{
					dispose.Dequeue().Dispose();
				}
				catch (ThreadAbortException)
				{
				}
				catch (Exception message)
				{
					Debug.LogError(message);
				}
			}
		}
	}
}
