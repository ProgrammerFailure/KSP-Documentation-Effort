using ns9;

public class ModuleOrbitalScanner : PartModule, IAnimatedModule
{
	[KSPField]
	public bool CheckForLock = true;

	public BaseEvent switchR;

	public BaseEvent toggleO;

	public BaseEvent setCol;

	public bool _ShowSurveyMenu;

	public bool _ShowOverlayMenu;

	public static string cacheAutoLOC_6001054;

	public static string cacheAutoLOC_6002358;

	public static string cacheAutoLOC_7003276;

	public static string cacheAutoLOC_7003277;

	public static string cacheAutoLOC_7003278;

	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001479")]
	public virtual void SwitchResource()
	{
		OverlayGenerator.Instance.FetchNextResource();
		ShowResourceName();
		if (OverlayGenerator.Instance.IsActive)
		{
			GenerateOverlay();
		}
	}

	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001480")]
	public virtual void ToggleOverlay()
	{
		if (OverlayGenerator.Instance.IsActive)
		{
			OverlayGenerator.Instance.IsActive = false;
			HideOverlay();
		}
		else if (ResourceMap.Instance.IsPlanetScanned(OverlayGenerator.Instance.DisplayBody.flightGlobalsIndex))
		{
			OverlayGenerator.Instance.IsActive = true;
			GenerateOverlay();
		}
	}

	public virtual void GenerateOverlay()
	{
		UpdateGUI();
		OverlayGenerator.Instance.GenerateOverlay(CheckForLock);
	}

	public virtual void HideOverlay()
	{
		OverlayGenerator.Instance.ClearDisplay();
	}

	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001481")]
	public virtual void SetColor()
	{
		int displayMode = (int)OverlayGenerator.Instance.DisplayMode;
		displayMode++;
		if (displayMode == 4)
		{
			displayMode = 0;
		}
		OverlayGenerator.Instance.DisplayMode = (MapDisplayTypes)displayMode;
		setCol.guiName = Localizer.Format("#autoLOC_6001482", GetOverlayColor(OverlayGenerator.Instance.DisplayMode));
		GenerateOverlay();
	}

	public virtual string GetOverlayColor(MapDisplayTypes dispType)
	{
		return dispType switch
		{
			MapDisplayTypes.Monochrome => cacheAutoLOC_7003278, 
			MapDisplayTypes.Inverse => cacheAutoLOC_7003277, 
			MapDisplayTypes.HeatMapGreen => cacheAutoLOC_7003276, 
			MapDisplayTypes.HeatMapBlue => cacheAutoLOC_6002358, 
			_ => dispType.ToString(), 
		};
	}

	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001483")]
	public virtual void IncreaseCutoff()
	{
		OverlayGenerator.Instance.Cutoff += 10;
		if (OverlayGenerator.Instance.Cutoff > 100)
		{
			OverlayGenerator.Instance.Cutoff = 100;
		}
		if (OverlayGenerator.Instance.IsActive)
		{
			GenerateOverlay();
		}
	}

	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001484")]
	public virtual void DecreaseCutoff()
	{
		OverlayGenerator.Instance.Cutoff -= 10;
		if (OverlayGenerator.Instance.Cutoff < 0)
		{
			OverlayGenerator.Instance.Cutoff = 0;
		}
		if (OverlayGenerator.Instance.IsActive)
		{
			GenerateOverlay();
		}
	}

	[KSPEvent(unfocusedRange = 3f, active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001485")]
	public virtual void ActivateScanner()
	{
		SetupMap();
	}

	public virtual void ToggleSurvey(bool state)
	{
		if (_ShowSurveyMenu != state)
		{
			switchR.active = state;
			toggleO.active = state;
			MonoUtilities.RefreshPartContextWindow(base.part);
			_ShowSurveyMenu = state;
		}
	}

	public virtual void ToggleMenu(bool state)
	{
		if (_ShowOverlayMenu != state)
		{
			setCol.active = state;
			base.Events["IncreaseCutoff"].active = state;
			base.Events["DecreaseCutoff"].active = state;
			UpdateGUI();
			_ShowOverlayMenu = state;
			MonoUtilities.RefreshPartContextWindow(base.part);
		}
	}

	public override void OnActive()
	{
		UpdateGUI();
		if (OverlayGenerator.Instance.IsActive)
		{
			GenerateOverlay();
		}
	}

	public virtual void UpdateGUI()
	{
		if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
		{
			string text = Localizer.Format(cacheAutoLOC_6001054, GetOverlayColor(OverlayGenerator.Instance.DisplayMode), OverlayGenerator.Instance.Cutoff.ToString("0"));
			if (setCol.guiName != text)
			{
				setCol.guiName = text;
				MonoUtilities.RefreshPartContextWindow(base.part);
			}
		}
	}

	public virtual void FixedUpdate()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			bool state = ResourceMap.Instance.IsPlanetScanned(base.vessel.mainBody.flightGlobalsIndex);
			bool isActive = OverlayGenerator.Instance.IsActive;
			if ((OverlayGenerator.Instance.DisplayBody == null || base.vessel.mainBody.flightGlobalsIndex != OverlayGenerator.Instance.DisplayBody.flightGlobalsIndex) && !OverlayGenerator.Instance.IsMapView)
			{
				OverlayGenerator.Instance.ClearDisplay();
				OverlayGenerator.Instance.DisplayBody = base.vessel.mainBody;
			}
			ToggleSurvey(state);
			ToggleMenu(isActive);
			UpdateGUI();
		}
	}

