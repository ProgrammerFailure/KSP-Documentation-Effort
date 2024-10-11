using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace EdyCommonTools;

public class UdpConnection
{
	private class UdpState
	{
		public UdpClient client;

		public IPEndPoint endPoint;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public UdpState()
		{
			throw null;
		}
	}

	private UdpClient m_client;

	private IPEndPoint m_listenPoint;

	private UdpState m_state;

	private byte[] m_receivedBytes;

	private bool m_received;

	private IPEndPoint m_sendPoint;

	private bool m_sent;

	private int m_lastBytesSent;

	public bool connected
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string localDescription
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string remoteDescription
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool messageReceived
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int receivedMessageSize
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int lastBytesSent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool canSendMessage
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UdpConnection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartConnection(int listenPort)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StopConnection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetMessageString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetMessageBinary(byte[] buffer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void QueryNextMessage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MessageReceived(IAsyncResult ar)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDestination(string dest, int port)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SendMessageBinary(byte[] bytesToSend)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SendMessageBinary(byte[] bytesToSend, int size)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SendMessageString(string message)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MessageSent(IAsyncResult ar)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SendMessageSync(string server, int port, byte[] bytesToSend)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SendMessageSync(string server, int port, string message)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SendMessageAsync(string server, int port, byte[] bytesToSend)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SendMessageAsync(string server, int port, string message)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void MessageSentA(IAsyncResult ar)
	{
		throw null;
	}
}
