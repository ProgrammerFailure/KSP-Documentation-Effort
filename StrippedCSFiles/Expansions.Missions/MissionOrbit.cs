using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

[Serializable]
public class MissionOrbit : OrbitSnapshot
{
	private double smaCache;

	private double degreesMNA;

	private double bodyRotationEditor;

	public Orbit Orbit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	[MEGUI_Dropdown(addDefaultOption = false, order = 0, SetDropDownItems = "SetCelestialBodies", gapDisplay = true, guiName = "#autoLOC_8200024")]
	public CelestialBody Body
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

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8100290", maxValue = 1200000f, clampTextInput = false, displayUnits = "", minValue = 0f, resetValue = "100000", group = "Base Parameters", order = 1, guiName = "#autoLOC_8100058")]
	public double SemiMajorAxis
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

	[MEGUI_InputField(group = "Base Parameters", ContentType = MEGUI_Control.InputContentType.DecimalNumber, order = 2, groupDisplayName = "#autoLOC_8100290", resetValue = "50000", guiName = "#autoLOC_8100059")]
	public double Apoapsis
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

	[MEGUI_InputField(group = "Base Parameters", ContentType = MEGUI_Control.InputContentType.DecimalNumber, order = 3, groupDisplayName = "#autoLOC_8100290", resetValue = "50000", guiName = "#autoLOC_8100060")]
	public double Periapsis
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

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8100301", canBeReset = true, maxValue = 0.9999f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = 0f, resetValue = "0.000", displayFormat = "F4", group = "Additional Parameters", order = 4, guiName = "#autoLOC_8100061")]
	public double Eccentricity
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

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8100301", canBeReset = true, maxValue = 180f, clampTextInput = true, roundToPlaces = 4, displayUnits = "°", minValue = 0f, resetValue = "0", displayFormat = "F2", group = "Additional Parameters", order = 5, guiName = "#autoLOC_8100062")]
	public double Inclination
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

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8100301", canBeReset = true, maxValue = 360f, clampTextInput = true, roundToPlaces = 4, displayUnits = "°", minValue = 0f, resetValue = "0.0", displayFormat = "F2", group = "Additional Parameters", order = 6, guiName = "#autoLOC_8100063")]
	public double Lan
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

	[MEGUI_NumberRange(groupDisplayName = "#autoLOC_8100301", canBeReset = true, maxValue = 360f, clampTextInput = true, roundToPlaces = 4, displayUnits = "°", minValue = 0f, resetValue = "0.0", displayFormat = "F2", group = "Additional Parameters", order = 7, guiName = "#autoLOC_8100064")]
	public double ArgumentOfPeriapsis
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

	public double Epoch
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

	[MEGUI_NumberRange(canBeReset = true, maxValue = 360f, clampTextInput = true, roundToPlaces = 4, displayUnits = "°", minValue = 0f, resetValue = "0.0", displayFormat = "F2", group = "Additional Parameters", order = 8, guiName = "#autoLOC_8100065")]
	public double MeanAnomalyAtEpoch
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
	public MissionOrbit(CelestialBody bodyRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionOrbit(Orbit orbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionOrbit(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Orbit RelativeOrbit(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double GetUTOrbitRotation(Orbit orbit, double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<MEGUIDropDownItem> SetCelestialBodies()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetNodeBodyParameterString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetEditorMaxOrbitRadius(CelestialBody body)
	{
		throw null;
	}
}
