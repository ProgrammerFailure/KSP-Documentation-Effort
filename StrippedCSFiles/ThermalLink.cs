using System.Runtime.CompilerServices;
using UnityEngine;

public class ThermalLink
{
	public Part remotePart;

	public double contactArea;

	public double contactAreaSqrt;

	public double temperatureDelta;

	public Vector3 partToNode;

	public Vector3 remoteToNode;

	public bool useOrientation;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ThermalLink(Part part, Part remotePart)
	{
		throw null;
	}
}
