using System.Runtime.CompilerServices;
using UnityEngine;

public class TutorialFromMun : TutorialScenario
{
	private Texture2D navBallVectors;

	private TutorialPage welcome;

	private TutorialPage deathBonusPage;

	private KFSMEvent onDeath;

	private FloatCurve velocityPitch;

	private double pitchDesired;

	private DirectionTarget pitchTarget;

	private ManeuverNode mNode;

	private Part decoupler;

	private ModuleParachute chute;

	private double terrainHeightLast;

	private double vSpeed;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TutorialFromMun()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAssetSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnTutorialSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnOnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateDirectionTarget()
	{
		throw null;
	}
}
