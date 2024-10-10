using System;
using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using ns9;
using UnityEngine;

public class ModuleDeployableSolarPanel : ModuleDeployablePart, IContractObjectiveModule
{
	public enum PanelType
	{
		FLAT,
		CYLINDRICAL,
		SPHERICAL
	}

	[KSPField]
	public PanelType panelType;

	[KSPField]
	public float raycastOffset = 0.25f;

	[KSPField]
	public string resourceName = "ElectricCharge";

	[KSPField]
	public float chargeRate = 1f;

	[KSPField]
	public string raycastTransformName = "panel3";

	[KSPField]
	public bool useRaycastForTrackingDot;

	[KSPField(guiFormat = "F2", guiActive = true, guiName = "#autoLOC_6001420", guiUnits = " ")]
	public float sunAOA;

	[KSPField(guiFormat = "F3", guiActive = true, guiName = "#autoLOC_6001421", guiUnits = " ")]
	public float flowRate;

	public double _flowRate;

	[KSPField]
	public string flowUnits = "";

	[KSPField]
	public bool flowUnitsUseSpace = true;

	[KSPField]
	public string flowFormat = "F3";

	[KSPField]
	public float flowMult = 1f;

	[KSPField]
	public bool showInfo = true;

	[KSPField]
	public double resMultForGetInfo = 1.0;

	[KSPField]
	public FloatCurve powerCurve;

	[KSPField]
	public FloatCurve temperatureEfficCurve;

	[KSPField]
	public FloatCurve timeEfficCurve;

	[KSPField(isPersistant = true)]
	public float efficiencyMult = 1f;

	public float originalEfficiencyMultiplier;

	[KSPField(isPersistant = true)]
	public double launchUT = -1.0;

	public double _distMult;

	public double _efficMult;

	public Transform trackingDotTransform;

	public int planetLayerMask;

	public int defaultLayerMask;

	public RaycastHit hit;

	public new List<AdjusterDeployableSolarPanelBase> adjusterCache = new List<AdjusterDeployableSolarPanelBase>();

	public static string cacheAutoLOC_438839;

	public static string cacheAutoLOC_235418;

	public static string cacheAutoLOC_235468;

	public override void OnAwake()
	{
		base.OnAwake();
		resHandler.moduleResourceBasedPrimaryIsInput = false;
		if (powerCurve == null)
		{
			powerCurve = new FloatCurve();
		}
		if (temperatureEfficCurve == null)
		{
			temperatureEfficCurve = new FloatCurve();
			temperatureEfficCurve.Add(4f, 1.2f, 0f, -0.0006f);
			temperatureEfficCurve.Add(300f, 1f, -0.0008f, -0.0008f);
			temperatureEfficCurve.Add(1200f, 0.134f, -0.00035f, -0.00035f);
			temperatureEfficCurve.Add(1900f, 0.02f, -3.72E-05f, -3.72E-05f);
			temperatureEfficCurve.Add(2500f, 0.01f, 0f, 0f);
		}
		if (timeEfficCurve == null)
		{
			timeEfficCurve = new FloatCurve();
			timeEfficCurve.Add(0f, 1f);
		}
		planetLayerMask = 1 << LayerMask.NameToLayer("Scaled Scenery");
		defaultLayerMask = (1 << LayerMask.NameToLayer("PhysicalObjects")) | (1 << LayerMask.NameToLayer("TerrainColliders")) | (1 << LayerMask.NameToLayer("Local Scenery")) | LayerUtil.DefaultEquivalent;
		originalEfficiencyMultiplier = efficiencyMult;
	}

	public new void OnDestroy()
	{
		GameEvents.onPartRepaired.Remove(OnPartRepaired);
	}

