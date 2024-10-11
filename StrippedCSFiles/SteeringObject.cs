using System.Runtime.CompilerServices;
using UnityEngine;

public class SteeringObject
{
	public enum ControlAxis
	{
		Forward,
		Right,
		Up
	}

	public enum AlignmentAxis
	{
		Forward,
		Up,
		Right,
		None
	}

	public Transform pivot;

	public Quaternion neutralAngle;

	public ControlAxis controlAxis;

	public AlignmentAxis alignmentAxis;

	public float wheelDriveInvert;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SteeringObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FindAlignmentAxis(Transform referenceTransform, Transform partTransform)
	{
		throw null;
	}
}
