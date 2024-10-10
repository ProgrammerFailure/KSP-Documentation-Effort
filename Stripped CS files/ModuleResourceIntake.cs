using ns9;
using UnityEngine;

public class ModuleResourceIntake : PartModule
{
	[KSPField]
	public string resourceName = "IntakeAir";

	[KSPField(guiFormat = "F2", isPersistant = false, guiActive = true, guiName = "#autoLOC_6001423", guiUnits = "#autoLOC_7001409")]
	public float airFlow;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001352")]
	public string status = "Nominal";

	[KSPField]
	public double area;

	[KSPField]
	public bool checkForOxygen = true;

	[KSPField(guiFormat = "F2", isPersistant = false, guiActive = true, guiName = "#autoLOC_6001424", guiUnits = "#autoLOC_7001415")]
	public float airSpeedGui;

	[KSPField]
	public double intakeSpeed = 30.0;

	[KSPField]
	public string intakeTransformName = "Intake";

	public Transform intakeTransform;

	[KSPField(isPersistant = true)]
	public bool intakeEnabled = true;

	[KSPField]
	public string occludeNode = "";

	[KSPField]
	public double unitScalar = 0.4;

	[KSPField]
	public FloatCurve machCurve;

	[KSPField]
	public double kPaThreshold = 0.1;

	[KSPField]
	public bool disableUnderwater;

	[KSPField]
	public bool underwaterOnly;

	public int resourceId;

	public double resourceUnits;

	public PartResourceDefinition resourceDef;

	public double densityRecip;

	public PartResource res;

	public AttachNode node;

	public bool checkNode;

	public static string cacheAutoLOC_235899;

	public static string cacheAutoLOC_235936;

	public static string cacheAutoLOC_235946;

	public static string cacheAutoLOC_235952;

	public override void OnAwake()
	{
		base.OnAwake();
		if (machCurve == null)
		{
			machCurve = new FloatCurve();
			machCurve.Add(0f, 1f);
		}
	}

	public override void OnStart(StartState state)
	{
		if (intakeTransform == null)
		{
			intakeTransform = base.part.FindModelTransform(intakeTransformName);
		}
		if (moduleIsEnabled)
		{
			if (intakeEnabled)
			{
				base.Events["Deactivate"].active = true;
				base.Events["Activate"].active = false;
			}
			else
			{
				base.Events["Deactivate"].active = false;
				base.Events["Activate"].active = true;
			}
			BaseField baseField = base.Fields["airFlow"];
			BaseField baseField2 = base.Fields["status"];
			base.Fields["airSpeedGui"].guiActive = true;
			baseField2.guiActive = true;
			baseField.guiActive = true;
			base.Actions["ToggleAction"].active = true;
		}
		else
		{
			base.Events["Deactivate"].active = false;
			base.Events["Activate"].active = false;
			BaseField baseField3 = base.Fields["airFlow"];
			BaseField baseField4 = base.Fields["status"];
			base.Fields["airSpeedGui"].guiActive = false;
			baseField4.guiActive = false;
			baseField3.guiActive = false;
			base.Actions["ToggleAction"].active = false;
		}
		resourceDef = PartResourceLibrary.Instance.GetDefinition(resourceName.GetHashCode());
		densityRecip = 1.0 / (double)resourceDef.density;
		if (!string.IsNullOrEmpty(occludeNode))
		{
			node = base.part.FindAttachNode(occludeNode);
			if (node != null)
			{
				checkNode = true;
			}
		}
		res = base.part.Resources[resourceName];
	}

	public override void OnLoad(ConfigNode node)
	{
		if (intakeEnabled)
		{
			base.Events["Deactivate"].active = false;
			base.Events["Activate"].active = true;
		}
		resourceId = resourceName.GetHashCode();
	}

	public override string GetInfo()
	{
		string text = "";
		text += Localizer.Format("#autoLOC_235842", PartResourceLibrary.Instance.GetDefinition(resourceName.GetHashCode()).displayName);
		text += Localizer.Format("#autoLOC_235843", (area * 100.0).ToString("N2"));
		if (intakeSpeed > 0.0)
		{
			text += Localizer.Format("#autoLOC_235845", intakeSpeed);
		}
		if (!moduleIsEnabled)
		{
			text += Localizer.Format("#autoLOC_235851");
		}
		return text;
	}

	[KSPAction("#autoLOC_6001425")]
	public void ToggleAction(KSPActionParam param)
	{
		if (intakeEnabled)
		{
			Deactivate();
		}
		else
		{
			Activate();
		}
	}

	public void ToggleAction(KSPActionType action)
	{
		if (action == KSPActionType.Deactivate)
		{
			Deactivate();
		}
		else
		{
			Activate();
		}
	}

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001426")]
	public void Deactivate()
	{
		base.Events["Deactivate"].active = false;
		base.Events["Activate"].active = true;
		intakeEnabled = false;
	}

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001427")]
	public void Activate()
	{
		base.Events["Deactivate"].active = true;
		base.Events["Activate"].active = false;
		intakeEnabled = true;
	}

	public void FixedUpdate()
	{
		if (intakeEnabled && moduleIsEnabled && base.vessel != null && intakeTransform != null)
		{
			if (!base.part.ShieldedFromAirstream && (!checkNode || !(node.attachedPart != null)))
			{
				if ((base.vessel.mainBody.atmosphereContainsOxygen || !checkForOxygen) && base.vessel.staticPressurekPa >= kPaThreshold && ((!disableUnderwater && !underwaterOnly) || (disableUnderwater && (!base.vessel.mainBody.ocean || FlightGlobals.getAltitudeAtPos((Vector3d)intakeTransform.position, base.vessel.mainBody) >= 0.0)) || (underwaterOnly && base.vessel.mainBody.ocean && FlightGlobals.getAltitudeAtPos((Vector3d)intakeTransform.position, base.vessel.mainBody) < 0.0)))
				{
					double num = UtilMath.Clamp01(Vector3.Dot(base.vessel.srf_vel_direction, intakeTransform.forward)) * base.vessel.srfSpeed + intakeSpeed;
					airSpeedGui = (float)num;
					num *= unitScalar * area * (double)machCurve.Evaluate((float)base.vessel.mach);
					double num2 = num * (underwaterOnly ? base.vessel.mainBody.oceanDensity : base.vessel.atmDensity);
					resourceUnits = num2 * densityRecip;
					if (resourceUnits > 0.0)
					{
						airFlow = (float)resourceUnits;
						resourceUnits *= TimeWarp.fixedDeltaTime;
						if (res.maxAmount - res.amount >= resourceUnits)
						{
							base.part.TransferResource(resourceId, resourceUnits);
						}
						else
						{
							base.part.RequestResource(resourceId, 0.0 - resourceUnits);
						}
					}
					else
					{
						resourceUnits = 0.0;
						airFlow = 0f;
					}
					status = cacheAutoLOC_235936;
				}
				else
				{
					airFlow = 0f;
					airSpeedGui = 0f;
					base.part.TransferResource(resourceId, double.MinValue);
					status = cacheAutoLOC_235946;
				}
			}
			else
			{
				airFlow = 0f;
				airSpeedGui = 0f;
				status = cacheAutoLOC_235899;
			}
		}
		else
		{
			status = cacheAutoLOC_235952;
		}
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003055");
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_235899 = Localizer.Format("#autoLOC_235899");
		cacheAutoLOC_235936 = Localizer.Format("#autoLOC_235936");
		cacheAutoLOC_235946 = Localizer.Format("#autoLOC_235946");
		cacheAutoLOC_235952 = Localizer.Format("#autoLOC_8005416");
	}
}
