using System;
using System.Collections.Generic;
using System.Text;
using ns9;
using UnityEngine;

public class ModuleResourceHarvester : BaseDrill, IResourceConsumer
{
	[KSPField]
	public bool CausesDepletion;

	[KSPField]
	public float DepletionRate;

	[KSPField]
	public float HarvestThreshold;

	[KSPField]
	public int HarvesterType;

	[KSPField]
	public string ResourceName = "";

	[KSPField]
	public double airSpeedStatic = 40.0;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "")]
	public string ResourceStatus = "n/a";

	public double _resFlow;

	public double _displayedResFlow;

	public bool _isValidSituation = true;

	public Transform impactTransformCache;

	public string intakeTransformName = "Intake";

	public Transform intakeTransform;

	public ConversionRecipe recipe = new ConversionRecipe();

	public RaycastHit impactHitInfo;

	public ResourceRatio eInput;

	public bool hasEInput;

	public bool cachedWasNotAsteroid = true;

	public bool cachedWasNotComet = true;

	public int partCountCache = -1;

	public static string cacheAutoLOC_259762;

	public static string cacheAutoLOC_259775;

	public static string cacheAutoLOC_259782;

	public static string cacheAutoLOC_259789;

	public static string cacheAutoLOC_259808;

	public static string cacheAutoLOC_259819;

	public static string cacheAutoLOC_259826;

	public static string cacheAutoLOC_259862;

	public static string cacheAutoLOC_259933;

	public virtual string GetLocationString()
	{
		return ((HarvestTypes)HarvesterType).displayDescription();
	}

	public override string GetInfo()
	{
		LoadRecipe(1.0);
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		stringBuilder.Append(ConverterName);
		stringBuilder.Append("\n");
		stringBuilder.Append(Localizer.Format("#autoLOC_259675", GetLocationString(), ((int)(Efficiency * 100f)).ToString()));
		stringBuilder.Append(Localizer.Format("#autoLOC_259676"));
		int count = recipe.Inputs.Count;
		for (int i = 0; i < count; i++)
		{
			ResourceRatio resourceRatio = recipe.Inputs[i];
			stringBuilder.Append("\n - ");
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(resourceRatio.ResourceName.GetHashCode());
			string text = "";
			text = ((definition == null) ? resourceRatio.ResourceName : definition.displayName);
			stringBuilder.Append(text);
			stringBuilder.Append(": ");
			if (resourceRatio.Ratio * (double)Efficiency < 0.0001)
			{
				stringBuilder.Append(Localizer.Format("#autoLOC_6001063", (resourceRatio.Ratio * (double)KSPUtil.dateTimeFormatter.Day * (double)Efficiency).ToString("0.00")));
			}
			else if (resourceRatio.Ratio * (double)Efficiency < 0.01)
			{
				stringBuilder.Append(Localizer.Format("#autoLOC_6001064", (resourceRatio.Ratio * (double)KSPUtil.dateTimeFormatter.Hour * (double)Efficiency).ToString("0.00")));
			}
			else
			{
				stringBuilder.Append(Localizer.Format("#autoLOC_6001065", (resourceRatio.Ratio * (double)Efficiency).ToString("0.00")));
			}
		}
		stringBuilder.Append(Localizer.Format("#autoLOC_259698"));
		int count2 = recipe.Outputs.Count;
		for (int j = 0; j < count2; j++)
		{
			ResourceRatio resourceRatio2 = recipe.Outputs[j];
			stringBuilder.Append("\n - ");
			PartResourceDefinition definition2 = PartResourceLibrary.Instance.GetDefinition(resourceRatio2.ResourceName.GetHashCode());
			string text2 = "";
			text2 = ((definition2 == null) ? resourceRatio2.ResourceName : definition2.displayName);
			stringBuilder.Append(text2);
			stringBuilder.Append(": ");
			if (resourceRatio2.Ratio * (double)Efficiency < 0.0001)
			{
				stringBuilder.Append(Localizer.Format("#autoLOC_6001066", (resourceRatio2.Ratio * (double)KSPUtil.dateTimeFormatter.Day * (double)Efficiency).ToString("0.00")));
			}
			else if (resourceRatio2.Ratio * (double)Efficiency < 0.01)
			{
				stringBuilder.Append(Localizer.Format("#autoLOC_6001067", (resourceRatio2.Ratio * (double)KSPUtil.dateTimeFormatter.Hour * (double)Efficiency).ToString("0.00")));
			}
			else
			{
				stringBuilder.Append(Localizer.Format("#autoLOC_6001068", (resourceRatio2.Ratio * (double)Efficiency).ToString("0.00")));
			}
		}
		int count3 = recipe.Requirements.Count;
		if (count3 > 0)
		{
			stringBuilder.Append(Localizer.Format("#autoLOC_259724"));
			for (int k = 0; k < count3; k++)
			{
				ResourceRatio resourceRatio3 = recipe.Outputs[k];
				stringBuilder.Append("\n - ");
				PartResourceDefinition definition3 = PartResourceLibrary.Instance.GetDefinition(resourceRatio3.ResourceName.GetHashCode());
				string text3 = "";
				text3 = ((definition3 == null) ? resourceRatio3.ResourceName : definition3.displayName);
				stringBuilder.Append(text3);
				stringBuilder.Append(": ");
				stringBuilder.AppendFormat("{0:0.00}", resourceRatio3.Ratio);
			}
			stringBuilder.Append("\n");
		}
		return stringBuilder.ToStringAndRelease();
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		if (HighLogic.LoadedSceneIsFlight)
		{
			_isValidSituation = IsSituationValid();
			if (HighLogic.LoadedSceneIsFlight)
			{
				isEnabled = _isValidSituation;
			}
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(ResourceName.GetHashCode());
			base.Fields["ResourceStatus"].guiName = Localizer.Format("#autoLOC_6001918", definition.displayName);
			impactTransformCache = base.part.FindModelTransform(ImpactTransform);
			if (intakeTransform == null)
			{
				intakeTransform = base.part.FindModelTransform(intakeTransformName);
			}
		}
	}

	public override ConversionRecipe PrepareRecipe(double deltaTime)
	{
		try
		{
			if (!HighLogic.LoadedSceneIsFlight)
			{
				return null;
			}
			if (HarvesterType == 0 && base.vessel.Landed && base.vessel.horizontalSrfSpeed > 10.0 && base.vessel.isActiveVessel && deltaTime < (double)ResourceScenario.Instance.gameSettings.MaxDeltaTime)
			{
				status = cacheAutoLOC_259762;
				IsActivated = false;
				return null;
			}
			if (HarvesterType == 0 && !base.vessel.Landed && ResourceUtilities.GetAltitude(base.vessel) > 1000.0)
			{
				status = cacheAutoLOC_259775;
				IsActivated = false;
				return null;
			}
			if (HarvesterType == 1 && !base.vessel.Splashed)
			{
				status = cacheAutoLOC_259782;
				IsActivated = false;
				return null;
			}
			if (!CheckForImpact() && deltaTime / 2.0 < (double)TimeWarp.fixedDeltaTime)
			{
				status = cacheAutoLOC_259789;
				IsActivated = false;
				return null;
			}
			AbundanceRequest abundanceRequest = default(AbundanceRequest);
			abundanceRequest.Altitude = base.vessel.altitude;
			abundanceRequest.BodyId = FlightGlobals.currentMainBody.flightGlobalsIndex;
			abundanceRequest.CheckForLock = false;
			abundanceRequest.Latitude = base.vessel.latitude;
			abundanceRequest.Longitude = base.vessel.longitude;
			abundanceRequest.ResourceType = (HarvestTypes)HarvesterType;
			abundanceRequest.ResourceName = ResourceName;
			AbundanceRequest request = abundanceRequest;
			float abundance = ResourceMap.Instance.GetAbundance(request);
			if ((double)abundance < 1E-09)
			{
				status = cacheAutoLOC_259808;
				IsActivated = false;
				return null;
			}
			if (abundance < HarvestThreshold)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001069", (HarvestThreshold * 100f).ToString("0.0")), 5f, ScreenMessageStyle.UPPER_CENTER);
				status = cacheAutoLOC_259819;
				IsActivated = false;
				return null;
			}
			if (!IsActivated)
			{
				status = cacheAutoLOC_259826;
				return null;
			}
			float num = abundance * Efficiency;
			if (HarvesterType == 2)
			{
				num *= (float)GetIntakeMultiplier();
			}
			_resFlow = num;
			LoadRecipe(num);
			return recipe;
		}
		catch (Exception ex)
		{
			Debug.LogError("[RESOURCES] - Error in - ModuleCrustalHarvester_ConversionRecipe - " + ex);
			return null;
		}
	}

	public double GetIntakeMultiplier()
	{
		double num = base.vessel.atmDensity;
		if (HarvesterType == 3)
		{
			num = 1.0;
		}
		double num2 = base.part.vessel.srfSpeed + airSpeedStatic;
		if (base.part.ShieldedFromAirstream)
		{
			return 0.0;
		}
		double num3 = 1.0;
		if (intakeTransform != null)
		{
			num3 = UtilMath.Clamp01(Vector3.Dot(base.vessel.srf_vel_direction, intakeTransform.forward));
		}
		double num4 = num2 * num;
		return num3 * num4;
	}

	public override void PostUpdateCleanup()
	{
		if (IsActivated)
		{
			if (_displayedResFlow != _resFlow)
			{
				_displayedResFlow = _resFlow;
				if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
				{
					ResourceStatus = Localizer.Format("#autoLOC_259857", _displayedResFlow.ToString("0.000000"));
				}
			}
		}
		else
		{
			ResourceStatus = cacheAutoLOC_259862;
			_displayedResFlow = -1.0;
		}
	}

	public virtual void LoadRecipe(double harvestRate)
	{
		recipe.Clear();
		try
		{
			recipe.Inputs.AddRange(inputList);
			bool dumpExcess = HarvesterType == 2;
			recipe.Outputs.Add(new ResourceRatio
			{
				ResourceName = ResourceName,
				Ratio = harvestRate,
				DumpExcess = dumpExcess,
				FlowMode = ResourceFlowMode.NULL
			});
			hasEInput = false;
			int count = recipe.Inputs.Count;
			do
			{
				if (count-- <= 0)
				{
					return;
				}
			}
			while (!(recipe.Inputs[count].ResourceName == "ElectricCharge"));
			eInput = recipe.Inputs[count];
			hasEInput = true;
		}
		catch (Exception)
		{
			MonoBehaviour.print("[RESOURCES] Error preparing recipe");
		}
	}

	public override void PostProcess(ConverterResults result, double deltaTime)
	{
		if (CausesDepletion)
		{
			float num = (float)Math.Min(1.0, result.TimeFactor / deltaTime);
			Vector2 depletionNode = ResourceMap.Instance.GetDepletionNode(base.vessel.latitude, base.vessel.longitude);
			float depletionNodeValue = ResourceMap.Instance.GetDepletionNodeValue(base.vessel.mainBody.flightGlobalsIndex, ResourceName, (int)depletionNode.x, (int)depletionNode.y);
			float num2 = DepletionRate * num;
			float value = depletionNodeValue - depletionNodeValue * num2;
			ResourceMap.Instance.SetDepletionNodeValue(base.vessel.mainBody.flightGlobalsIndex, ResourceName, (int)depletionNode.x, (int)depletionNode.y, value);
		}
		base.PostProcess(result, deltaTime);
		if (hasEInput && !(eInput.Ratio < 1E-09))
		{
			double num3 = eInput.Ratio * deltaTime;
			double num4 = _resBroker.AmountAvailable(base.part, "ElectricCharge", deltaTime, ResourceFlowMode.NULL);
			if (num3 > num4)
			{
				ScreenMessages.PostScreenMessage(cacheAutoLOC_259933, 5f, ScreenMessageStyle.UPPER_CENTER);
				StopResourceConverter();
			}
		}
	}

	public virtual bool CheckForImpact()
	{
		if (!string.IsNullOrEmpty(ImpactTransform) && !(impactTransformCache == null))
		{
			Vector3 position = impactTransformCache.position;
			return Physics.Raycast(new Ray(position, impactTransformCache.forward), out impactHitInfo, ImpactRange, 32768);
		}
		return true;
	}

	public override bool IsSituationValid()
	{
		if (HighLogic.LoadedScene != GameScenes.FLIGHT)
		{
			return false;
		}
		int count = base.vessel.parts.Count;
		if (count != partCountCache)
		{
			partCountCache = count;
			int index = count;
			while (index-- > 0)
			{
				Part part = base.vessel.parts[index];
				int count2 = part.Modules.Count;
				while (count2-- > 0)
				{
					if (!(part.Modules[count2] is ModuleAsteroid))
					{
						if (part.Modules[count2] is ModuleComet)
						{
							cachedWasNotComet = false;
							return false;
						}
						continue;
					}
					cachedWasNotAsteroid = false;
					return false;
				}
			}
			cachedWasNotAsteroid = true;
			cachedWasNotComet = true;
		}
		else
		{
			if (!cachedWasNotAsteroid)
			{
				return false;
			}
			if (!cachedWasNotComet)
			{
				return false;
			}
		}
		switch ((HarvestTypes)HarvesterType)
		{
		default:
			return false;
		case HarvestTypes.Planetary:
		{
			bool result;
			if (result = base.vessel.situation == Vessel.Situations.LANDED)
			{
				return result;
			}
			if (ResourceUtilities.GetAltitude(base.vessel) <= 500.0)
			{
				result = true;
			}
			return result;
		}
		case HarvestTypes.Oceanic:
			return base.vessel.situation == Vessel.Situations.SPLASHED;
		case HarvestTypes.Atmospheric:
			return base.vessel.atmDensity > 1E-09;
		case HarvestTypes.Exospheric:
			return base.vessel.situation == Vessel.Situations.ORBITING;
		}
	}

	public override void PreProcessing()
	{
		bool flag = isEnabled;
		_isValidSituation = IsSituationValid();
		if (HighLogic.LoadedSceneIsFlight)
		{
			isEnabled = _isValidSituation;
		}
		if (isEnabled != flag)
		{
			MonoUtilities.RefreshPartContextWindow(base.part);
		}
		base.OnUpdate();
	}

	public virtual List<PartResourceDefinition> GetConsumedResources()
	{
		return new List<PartResourceDefinition> { PartResourceLibrary.Instance.GetDefinition("ElectricCharge") };
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003054");
	}

	public new static void CacheLocalStrings()
	{
		cacheAutoLOC_259762 = Localizer.Format("#autoLOC_259762");
		cacheAutoLOC_259775 = Localizer.Format("#autoLOC_259775");
		cacheAutoLOC_259782 = Localizer.Format("#autoLOC_259782");
		cacheAutoLOC_259789 = Localizer.Format("#autoLOC_259789");
		cacheAutoLOC_259808 = Localizer.Format("#autoLOC_259808");
		cacheAutoLOC_259819 = Localizer.Format("#autoLOC_259819");
		cacheAutoLOC_259826 = Localizer.Format("#autoLOC_259826");
		cacheAutoLOC_259862 = Localizer.Format("#autoLOC_259862");
		cacheAutoLOC_259933 = Localizer.Format("#autoLOC_259933");
	}
}
