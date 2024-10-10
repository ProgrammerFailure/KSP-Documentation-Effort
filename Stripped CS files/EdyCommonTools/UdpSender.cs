using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace EdyCommonTools;

public class UdpSender
{
	public UdpClient m_client;

	public IPEndPoint m_sendPoint;

	public UdpSender(string server, int port)
	{
		m_client = new UdpClient();
		IPAddress address = IPAddress.Parse(server);
		m_sendPoint = new IPEndPoint(address, port);
	}

	~UdpSender()
	{
		Close();
	}

	public void SendSync(byte[] bytesToSend, int maxBytes = -1)
	{
		maxBytes = Mathf.Min(bytesToSend.Length, maxBytes);
		if (maxBytes < 0)
		{
			maxBytes = bytesToSend.Length;
		}
		m_client.Send(bytesToSend, maxBytes, m_sendPoint);
	}

	public void SendAsync(byte[] bytesToSend, int maxBytes = -1)
	{
		maxBytes = Mathf.Min(bytesToSend.Length, maxBytes);
		if (maxBytes < 0)
		{
			maxBytes = bytesToSend.Length;
		}
		m_client.BeginSend(bytesToSend, maxBytes, m_sendPoint, MessageSentA, m_client);
	}

	public void MessageSentA(IAsyncResult ar)
	{
		((UdpClient)ar.AsyncState).EndSend(ar);
	}

	public void Close()
	{
		m_client.Close();
	}
}
