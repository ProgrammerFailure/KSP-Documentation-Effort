using System.Collections.Generic;
using UnityEngine;

namespace VehiclePhysics;

public class DataRecorder
{
	public class FloatData
	{
		public int frame;

		public float value;
	}

	public List<FloatData[]> m_floatChannels = new List<FloatData[]>();

	public List<Vector3[]> m_vector3Channels = new List<Vector3[]>();

	public List<Quaternion[]> m_quaternionChannels = new List<Quaternion[]>();

	public float m_deltaTime;

	public int m_frameCapacity;

	public int m_writeFrame;

	public int m_readFrame;

	public int m_firstFrame;

	public int m_lastFrame;

	public DataRecorder(float recordTime, float recordDeltaTime)
	{
		if (recordDeltaTime < 0.001f)
		{
			recordDeltaTime = 0.001f;
		}
		m_frameCapacity = Mathf.RoundToInt(recordTime / recordDeltaTime);
		if (m_frameCapacity <= 0)
		{
			m_frameCapacity = 1;
		}
		m_deltaTime = recordDeltaTime;
	}

	public void NextWriteFrame()
	{
		m_writeFrame++;
		m_firstFrame = m_writeFrame - m_frameCapacity;
		if (m_firstFrame < 0)
		{
			m_firstFrame = 0;
		}
		m_lastFrame = m_writeFrame - 1;
		if (m_readFrame < m_firstFrame)
		{
			m_readFrame = m_firstFrame;
		}
	}

	public void RewriteFromRead()
	{
		if (m_writeFrame != 0)
		{
			m_writeFrame = m_readFrame + 1;
			m_lastFrame = m_readFrame;
		}
	}

	public int GetWriteFrame()
	{
		return m_writeFrame;
	}

	public int GetFirstFrame()
	{
		return m_firstFrame;
	}

	public int GetLastFrame()
	{
		return m_lastFrame;
	}

	public void ResetData()
	{
		m_writeFrame = 0;
		m_readFrame = 0;
		m_firstFrame = 0;
		m_lastFrame = 0;
	}

	public void GetReadPointers(float fromTime, float toTime, out int fromFrame, out int frameCount)
	{
		if (m_writeFrame == 0)
		{
			fromFrame = 0;
			frameCount = 0;
			return;
		}
		fromFrame = (int)(fromTime / m_deltaTime);
		fromFrame = Mathf.Clamp(fromFrame, m_firstFrame, m_lastFrame);
		int value = Mathf.CeilToInt(toTime / m_deltaTime);
		value = Mathf.Clamp(value, fromFrame, m_lastFrame);
		frameCount = value - fromFrame + 1;
	}

	public int ReadTimeSpan(float fromTime, float toTime)
	{
		GetReadPointers(fromTime, toTime, out var fromFrame, out var frameCount);
		if (frameCount > 0)
		{
			m_readFrame = fromFrame;
		}
		return frameCount;
	}

	public void MoveReadToFrame(int frame)
	{
		m_readFrame = Mathf.Clamp(frame, m_firstFrame, m_lastFrame);
	}

	public void MoveReadToTime(float time)
	{
		MoveReadToFrame((int)(time / m_deltaTime));
	}

	public void MoveReadToStart()
	{
		m_readFrame = m_firstFrame;
	}

	public void MoveReadToEnd()
	{
		m_readFrame = m_lastFrame;
	}

	public bool NextReadFrame()
	{
		if (m_readFrame < m_lastFrame)
		{
			m_readFrame++;
			return true;
		}
		return false;
	}

	public bool PrevReadFrame()
	{
		if (m_readFrame > m_firstFrame)
		{
			m_readFrame--;
			return true;
		}
		return false;
	}

	public int NewFloatChannel()
	{
		FloatData[] array = new FloatData[m_frameCapacity];
		for (int i = 0; i < m_frameCapacity; i++)
		{
			array[i] = new FloatData();
		}
		m_floatChannels.Add(array);
		return m_floatChannels.Count - 1;
	}

	public void WriteFloatValue(int channel, float value)
	{
		if (channel >= 0 && channel < m_floatChannels.Count)
		{
			FloatData obj = m_floatChannels[channel][m_writeFrame % m_frameCapacity];
			obj.frame = m_writeFrame;
			obj.value = value;
		}
	}

	public float ReadFloatValue(int channel)
	{
		if (channel >= 0 && channel < m_floatChannels.Count)
		{
			FloatData floatData = m_floatChannels[channel][m_readFrame % m_frameCapacity];
			if (floatData.frame != m_readFrame)
			{
				return float.NaN;
			}
			return floatData.value;
		}
		return float.NaN;
	}

	public int NewVector3Channel()
	{
		Vector3[] item = new Vector3[m_frameCapacity];
		m_vector3Channels.Add(item);
		return m_vector3Channels.Count - 1;
	}

	public void WriteVector3Value(int channel, Vector3 value)
	{
		if (channel >= 0 && channel < m_vector3Channels.Count)
		{
			m_vector3Channels[channel][m_writeFrame % m_frameCapacity] = value;
		}
	}

	public Vector3 ReadVector3Value(int channel)
	{
		if (channel >= 0 && channel < m_vector3Channels.Count)
		{
			return m_vector3Channels[channel][m_readFrame % m_frameCapacity];
		}
		return Vector3.zero;
	}

	public int NewQuaternionChannel()
	{
		Quaternion[] item = new Quaternion[m_frameCapacity];
		m_quaternionChannels.Add(item);
		return m_vector3Channels.Count - 1;
	}

	public void WriteQuaternionValue(int channel, Quaternion value)
	{
		if (channel >= 0 && channel < m_quaternionChannels.Count)
		{
			m_quaternionChannels[channel][m_writeFrame % m_frameCapacity] = value;
		}
	}

	public Quaternion ReadQuaternionValue(int channel)
	{
		if (channel >= 0 && channel < m_quaternionChannels.Count)
		{
			return m_quaternionChannels[channel][m_readFrame % m_frameCapacity];
		}
		return Quaternion.identity;
	}
}
