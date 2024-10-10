using ns9;

public class ModuleToggleCrossfeed : PartModule, IToggleCrossfeed
{
	[KSPField(isPersistant = true)]
	public bool crossfeedStatus;

	[KSPField]
	public bool eventPropagatesInEditor = true;

	[KSPField]
	public bool eventPropagatesInFlight;

	[KSPField]
	public bool toggleEditor = true;

	[KSPField]
	public bool toggleFlight;

	[KSPField]
	public string enableText = "#autoLOC_236028";

	[KSPField]
	public string disableText = "#autoLOC_236030";

	[KSPField]
	public string toggleText = "#autoLOC_236032";

	[KSPField]
	public string techRequired = "";

	public BaseEvent toggleE;

	public BaseAction toggleA;

	public BaseAction enableA;

	public BaseAction disableA;

	public bool defaultCrossfeedStatus;

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_236030")]
	public void ToggleEvent()
	{
		crossfeedStatus = !crossfeedStatus;
		if ((HighLogic.LoadedSceneIsEditor && eventPropagatesInEditor) || (HighLogic.LoadedSceneIsFlight && eventPropagatesInFlight))
		{
			int count = base.part.symmetryCounterparts.Count;
			while (count-- > 0)
			{
				Part part = base.part.symmetryCounterparts[count];
				if (part != base.part)
				{
					ModuleToggleCrossfeed module = part.Modules.GetModule<ModuleToggleCrossfeed>();
					if (module != null)
					{
						module.crossfeedStatus = crossfeedStatus;
						module.UpdateCrossfeed(fireEvent: false);
					}
				}
			}
		}
		UpdateCrossfeed(fireEvent: true);
	}

	[KSPAction("#autoLOC_236032")]
	public void ToggleAction(KSPActionParam param)
	{
		bool flag = eventPropagatesInEditor;
		bool flag2 = eventPropagatesInFlight;
		eventPropagatesInEditor = false;
		eventPropagatesInFlight = false;
		ToggleEvent();
		eventPropagatesInEditor = flag;
		eventPropagatesInFlight = flag2;
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

	[KSPAction("#autoLOC_236028")]
	public void EnableAction(KSPActionParam param)
	{
		Activate();
	}

	public void Activate()
	{
		crossfeedStatus = true;
		UpdateCrossfeed(fireEvent: true);
	}

	[KSPAction("#autoLOC_236030")]
	public void DisableAction(KSPActionParam param)
	{
		Deactivate();
	}

	public void Deactivate()
	{
		crossfeedStatus = false;
		UpdateCrossfeed(fireEvent: true);
	}

	public void Start()
	{
		UpdateCrossfeed(fireEvent: false);
	}

	public override void OnAwake()
	{
		base.OnAwake();
		toggleE = base.Events["ToggleEvent"];
		toggleA = base.Actions["ToggleAction"];
		enableA = base.Actions["EnableAction"];
		disableA = base.Actions["DisableAction"];
		if (base.part.partInfo == null || base.part.partInfo.partPrefab == null)
		{
			defaultCrossfeedStatus = base.part.fuelCrossFeed;
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		UpdateCrossfeed(fireEvent: false);
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		bool flag = true;
		if (CrossfeedRequiresTech())
		{
			flag = CrossfeedHasTech();
		}
		if (!flag || !moduleIsEnabled)
		{
			crossfeedStatus = defaultCrossfeedStatus;
		}
		UpdateCrossfeed(fireEvent: true);
		toggleE.guiActiveEditor = flag && toggleEditor && moduleIsEnabled;
		toggleE.guiActive = (toggleA.active = (enableA.active = (disableA.active = flag && toggleFlight && moduleIsEnabled)));
	}

	public void UpdateCrossfeed(bool fireEvent)
	{
		bool fuelCrossFeed = base.part.fuelCrossFeed;
		base.part.fuelCrossFeed = crossfeedStatus;
		if (fuelCrossFeed != crossfeedStatus)
		{
			GameEvents.onPartCrossfeedStateChange.Fire(base.part);
		}
		if (crossfeedStatus)
		{
			toggleE.guiName = disableText;
		}
		else
		{
			toggleE.guiName = enableText;
		}
		toggleA.guiName = toggleText;
		enableA.guiName = enableText;
		disableA.guiName = disableText;
	}

	public bool CrossfeedToggleableEditor()
	{
		return toggleEditor;
	}

	public bool CrossfeedToggleableFlight()
	{
		return toggleFlight;
	}

	public bool CrossfeedRequiresTech()
	{
		return !string.IsNullOrEmpty(techRequired);
	}

	public string CrossfeedTech()
	{
		return techRequired;
	}

	public bool CrossfeedHasTech()
	{
		if (HighLogic.CurrentGame != null && (HighLogic.CurrentGame.Mode == Game.Modes.CAREER || HighLogic.CurrentGame.Mode == Game.Modes.SCIENCE_SANDBOX))
		{
			return ResearchAndDevelopment.GetTechnologyState(techRequired) == RDTech.State.Available;
		}
		return true;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_236032");
	}
}
