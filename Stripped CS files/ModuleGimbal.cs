using System;
using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using ns9;
using UnityEngine;

public class ModuleGimbal : PartModule, ITorqueProvider
{
	[KSPField]
	public string gimbalTransformName = "thrustTransform";

	[KSPField]
	public string actionSuffix = "";

	[UI_Toggle(disabledText = "#autoLOC_7000036", scene = UI_Scene.All, enabledText = "#autoLOC_7000035", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = true, guiName = "#autoLoc_6003043")]
	public bool gimbalLock;

	[KSPField(isPersistant = true, guiActive = true, guiName = "#autoLOC_6001383")]
	[UI_FloatRange(minValue = 0f, stepIncrement = 1f, maxValue = 100f, affectSymCounterparts = UI_Scene.All)]
	public float gimbalLimiter = 100f;

	[KSPField]
	public float gimbalRange = 10f;

	[KSPField]
	public float gimbalRangeXP = -1f;

	[KSPField]
	public float gimbalRangeYP = -1f;

	[KSPField]
	public float gimbalRangeXN = -1f;

	[KSPField]
	public float gimbalRangeYN = -1f;

	[KSPField]
	public bool flipYZ;

	[KSPField]
	public float xMult = 1f;

	[KSPField]
	public float yMult = 1f;

	[KSPField]
	public bool showToggles = true;

	[KSPField(isPersistant = true)]
	public bool currentShowToggles;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001331")]
	public bool enableYaw = true;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001330")]
	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	public bool enablePitch = true;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001332")]
	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	public bool enableRoll = true;

	[KSPField]
	public float minRollOffset = 0.1f;

	[KSPField]
	public bool useGimbalResponseSpeed;

	[KSPField]
	public float gimbalResponseSpeed = 10f;

	[KSPField(isPersistant = true)]
	public bool gimbalActive;

	public List<Transform> gimbalTransforms;

	public List<Quaternion> initRots;

	public Vector3 actuationLocal;

	public Vector3[] oldActuationLocal;

	public Vector3 actuation;

	public Transform t;

	public float targetAngleYaw;

	public float targetAnglePitch;

	public float targetAngleRoll;

	public Vector3 rCoM;

	public Vector3 toRollAxis;

	public List<List<KeyValuePair<ModuleEngines, float>>> engineMultsList;

	public List<AdjusterGimbalBase> adjusterCache = new List<AdjusterGimbalBase>();

	public static string cacheAutoLOC_221352;

	public static string cacheAutoLOC_7000023;

	public static string cacheAutoLOC_221424;

	public static string cacheAutoLOC_6003043;

	[KSPEvent(advancedTweakable = true, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001384")]
	public void ToggleToggles()
	{
		currentShowToggles = !currentShowToggles;
		UpdateToggles();
	}

	[KSPAction("#autoLOC_6001385")]
	public void ToggleAction(KSPActionParam param)
	{
		if (gimbalLock)
		{
			FreeAction(param);
		}
		else
		{
			LockAction(param);
		}
	}

	[KSPAction("#autoLOC_6001386")]
	public void LockAction(KSPActionParam param)
	{
		gimbalLock = true;
	}

	[KSPAction("#autoLOC_6001387")]
	public void FreeAction(KSPActionParam param)
	{
		gimbalLock = false;
	}

	[KSPAction("#autoLOC_6001388", KSPActionGroup.None, true)]
	public void TogglePitchAction(KSPActionParam param)
	{
		enablePitch = !enablePitch;
	}

	[KSPAction("#autoLOC_6001389", KSPActionGroup.None, true)]
	public void ToggleYawAction(KSPActionParam param)
	{
		enableYaw = !enableYaw;
	}

	[KSPAction("#autoLOC_6001390", KSPActionGroup.None, true)]
	public void ToggleRollAction(KSPActionParam param)
	{
		enableRoll = !enableRoll;
	}

	public void FixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight || !gimbalActive || !moduleIsEnabled || base.vessel == null || base.vessel.ReferenceTransform == null)
		{
			return;
		}
		if (gimbalTransforms == null)
		{
			OnStart(StartState.Flying);
		}
		Vector3 localCoM = base.vessel.ReferenceTransform.InverseTransformPoint(base.vessel.CurrentCoM);
		float num = gimbalResponseSpeed * TimeWarp.fixedDeltaTime;
		int i = 0;
		for (int count = gimbalTransforms.Count; i < count; i++)
		{
			t = gimbalTransforms[i];
			t.localRotation = initRots[i];
			if (!gimbalLock)
			{
				actuation.x = (enablePitch ? base.vessel.ctrlState.pitch : 0f);
				actuation.y = (enableRoll ? base.vessel.ctrlState.roll : 0f);
				actuation.z = (enableYaw ? base.vessel.ctrlState.yaw : 0f);
				actuation = ApplyControlAdjustments(actuation);
				actuationLocal = GimbalRotation(t, actuation, localCoM);
				if (useGimbalResponseSpeed)
				{
					actuationLocal.x = Mathf.Lerp(oldActuationLocal[i].x, actuationLocal.x, num);
					actuationLocal.y = Mathf.Lerp(oldActuationLocal[i].y, actuationLocal.y, num);
					oldActuationLocal[i] = actuationLocal;
				}
				t.localRotation = initRots[i] * Quaternion.AngleAxis(actuationLocal.x, xMult * Vector3.right) * Quaternion.AngleAxis(actuationLocal.y, yMult * (flipYZ ? Vector3.forward : Vector3.up));
			}
		}
	}

