using System;
using System.Threading;
using UnityEngine;

namespace EdyCommonTools;

public class UdpListenThread
{
	public delegate void OnReceiveData();

	public int threadStopTimeoutMs = 500;

	public int threadSleepIntervalMs = 5;

	public bool debug;

	public UdpConnection m_connection;

	public OnReceiveData m_onReceiveDataCb;

	public Thread m_thread;

	public bool m_threadExit;

	~UdpListenThread()
	{
		Stop();
	}

	public void Start(UdpConnection connection, OnReceiveData receiveDataCb)
	{
		if (m_thread != null)
		{
			Stop();
		}
		m_thread = new Thread(ListenThread);
		m_threadExit = false;
		m_connection = connection;
		m_onReceiveDataCb = receiveDataCb;
		if (debug)
		{
			Debug.Log("Thread started");
		}
		m_thread.Start();
	}

	public void Stop()
	{
		if (m_thread != null)
		{
			m_threadExit = true;
			m_thread.Join(threadStopTimeoutMs);
			if (debug)
			{
				Debug.Log("Thread ended: " + !m_thread.IsAlive);
			}
			m_thread = null;
		}
	}

	public void ListenThread()
	{
		try
		{
			while (!m_threadExit)
			{
				if (m_connection.messageReceived && m_onReceiveDataCb != null)
				{
					m_onReceiveDataCb();
				}
				Thread.Sleep(threadSleepIntervalMs);
			}
		}
		catch (Exception ex)
		{
			if (ex is ThreadAbortException)
			{
				Debug.LogWarning("Thread aborted");
			}
			else
			{
				Debug.LogError("Exception inside thread: " + ex.ToString());
			}
		}
	}
}
