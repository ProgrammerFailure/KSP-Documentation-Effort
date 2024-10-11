using System.Runtime.CompilerServices;
using UnityEngine;

namespace LibNoise;

public class FastTurbulence : IModule
{
	private FastNoise XDistort;

	private FastNoise YDistort;

	private FastNoise ZDistort;

	public IModule SourceModule
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public double Power
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public double Frequency
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public int Roughness
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public int Seed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FastTurbulence(IModule sourceModule)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetValue(Vector3d coordinate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetValue(Vector3 coordinate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetValue(double x, double y, double z)
	{
		throw null;
	}
}
