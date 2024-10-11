using System.Runtime.CompilerServices;
using UnityEngine;

public class FXModuleThrottleEffects : PartModuleFXSetter
{
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

	[KSPField(isPersistant = true)]
	public float state;

	private Animation anim;

	private IEngineStatus engineReference;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FXModuleThrottleEffects()
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
	public void Update()
	{
		throw null;
	}
}