	public override void OnStart(StartState state)
	{
		if (!HighLogic.LoadedSceneIsEditor && !HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		GameEvents.onPartRepaired.Add(OnPartRepaired);
		base.Fields["flowRate"].guiFormat = flowFormat;
		base.Fields["flowRate"].guiUnits = (flowUnitsUseSpace ? " " : "") + flowUnits;
		base.OnStart(state);
		if (secondaryTransform == null)
		{
			Debug.LogError("Couldn't access secondaryTransform for raycasts");
		}
		if (useRaycastForTrackingDot)
		{
			trackingDotTransform = secondaryTransform;
		}
		else
		{
			trackingDotTransform = panelRotationTransform;
		}
		if (HighLogic.LoadedSceneIsFlight && anim != null)
		{
			float normalizedTime = ((deployState == DeployState.EXTENDED) ? 1f : 0f);
			anim[animationName].normalizedTime = normalizedTime;
			anim[animationName].enabled = true;
			anim[animationName].weight = 1f;
			if (deployState == DeployState.RETRACTED)
			{
				anim.Stop(animationName);
			}
		}
	}

	public override string GetInfo()
	{
		if (showInfo)
		{
			return base.GetInfo() + resHandler.PrintModuleResources(resMultForGetInfo);
		}
		return string.Empty;
	}

	public override void OnLoad(ConfigNode node)
	{
		if (string.IsNullOrEmpty(secondaryTransformName))
		{
			secondaryTransformName = raycastTransformName;
		}
		base.OnLoad(node);
		subPartName = Localizer.Format("#autoLOC_235328");
		partType = Localizer.Format("#autoLOC_235329");
		if (node.HasValue("type"))
		{
			panelType = (PanelType)Enum.Parse(typeof(PanelType), node.GetValue("type"));
		}
		if (resHandler.outputResources.Count == 0 && node.HasValue("chargeRate"))
		{
			ModuleResource moduleResource = new ModuleResource();
			moduleResource.name = resourceName;
			moduleResource.title = KSPUtil.PrintModuleName(resourceName);
			moduleResource.id = resourceName.GetHashCode();
			moduleResource.rate = chargeRate;
			resHandler.outputResources.Add(moduleResource);
		}
	}

	public override void FixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsMissionBuilder)
		{
			if ((HighLogic.LoadedSceneIsEditor || HighLogic.LoadedSceneIsFlight) && base.vessel != null && (launchUT < 0.0 || base.vessel.situation == Vessel.Situations.PRELAUNCH))
			{
				launchUT = Planetarium.GetUniversalTime();
			}
			efficiencyMult = ApplyEfficiencyAdjustments(originalEfficiencyMultiplier);
			base.FixedUpdate();
		}
	}

	public override void PostFSMUpdate()
	{
		if (deployState != DeployState.EXTENDED)
		{
			sunAOA = 0f;
			flowRate = 0f;
		}
	}

	public override bool CalculateTrackingLOS(Vector3 trackingDirection, ref string blocker)
	{
		bool result;
		if (result = base.CalculateTrackingLOS(trackingDirection, ref blocker))
		{
			Ray ray = new Ray(ScaledSpace.LocalToScaledSpace(secondaryTransform.position), (ScaledSpace.LocalToScaledSpace(trackingTransformLocal.position) - ScaledSpace.LocalToScaledSpace(panelRotationTransform.position)).normalized);
			float maxDistance = float.MaxValue;
			if (Physics.Raycast(ray, out hit, maxDistance, planetLayerMask) && hit.transform != trackingTransformScaled)
			{
				blocker = hit.transform.gameObject.name;
				result = false;
			}
			else
			{
				ray = new Ray(secondaryTransform.position + trackingDirection * raycastOffset, (trackingTransformLocal.position - panelRotationTransform.position).normalized);
				Debug.DrawRay(ray.origin, ray.direction, Color.green);
				if (Physics.Raycast(ray, out hit, maxDistance, defaultLayerMask))
				{
					if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
					{
						if ((bool)hit.transform.gameObject && (bool)hit.transform.gameObject.GetComponent<GClass3>())
						{
							blocker = cacheAutoLOC_438839;
						}
						else
						{
							Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(hit.transform.gameObject);
							if (partUpwardsCached != null)
							{
								blocker = partUpwardsCached.partInfo.title;
							}
							else if (hit.transform.gameObject.tag.Contains("KSC"))
							{
								blocker = ResearchAndDevelopment.GetMiniBiomedisplayNameByUnityTag(hit.transform.gameObject.tag, formatted: true);
							}
							else
							{
								blocker = hit.transform.gameObject.name;
							}
						}
					}
					result = false;
				}
			}
		}
		return result;
	}

	public override void PostCalculateTracking(bool trackingLOS, Vector3 trackingDirection)
	{
		if (trackingLOS)
		{
			status = cacheAutoLOC_235418;
			if (panelType == PanelType.FLAT && trackingDotTransform != null)
			{
				sunAOA = Mathf.Clamp(Vector3.Dot(trackingDotTransform.forward, trackingDirection), 0f, 1f);
			}
			else if (panelType == PanelType.CYLINDRICAL)
			{
				Vector3 lhs = ((alignType == PanelAlignType.PIVOT) ? trackingDotTransform.forward : ((alignType == PanelAlignType.const_1) ? base.part.partTransform.right : ((alignType != PanelAlignType.const_2) ? base.part.partTransform.forward : base.part.partTransform.up)));
				sunAOA = (1f - Mathf.Abs(Vector3.Dot(lhs, trackingDirection))) * (1f / (float)Math.PI);
			}
			else
			{
				sunAOA = 0.25f;
			}
		}
		else
		{
			sunAOA = 0f;
		}
		_distMult = 1.0;
		if (useCurve)
		{
			_distMult = powerCurve.Evaluate((trackingTransformLocal.position - panelRotationTransform.position).magnitude);
		}
		else
		{
			_distMult = base.vessel.solarFlux / PhysicsGlobals.SolarLuminosityAtHome;
		}
		_efficMult = temperatureEfficCurve.Evaluate((float)base.part.skinTemperature) * timeEfficCurve.Evaluate((float)((Planetarium.GetUniversalTime() - launchUT) * 1.1574074074074073E-05)) * efficiencyMult;
		_flowRate = (double)sunAOA * _efficMult * _distMult;
		if (base.part.submergedPortion > 0.0)
		{
			double num = 0.0 - FlightGlobals.getAltitudeAtPos((Vector3d)secondaryTransform.position, base.vessel.mainBody);
			num = (num * 3.0 + base.part.maxDepth) * 0.25;
			if (num < 0.5)
			{
				num = 0.5;
			}
			double num2 = 1.0 / (1.0 + num * base.part.vessel.mainBody.oceanDensity);
			if (base.part.submergedPortion < 1.0)
			{
				_flowRate *= UtilMath.LerpUnclamped(1.0, num2, base.part.submergedPortion);
			}
			else
			{
				_flowRate *= num2;
			}
			status += cacheAutoLOC_235468;
		}
		flowRate = (float)(resHandler.UpdateModuleResourceOutputs(_flowRate) * (double)flowMult);
	}

	public override void OnInventoryModeDisable()
	{
		base.OnInventoryModeDisable();
		if (base.part.protoPartSnapshot != null)
		{
			ProtoPartModuleSnapshot protoPartModuleSnapshot = base.part.protoPartSnapshot.FindModule("ModuleDeployableSolarPanel");
			if (protoPartModuleSnapshot != null && protoPartModuleSnapshot.moduleValues != null)
			{
				protoPartModuleSnapshot.moduleValues.SetValue("deployState", deployState.ToString());
			}
		}
	}

	public string GetContractObjectiveType()
	{
		return "Generator";
	}

	public bool CheckContractObjectiveValidity()
	{
		return true;
	}

	public void OnPartRepaired(Part p)
	{
		if (p.persistentId.Equals(base.part.persistentId))
		{
			if (useRaycastForTrackingDot)
			{
				trackingDotTransform = secondaryTransform;
			}
			else
			{
				trackingDotTransform = panelRotationTransform;
			}
		}
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003038");
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterDeployableSolarPanelBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterDeployableSolarPanelBase item = adjuster as AdjusterDeployableSolarPanelBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public float ApplyEfficiencyAdjustments(float efficiency)
	{
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			efficiency = adjusterCache[i].ApplyEfficiencyAdjustment(efficiency);
		}
		return efficiency;
	}

	public new static void CacheLocalStrings()
	{
		cacheAutoLOC_438839 = Localizer.Format("#autoLOC_438839");
		cacheAutoLOC_235418 = Localizer.Format("#autoLOC_235418");
		cacheAutoLOC_235468 = Localizer.Format("#autoLOC_235468");
	}
}
