using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity.RobotArmFX;

public class RobotArmFXDrill : RobotArmScannerFX
{
	protected RaycastHit hitInfo;

	public string drillBaseTransformName;

	public string drillTipTransformName;

	public string drillEffectTransformName;

	protected Transform drillBaseTransform;

	protected Transform drillTipTransform;

	protected Transform drillEffectTransform;

	protected List<KSPParticleEmitter> drillImpactParticleEmitters;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RobotArmFXDrill()
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
