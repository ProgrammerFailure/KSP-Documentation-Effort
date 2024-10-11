using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_VesselGroundLocation]
public class MEGUIParameterCelestialBody_VesselGroundLocation : MEGUICompoundParameter
{
	private GAPCelestialBody gapCB;

	private GAPCelestialBody_SurfaceGizmo_PlaceVessel gizmoRef;

	private MEGUIParameterDropdownList dropDownBodies;

	private MEGUIParameterNumberRange rangeLatitude;

	private MEGUIParameterNumberRange rangeLongitude;

	private MEGUIParameterInputField inputAltitude;

	private MEGUIParameterQuaternion quaternionVessel;

	private bool disableRotateX;

	private bool disableRotateY;

	private bool disableRotateZ;

	public VesselGroundLocation FieldValue
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
	public MEGUIParameterCelestialBody_VesselGroundLocation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name, object value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGizmoIconChange(VesselGroundLocation.GizmoIcon newIcon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputLatitude(float inputValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputLongitude(float inputValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputRot(Quaternion quaternion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDropDownBody(int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGapLeftArrow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGapRightArrow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGAPPointTranslate(double latitude, double longitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGapPointRotate(Quaternion quaternion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateGizmo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void DisplayGAP()
	{
		throw null;
	}
}
