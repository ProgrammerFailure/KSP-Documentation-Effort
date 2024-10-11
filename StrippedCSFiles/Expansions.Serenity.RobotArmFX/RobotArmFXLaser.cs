using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity.RobotArmFX;

public class RobotArmFXLaser : RobotArmScannerFX
{
	public string laserEffectTransformName;

	public string configLaserEffectColor;

	public Color laserEffectColor;

	public float laserEffectWidth;

	public Transform laserEffectTransform;

	public LineRenderer laserLineRenderer;

	public bool hasPerformedSetupRaycast;

	public Vector3 laserTarget;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RobotArmFXLaser()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEffectStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate(float animationTime, float distanceFromSurface, Vector3 instrumentTargetPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEffectStop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}
}