	public virtual void SetupMap()
	{
		if (OverlayGenerator.Instance.IsActive)
		{
			OverlayGenerator.Instance.ClearDisplay();
		}
		OverlayGenerator.Instance.DisplayBody = base.vessel.mainBody;
		OverlayGenerator.Instance.GenerateOverlay(CheckForLock);
		OverlayGenerator.Instance.IsActive = true;
		ShowResourceName();
	}

	public virtual void Start()
	{
		toggleO = base.Events["ToggleOverlay"];
		switchR = base.Events["SwitchResource"];
		setCol = base.Events["SetColor"];
		if (HighLogic.LoadedSceneIsFlight && !ResourceMap.Initialized)
		{
			GameEvents.OnResourceMapLoaded.Add(OnResourceMapLoaded);
		}
	}

	public virtual void ShowResourceName()
	{
		if (OverlayGenerator.Instance.DisplayResource == null && OverlayGenerator.Instance.ResourceList.Count > 0)
		{
			OverlayGenerator.Instance.DisplayResource = OverlayGenerator.Instance.ResourceList[0];
		}
		OverlayGenerator.Instance.DisplayBody = FlightGlobals.currentMainBody;
		float num = 0f;
		float num2 = 1f;
		int count = ResourceCache.Instance.AbundanceCache.Count;
		for (int i = 0; i < count; i++)
		{
			ResourceCache.AbundanceSummary abundanceSummary = ResourceCache.Instance.AbundanceCache[i];
			if (abundanceSummary.BodyId == OverlayGenerator.Instance.DisplayBody.flightGlobalsIndex && !(abundanceSummary.ResourceName != OverlayGenerator.Instance.DisplayResource.name) && abundanceSummary.HarvestType == OverlayGenerator.Instance.DisplayResourceType)
			{
				if (abundanceSummary.Abundance > num)
				{
					num = abundanceSummary.Abundance;
				}
				if (abundanceSummary.Abundance < num2)
				{
					num2 = abundanceSummary.Abundance;
				}
			}
		}
		string text = $"{(double)(num2 + num) / 0.02:0.00}%";
		switchR.guiName = OverlayGenerator.Instance.DisplayResource.displayName + Localizer.Format("#autoLOC_6002267", text);
	}

	public virtual void EnableModule()
	{
		isEnabled = true;
	}

	public virtual void DisableModule()
	{
		if (isEnabled)
		{
			OverlayGenerator.Instance.ClearDisplay();
		}
		isEnabled = false;
	}

	public virtual bool ModuleIsActive()
	{
		return OverlayGenerator.Instance.IsActive;
	}

	public virtual bool IsSituationValid()
	{
		if (base.vessel.mainBody.BiomeMap == null)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001055", base.vessel.mainBody.displayName), 5f, ScreenMessageStyle.UPPER_CENTER);
			return false;
		}
		return true;
	}

	public virtual void OnDestroy()
	{
		GameEvents.OnResourceMapLoaded.Remove(OnResourceMapLoaded);
	}

	public virtual void OnResourceMapLoaded()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			ShowResourceName();
			_ShowSurveyMenu = true;
			_ShowOverlayMenu = true;
			ToggleMenu(state: false);
			ToggleSurvey(state: false);
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6001054 = Localizer.Format("#autoLOC_6001054");
		cacheAutoLOC_6002358 = Localizer.Format("#autoLOC_6002358");
		cacheAutoLOC_7003276 = Localizer.Format("#autoLOC_7003276");
		cacheAutoLOC_7003277 = Localizer.Format("#autoLOC_7003277");
		cacheAutoLOC_7003278 = Localizer.Format("#autoLOC_7003278");
	}
}
