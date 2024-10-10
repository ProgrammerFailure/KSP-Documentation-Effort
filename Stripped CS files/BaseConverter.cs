using System;
using System.Collections.Generic;
using ns9;
using UnityEngine;

public abstract class BaseConverter : PartModule, IAnimatedModule, IOverheatDisplay, IConstruction
{
	public List<ResourceRatio> inputList;

	public List<ResourceRatio> outputList;

	public List<ResourceRatio> reqList;

	[KSPField]
	public string ConverterName = "";

	[KSPField]
	public bool GeneratesHeat;

	[KSPField]
	public bool UseSpecialistBonus;

	[KSPField]
	public bool UseSpecialistHeatBonus;

	[KSPField]
	public float SpecialistBonusBase = 0.05f;

	[KSPField]
	public bool AutoShutdown;

	[KSPField]
	public bool DirtyFlag = true;

	[KSPField(isPersistant = true)]
	public float EfficiencyBonus = 1f;

	[KSPField]
	public float FillAmount = 1f;

	[KSPField(isPersistant = true)]
	public bool IsActivated;

	[KSPField]
	public string StartActionName = "#autoLOC_6001471";

	[KSPField]
	public string StopActionName = "#autoLOC_6001472";

	[KSPField]
	public string ToggleActionName = "#autoLOC_6001473";

	[KSPField]
	public string resourceOutputName = "";

	[KSPField]
	public float TakeAmount = 1f;

	[KSPField]
	public bool AlwaysActive;

	[KSPField]
	public FloatCurve ThermalEfficiency;

	[KSPField]
	public FloatCurve TemperatureModifier;

	[KSPField]
	public float SpecialistEfficiencyFactor = 0.1f;

	[KSPField]
	public float SpecialistHeatFactor = 0.1f;

	[KSPField]
	public float DefaultShutoffTemp = 0.8f;

	[KSPField]
	public string ExperienceEffect = "ConverterSkill";

	public IResourceBroker _resBroker;

	public ResourceConverter _resConverter;

	public double lastUpdateTime;

	public double lastHeatFlux;

	public double lastTimeFactor;

	public double statusPercent;

	public BaseEvent startEvt;

	public BaseEvent stopEvt;