	public Vector3 GimbalRotation(Transform t, Vector3 vector3_0, Vector3 localCoM)
	{
		Vector3 vector = base.vessel.ReferenceTransform.InverseTransformPoint(t.position);
		float num = 1f;
		if (localCoM.y < vector.y)
		{
			num = -1f;
		}
		vector3_0.x *= num;
		vector3_0.z *= num;
		if (vector3_0.y != 0f && enableRoll)
		{
			if (vector.x > minRollOffset)
			{
				vector3_0.x += vector3_0.y;
			}
			else if (vector.x < 0f - minRollOffset)
			{
				vector3_0.x -= vector3_0.y;
			}
			if (vector.z > minRollOffset)
			{
				vector3_0.z += vector3_0.y;
			}
			else if (vector.z < 0f - minRollOffset)
			{
				vector3_0.z -= vector3_0.y;
			}
		}
		Vector3 result = t.InverseTransformDirection(base.vessel.ReferenceTransform.TransformDirection(vector3_0));
		result.x = Mathf.Clamp(result.x, -1f, 1f) * ((result.x > 0f) ? gimbalRangeXP : gimbalRangeXN) * gimbalLimiter * 0.01f;
		result.y = Mathf.Clamp(result.y, -1f, 1f) * ((result.y > 0f) ? gimbalRangeYP : gimbalRangeYN) * gimbalLimiter * 0.01f;
		return result;
	}

	public void UpdateToggles()
	{
		bool flag = showToggles && currentShowToggles && moduleIsEnabled;
		BaseField baseField = base.Fields["enableYaw"];
		bool guiActive = (base.Fields["enableYaw"].guiActiveEditor = flag);
		baseField.guiActive = guiActive;
		BaseField baseField2 = base.Fields["enablePitch"];
		guiActive = (base.Fields["enablePitch"].guiActiveEditor = flag);
		baseField2.guiActive = guiActive;
		BaseField baseField3 = base.Fields["enableRoll"];
		guiActive = (base.Fields["enableRoll"].guiActiveEditor = flag);
		baseField3.guiActive = guiActive;
		base.Events["ToggleToggles"].guiActive = (base.Events["ToggleToggles"].guiActiveEditor = showToggles && moduleIsEnabled);
		base.Events["ToggleToggles"].guiName = (currentShowToggles ? cacheAutoLOC_221352 : cacheAutoLOC_7000023);
	}

	public override void OnStart(StartState state)
	{
		gimbalTransforms = new List<Transform>(base.part.FindModelTransforms(gimbalTransformName));
		initRots = new List<Quaternion>();
		oldActuationLocal = new Vector3[gimbalTransforms.Count];
		int num = 0;
		int i = 0;
		for (int count = gimbalTransforms.Count; i < count; i++)
		{
			Transform transform = gimbalTransforms[i];
			initRots.Add(transform.localRotation);
			oldActuationLocal[num++] = Vector3.zero;
			if (!string.IsNullOrEmpty(actionSuffix))
			{
				SetActionsSuffix();
			}
		}
		UpdatePAWUI();
	}

