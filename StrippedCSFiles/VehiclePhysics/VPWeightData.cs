using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

public class VPWeightData : VehicleBehaviour
{
	[Serializable]
	public class AxleGroup
	{
		public int[] axles;

		public float specification;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AxleGroup()
		{
			throw null;
		}
	}

	public bool showAxleGroups;

	public AxleGroup[] axleGroups;

	public Vector2 screenPosition;

	public Font font;

	private GUIStyle m_textStyle;

	private string m_text;

	private string m_groups;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPWeightData()
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
	public override void FixedUpdateVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTextStyle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTelemetry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetWeightInAxle(int axle)
	{
		throw null;
	}
}
