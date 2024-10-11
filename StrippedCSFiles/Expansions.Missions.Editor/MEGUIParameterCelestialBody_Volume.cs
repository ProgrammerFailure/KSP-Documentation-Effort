using System.Runtime.CompilerServices;

namespace Expansions.Missions.Editor;

[MEGUI_SurfaceVolume]
public class MEGUIParameterCelestialBody_Volume : MEGUICompoundParameter
{
	private GAPCelestialBody gapCB;

	private GAPCelestialBody_SurfaceGizmo_Volume gizmoRef;

	private MEGUIParameterDropdownList dropDownBodies;

	private MEGUIParameterNumberRange rangeLatitude;

	private MEGUIParameterNumberRange rangeLongitude;

	private MEGUIParameterDropdownList dropDownShape;

	private MEGUIParameterNumberRange rangeRadius;

	private MEGUIParameterNumberRange rangeHeightConeMin;

	private MEGUIParameterNumberRange rangeHeightConeMax;

	private MEGUIParameterNumberRange rangeHeightSphere;

	private CelestialBody pastBody;

	public SurfaceVolume FieldValue
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
	public MEGUIParameterCelestialBody_Volume()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AttachCallbacks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDragRadius(float newRadius)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputLatitude(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputLongitude(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MinValuesUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshMinHeightValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputRadius(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputHeightConeMin(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputHeightConeMax(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputHeightSphere(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDropDownBody(int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDropDownShape(int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateGizmo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateGizmoShape()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeftGapClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRightGapClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGAPPointTranslate(double latitude, double longitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGAPSliderSphere(float newHeight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGAPSliderConeBounds(float minValue, float maxValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGAPShapeChange(SurfaceVolume.VolumeShape newShape)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void DisplayGAP()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateMaxValues()
	{
		throw null;
	}
}
