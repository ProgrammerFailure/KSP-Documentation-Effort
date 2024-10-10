using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Serenity;

[Serializable]
public class ControlledAxis : ControlledBase
{
	[SerializeField]
	public string axisName = "";

	[SerializeField]
	public FloatCurve timeValue;

	[SerializeField]
	public BaseAxisFieldList SymmetryFields;

	public static readonly FloatCurve refSineCurve;

	public static readonly FloatCurve refSquareCurve;

	public static readonly FloatCurve refTriangleCurve;

	public static readonly FloatCurve refSawCurve;

	public static readonly FloatCurve refRevSawCurve;

	public string lastUsedRefName;

	public float lastUsedRefMin;

	public float lastUsedRefMax;

	public FloatCurve lastUsedRefCurve;

	public AnimationCurve lastUsedCurve;

	public List<Keyframe> lastUsedRefKeys;

	public int refCurveCyclesStepIncrement = 10;

	public int refCurveCyclesPadding = 4;

	public Keyframe tempKeyframe;

	public BaseAxisField AxisField { get; set; }

	public override string BaseName => axisName;

	public float axisMin => AxisField.minValue;

	public float axisMax => AxisField.maxValue;

	public ControlledAxis(ControlledAxis sourceAxis)
		: this(sourceAxis.Part, sourceAxis.Module, sourceAxis.AxisField, sourceAxis.Controller)
	{
		timeValue = new FloatCurve(sourceAxis.timeValue.Curve.keys);
	}

	public ControlledAxis(Part part, PartModule module, BaseAxisField axisField, ModuleRoboticController controller)
		: base(part, module, controller)
	{
		axisName = axisField.name;
		AxisField = axisField;
		timeValue = new FloatCurve(new Keyframe[2]
		{
			new Keyframe(0f, axisField.minValue),
			new Keyframe(1f, axisField.minValue)
		});
		RebuildSymmetryList();
	}

