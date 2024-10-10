using System;
using ns9;
using UnityEngine;

public class ModuleAeroSurface : ModuleControlSurface, IMultipleDragCube
{
	[KSPField]
	public float uncasedTemp = -1f;

	[KSPField]
	public float casedTemp = -1f;

	[KSPField]
	public float ctrlRangeFactor = 0.2f;

	[KSPField]
	public bool brakeDeployInvert;

	[KSPAxisField(minValue = 0f, incrementalSpeed = 20f, isPersistant = true, axisMode = KSPAxisMode.Incremental, maxValue = 100f, guiActive = false, guiName = "#autoLOC_6001336")]
	public float aeroAuthorityLimiter = 100f;

	[KSPField(guiActiveUnfocused = true, guiFormat = "0", isPersistant = false, guiActive = true, unfocusedRange = 25f, guiName = "#autoLOC_6001336", guiUnits = "°")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 0.05f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float aeroAuthorityLimiterUI = 100f;

	[KSPAxisField(unfocusedRange = 25f, isPersistant = true, guiActiveUnfocused = true, maxValue = 100f, incrementalSpeed = 50f, guiFormat = "0", axisMode = KSPAxisMode.Incremental, minValue = -100f, guiActive = true, guiName = "#autoLOC_6013041", guiUnits = "°")]
	[UI_FloatRange(scene = UI_Scene.All, stepIncrement = 0.1f, maxValue = 100f, minValue = 0f, affectSymCounterparts = UI_Scene.All)]
	public float aeroDeployAngle = float.NaN;

	public Vector2 aeroDeployAngleLimits = new Vector2(float.NaN, 0f);

	public static string cacheAutoLOC_6003025;

	public override void OnAwake()
	{
		base.OnAwake();
		deployInvert = brakeDeployInvert;
	}

	public void Start()
	{
		base.Actions["ActionToggle"].active = false;
	}

	public void ModifyAeroAuthorityLimiter(object field)
	{
		aeroAuthorityLimiterUI = ctrlSurfaceRange * aeroAuthorityLimiter * 0.01f;
		if (UIPartActionController.Instance != null)
		{
			UIPartActionWindow item = UIPartActionController.Instance.GetItem(base.part);
			if (item != null)
			{
				item.displayDirty = true;
			}
		}
	}

	public void ModifyAeroAuthorityLimiterUI(object field)
	{
		aeroAuthorityLimiter = 100f * aeroAuthorityLimiterUI / ctrlSurfaceRange;
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		if (float.IsNaN(aeroDeployAngle))
		{
			aeroDeployAngle = ctrlSurfaceRange;
		}
		if (float.IsNaN(aeroDeployAngleLimits.x))
		{
			aeroDeployAngleLimits.x = 0f;
		}
		if (float.IsNaN(aeroDeployAngleLimits.y))
		{
			aeroDeployAngleLimits.y = ctrlSurfaceRange * 1.5f;
		}
		if (HighLogic.LoadedSceneIsGame)
		{
			ctrlSurface = base.part.FindModelTransform(transformName);
			if (ctrlSurface != null)
			{
				neutral = ctrlSurface.localRotation;
			}
		}
		deployInvert = brakeDeployInvert;
		base.Fields["deployInvert"].guiActive = false;
		base.Fields["deployInvert"].guiActiveEditor = false;
		ignoreRoll = true;
		base.Fields["ignoreRoll"].guiActive = false;
		base.Fields["ignoreRoll"].guiActiveEditor = false;
		authorityLimiter = 100f;
		base.Fields["authorityLimiter"].guiActive = false;
		base.Fields["authorityLimiter"].guiActiveEditor = false;
		(base.Fields["authorityLimiter"] as BaseAxisField).active = false;
		base.Fields["authorityLimiterUI"].guiActive = false;
		base.Fields["authorityLimiterUI"].guiActiveEditor = false;
		base.Fields["deployAngle"].guiActive = false;
		base.Fields["deployAngle"].guiActiveEditor = false;
		(base.Fields["deployAngle"] as BaseAxisField).active = false;
		base.Fields["aeroAuthorityLimiter"].OnValueModified += ModifyAeroAuthorityLimiter;
		base.Fields["aeroAuthorityLimiterUI"].OnValueModified += ModifyAeroAuthorityLimiterUI;
		if (base.Fields.TryGetFieldUIControl<UI_FloatRange>("aeroAuthorityLimiterUI", out var control))
		{
			control.minValue = 0f;
			control.maxValue = ctrlSurfaceRange;
			control.stepIncrement = 0.05f * ctrlSurfaceRange;
		}
		ModifyAeroAuthorityLimiter(null);
	}

	public override void OnInventoryModeDisable()
	{
		base.OnInventoryModeDisable();
		if (base.part.protoPartSnapshot != null)
		{
			ProtoPartModuleSnapshot protoPartModuleSnapshot = base.part.protoPartSnapshot.FindModule("ModuleAeroSurface");
			if (protoPartModuleSnapshot != null && protoPartModuleSnapshot.moduleValues != null)
			{
				protoPartModuleSnapshot.moduleValues.SetValue("deploy", deploy.ToString());
			}
		}
	}

	public override void OnInventoryModeEnable()
	{
		if (ctrlSurface == null)
		{
			ctrlSurface = base.part.FindModelTransform(transformName);
		}
		if (ctrlSurface != null)
		{
			if (deploy)
			{
				deflection = aeroDeployAngle;
				ctrlSurface.localRotation = Quaternion.AngleAxis(deflection, Vector3.right) * neutral;
			}
			else
			{
				deflection = 0f;
				ctrlSurface.localRotation = neutral;
			}
		}
	}

	public override void OnStoredInInventory(ModuleInventoryPart moduleInventoryPart)
	{
		base.OnStoredInInventory(moduleInventoryPart);
		deploy = false;
	}

	[KSPAction("#autoLOC_6001329", KSPActionGroup.Brakes)]
	public void ActionToggleBrakes(KSPActionParam act)
	{
		if (!HighLogic.LoadedSceneIsEditor && act.group == KSPActionGroup.Brakes)
		{
			deploy = base.vessel.ActionGroups[act.group];
		}
		else
		{
			deploy = !deploy;
		}
	}

	public override string GetInfo()
	{
		string text = Localizer.Format("#autoLOC_213511", deflectionLiftCoeff) + "\n" + Localizer.Format("#autoLOC_213512", ctrlSurfaceArea * 100f) + "\n" + Localizer.Format("#autoLOC_213513", ctrlSurfaceRange.ToString()) + "\n" + Localizer.Format("#autoLOC_213514", actuatorSpeed.ToString());
		if (uncasedTemp > 0f && casedTemp > 0f)
		{
			text += Localizer.Format("#autoLOC_213516", uncasedTemp.ToString("F0"), casedTemp.ToString("F0"));
		}
		return text;
	}

	public override void CtrlSurfaceUpdate(Vector3 vel)
	{
		Vessel vessel = base.vessel;
		Transform referenceTransform = vessel.ReferenceTransform;
		Vector3 up = referenceTransform.up;
		Vector3 vector = vessel.CurrentCoM - baseTransform.position;
		inputVector = referenceTransform.rotation * new Vector3(ignorePitch ? 0f : vessel.ctrlState.pitch, 0f, ignoreYaw ? 0f : vessel.ctrlState.yaw);
		Vector3 rhs = Vector3.Cross(Vector3.Project(baseTransform.up, -up), Vector3.ProjectOnPlane(vector, up));
		action = Mathf.Clamp01(Vector3.Dot(inputVector, rhs)) * ctrlSurfaceRange * aeroAuthorityLimiter * 0.01f;
		if (deploy)
		{
			action += (deployInvert ? (-1f) : 1f) * aeroDeployAngle;
		}
		float num = 1.5f * ctrlSurfaceRange;
		action = Mathf.Clamp(action, 0f - num, num);
		if (uncasedTemp > 0f && casedTemp > 0f)
		{
			base.part.skinMaxTemp = (((double)Math.Abs(action) > 1E-09) ? uncasedTemp : casedTemp);
		}
		if (!useExponentialSpeed)
		{
			deflection = Mathf.MoveTowards(deflection, action, actuatorSpeed * TimeWarp.fixedDeltaTime);
		}
		else
		{
			deflection = Mathf.Lerp(deflection, action, actuatorSpeed * TimeWarp.fixedDeltaTime);
		}
		ctrlSurface.localRotation = Quaternion.AngleAxis(deflection, Vector3.right) * neutral;
	}

	public override void CtrlSurfaceEditorUpdate(Vector3 CoM)
	{
		action = 0f;
		if (deploy)
		{
			action += (deployInvert ? (-1f) : 1f) * aeroAuthorityLimiter;
		}
		float num = 1.5f * ctrlSurfaceRange;
		action = Mathf.Clamp(action, 0f - num, num);
		if (!useExponentialSpeed)
		{
			deflection = Mathf.MoveTowards(deflection, action, actuatorSpeed * TimeWarp.fixedDeltaTime);
		}
		else
		{
			deflection = Mathf.Lerp(deflection, action, actuatorSpeed * TimeWarp.fixedDeltaTime);
		}
		ctrlSurface.localRotation = Quaternion.AngleAxis(deflection, Vector3.right) * neutral;
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_6003025;
	}

	public new static void CacheLocalStrings()
	{
		cacheAutoLOC_6003025 = Localizer.Format("#autoLoc_6003025");
	}
}
