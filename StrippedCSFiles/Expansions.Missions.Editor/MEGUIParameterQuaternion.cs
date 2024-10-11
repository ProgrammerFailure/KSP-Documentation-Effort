using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_Quaternion]
public class MEGUIParameterQuaternion : MEGUICompoundParameter
{
	public delegate void OnValueChanged(Quaternion newValue);

	public enum EulerAxis
	{
		x,
		y,
		z
	}

	public OnValueChanged onValueChanged;

	private MEGUIParameterNumberRange rangeX;

	private MEGUIParameterNumberRange rangeY;

	private MEGUIParameterNumberRange rangeZ;

	public MissionQuaternion FieldValue
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameterQuaternion()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValueChange(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetQuaternion(Quaternion quaternion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleEulerAxis(EulerAxis axis, bool toggleValue)
	{
		throw null;
	}
}
