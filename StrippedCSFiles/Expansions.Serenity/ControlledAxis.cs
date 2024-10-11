using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity;

[Serializable]
public class ControlledAxis : ControlledBase
{
	[SerializeField]
	internal string axisName;

	[SerializeField]
	public FloatCurve timeValue;

	[SerializeField]
	internal BaseAxisFieldList SymmetryFields;

	private static readonly FloatCurve refSineCurve;

	private static readonly FloatCurve refSquareCurve;

	private static readonly FloatCurve refTriangleCurve;

	private static readonly FloatCurve refSawCurve;

	private static readonly FloatCurve refRevSawCurve;

	private string lastUsedRefName;

	private float lastUsedRefMin;

	private float lastUsedRefMax;

	private FloatCurve lastUsedRefCurve;

	private AnimationCurve lastUsedCurve;

	private List<Keyframe> lastUsedRefKeys;

	private int refCurveCyclesStepIncrement;

	private int refCurveCyclesPadding;

	private Keyframe tempKeyframe;

	public BaseAxisField AxisField
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	internal override string BaseName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float axisMin
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float axisMax
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ControlledAxis(ControlledAxis sourceAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ControlledAxis(Part part, PartModule module, BaseAxisField axisField, ModuleRoboticController controller)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ControlledAxis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ControlledAxis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReverseCurve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InvertCurve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RescaleCurveTime(float adjustmentRatio, float minSpace = 0.01f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AlignCurveEnds()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClampAllPointValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClampPointValue(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PresetFlat()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PresetSine(float cycles, float phase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PresetSquare(float cycles, float phase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PresetTriangle(float cycles, float phase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PresetSaw(float cycles, float phase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PresetRevSaw(float cycles, float phase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdatePreset(float cycles, float phase)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ClearPresetRefs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool GenerateCurveFromReference(string refCurveID, float cycles, float phase, FloatCurve referenceCurve, out FloatCurve generatedCurve)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool OnChangeSymmetryMaster(Part newPart, out uint oldPartId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFieldValue(float time, float valueMultiple = 1f, bool updateSymmetryPartners = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSoftLimits(Vector2 newLimits, bool updateSymmetryPartners = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool OnAssignReferenceVars()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void ClearSymmetryLists()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void AddSymmetryPart(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSave(ConfigNode node)
	{
		throw null;
	}
}
