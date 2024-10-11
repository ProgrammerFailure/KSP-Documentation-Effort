using System.Runtime.CompilerServices;

namespace EdyCommonTools;

public class SmoothFloat
{
	private float m_sum;

	private float m_smoothValue;

	private float[] m_buffer;

	private int m_size;

	private int m_start;

	private int m_end;

	private int m_count;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SmoothFloat(int size)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetValue(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Resize(int targetSize)
	{
		throw null;
	}
}
