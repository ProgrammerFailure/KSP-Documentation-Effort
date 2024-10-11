using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleSeeThroughObject : PartModule
{
	[KSPField]
	public string transformName;

	[KSPField]
	public string shaderName;

	[KSPField]
	public float screenRadius;

	[KSPField]
	public float proximityBias;

	[KSPField]
	public float minOpacity;

	[KSPField]
	public int leadModuleIndex;

	[KSPField]
	public float leadModuleTgtValue;

	[KSPField]
	public float leadModuleTgtGain;

	[NonSerialized]
	private MeshRenderer[] mrs;

	[NonSerialized]
	private Transform trf;

	private bool setup;

	private float opacity;

	private float screenHeightRecip;

	[NonSerialized]
	private Shader seeThroughShader;

	[NonSerialized]
	private IScalarModule leadModule;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleSeeThroughObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseFadeUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetCursorProximity(Vector3 cursorPosition, float range, Transform trf, Camera referenceCamera)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetOpacity(float o)
	{
		throw null;
	}
}
