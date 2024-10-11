using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class ManeuverNode : IConfigNode
{
	public double UT;

	public Vector3d DeltaV;

	public Quaternion nodeRotation;

	public ManeuverGizmo attachedGizmo;

	public PatchedConicSolver solver;

	public MapObject scaledSpaceTarget;

	public Orbit patch;

	public Orbit nextPatch;

	public bool refocusCamera;

	public double startBurnIn
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AttachGizmo(GameObject gizmoPrefab, PatchedConicRenderer renderer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnGizmoUpdated(Vector3d dV, double ut)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DetachGizmo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveSelf()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetBurnVector(Orbit currentOrbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetPartialDv()
	{
		throw null;
	}
}
