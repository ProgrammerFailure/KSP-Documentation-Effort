using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

public class VPBlockDebugger : VehicleBehaviour
{
	public enum AngularVelocityUnits
	{
		RadsPerSec,
		RevsPerMin
	}

	public enum ChangeDetection
	{
		Exact,
		Tolerance
	}

	public AngularVelocityUnits angularVelocityUnits;

	public ChangeDetection changeDetection;

	public float changeTolerance;

	[Space(5f)]
	public Font font;

	public Texture2D boxTexture;

	public Vector2 screenPosition;

	private List<VehicleBase.SolverState> m_states;

	private GUIStyle m_style;

	private GUIStyle m_boxStyle;

	private string m_text;

	private int m_charWidth;

	private int m_lineLength;

	private int m_lines;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPBlockDebugger()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableComponent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValidate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDisableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RecordSolverState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTextProperties()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTelemetryData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetAngularVelocity(VehicleBase.BlockState blockState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetIntString(int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetFloatString(float value, float prevValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetFloatString(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetTypeString(Type type)
	{
		throw null;
	}
}