	public void SetActionsSuffix()
	{
		base.Actions["ToggleAction"].guiName = base.Actions["ToggleAction"].guiName + " (" + actionSuffix + ")";
		base.Actions["LockAction"].guiName = base.Actions["LockAction"].guiName + " (" + actionSuffix + ")";
		base.Actions["FreeAction"].guiName = base.Actions["FreeAction"].guiName + " (" + actionSuffix + ")";
		base.Actions["TogglePitchAction"].guiName = base.Actions["TogglePitchAction"].guiName + " (" + actionSuffix + ")";
		base.Actions["ToggleYawAction"].guiName = base.Actions["ToggleYawAction"].guiName + " (" + actionSuffix + ")";
		base.Actions["ToggleRollAction"].guiName = base.Actions["ToggleRollAction"].guiName + " (" + actionSuffix + ")";
		base.Fields["gimbalLimiter"].guiName = base.Fields["gimbalLimiter"].guiName + " (" + actionSuffix + ")";
		base.Fields["gimbalLock"].guiName = base.Fields["gimbalLock"].guiName + " (" + actionSuffix + ")";
	}

	public void UpdatePAWUI()
	{
		BaseField baseField = base.Fields["gimbalLock"];
		bool guiActive = (base.Fields["gimbalLock"].guiActiveEditor = moduleIsEnabled);
		baseField.guiActive = guiActive;
		BaseField baseField2 = base.Fields["gimbalLimiter"];
		guiActive = (base.Fields["gimbalLimiter"].guiActiveEditor = moduleIsEnabled);
		baseField2.guiActive = guiActive;
		base.Actions["ToggleAction"].active = moduleIsEnabled;
		base.Actions["LockAction"].active = moduleIsEnabled;
		base.Actions["FreeAction"].active = moduleIsEnabled;
		base.Actions["TogglePitchAction"].active = moduleIsEnabled;
		base.Actions["ToggleYawAction"].active = moduleIsEnabled;
		base.Actions["ToggleRollAction"].active = moduleIsEnabled;
		UpdateToggles();
	}

	public override void OnLoad(ConfigNode node)
	{
		if (gimbalRangeXP < 0f)
		{
			gimbalRangeXP = gimbalRange;
		}
		if (gimbalRangeYP < 0f)
		{
			gimbalRangeYP = gimbalRange;
		}
		if (gimbalRangeXN < 0f)
		{
			gimbalRangeXN = gimbalRangeXP;
		}
		if (gimbalRangeYN < 0f)
		{
			gimbalRangeYN = gimbalRangeYP;
		}
	}

	public override string GetInfo()
	{
		string text = ((gimbalRangeXN == gimbalRangeXP) ? ((gimbalRangeYN != gimbalRangeYP) ? Localizer.Format("#autoLOC_221413", gimbalRangeXP.ToString("F2"), gimbalRangeYP.ToString("F2"), gimbalRangeYN.ToString("F2")) : ((gimbalRangeXP != gimbalRangeYP) ? Localizer.Format("#autoLOC_221410", gimbalRangeXP.ToString("F2"), gimbalRangeYP.ToString("F2")) : Localizer.Format("#autoLOC_221408", gimbalRange.ToString("F2")))) : ((gimbalRangeYN != gimbalRangeYP) ? Localizer.Format("#autoLOC_221420", gimbalRangeXP.ToString("F2"), gimbalRangeXN.ToString("F2"), gimbalRangeYP.ToString("F2"), gimbalRangeYN.ToString("F2")) : Localizer.Format("#autoLOC_221418", gimbalRangeXP.ToString("F2"), gimbalRangeXN.ToString("F2"), gimbalRangeYP.ToString("F2"))));
		if (!moduleIsEnabled)
		{
			text += cacheAutoLOC_221424;
		}
		return text;
	}

	public override bool IsStageable()
	{
		return false;
	}

	public override void OnActive()
	{
		gimbalActive = true;
	}

	public void CreateEngineList()
	{
		if (engineMultsList == null)
		{
			engineMultsList = new List<List<KeyValuePair<ModuleEngines, float>>>();
		}
		else
		{
			engineMultsList.Clear();
		}
		int i = 0;
		for (int count = gimbalTransforms.Count; i < count; i++)
		{
			Transform parent = gimbalTransforms[i];
			List<KeyValuePair<ModuleEngines, float>> list = new List<KeyValuePair<ModuleEngines, float>>();
			int count2 = base.part.Modules.Count;
			while (count2-- > 0)
			{
				if (!(base.part.Modules[count2] is ModuleEngines moduleEngines))
				{
					continue;
				}
				KeyValuePair<ModuleEngines, float> item = new KeyValuePair<ModuleEngines, float>(moduleEngines, 0f);
				int count3 = moduleEngines.thrustTransforms.Count;
				while (count3-- > 0)
				{
					Transform find = moduleEngines.thrustTransforms[count3];
					if ((bool)Part.FindTransformInChildrenExplicit(parent, find))
					{
						item = new KeyValuePair<ModuleEngines, float>(moduleEngines, item.Value + moduleEngines.thrustTransformMultipliers[count3]);
					}
				}
				list.Add(item);
			}
			engineMultsList.Add(list);
		}
	}

