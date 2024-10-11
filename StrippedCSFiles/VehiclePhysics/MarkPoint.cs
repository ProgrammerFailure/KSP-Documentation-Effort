using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

public class MarkPoint
{
	public Vector3 pos;

	public Vector3 normal;

	public Vector4 tangent;

	public Vector3 posl;

	public Vector3 posr;

	public float intensity;

	public int lastIndex;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MarkPoint()
	{
		throw null;
	}
}
