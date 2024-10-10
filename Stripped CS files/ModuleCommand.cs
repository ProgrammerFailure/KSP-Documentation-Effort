using System.Collections.Generic;
using CommNet;
using Experience.Effects;
using ns9;
using UnityEngine;

public class ModuleCommand : PartModule, ICommNetControlSource, IResourceConsumer
{
	public enum ModuleControlState
	{
		NotEnoughCrew,
		NotEnoughResources,
		PartialManned,
		NoControlPoint,
		TouristCrew,
		PartialProbe,
		Nominal
	}

	public ModuleControlState moduleState;

	public VesselControlState localVesselControlState;

	public bool commCapable = true;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001355")]
	public string commNetSignal = Localizer.Format("#autoLOC_217146");

	[KSPField(guiActive = true, guiName = "#autoLOC_6001356")]
	public string commNetFirstHopDistance = Localizer.Format("#autoLOC_217149");

	[KSPField(guiActive = true, guiName = "#autoLOC_6001357")]
	public string controlSrcStatusText = "NA";

	[KSPField]
	public bool hasHibernation;

	[UI_Toggle(disabledText = "#autoLOC_6001073", scene = UI_Scene.All, enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001358")]
	public bool hibernation;

	[KSPField]
	public double hibernationMultiplier = 0.01;

	[UI_Toggle(disabledText = "#autoLOC_7001001", scene = UI_Scene.All, enabledText = "#autoLOC_7001000", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001359")]
	public bool hibernateOnWarp;

	[KSPField]
	public bool requiresPilot = true;

	[KSPField]
	public bool remoteControl = true;

	[KSPField]
	public bool requiresTelemetry = true;

	[KSPField]
	public int minimumCrew;

	public ModuleResource lackingResource;

	public int crewCount;

	public int totalCrewCount;

	public int pilots;

	public bool networkInitialised;

	[KSPField]
	public string defaultControlPointDisplayName = Localizer.Format("#autoLOC_6011000");

	public string controlPointDisplayName;

	[KSPField(isPersistant = true)]
	public string activeControlPointName = "_default";

	public ControlPoint activeControlPoint;

	public bool showControlPointVisual;

	public float cpArrowLength = 2f;

	public ArrowPointer cpforwardArrow;

	public ArrowPointer cpUpArrow;

	public ArrowPointer cpRightArrow;

	[SerializeField]
	public Color cpForwardColor = Color.blue;

	[SerializeField]
	public Color cpUpColor = Color.green;

	[SerializeField]
	public Color cpRightColor = Color.red;

	[SerializeField]
	public List<ControlPoint> controlPointsList;

	public DictionaryValueList<string, ControlPoint> controlPoints;

	public List<PartResourceDefinition> consumedResources;

	public static string cacheAutoLOC_7001411;

	public static string cacheAutoLOC_217448;

	public static string cacheAutoLOC_217464;

	public static string cacheAutoLOC_217408;

	public static string cacheAutoLOC_217417;

	public static string cacheAutoLOC_217429;

	public static string cacheAutoLOC_217437;

	public static string cacheAutoLOC_217509;

	public static string cacheAutoLOC_217513;

	public static string cacheAutoLOC_217517;

	public static string cacheAutoLOC_6003031;

	public CommNetVessel Connection => base.vessel.connection;

	public ModuleControlState ModuleState => moduleState;

	public VesselControlState VesselControlState => base.vessel.connection.ControlState;

	public CommPath VesselControlPath => base.vessel.connection.ControlPath;

	public double VesselSignalStrength => base.vessel.connection.ControlPath.signalStrength;

	public SignalStrength VesselSignal => base.vessel.connection.ControlPath.signal;

	public bool SignalRequired
	{
		get
		{
			if (CommNetScenario.CommNetEnabled)
			{
				return HighLogic.CurrentGame.Parameters.CustomParams<CommNetParams>().requireSignalForControl;
			}
			return false;
		}
	}

	public bool IsHibernating
	{
		get
		{
			if (!hibernation)
			{
				if (hibernateOnWarp)
				{
					if (TimeWarp.CurrentRate > 1f && TimeWarp.WarpMode == TimeWarp.Modes.HIGH)
					{
						return true;
					}
					return base.part.packed;
				}
				return false;
			}
			return true;
		}
	}

	public string ActiveControlPointName => activeControlPointName;

	string ICommNetControlSource.name => base.name;

	[KSPEvent(advancedTweakable = true, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8003232")]
	public void ToggleControlPointVisual()
	{
		if (ControlPointsExist())
		{
			showControlPointVisual = !showControlPointVisual;
			if (!showControlPointVisual)
			{
				base.Events["ToggleControlPointVisual"].guiName = Localizer.Format("#autoLOC_8003232");
			}
			else
			{
				base.Events["ToggleControlPointVisual"].guiName = Localizer.Format("#autoLOC_8003233");
			}
		}
	}

	public virtual void UpdateControlPointVisuals()
	{
		if (!ControlPointsExist())
		{
			return;
		}
		if (showControlPointVisual)
		{
			if (cpforwardArrow == null)
			{
				cpforwardArrow = ArrowPointer.Create(base.transform, Vector3.zero, activeControlPoint.transform.up, cpArrowLength, cpForwardColor, worldSpace: true);
				cpforwardArrow.gameObject.name = "ArrowPointer-cpForward";
			}
			else
			{
				cpforwardArrow.Direction = activeControlPoint.transform.up;
				cpforwardArrow.Length = cpArrowLength;
			}
			if (cpUpArrow == null)
			{
				cpUpArrow = ArrowPointer.Create(base.transform, Vector3.zero, -activeControlPoint.transform.forward, cpArrowLength, cpUpColor, worldSpace: true);
				cpUpArrow.gameObject.name = "ArrowPointer-cpUp";
			}
			else
			{
				cpUpArrow.Direction = -activeControlPoint.transform.forward;
				cpUpArrow.Length = cpArrowLength;
			}
			if (cpRightArrow == null)
			{
				cpRightArrow = ArrowPointer.Create(base.transform, Vector3.zero, activeControlPoint.transform.right, cpArrowLength, cpRightColor, worldSpace: true);
				cpRightArrow.gameObject.name = "ArrowPointer-cpRight";
			}
			else
			{
				cpRightArrow.Direction = activeControlPoint.transform.right;
				cpRightArrow.Length = cpArrowLength;
			}
		}
		else
		{
			DestroyCPArrows();
		}
	}

	public void DestroyCPArrows()
	{
		if ((object)cpforwardArrow != null)
		{
			Object.Destroy(cpforwardArrow.gameObject);
			cpforwardArrow = null;
		}
		if ((object)cpUpArrow != null)
		{
			Object.Destroy(cpUpArrow.gameObject);
			cpUpArrow = null;
		}
		if ((object)cpRightArrow != null)
		{
			Object.Destroy(cpRightArrow.gameObject);
			cpRightArrow = null;
		}
	}

	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "Change Control Point")]
	public void ChangeControlPoint()
	{
		if (controlPoints == null)
		{
			Debug.LogErrorFormat("[ModuleCommand]: There are no alternate control points for {0}", base.part.partInfo.title);
			return;
		}
		int num = controlPoints.KeysList.IndexOf(activeControlPointName);
		num++;
		if (num >= controlPoints.Count)
		{
			num = 0;
		}
		string controlPoint = controlPoints.KeyAt(num);
		SetControlPoint(controlPoint);
	}

	public void SetControlPoint(string newControlPointName = "_default")
	{
		if (controlPoints == null)
		{
			Debug.LogErrorFormat("[ModuleCommand]: There are no alternate control points for {0}", base.part.partInfo.title);
			return;
		}
		if (!controlPoints.ContainsKey(newControlPointName))
		{
			if (!controlPoints.ContainsKey("_default"))
			{
				Debug.LogWarningFormat("[ModuleCommand]: Control Point name does not exist and cant find _default: {0}", newControlPointName);
				return;
			}
			Debug.LogWarningFormat("[ModuleCommand]: Control Point name does not exist. using _default: {0}", newControlPointName);
			newControlPointName = "_default";
		}
		ControlPoint controlPoint = controlPoints[newControlPointName];
		if (HighLogic.LoadedScene == GameScenes.FLIGHT)
		{
			base.part.SetReferenceTransform(controlPoint.transform);
		}
		controlPointDisplayName = Localizer.Format(controlPoint.displayName);
		activeControlPointName = newControlPointName;
		activeControlPoint = controlPoint;
		UpdateControlPointEvent();
		GameEvents.OnControlPointChanged.Fire(base.part, controlPoints[newControlPointName]);
	}

	public void UpdateControlPointEvent()
	{
		bool flag = controlPoints != null && controlPoints.Count > 1;
		base.Events["ChangeControlPoint"].active = flag;
		if (flag)
		{
			base.Events["ChangeControlPoint"].guiName = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_6011002"), controlPointDisplayName);
		}
	}

	public List<PartResourceDefinition> GetConsumedResources()
	{
		return consumedResources;
	}

	public virtual void Start()
	{
		if (HighLogic.LoadedSceneIsGame && !HighLogic.LoadedSceneIsEditor && HighLogic.LoadedScene != GameScenes.SPACECENTER && !(base.vessel == null))
		{
			networkInitialised = false;
			if (CommNetNetwork.Initialized && base.vessel.connection != null)
			{
				OnNetworkInitialised();
			}
			GameEvents.CommNet.OnNetworkInitialized.Add(OnNetworkInitialised);
		}
	}

	public virtual void OnDestroy()
	{
		if (base.vessel != null && base.vessel.connection != null)
		{
			base.vessel.connection.UnregisterCommandSource(this);
		}
		GameEvents.CommNet.OnNetworkInitialized.Remove(OnNetworkInitialised);
	}

	public virtual void OnNetworkInitialised()
	{
		base.vessel.connection.RegisterCommandSource(this);
		networkInitialised = true;
	}

	public virtual void UpdateNetwork()
	{
		if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false) && Connection != null)
		{
			commNetSignal = KSPUtil.LocalizeNumber(Connection.SignalStrength, "F2");
			if (Connection.IsConnected)
			{
				commNetFirstHopDistance = KSPUtil.PrintSI(Connection.ControlPath.First.cost, cacheAutoLOC_7001411);
			}
			else
			{
				commNetFirstHopDistance = "-";
			}
		}
	}

	public override void OnAwake()
	{
		if (consumedResources == null)
		{
			consumedResources = new List<PartResourceDefinition>();
		}
		else
		{
			consumedResources.Clear();
		}
		int i = 0;
		for (int count = resHandler.inputResources.Count; i < count; i++)
		{
			consumedResources.Add(PartResourceLibrary.Instance.GetDefinition(resHandler.inputResources[i].name));
		}
		base.part.isControlSource = Vessel.ControlLevel.FULL;
		BaseAction baseAction = base.Actions["HibernateToggle"];
		BaseField baseField = base.Fields["hibernation"];
		BaseField baseField2 = base.Fields["hibernation"];
		BaseField baseField3 = base.Fields["hibernateOnWarp"];
		bool flag2 = (base.Fields["hibernateOnWarp"].guiActive = hasHibernation);
		bool flag4 = (baseField3.guiActiveEditor = flag2);
		bool flag6 = (baseField2.guiActiveEditor = flag4);
		bool active = (baseField.guiActive = flag6);
		baseAction.active = active;
		if (controlPointsList != null && controlPoints == null)
		{
			controlPoints = new DictionaryValueList<string, ControlPoint>();
			ControlPoint controlPoint = new ControlPoint("_default", defaultControlPointDisplayName, base.part.gameObject.transform, Vector3.zero);
			controlPointDisplayName = Localizer.Format(defaultControlPointDisplayName);
			controlPoints.Add(controlPoint.name, controlPoint);
			for (int j = 0; j < controlPointsList.Count; j++)
			{
				controlPoints.Add(controlPointsList[j].name, controlPointsList[j]);
			}
		}
		cpArrowLength = GameSettings.CONTROLPOINT_ARROWLENGTH;
		cpForwardColor = GameSettings.COLOR_CONTROLPOINT_COLOR_FORWARD;
		cpUpColor = GameSettings.COLOR_CONTROLPOINT_COLOR_UP;
		cpRightColor = GameSettings.COLOR_CONTROLPOINT_COLOR_RIGHT;
		base.Events["ToggleControlPointVisual"].active = (base.Events["ToggleControlPointVisual"].guiActive = (base.Events["ToggleControlPointVisual"].guiActiveEditor = GameSettings.CONTROLPOINT_VISUALS_ENABLED && ControlPointsExist()));
	}

	public override void OnStart(StartState state)
	{
		BaseAction baseAction = base.Actions["HibernateToggle"];
		BaseField baseField = base.Fields["hibernation"];
		BaseField baseField2 = base.Fields["hibernation"];
		BaseField baseField3 = base.Fields["hibernateOnWarp"];
		bool flag2 = (base.Fields["hibernateOnWarp"].guiActive = hasHibernation);
		bool flag4 = (baseField3.guiActiveEditor = flag2);
		bool flag6 = (baseField2.guiActiveEditor = flag4);
		bool active = (baseField.guiActive = flag6);
		baseAction.active = active;
		if (controlPoints != null && (HighLogic.LoadedScene == GameScenes.EDITOR || HighLogic.LoadedScene == GameScenes.FLIGHT))
		{
			for (int i = 0; i < controlPoints.Count; i++)
			{
				string key = controlPoints.KeysList[i];
				GameObject gameObject = new GameObject(key);
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.Euler(controlPoints[key].orientation);
				controlPoints[key].transform = gameObject.transform;
			}
		}
		SetControlPoint(activeControlPointName);
		UpdateControlPointEvent();
		base.Events["ToggleControlPointVisual"].active = (base.Events["ToggleControlPointVisual"].guiActive = (base.Events["ToggleControlPointVisual"].guiActiveEditor = GameSettings.CONTROLPOINT_VISUALS_ENABLED && ControlPointsExist()));
		for (int j = 0; j < base.part.protoModuleCrew.Count; j++)
		{
			base.part.protoModuleCrew[j].persistentID = FlightGlobals.GetUniquepersistentId();
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (controlPointsList != null)
		{
			return;
		}
		ConfigNode[] nodes = node.GetNodes("CONTROLPOINT");
		if (nodes.Length != 0)
		{
			controlPointsList = new List<ControlPoint>();
			for (int i = 0; i < nodes.Length; i++)
			{
				ControlPoint controlPoint = new ControlPoint();
				controlPoint.Load(nodes[i]);
				controlPointsList.Add(controlPoint);
			}
		}
	}

	public override void OnSave(ConfigNode node)
	{
	}

	public virtual void FixedUpdate()
	{
		UpdateControlState();
	}

	public void UpdateControlState()
	{
		if (HighLogic.LoadedSceneIsFlight && !FlightDriver.Pause && base.vessel != null)
		{
			localVesselControlState = UpdateControlSourceState();
			if ((localVesselControlState & VesselControlState.Full) > VesselControlState.None)
			{
				base.part.isControlSource = Vessel.ControlLevel.FULL;
			}
			else if ((localVesselControlState & VesselControlState.Partial) > VesselControlState.None)
			{
				if ((localVesselControlState & VesselControlState.Kerbal) > VesselControlState.None)
				{
					base.part.isControlSource = Vessel.ControlLevel.PARTIAL_MANNED;
				}
				else
				{
					base.part.isControlSource = Vessel.ControlLevel.PARTIAL_UNMANNED;
				}
			}
			else
			{
				base.part.isControlSource = Vessel.ControlLevel.NONE;
			}
		}
		UpdateControlPointVisuals();
	}

	public VesselControlState GetControlSourceState()
	{
		return localVesselControlState;
	}

	public bool IsCommCapable()
	{
		return commCapable;
	}

	public virtual VesselControlState UpdateControlSourceState()
	{
		commCapable = false;
		bool isHibernating = IsHibernating;
		if (!resHandler.UpdateModuleResourceInputs(ref controlSrcStatusText, isHibernating ? hibernationMultiplier : 1.0, 0.9, returnOnFirstLack: true))
		{
			moduleState = ModuleControlState.NotEnoughResources;
			if (minimumCrew > 0)
			{
				return VesselControlState.Kerbal;
			}
			return VesselControlState.Probe;
		}
		totalCrewCount = 0;
		crewCount = 0;
		pilots = 0;
		int count = base.part.protoModuleCrew.Count;
		while (count-- > 0)
		{
			ProtoCrewMember protoCrewMember = base.part.protoModuleCrew[count];
			if (protoCrewMember.HasEffect<FullVesselControlSkill>() && !protoCrewMember.inactive)
			{
				pilots++;
			}
			if (protoCrewMember.type != ProtoCrewMember.KerbalType.Tourist)
			{
				totalCrewCount++;
				if (!protoCrewMember.inactive)
				{
					crewCount++;
				}
			}
		}
		bool flag = requiresPilot && pilots == 0 && base.part.CrewCapacity > 0;
		if (crewCount < minimumCrew)
		{
			if (totalCrewCount < base.part.protoModuleCrew.Count)
			{
				if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
				{
					controlSrcStatusText = Localizer.Format("#autoLOC_7000069", crewCount, minimumCrew);
				}
				moduleState = ModuleControlState.TouristCrew;
				return VesselControlState.Kerbal;
			}
			if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
			{
				controlSrcStatusText = Localizer.Format("#autoLOC_7000068", crewCount, minimumCrew);
			}
			moduleState = ModuleControlState.NotEnoughCrew;
			return VesselControlState.Kerbal;
		}
		commCapable = true;
		if (CommNetScenario.CommNetEnabled && CommNetScenario.Instance != null)
		{
			bool flag2 = Connection != null && Connection.IsConnected;
			if (!remoteControl && flag)
			{
				if (controlSrcStatusText != "No Pilot")
				{
					controlSrcStatusText = cacheAutoLOC_217408;
				}
				moduleState = ModuleControlState.PartialManned;
				return VesselControlState.KerbalPartial;
			}
			if (!flag2)
			{
				if (flag)
				{
					if (controlSrcStatusText != "No Pilot")
					{
						controlSrcStatusText = cacheAutoLOC_217417;
					}
					moduleState = ModuleControlState.PartialManned;
					return VesselControlState.KerbalPartial;
				}
				if (totalCrewCount == 0)
				{
					if (SignalRequired || !remoteControl)
					{
						if (controlSrcStatusText != "No Telemetry")
						{
							controlSrcStatusText = cacheAutoLOC_217429;
						}
						moduleState = ModuleControlState.NoControlPoint;
						return VesselControlState.Probe;
					}
					if (requiresTelemetry)
					{
						if (controlSrcStatusText != "Partial Control")
						{
							controlSrcStatusText = cacheAutoLOC_217437;
						}
						moduleState = ModuleControlState.PartialProbe;
						return VesselControlState.ProbePartial;
					}
				}
			}
		}
		if (isHibernating)
		{
			controlSrcStatusText = cacheAutoLOC_217448;
			if (minimumCrew > 0)
			{
				moduleState = ModuleControlState.PartialManned;
				return VesselControlState.KerbalPartial;
			}
			moduleState = ModuleControlState.PartialProbe;
			return VesselControlState.ProbePartial;
		}
		controlSrcStatusText = cacheAutoLOC_217464;
		if (minimumCrew > 0)
		{
			moduleState = ModuleControlState.Nominal;
			return VesselControlState.KerbalFull;
		}
		moduleState = ModuleControlState.Nominal;
		return VesselControlState.ProbeFull;
	}

	[KSPAction("#autoLOC_6001360")]
	public void MakeReferenceToggle(KSPActionParam act)
	{
		MakeReference();
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001360")]
	public virtual void MakeReference()
	{
		base.vessel.SetReferenceTransform(base.part);
	}

	[KSPEvent(guiActiveUncommand = true, guiActive = true, guiName = "#autoLOC_900678")]
	public virtual void RenameVessel()
	{
		base.vessel.RenameVessel();
	}

	[KSPAction("#autoLOC_6001361")]
	public virtual void HibernateToggle(KSPActionParam param)
	{
		hibernation = !hibernation;
	}

	public override string GetInfo()
	{
		string text = "";
		text = ((minimumCrew <= 0) ? (text + cacheAutoLOC_217509) : (text + Localizer.Format("#autoLOC_217505", minimumCrew)));
		if (requiresPilot && minimumCrew > 0)
		{
			text += cacheAutoLOC_217513;
		}
		if (remoteControl)
		{
			text += cacheAutoLOC_217517;
		}
		if (hasHibernation)
		{
			text += (hibernateOnWarp ? Localizer.Format("#autoLOC_6004044", hibernationMultiplier) : Localizer.Format("#autoLOC_217522", hibernationMultiplier));
		}
		return text + resHandler.PrintModuleResources();
	}

	public ControlPoint GetControlPoint(string name)
	{
		if (controlPoints != null && controlPoints.Count > 0)
		{
			if (controlPoints.ContainsKey(name))
			{
				return controlPoints[name];
			}
			return null;
		}
		return null;
	}

	public bool ControlPointsExist()
	{
		if (controlPoints != null && controlPoints.Count > 0)
		{
			return true;
		}
		return false;
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_6003031;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_7001411 = Localizer.Format("#autoLOC_7001411");
		cacheAutoLOC_217448 = Localizer.Format("#autoLOC_217448");
		cacheAutoLOC_217464 = Localizer.Format("#autoLOC_217464");
		cacheAutoLOC_217408 = Localizer.Format("#autoLOC_217408");
		cacheAutoLOC_217417 = Localizer.Format("#autoLOC_217417");
		cacheAutoLOC_217429 = Localizer.Format("#autoLOC_217429");
		cacheAutoLOC_217437 = Localizer.Format("#autoLOC_217437");
		cacheAutoLOC_217509 = Localizer.Format("#autoLOC_217509");
		cacheAutoLOC_217513 = Localizer.Format("#autoLOC_217513");
		cacheAutoLOC_217517 = Localizer.Format("#autoLOC_217517");
		cacheAutoLOC_6003031 = Localizer.Format("#autoLoc_6003031");
	}
}
