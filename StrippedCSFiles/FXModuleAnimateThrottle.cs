using System.Runtime.CompilerServices;
using UnityEngine;

public class FXModuleAnimateThrottle : PartModule
{
	[KSPField]
	public string animationName;

	[KSPField]
	public int layer;

	[KSPField]
	public float responseSpeed;

	[KSPField]
	public bool dependOnEngineState;

	[KSPField]
	public bool dependOnOutput;

	[KSPField]
	public bool dependOnThrottle;

	[KSPField]
	public bool preferMultiMode;

	[KSPField]
	public int engineIndex;

	[KSPField]
	public string engineName;

	[KSPField]
	public bool weightOnOperational;

	[KSPField]
	public float baseAnimSpeed;

	[KSPField]
	public int animWrapMode;

	[KSPField]
	public bool affectTime;

	[KSPField]
	public float baseAnimSpeedMult;

	[KSPField]
	public bool playInEditor;

	[KSPField(isPersistant = true)]
	public float animState;

	private Animation anim;

	private IEngineStatus engineReference;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FXModuleAnimateThrottle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeDisable()
	{
		throw null;
	}
}
