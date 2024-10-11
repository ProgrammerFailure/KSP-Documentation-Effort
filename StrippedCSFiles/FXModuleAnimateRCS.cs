using System.Runtime.CompilerServices;
using UnityEngine;

public class FXModuleAnimateRCS : PartModule
{
	[KSPField]
	public string animationName;

	[KSPField]
	public int layer;

	[KSPField]
	public float responseSpeed;

	[KSPField]
	public float baseAnimSpeed;

	[KSPField]
	public int animWrapMode;

	[KSPField]
	public bool affectTime;

	[KSPField]
	public float baseAnimSpeedMult;

	[KSPField]
	public float thrustForceMult;

	public float[] animState;

	[SerializeField]
	private Animation[] anims;

	[SerializeField]
	private ModuleRCS moduleRCSReference;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FXModuleAnimateRCS()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}
}