	public bool _preCalculateEfficiency;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "")]
	public string status = "";

	public Dictionary<string, float> EfficiencyModifiers;

	public float _totalEfficiencyModifiers;

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001883")]
	public string debugEffBonus = "";

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001884")]
	public string debugDelta = "";

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001885")]
	public string debugTimeFac = "";

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001886")]
	public string debugFinBon = "";

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001887")]
	public string debugCrewBon = "";

	public ModuleAnimationGroup partAnimationModule;

	public bool hasAnimationGroup;

	public int catchupRetries;

	public const int MAX_CATCHUP_RETRIES = 10;

	public static string cacheAutoLOC_257237;

	public virtual ResourceConverter ResConverter => _resConverter ?? (_resConverter = new ResourceConverter(ResBroker));

	public virtual IResourceBroker ResBroker => _resBroker ?? (_resBroker = new ResourceBroker());

	public BaseConverter()
	{
	}

	public virtual void UpdateDebugInfo(ConverterResults result, double deltaTime)
	{
		if (ResourceSetup.Instance.ResConfig.ShowDebugOptions)
		{
			debugEffBonus = EfficiencyBonus.ToString();
			debugDelta = deltaTime.ToString();
			debugTimeFac = result.TimeFactor.ToString();
		}
	}

	public virtual float GetCrewEfficiencyBonus()
	{
		float result = 1f;
		if (UseSpecialistBonus)
		{
			result = SpecialistBonusBase + (1f + (float)HasSpecialist(ExperienceEffect)) * SpecialistEfficiencyFactor;
		}
		if (ResourceSetup.Instance.ResConfig.ShowDebugOptions)
		{
			debugCrewBon = result.ToString();
		}
		return result;
	}

	public virtual float GetCrewHeatBonus()
	{
		float result = 0f;
		if (UseSpecialistHeatBonus)
		{
			result = (1f + (float)HasSpecialist(ExperienceEffect)) * SpecialistHeatFactor;
			result = Math.Min(result, 1f);
		}
		return result;
	}

	public virtual void SetupDebugging()
	{
		base.Fields["debugEffBonus"].guiActive = ResourceSetup.Instance.ResConfig.ShowDebugOptions;
		base.Fields["debugDelta"].guiActive = ResourceSetup.Instance.ResConfig.ShowDebugOptions;
		base.Fields["debugTimeFac"].guiActive = ResourceSetup.Instance.ResConfig.ShowDebugOptions;
		base.Fields["debugFinBon"].guiActive = ResourceSetup.Instance.ResConfig.ShowDebugOptions;
		base.Fields["debugCrewBon"].guiActive = ResourceSetup.Instance.ResConfig.ShowDebugOptions;
	}

	public virtual void EnableModule()
	{
		isEnabled = true;
		DirtyFlag = !IsActivated;
		UpdateConverterStatus();
	}

	public virtual void DisableModule()
	{
		isEnabled = false;
		IsActivated = false;
		DirtyFlag = !IsActivated;
		UpdateConverterStatus();
	}

	public virtual bool ModuleIsActive()
	{
		return IsActivated;
	}

	public bool IsAnimationGroupDeployed()
	{
		if (hasAnimationGroup && !partAnimationModule.isDeployed)
		{
			return false;
		}
		return true;
	}

	[KSPEvent(active = false, guiActive = true, guiName = "#autoLOC_6001471")]
	public virtual void StartResourceConverter()
	{
		if (hasAnimationGroup && !IsAnimationGroupDeployed())
		{
			string text = Localizer.Format(partAnimationModule.deployActionName, partAnimationModule.moduleType);
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6010007", text), 5f, ScreenMessageStyle.UPPER_CENTER, Color.yellow);
		}
		else
		{
			IsActivated = true;
		}
	}

	[KSPEvent(active = false, guiActive = true, guiName = "#autoLOC_6001472")]
	public virtual void StopResourceConverter()
	{
		IsActivated = false;
		status = Localizer.Format("#autoLOC_257112");
	}

	[KSPAction("#autoLOC_6001472")]
	public virtual void StopResourceConverterAction(KSPActionParam param)
	{
		StopResourceConverter();
	}

	[KSPAction("#autoLOC_6001471")]
	public virtual void StartResourceConverterAction(KSPActionParam param)
	{
		StartResourceConverter();
	}

	[KSPAction("#autoLOC_6001473")]
	public virtual void ToggleResourceConverterAction(KSPActionParam param)
	{
		if (IsActivated)
		{
			StopResourceConverter();
		}
		else
		{
			StartResourceConverter();
		}
	}

	public virtual void SetEfficiencyBonus(float bonus)
	{
		EfficiencyBonus = bonus;
	}

	public virtual double GetDeltaTime()
	{
		try
		{
			if (!(Time.timeSinceLevelLoad < 1f) && FlightGlobals.ready)
			{
				if (Math.Abs(lastUpdateTime) < 1E-09)
				{
					lastUpdateTime = Planetarium.GetUniversalTime();
					return -1.0;
				}
				double num = Math.Min(Planetarium.GetUniversalTime() - lastUpdateTime, ResourceUtilities.GetMaxDeltaTime());
				double num2 = Math.Min(TimeWarp.fixedDeltaTime, 0.02);
				if (num > num2 && catchupRetries < 10)
				{
					double bestDeltaTime = GetBestDeltaTime(num);
					if (bestDeltaTime < num)
					{
						catchupRetries++;
						num = Math.Max(num2, bestDeltaTime);
					}
				}
				lastUpdateTime += num;
				return num;
			}
			return -1.0;
		}
		catch (Exception ex)
		{
			Debug.LogError("[RESOURCES] - Error in - BaseConverter_GetDeltaTime - " + ex);
			return 0.0;
		}
	}

	public double GetBestDeltaTime(double deltaTime)
	{
		ConversionRecipe conversionRecipe = PrepareRecipe(deltaTime);
		if (conversionRecipe == null)
		{
			return deltaTime;
		}
		int count = conversionRecipe.Inputs.Count;
		double num = deltaTime;
		for (int i = 0; i < count; i++)
		{
			ResourceRatio resourceRatio = conversionRecipe.Inputs[i];
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(resourceRatio.ResourceName);
			double num2 = ResBroker.AmountAvailable(base.part, definition.id, deltaTime, definition.resourceFlowMode);
			double num3 = deltaTime * (num2 / (resourceRatio.Ratio * deltaTime));
			if (num3 < num)
			{
				num = num3;
			}
		}
		int count2 = conversionRecipe.Outputs.Count;
		for (int j = 0; j < count2; j++)
		{
			ResourceRatio resourceRatio2 = conversionRecipe.Outputs[j];
			PartResourceDefinition definition2 = PartResourceLibrary.Instance.GetDefinition(resourceRatio2.ResourceName);
			double num4 = ResBroker.StorageAvailable(base.part, definition2.id, deltaTime, definition2.resourceFlowMode, 1.0);
			double num5 = deltaTime * (num4 / (resourceRatio2.Ratio * deltaTime));
			if (num5 < num)
			{
				num = num5;
			}
		}
		return num;
	}

	public virtual void UpdateConverterStatus()
	{
		if (DirtyFlag != IsActivated)
		{
			DirtyFlag = IsActivated;
			startEvt.active = !IsActivated && isEnabled;
			stopEvt.active = IsActivated;
			if (IsActivated)
			{
				status = cacheAutoLOC_257237;
			}
			MonoUtilities.RefreshPartContextWindow(base.part);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		status = Localizer.Format("#autoLOC_257112");
		if (node.HasNode("INPUT_RESOURCE"))
		{
			inputList.Clear();
		}
		if (node.HasNode("OUTPUT_RESOURCE"))
		{
			outputList.Clear();
		}
		if (node.HasNode("REQUIRED_RESOURCE"))
		{
			reqList.Clear();
		}
		int countNodes = node.CountNodes;
		for (int i = 0; i < countNodes; i++)
		{
			ConfigNode configNode = node.nodes[i];
			ResourceRatio resourceRatio = default(ResourceRatio);
			resourceRatio.FlowMode = ResourceFlowMode.NULL;
			ResourceRatio item = resourceRatio;
			if (!configNode.HasValue("ResourceName") && configNode.name.EndsWith("_RESOURCE"))
			{
				Debug.Log("Resource must have value 'ResourceName'");
				continue;
			}
			item.Load(configNode);
			switch (configNode.name)
			{
			case "REQUIRED_RESOURCE":
				reqList.Add(item);
				break;
			case "OUTPUT_RESOURCE":
				outputList.Add(item);
				break;
			case "INPUT_RESOURCE":
				inputList.Add(item);
				break;
			}
		}
		if (base.vessel == null)
		{
			return;
		}
		try
		{
			base.OnLoad(node);
			lastUpdateTime = ResourceUtilities.GetValue(node, "lastUpdateTime", lastUpdateTime);
			SetupModule();
		}
		catch (Exception ex)
		{
			Debug.LogError("[RESOURCES] - Error in - BaseConverter_OnLoad - " + ex);
		}
	}

	public virtual void SetupLabels()
	{
		startEvt.guiName = StartActionName;
		startEvt.guiActiveEditor = true;
		startEvt.active = !IsActivated && isEnabled;
		stopEvt.guiName = StopActionName;
		stopEvt.guiActiveEditor = true;
		stopEvt.active = IsActivated;
		base.Actions["StartResourceConverterAction"].guiName = StartActionName;
		base.Actions["StopResourceConverterAction"].guiName = StopActionName;
		base.Actions["ToggleResourceConverterAction"].guiName = ToggleActionName;
		if (!string.IsNullOrEmpty(resourceOutputName))
		{
			base.Actions["ToggleResourceConverterAction"].guiName = base.Actions["ToggleResourceConverterAction"].guiName + " (" + resourceOutputName + ")";
		}
		base.Fields["status"].guiName = ConverterName;
		base.Fields["status"].guiActiveEditor = true;
	}

	public virtual void SetupModule()
	{
		try
		{
			base.Fields["status"].guiName = ConverterName;
			if (!base.part.Modules.Contains("ModuleAnimationGroup"))
			{
				EnableModule();
			}
		}
		catch (Exception ex)
		{
			MonoBehaviour.print("[RESOURCES] - Error in - BaseConverter_SetupModule - " + ex);
		}
	}

	public override void OnStart(StartState state)
	{
		SetupDebugging();
		SetupLabels();
		catchupRetries = 0;
		EfficiencyModifiers = new Dictionary<string, float>();
		TallyEfficiencyModifiers();
		DirtyFlag = !IsActivated;
		if (base.vessel == null)
		{
			return;
		}
		try
		{
			base.OnStart(state);
			SetupModule();
			UpdateConverterStatus();
			partAnimationModule = base.part.FindModuleImplementing<ModuleAnimationGroup>();
			hasAnimationGroup = partAnimationModule != null;
		}
		catch (Exception ex)
		{
			Debug.LogError("[RESOURCES] - Error in - BaseConverter_OnStart - " + ex);
		}
	}

	public override void OnAwake()
	{
		if (inputList == null)
		{
			inputList = new List<ResourceRatio>();
		}
		if (outputList == null)
		{
			outputList = new List<ResourceRatio>();
		}
		if (reqList == null)
		{
			reqList = new List<ResourceRatio>();
		}
		if (ThermalEfficiency == null)
		{
			ThermalEfficiency = new FloatCurve();
			ThermalEfficiency.Add(0f, 1f);
		}
		if (TemperatureModifier == null)
		{
			TemperatureModifier = new FloatCurve();
			TemperatureModifier.Add(0f, 100f);
		}
		startEvt = base.Events["StartResourceConverter"];
		stopEvt = base.Events["StopResourceConverter"];
	}

	public override void OnSave(ConfigNode node)
	{
		base.OnSave(node);
		node.AddValue("lastUpdateTime", lastUpdateTime);
	}

	public virtual double GetEfficiencyMultiplier()
	{
		float heatThrottle = GetHeatThrottle();
		return EfficiencyBonus * heatThrottle * _totalEfficiencyModifiers * GetCrewEfficiencyBonus();
	}

	public virtual void FixedUpdate()
	{
		if (!(ResourceScenario.Instance == null) && !(base.vessel == null))
		{
			PreProcessing();
			double deltaTime = GetDeltaTime();
			if (deltaTime < 1E-09 && !HighLogic.LoadedSceneIsEditor)
			{
				return;
			}
			if (AlwaysActive)
			{
				IsActivated = true;
			}
			UpdateConverterStatus();
			if (IsActivated)
			{
				ConversionRecipe conversionRecipe = PrepareRecipe(deltaTime);
				if (conversionRecipe != null)
				{
					conversionRecipe.FillAmount = FillAmount;
					conversionRecipe.TakeAmount = TakeAmount;
					double num = 1.0;
					if (!_preCalculateEfficiency)
					{
						num = GetEfficiencyMultiplier();
					}
					if (ResourceSetup.Instance.ResConfig.ShowDebugOptions)
					{
						debugFinBon = num.ToString();
					}
					if (!HighLogic.LoadedSceneIsEditor)
					{
						ConverterResults result = ResConverter.ProcessRecipe(deltaTime, conversionRecipe, base.part, this, (float)num);
						PostProcess(result, deltaTime);
						if (ResourceSetup.Instance.ResConfig.ShowDebugOptions)
						{
							UpdateDebugInfo(result, deltaTime);
						}
						double num2 = result.TimeFactor / deltaTime;
						if (Math.Abs(num2 - lastTimeFactor) > 1E-09)
						{
							lastTimeFactor = num2;
							GameEvents.OnEfficiencyChange.Fire(this, base.part, lastTimeFactor);
						}
					}
				}
			}
			CheckForShutdown();
			PostUpdateCleanup();
		}
		else if (HighLogic.LoadedSceneIsEditor)
		{
			UpdateConverterStatus();
		}
	}

	public virtual void TallyEfficiencyModifiers()
	{
		_totalEfficiencyModifiers = 1f;
		foreach (KeyValuePair<string, float> efficiencyModifier in EfficiencyModifiers)
		{
			_totalEfficiencyModifiers *= efficiencyModifier.Value;
		}
	}

	public virtual void ConvertRecipeToUnits(ConversionRecipe recipe)
	{
		int count = recipe.Inputs.Count;
		int count2 = recipe.Outputs.Count;
		int count3 = recipe.Requirements.Count;
		List<ResourceRatio> list = new List<ResourceRatio>();
		List<ResourceRatio> list2 = new List<ResourceRatio>();
		List<ResourceRatio> list3 = new List<ResourceRatio>();
		for (int i = 0; i < count; i++)
		{
			PartResourceDefinition partResourceDefinition = PartResourceLibrary.Instance.resourceDefinitions[recipe.Inputs[i].ResourceName];
			if ((double)partResourceDefinition.density > 1E-09)
			{
				double ratio = recipe.Inputs[i].Ratio / (double)partResourceDefinition.density;
				ResourceRatio item = new ResourceRatio(recipe.Inputs[i].ResourceName, ratio, recipe.Inputs[i].DumpExcess, recipe.Inputs[i].FlowMode);
				list.Add(item);
			}
			else
			{
				list.Add(recipe.Inputs[i]);
			}
		}
		for (int j = 0; j < count2; j++)
		{
			PartResourceDefinition partResourceDefinition2 = PartResourceLibrary.Instance.resourceDefinitions[recipe.Outputs[j].ResourceName];
			if ((double)partResourceDefinition2.density > 1E-09)
			{
				double ratio2 = recipe.Outputs[j].Ratio / (double)partResourceDefinition2.density;
				ResourceRatio item2 = new ResourceRatio(recipe.Outputs[j].ResourceName, ratio2, recipe.Outputs[j].DumpExcess, recipe.Outputs[j].FlowMode);
				list2.Add(item2);
			}
			else
			{
				list2.Add(recipe.Outputs[j]);
			}
		}
		for (int k = 0; k < count3; k++)
		{
			PartResourceDefinition partResourceDefinition3 = PartResourceLibrary.Instance.resourceDefinitions[recipe.Requirements[k].ResourceName];
			if ((double)partResourceDefinition3.density > 1E-09)
			{
				double ratio3 = recipe.Requirements[k].Ratio / (double)partResourceDefinition3.density;
				ResourceRatio item3 = new ResourceRatio(recipe.Requirements[k].ResourceName, ratio3, recipe.Requirements[k].DumpExcess, recipe.Requirements[k].FlowMode);
				list2.Add(item3);
			}
			else
			{
				list3.Add(recipe.Requirements[k]);
			}
		}
		recipe.SetInputs(list);
		recipe.SetOutputs(list2);
		recipe.SetRequirements(list3);
	}

	public virtual float GetHeatThrottle()
	{
		if (!IsActivated)
		{
			return 1f;
		}
		ModuleCoreHeat moduleCoreHeat = base.part.FindModuleImplementing<ModuleCoreHeat>();
		if (moduleCoreHeat != null)
		{
			return ThermalEfficiency.Evaluate((float)moduleCoreHeat.CoreTemperature);
		}
		return ThermalEfficiency.Evaluate((float)base.part.temperature);
	}

	public virtual void CheckForShutdown()
	{
		if (AutoShutdown && GeneratesHeat)
		{
			double num = base.part.maxTemp * (double)DefaultShutoffTemp;
			if (base.part.temperature > num)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_257466"), 5f, ScreenMessageStyle.UPPER_CENTER);
				StopResourceConverter();
			}
		}
	}

	public virtual int HasSpecialist(string effect)
	{
		int num = -1;
		List<ProtoCrewMember> vesselCrew = base.vessel.GetVesselCrew();
		if (vesselCrew.Count > 0)
		{
			int count = vesselCrew.Count;
			while (count-- > 0)
			{
				ProtoCrewMember protoCrewMember = vesselCrew[count];
				if (protoCrewMember.HasEffect(effect) && protoCrewMember.experienceLevel > num)
				{
					num = protoCrewMember.experienceLevel;
				}
				if (num == 5)
				{
					break;
				}
			}
		}
		return num;
	}

	public virtual bool IsOverheating()
	{
		ModuleCoreHeat moduleCoreHeat = base.part.FindModuleImplementing<ModuleCoreHeat>();
		if (moduleCoreHeat != null)
		{
			return moduleCoreHeat.CoreTemperature > base.part.temperature;
		}
		return false;
	}

	public virtual void PreProcessing()
	{
	}

	public virtual void PostUpdateCleanup()
	{
	}

	public virtual void PostProcess(ConverterResults result, double deltaTime)
	{
		if (result.TimeFactor <= 1E-09)
		{
			status = result.Status;
			statusPercent = 0.0;
			return;
		}
		double num = Math.Round(result.TimeFactor / deltaTime * 100.0, 2);
		if (!Mathf.Approximately((float)statusPercent, (float)num))
		{
			statusPercent = num;
			if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
			{
				status = Localizer.Format("#autoLOC_257529", statusPercent.ToString("0.00"));
			}
		}
	}

	public virtual ConversionRecipe PrepareRecipe(double deltatime)
	{
		MonoBehaviour.print("[RESOURCES] No Implementation of PrepareRecipe in derived class");
		return null;
	}

	public virtual bool IsSituationValid()
	{
		MonoBehaviour.print("[RESOURCES] No Implementation of IsSituationValid in derived class");
		return true;
	}

	public virtual void OnOverheat(double amount = 1.0)
	{
		if (AutoShutdown && GeneratesHeat)
		{
		}
	}

	public virtual double GetFlux()
	{
		return lastHeatFlux;
	}

	public virtual bool DisplayCoreHeat()
	{
		return base.part.HasModuleImplementing<ModuleCoreHeat>();
	}

	public virtual double GetCoreTemperature()
	{
		ModuleCoreHeat moduleCoreHeat = base.part.FindModuleImplementing<ModuleCoreHeat>();
		if (moduleCoreHeat == null)
		{
			return base.part.temperature;
		}
		return moduleCoreHeat.CoreTemperature;
	}

	public virtual double GetGoalTemperature()
	{
		ModuleCoreHeat moduleCoreHeat = base.part.FindModuleImplementing<ModuleCoreHeat>();
		if (moduleCoreHeat == null)
		{
			return base.part.temperature;
		}
		return moduleCoreHeat.CoreTempGoal + moduleCoreHeat.CoreTempGoalAdjustment;
	}

	public virtual bool CanBeDetached()
	{
		return !IsActivated;
	}

	public virtual bool CanBeOffset()
	{
		return !IsActivated;
	}

	public virtual bool CanBeRotated()
	{
		return !IsActivated;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003020");
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_257237 = Localizer.Format("#autoLOC_257237");
	}
}
