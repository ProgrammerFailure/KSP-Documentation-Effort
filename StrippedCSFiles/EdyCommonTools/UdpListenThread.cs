using System.Runtime.CompilerServices;
using System.Threading;

namespace EdyCommonTools;

public class UdpListenThread
{
	public delegate void OnReceiveData();

	public int threadStopTimeoutMs;

	public int threadSleepIntervalMs;

	public bool debug;

	private UdpConnection m_connection;

	private OnReceiveData m_onReceiveDataCb;

	private Thread m_thread;

	private bool m_threadExit;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UdpListenThread()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	~UdpListenThread()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start(UdpConnection connection, OnReceiveData receiveDataCb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Stop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ListenThread()
	{
		throw null;
	}
}
