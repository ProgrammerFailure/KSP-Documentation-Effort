using System.Runtime.CompilerServices;

namespace Expansions.Missions.Editor;

[MEGUI_SurfaceArea]
public class MEGUIParameterCelestialBody_Area : MEGUICompoundParameter
{
	private GAPCelestialBody gapCB;

	private GAPCelestialBody_SurfaceGizmo_Area gizmoRef;

	private MEGUIParameterDropdownList dropDownBodies;

	private MEGUIParameterNumberRange rangeLatitude;

	private MEGUIParameterNumberRange rangeLongitude;

	private MEGUIParameterNumberRange rangeRadius;

	private CelestialBody pastBody;

	public SurfaceArea FieldValue
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
	public MEGUIParameterCelestialBody_Area()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
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
	private void OnInputRadius(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDropDownBody(int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateGizmo()
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
	public override void DisplayGAP()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateMaxRadiusValues()
	{
		throw null;
	}
}