	public void GetPotentialTorque(out Vector3 pos, out Vector3 neg)
	{
		pos = (neg = Vector3.zero);
		if (gimbalLock || !moduleIsEnabled)
		{
			return;
		}
		if (engineMultsList == null)
		{
			CreateEngineList();
		}
		Vector3 currentCoM = base.vessel.CurrentCoM;
		Vector3 localCoM = base.vessel.ReferenceTransform.InverseTransformPoint(currentCoM);
		int count = gimbalTransforms.Count;
		while (count-- > 0)
		{
			Transform transform = gimbalTransforms[count];
			Vector3 vector = transform.position - currentCoM;
			float magnitude = vector.magnitude;
			float magnitude2 = Vector3.ProjectOnPlane(vector, base.vessel.ReferenceTransform.up).magnitude;
			int count2 = engineMultsList[count].Count;
			while (count2-- > 0)
			{
				KeyValuePair<ModuleEngines, float> keyValuePair = engineMultsList[count][count2];
				float num = keyValuePair.Value * keyValuePair.Key.finalThrust;
				if (num > 0f)
				{
					float num2 = magnitude * num;
					Vector3 vector2 = GimbalRotation(transform, Vector3.right, localCoM);
					float num3 = Mathf.Sin(Mathf.Abs(vector2.x) * ((float)Math.PI / 180f)) + Mathf.Sin(Mathf.Abs(vector2.y) * ((float)Math.PI / 180f));
					pos.x += num3 * num2;
					vector2 = GimbalRotation(transform, -Vector3.right, localCoM);
					num3 = Mathf.Sin(Mathf.Abs(vector2.x) * ((float)Math.PI / 180f)) + Mathf.Sin(Mathf.Abs(vector2.y) * ((float)Math.PI / 180f));
					neg.x += num3 * num2;
					vector2 = GimbalRotation(transform, Vector3.forward, localCoM);
					num3 = Mathf.Sin(Mathf.Abs(vector2.x) * ((float)Math.PI / 180f)) + Mathf.Sin(Mathf.Abs(vector2.y) * ((float)Math.PI / 180f));
					pos.z += num3 * num2;
					vector2 = GimbalRotation(transform, -Vector3.forward, localCoM);
					num3 = Mathf.Sin(Mathf.Abs(vector2.x) * ((float)Math.PI / 180f)) + Mathf.Sin(Mathf.Abs(vector2.y) * ((float)Math.PI / 180f));
					neg.z += num3 * num2;
					if (magnitude2 > minRollOffset)
					{
						float num4 = magnitude2 * num;
						vector2 = GimbalRotation(transform, Vector3.up, localCoM);
						num3 = Mathf.Sin(Mathf.Abs(vector2.x) * ((float)Math.PI / 180f)) + Mathf.Sin(Mathf.Abs(vector2.y) * ((float)Math.PI / 180f));
						pos.y += num3 * num4;
						vector2 = GimbalRotation(transform, -Vector3.up, localCoM);
						num3 = Mathf.Sin(Mathf.Abs(vector2.x) * ((float)Math.PI / 180f)) + Mathf.Sin(Mathf.Abs(vector2.y) * ((float)Math.PI / 180f));
						neg.y += num3 * num4;
					}
				}
			}
		}
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_6003043;
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterGimbalBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterGimbalBase item = adjuster as AdjusterGimbalBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public Vector3 ApplyControlAdjustments(Vector3 control)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			control = adjusterCache[i].ApplyControlAdjustment(control);
		}
		return control;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_221352 = Localizer.Format("#autoLOC_221352");
		cacheAutoLOC_7000023 = Localizer.Format("#autoLOC_7000023");
		cacheAutoLOC_221424 = Localizer.Format("#autoLOC_221424");
		cacheAutoLOC_6003043 = Localizer.Format("#autoLoc_6003043");
	}
}
