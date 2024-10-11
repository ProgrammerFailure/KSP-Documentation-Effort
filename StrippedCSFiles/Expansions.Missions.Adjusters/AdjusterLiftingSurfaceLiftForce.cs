using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using UnityEngine;

namespace Expansions.Missions.Adjusters;

public class AdjusterLiftingSurfaceLiftForce : AdjusterLiftingSurfaceBase
{
	[MEGUI_NumberRange(minValue = 0f, roundToPlaces = 0, displayUnits = "%", displayFormat = "0.##", maxValue = 200f, resetValue = "10", guiName = "#autoLOC_8100243")]
	public float liftForceMultiplier;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterLiftingSurfaceLiftForce()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AdjusterLiftingSurfaceLiftForce(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Vector3 ApplyLiftForceAdjustment(Vector3 liftForce)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}
}
