using System;
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

	public float grip = 1f;

	public float drag;

	public VPGroundMarksRenderer marksRenderer;

	public VPGroundParticleEmitter particleEmitter;

	public SurfaceType surfaceType;

	public object customData;
}
