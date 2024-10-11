using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleDisplaceTweak : PartModule
{
	[KSPField]
	public string tgtTransformName;

	[KSPField]
	public string tweakName;

	[KSPField]
	public Vector3 axis;

	[KSPField]
	public float displaceMax;

	[KSPField]
	public float displaceMin;

	[KSPField(guiFormat = "0.0", isPersistant = true, guiActiveEditor = true, guiName = "#autoLOC_6001809")]
	public float scalar;

	private Transform tgtTrf;

	private Vector3 p0;

	private Vector3 pMax;

	private Vector3 pMin;

	private int mIndex;

	private ModuleDisplaceTweak MDprev;

	private ModuleDisplaceTweak MDnext;

	private ModuleDisplaceTweak mD;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleDisplaceTweak()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartEvent(ConstructionEventType evt, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateMorph(float t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetFinalPosition()
	{
		throw null;
	}
}
