using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[Serializable]
public class GroundMaterial
{
	public enum SurfaceType
	{
		Hard,
		Soft
	}

	public PhysicMaterial physicMaterial;

	public float grip;

	public float drag;

	public VPGroundMarksRenderer marksRenderer;

	public VPGroundParticleEmitter particleEmitter;

	public SurfaceType surfaceType;

	public object customData;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GroundMaterial()
	{
		throw null;
	}
}
