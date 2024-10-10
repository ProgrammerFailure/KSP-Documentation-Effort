namespace EdyCommonTools;

public class SmoothFloat
{
	public float m_sum;

	public float m_smoothValue;

	public float[] m_buffer;

	public int m_size;

	public int m_start;

	public int m_end;

	public int m_count;

	public SmoothFloat(int size)
	{
		if (size < 0)
		{
			size = 0;
		}
		m_buffer = new float[size];
		m_size = size;
		Reset();
	}

	public void SetValue(float value)
	{
		if (m_size == 0)
		{
			m_smoothValue = value;
			return;
		}
		m_buffer[m_end++] = value;
		if (m_end >= m_size)
		{
			m_end = 0;
		}
		m_count++;
		m_sum += value;
		m_smoothValue = m_sum / (float)m_count;
		if (m_count == m_size)
		{
			m_sum -= m_buffer[m_start++];
			if (m_start >= m_size)
			{
				m_start = 0;
			}
			m_count--;
		}
	}

	public float GetValue()
	{
		return m_smoothValue;
	}

	public void Reset()
	{
		m_sum = 0f;
		m_smoothValue = 0f;
		m_start = 0;
		m_end = 0;
		m_count = 0;
	}

	public void Resize(int targetSize)
	{
		if (targetSize < 0)
		{
			targetSize = 0;
		}
		if (targetSize != m_size)
		{
			m_buffer = new float[targetSize];
			m_size = targetSize;
			float smoothValue = m_smoothValue;
			Reset();
			SetValue(smoothValue);
		}
	}
}