	public ControlledAxis()
	{
		AxisField = null;
		timeValue = new FloatCurve(new Keyframe[2]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 0f)
		});
		SymmetryFields = new BaseAxisFieldList();
	}

	static ControlledAxis()
	{
		refSineCurve = new FloatCurve(new Keyframe[3]
		{
			new Keyframe(0f, 1f, 0f, 0f),
			new Keyframe(0.5f, 0f, 0f, 0f),
			new Keyframe(1f, 1f, 0f, 0f)
		});
		refSquareCurve = new FloatCurve(new Keyframe[5]
		{
			new Keyframe(0f, 0f, 0f, 0f),
			new Keyframe(0.499f, 0f, 0f, 1000f),
			new Keyframe(0.5f, 1f, 1000f, 0f),
			new Keyframe(0.999f, 1f, 0f, -1000f),
			new Keyframe(1f, 0f, -1000f, 0f)
		});
		refTriangleCurve = new FloatCurve(new Keyframe[3]
		{
			new Keyframe(0f, 0f, -2f, 2f),
			new Keyframe(0.5f, 1f, 2f, -2f),
			new Keyframe(1f, 0f, -2f, 2f)
		});
		refSawCurve = new FloatCurve(new Keyframe[3]
		{
			new Keyframe(0f, 0f, 0f, 1f),
			new Keyframe(0.999f, 1f, 1f, -1000f),
			new Keyframe(1f, 0f, -1000f, 1f)
		});
		refRevSawCurve = new FloatCurve(new Keyframe[3]
		{
			new Keyframe(0f, 1f, 0f, -1f),
			new Keyframe(0.999f, 0f, -1f, 1000f),
			new Keyframe(1f, 1f, 1000f, -1f)
		});
	}

	public void ReverseCurve()
	{
		Keyframe[] array = new Keyframe[timeValue.Curve.length];
		for (int i = 0; i < timeValue.Curve.length; i++)
		{
			tempKeyframe = timeValue.Curve[timeValue.Curve.length - 1 - i];
			array[i].time = timeValue.maxTime - tempKeyframe.time;
			array[i].value = tempKeyframe.value;
			array[i].inTangent = 0f - tempKeyframe.outTangent;
			array[i].outTangent = 0f - tempKeyframe.inTangent;
		}
		timeValue = new FloatCurve(array);
	}

	public void InvertCurve()
	{
		for (int i = 0; i < timeValue.Curve.length; i++)
		{
			tempKeyframe = timeValue.Curve[i];
			tempKeyframe.value = axisMax - (tempKeyframe.value - axisMin);
			tempKeyframe.inTangent = 0f - tempKeyframe.inTangent;
			tempKeyframe.outTangent = 0f - tempKeyframe.outTangent;
			timeValue.Curve.MoveKey(i, tempKeyframe);
		}
	}

	public void RescaleCurveTime(float adjustmentRatio, float minSpace = 0.01f)
	{
		float maxTime = timeValue.maxTime;
		for (int i = 0; i < timeValue.Curve.length; i++)
		{
			tempKeyframe = timeValue.Curve[i];
			tempKeyframe.time *= adjustmentRatio;
			if (adjustmentRatio > 1f)
			{
				tempKeyframe.time = Mathf.Min(tempKeyframe.time, maxTime - minSpace * (float)(timeValue.Curve.length - i - 1));
			}
			timeValue.Curve.MoveKey(i, tempKeyframe);
		}
		if (adjustmentRatio < 1f)
		{
			timeValue.Add(maxTime, timeValue.Curve[timeValue.Curve.length - 1].value);
			tempKeyframe = timeValue.Curve[timeValue.Curve.length - 2];
			tempKeyframe.outTangent = 0f;
			timeValue.Curve.MoveKey(timeValue.Curve.length - 2, tempKeyframe);
			tempKeyframe = timeValue.Curve[timeValue.Curve.length - 1];
			tempKeyframe.inTangent = 0f;
			timeValue.Curve.MoveKey(timeValue.Curve.length - 1, tempKeyframe);
		}
	}

	public void AlignCurveEnds()
	{
		Keyframe key = timeValue.Curve[timeValue.Curve.length - 1];
		key.value = timeValue.Curve[0].value;
		key.inTangent = timeValue.Curve[0].outTangent;
		timeValue.Curve.MoveKey(timeValue.Curve.length - 1, key);
	}

	public void ClampAllPointValues()
	{
		for (int i = 0; i < timeValue.Curve.length; i++)
		{
			tempKeyframe = timeValue.Curve[i];
			tempKeyframe.value = Mathf.Clamp(tempKeyframe.value, axisMin, axisMax);
			timeValue.Curve.MoveKey(i, tempKeyframe);
		}
	}

	public void ClampPointValue(int index)
	{
		if (index >= 0 && index <= timeValue.Curve.length)
		{
			tempKeyframe = timeValue.Curve[index];
			tempKeyframe.value = Mathf.Clamp(tempKeyframe.value, axisMin, axisMax);
			timeValue.Curve.MoveKey(index, tempKeyframe);
		}
	}

	public void PresetFlat()
	{
		float maxTime = timeValue.maxTime;
		float value = (axisMax - axisMin) / 2f;
		if (AxisField.module is IAxisFieldLimits axisFieldLimits && axisFieldLimits.HasAxisFieldLimit(axisName))
		{
			AxisFieldLimit axisFieldLimit = axisFieldLimits.GetAxisFieldLimit(axisName);
			value = (axisFieldLimit.softLimits.y - axisFieldLimit.softLimits.x) / 2f;
		}
		timeValue = new FloatCurve(new Keyframe[2]
		{
			new Keyframe(0f, value),
			new Keyframe(maxTime, value)
		});
		lastUsedRefName = "stockFlat";
	}

	public void PresetSine(float cycles, float phase)
	{
		if (GenerateCurveFromReference("stockSine", cycles, phase, refSineCurve, out var generatedCurve))
		{
			timeValue = generatedCurve;
		}
	}

	public void PresetSquare(float cycles, float phase)
	{
		if (GenerateCurveFromReference("stockSquare", cycles, phase, refSquareCurve, out var generatedCurve))
		{
			timeValue = generatedCurve;
		}
	}

	public void PresetTriangle(float cycles, float phase)
	{
		if (GenerateCurveFromReference("stockTriangle", cycles, phase, refTriangleCurve, out var generatedCurve))
		{
			timeValue = generatedCurve;
		}
	}

	public void PresetSaw(float cycles, float phase)
	{
		if (GenerateCurveFromReference("stockSaw", cycles, phase, refSawCurve, out var generatedCurve))
		{
			timeValue = generatedCurve;
		}
	}

	public void PresetRevSaw(float cycles, float phase)
	{
		if (GenerateCurveFromReference("stockRevSaw", cycles, phase, refRevSawCurve, out var generatedCurve))
		{
			timeValue = generatedCurve;
		}
	}

	public void UpdatePreset(float cycles, float phase)
	{
		if (!(lastUsedRefName == "stockFlat") && GenerateCurveFromReference(lastUsedRefName, cycles, phase, null, out var generatedCurve))
		{
			timeValue = generatedCurve;
		}
	}

	public void ClearPresetRefs()
	{
		lastUsedRefName = "";
		lastUsedRefCurve = null;
	}

	public bool GenerateCurveFromReference(string refCurveID, float cycles, float phase, FloatCurve referenceCurve, out FloatCurve generatedCurve)
	{
		if (lastUsedRefCurve == null && referenceCurve == null)
		{
			Debug.LogError("[ControlledAxis] Unable to Generate curve - no reference supplied and no existing reference to use");
			generatedCurve = null;
			return false;
		}
		float num = axisMax;
		float num2 = axisMin;
		if (AxisField.module as ModuleRoboticController != null)
		{
			num = 1f;
			num2 = 0f;
		}
		if (AxisField.module is IAxisFieldLimits axisFieldLimits && axisFieldLimits.HasAxisFieldLimit(axisName))
		{
			AxisFieldLimit axisFieldLimit = axisFieldLimits.GetAxisFieldLimit(axisName);
			num2 = axisFieldLimit.softLimits.x;
			num = axisFieldLimit.softLimits.y;
		}
		int num3 = Math.Max(Mathf.CeilToInt((cycles + (float)refCurveCyclesPadding) / (float)refCurveCyclesStepIncrement), 2) * refCurveCyclesStepIncrement;
		if (lastUsedRefName != refCurveID || lastUsedRefKeys == null || lastUsedCurve == null || lastUsedRefMax != num || lastUsedRefMin != num2 || lastUsedCurve.length < num3)
		{
			lastUsedRefMin = num2;
			lastUsedRefMax = num;
			lastUsedRefName = refCurveID;
			if (referenceCurve != null)
			{
				lastUsedRefCurve = referenceCurve;
			}
			else
			{
				referenceCurve = lastUsedRefCurve;
			}
			lastUsedRefKeys = new List<Keyframe>();
			lastUsedRefKeys.Add(new Keyframe(referenceCurve.Curve.keys[0].time, num2 + referenceCurve.Curve.keys[0].value * (num - num2), referenceCurve.Curve.keys[0].inTangent * (num - num2), referenceCurve.Curve.keys[0].outTangent * (num - num2)));
			for (int i = 0; i < num3; i++)
			{
				for (int j = 1; j < referenceCurve.Curve.length; j++)
				{
					Keyframe keyframe = referenceCurve.Curve.keys[j];
					lastUsedRefKeys.Add(new Keyframe(keyframe.time + (float)i, num2 + keyframe.value * (num - num2), keyframe.inTangent * (num - num2), keyframe.outTangent * (num - num2)));
				}
			}
			lastUsedCurve = new AnimationCurve(lastUsedRefKeys.ToArray());
		}
		float num4 = phase / 100f;
		List<Keyframe> list = new List<Keyframe>();
		for (int k = 0; k < lastUsedRefKeys.Count; k++)
		{
			Keyframe item = lastUsedRefKeys[k];
			item.time -= num4;
			if (item.time <= cycles)
			{
				if (list.Count < 1)
				{
					if (item.time < 0f)
					{
						continue;
					}
					if (item.time != num4)
					{
						Keyframe item2 = new Keyframe(0f, lastUsedCurve.Evaluate(num4), 0f, (lastUsedCurve.Evaluate(num4 + 0.001f) - lastUsedCurve.Evaluate(num4)) / 0.001f * cycles);
						list.Add(item2);
					}
				}
				item.time /= cycles;
				item.inTangent *= cycles;
				item.outTangent *= cycles;
				list.Add(item);
				continue;
			}
			if (lastUsedRefKeys[k - 1].time - num4 < cycles)
			{
				Keyframe item3 = new Keyframe(1f, lastUsedCurve.Evaluate(cycles + num4), (lastUsedCurve.Evaluate(cycles + num4) - lastUsedCurve.Evaluate(cycles + num4 - 0.001f)) / 0.001f * cycles, 0f);
				list.Add(item3);
			}
			break;
		}
		generatedCurve = new FloatCurve(list.ToArray());
		return true;
	}

	public override bool OnChangeSymmetryMaster(Part newPart, out uint oldPartId)
	{
		oldPartId = 0u;
		BaseAxisField baseAxisField = null;
		for (int i = 0; i < newPart.Modules.Count; i++)
		{
			baseAxisField = newPart.Modules[i].Fields[axisName] as BaseAxisField;
			if (baseAxisField != null)
			{
				break;
			}
		}
		if (baseAxisField == null)
		{
			Debug.LogErrorFormat("[ControlledAxis]: Cannot change Symmetry Master to {0}. No {1} field on this part", newPart.persistentId, axisName);
			return false;
		}
		oldPartId = partId;
		AxisField = baseAxisField;
		return true;
	}

	public void UpdateFieldValue(float time, float valueMultiple = 1f, bool updateSymmetryPartners = true)
	{
		if (AxisField == null)
		{
			return;
		}
		float num = timeValue.Evaluate(time) * valueMultiple;
		if (RoboticControllerManager.Instance != null)
		{
			RoboticControllerManager.Instance.QueueFieldUpdate(AxisField, num, base.Controller.Priority);
			if (updateSymmetryPartners)
			{
				for (int i = 0; i < SymmetryFields.Count; i++)
				{
					RoboticControllerManager.Instance.QueueFieldUpdate(SymmetryFields[i], num, base.Controller.Priority);
				}
			}
			return;
		}
		AxisField.SetValue(num, AxisField.module);
		if (updateSymmetryPartners)
		{
			for (int j = 0; j < SymmetryFields.Count; j++)
			{
				SymmetryFields[j].SetValue(num, SymmetryFields[j].module);
			}
		}
	}

	public void UpdateSoftLimits(Vector2 newLimits, bool updateSymmetryPartners = true)
	{
		if (AxisField == null || !(AxisField.module is IAxisFieldLimits axisFieldLimits))
		{
			return;
		}
		axisFieldLimits.SetSoftLimits(axisName, newLimits);
		if (!updateSymmetryPartners)
		{
			return;
		}
		for (int i = 0; i < SymmetryFields.Count; i++)
		{
			if (SymmetryFields[i].module is IAxisFieldLimits axisFieldLimits2)
			{
				axisFieldLimits2.SetSoftLimits(axisName, newLimits);
			}
		}
	}

	public override bool OnAssignReferenceVars()
	{
		if (base.Module != null)
		{
			if (base.Module.Fields[axisName] is BaseAxisField axisField)
			{
				AxisField = axisField;
			}
		}
		else
		{
			for (int i = 0; i < base.Part.Modules.Count; i++)
			{
				for (int j = 0; j < base.Part.Modules[i].Fields.Count; j++)
				{
					if (base.Part.Modules[i].Fields[j] is BaseAxisField baseAxisField && baseAxisField.name == axisName)
					{
						AxisField = baseAxisField;
						base.Module = base.Part.Modules[i];
						moduleId = base.Module.GetPersistentId();
						persistentActionId = base.Module.GetPersistenActiontId();
						break;
					}
				}
			}
		}
		if (AxisField == null)
		{
			Debug.LogWarningFormat("[ModuleRoboticController]: Unable to find AxisField in vessel part: {0} - {1}", base.Part.name, axisName);
			return false;
		}
		return true;
	}

	public override void ClearSymmetryLists()
	{
		if (SymmetryFields == null)
		{
			SymmetryFields = new BaseAxisFieldList();
		}
		else
		{
			SymmetryFields.Clear();
		}
	}

	public override void AddSymmetryPart(Part part)
	{
		if (part.Modules.Contains(AxisField.module.ClassName) && part.Modules[AxisField.module.ClassName].Fields[axisName] is BaseAxisField item)
		{
			SymmetryFields.Add(item);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		node.TryGetValue("axisName", ref axisName);
		ConfigNode node2 = new ConfigNode();
		if (node.TryGetNode("timeValueCurve", ref node2))
		{
			timeValue.Load(node2);
		}
		if (timeValue.Curve.length == 0)
		{
			timeValue.Curve.AddKey(0f, 0f);
			timeValue.Curve.AddKey(1f, 0f);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("axisName", axisName);
		timeValue.Save(node.AddNode("timeValueCurve"));
	}
}
