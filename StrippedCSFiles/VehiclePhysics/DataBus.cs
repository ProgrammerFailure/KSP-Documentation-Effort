using System.Runtime.CompilerServices;

namespace VehiclePhysics;

public class DataBus
{
	private int[][] m_data;

	public int[][] bus
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DataBus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int Get(int idChannel, int idValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Set(int idChannel, int idValue, int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int[] Get(int idChannel)
	{
		throw null;
	}
}
