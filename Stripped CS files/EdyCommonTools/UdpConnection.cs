using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace EdyCommonTools;

public class UdpConnection
{
	public class UdpState
	{
		public UdpClient client;

		public IPEndPoint endPoint;
	}

	public UdpClient m_client;

	public IPEndPoint m_listenPoint;

	public UdpState m_state;

	public byte[] m_receivedBytes;

	public bool m_received;

	public IPEndPoint m_sendPoint;

	public bool m_sent;

	public int m_lastBytesSent;

	public bool connected => m_client != null;

	public string localDescription
	{
		get
		{
			if (m_client == null)
			{
				return "none";
			}
			return m_listenPoint.ToString();
		}
	}

	public string remoteDescription
	{
		get
		{
			if (m_client == null)
			{
				return "none";
			}
			return m_sendPoint.ToString();
		}
	}

	public bool messageReceived
	{
		get
		{
			if (m_client != null)
			{
				return m_received;
			}
			return false;
		}
	}

	public int receivedMessageSize
	{
		get
		{
			if (!messageReceived)
			{
				return 0;
			}
			return m_receivedBytes.Length;
		}
	}

	public int lastBytesSent
	{
		get
		{
			if (m_client == null)
			{
				return 0;
			}
			return m_lastBytesSent;
		}
	}

	public bool canSendMessage
	{
		get
		{
			if (m_client != null && m_sendPoint.Address != IPAddress.Any)
			{
				return m_sent;
			}
			return false;
		}
	}

	public void StartConnection(int listenPort)
	{
		if (m_client != null)
		{
			StopConnection();
		}
		m_listenPoint = new IPEndPoint(IPAddress.Any, listenPort);
		m_client = new UdpClient(m_listenPoint);
		m_state = new UdpState();
		m_state.endPoint = m_listenPoint;
		m_state.client = m_client;
		m_sendPoint = new IPEndPoint(IPAddress.Any, listenPort);
		m_sent = true;
		QueryNextMessage();
	}

	public void StopConnection()
	{
		if (m_client != null)
		{
			m_client.Close();
			m_client = null;
		}
	}

	public string GetMessageString()
	{
		if (m_client != null && m_received)
		{
			string @string = Encoding.ASCII.GetString(m_receivedBytes);
			m_received = false;
			QueryNextMessage();
			return @string;
		}
		return "";
	}

	public int GetMessageBinary(byte[] buffer)
	{
		if (m_client != null && m_received)
		{
			int num = Mathf.Min(buffer.Length, m_receivedBytes.Length);
			for (int i = 0; i < num; i++)
			{
				buffer[i] = m_receivedBytes[i];
			}
			m_received = false;
			QueryNextMessage();
			return num;
		}
		return 0;
	}

	public void QueryNextMessage()
	{
		m_received = false;
		m_client.BeginReceive(MessageReceived, m_state);
	}

	public void MessageReceived(IAsyncResult ar)
	{
		UdpState udpState = (UdpState)ar.AsyncState;
		m_receivedBytes = udpState.client.EndReceive(ar, ref udpState.endPoint);
		m_received = true;
		if (m_sendPoint.Address == IPAddress.Any)
		{
			m_sendPoint.Address = udpState.endPoint.Address;
		}
	}

	public void SetDestination(string dest, int port)
	{
		if (m_client != null)
		{
			if (!IPAddress.TryParse(dest, out var address))
			{
				address = Dns.GetHostEntry(dest).AddressList[0];
			}
			m_sendPoint = new IPEndPoint(address, port);
		}
	}

	public void SendMessageBinary(byte[] bytesToSend)
	{
		SendMessageBinary(bytesToSend, bytesToSend.Length);
	}

	public void SendMessageBinary(byte[] bytesToSend, int size)
	{
		if (canSendMessage && bytesToSend.Length != 0)
		{
			m_sent = false;
			m_client.BeginSend(bytesToSend, size, m_sendPoint, MessageSent, m_client);
		}
	}

	public void SendMessageString(string message)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(message);
		SendMessageBinary(bytes);
	}

	public void MessageSent(IAsyncResult ar)
	{
		UdpClient udpClient = (UdpClient)ar.AsyncState;
		m_lastBytesSent = udpClient.EndSend(ar);
		m_sent = true;
	}

	public static void SendMessageSync(string server, int port, byte[] bytesToSend)
	{
		UdpClient udpClient = new UdpClient();
		try
		{
			udpClient.Send(bytesToSend, bytesToSend.Length, server, port);
		}
		catch (Exception ex)
		{
			Debug.LogError("SendMessageSync error: " + ex.ToString());
		}
	}

	public static void SendMessageSync(string server, int port, string message)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(message);
		SendMessageSync(server, port, bytes);
	}

	public static void SendMessageAsync(string server, int port, byte[] bytesToSend)
	{
		UdpClient udpClient = new UdpClient();
		IPEndPoint endPoint = new IPEndPoint(Dns.GetHostEntry(server).AddressList[0], port);
		udpClient.BeginSend(bytesToSend, bytesToSend.Length, endPoint, MessageSentA, udpClient);
	}

	public static void SendMessageAsync(string server, int port, string message)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(message);
		SendMessageAsync(server, port, bytes);
	}

	public static void MessageSentA(IAsyncResult ar)
	{
		UdpClient obj = (UdpClient)ar.AsyncState;
		obj.EndSend(ar);
		obj.Close();
	}
}
