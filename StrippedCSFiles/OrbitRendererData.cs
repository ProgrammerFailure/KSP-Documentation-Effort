using System.Runtime.CompilerServices;
using UnityEngine;

public class OrbitRendererData
{
	public CelestialBody cb;

	public Color orbitColor;

	public float lowerCamVsSmaRatio;

	public float upperCamVsSmaRatio;

	internal Color nodeColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public OrbitRendererData(CelestialBody cb)
	{
		throw null;
	}
}
