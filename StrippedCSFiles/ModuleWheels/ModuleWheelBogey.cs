using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ModuleWheels;

public class ModuleWheelBogey : ModuleWheelSubmodule
{
	[KSPField]
	public string wheelTransformRefName;

	[KSPField]
	public string wheelTransformBaseName;

	[KSPField]
	public string bogeyTransformName;

	[KSPField]
	public string bogeyRefTransformName;

	[KSPField]
	public float maxPitch;

	[KSPField]
	public float minPitch;

	[KSPField]
	public float restPitch;

	[KSPField]
	public float pitchResponse;

	[KSPField]
	public Vector3 bogeyAxis;

	[KSPField]
	public Vector3 bogeyUpAxis;

	[KSPField]
	public int deployModuleIndex;

	private Transform wheelTransformRef;

	private Transform[] wheelTrfs;

	private Transform bogey;

	private Transform bogeyRef;

	public Vector3 bogeyUp;

	private Vector3 bogeyUp0;

	public Quaternion bogeyRot;

	private Quaternion bogeyRot0;

	private Quaternion bogeyRotLast;

	public float bogeyAngle;

	[NonSerialized]
	public ModuleWheelDeployment deployModule;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleWheelBogey()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnWheelSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string OnGatherInfo()
	{
		throw null;
	}
}
