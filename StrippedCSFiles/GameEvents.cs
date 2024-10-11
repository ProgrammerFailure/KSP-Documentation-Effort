using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;
using Expansions.Missions;
using Expansions.Missions.Adjusters;
using Expansions.Missions.Editor;
using Expansions.Serenity;
using Expansions.Serenity.DeployedScience.Runtime;
using FinePrint;
using KSP.UI;
using KSP.UI.Screens;
using KSPAchievements;
using UnityEngine;
using Upgradeables;

public class GameEvents : GameEventsBase
{
	public struct ExplosionReaction
	{
		public float distance;

		public float magnitude;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ExplosionReaction(float distance, float magnitude)
		{
			throw null;
		}
	}

	public struct HostTargetAction<A, B>
	{
		public A host;

		public B target;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public HostTargetAction(A host, B target)
		{
			throw null;
		}
	}

	public struct FromToAction<A, B>
	{
		public A from;

		public B to;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public FromToAction(A from, B to)
		{
			throw null;
		}
	}

	public struct HostedFromToAction<A, B>
	{
		public A host;

		public B from;

		public B to;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public HostedFromToAction(A host, B from, B to)
		{
			throw null;
		}
	}

	public struct VesselSpawnInfo
	{
		public string craftSubfolder;

		public string profileName;

		public VesselSpawnDialog.CraftSelectedCallback OnFileSelected;

		public LaunchSiteFacility callingFacility;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public VesselSpawnInfo(string craftSubfolder, string profileName, VesselSpawnDialog.CraftSelectedCallback OnFileSelected, LaunchSiteFacility callingFacility)
		{
			throw null;
		}
	}

	public static class VesselSituation
	{
		public static EventData<Vessel, CelestialBody> onLand;

		public static EventData<Vessel, CelestialBody> onOrbit;

		public static EventData<Vessel, CelestialBody> onFlyBy;

		public static EventData<Vessel, CelestialBody> onReturnFromSurface;

		public static EventData<Vessel, CelestialBody> onReturnFromOrbit;

		public static EventData<Vessel, CelestialBody> onEscape;

		public static EventData<Vessel> onReachSpace;

		public static EventData<Vessel> onLaunch;

		public static EventData<Vessel, string, ReturnFrom> onTargetedLanding;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static VesselSituation()
		{
			throw null;
		}
	}

	public static class Contract
	{
		public static EventData<Contracts.Contract> onOffered;

		public static EventData<Contracts.Contract> onAccepted;

		public static EventData<Contracts.Contract> onDeclined;

		public static EventData<Contracts.Contract> onCompleted;

		public static EventData<Contracts.Contract> onFailed;

		public static EventData<Contracts.Contract> onCancelled;

		public static EventData<Contracts.Contract> onFinished;

		public static EventData<Contracts.Contract> onSeen;

		public static EventData<Contracts.Contract> onRead;

		public static EventData<Contracts.Contract, ContractParameter> onParameterChange;

		public static EventVoid onContractsLoaded;

		public static EventVoid onContractsListChanged;

		public static EventData<Vessel> onContractPreBuiltVesselSpawned;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Contract()
		{
			throw null;
		}
	}

	public static class Mission
	{
		public static EventData<Expansions.Missions.Mission> onStarted;

		public static EventData<Expansions.Missions.Mission> onFinished;

		public static EventData<Expansions.Missions.Mission> onCompleted;

		public static EventData<Expansions.Missions.Mission> onFailed;

		public static EventData<Expansions.Missions.Mission, FromToAction<MENode, MENode>> onActiveNodeChanging;

		public static EventData<Expansions.Missions.Mission, FromToAction<MENode, MENode>> onActiveNodeChanged;

		public static EventData<Expansions.Missions.Mission, Expansions.Missions.VesselSituation> onMissionCurrentVesselToBuildChanged;

		public static EventData<MENode> onNodeActivated;

		public static EventData<MENode> onNodeDeactivated;

		public static EventData<TestGroup> onTestGroupInitialized;

		public static EventData<TestGroup> onTestGroupCleared;

		public static EventData<TestModule> onTestInitialized;

		public static EventData<TestModule> onTestCleared;

		public static EventData<MENode, Vessel, Part, ProtoPartSnapshot> onNodeChangedVesselResources;

		public static EventVoid onMissionsLoaded;

		public static EventData<MissionBriefingDialog> onMissionBriefingCreated;

		public static EventData<Expansions.Missions.Mission> onMissionBriefingChanged;

		public static EventData<string> onMissionTagRemoved;

		public static EventVoid onMissionImported;

		public static EventVoid onMissionLoaded;

		public static EventVoid onLocalizationLockOverriden;

		public static EventVoid onFailureListChanged;

		public static EventVoid onVesselSituationChanged;

		public static EventVoid onMissionsBuilderPQSLoaded;

		public static EventData<MENode> onBuilderNodeAdded;

		public static EventData<MENode> onBuilderNodeDeleted;

		public static EventData<MENode> onBuilderNodeFocused;

		public static EventData<FromToAction<MENode, MENode>> onBuilderNodeConnection;

		public static EventData<FromToAction<MENode, MENode>> onBuilderNodeDisconnection;

		public static EventData<MENode> onBuilderNodeSelectionChanged;

		public static EventVoid onBuilderMissionDifficultyChanged;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Mission()
		{
			throw null;
		}
	}

	public class GameModifiers
	{
		public EventDataModifier<float, TransactionReasons> ReputationGain;

		public EventDataModifier<float, TransactionReasons> ScienceGain;

		public EventDataModifier<double, TransactionReasons> FundsGain;

		public EventData<CurrencyModifierQuery> OnCurrencyModifierQuery;

		public EventData<CurrencyModifierQuery> OnCurrencyModified;

		public EventData<ValueModifierQuery> onValueModifierQuery;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public GameModifiers()
		{
			throw null;
		}
	}

	public static class StageManager
	{
		public static EventData<bool> SortIcons;

		public static EventData<Part> OnPartUpdateStageability;

		public static EventVoid OnGUIStageSequenceModified;

		public static EventData<int> OnGUIStageAdded;

		public static EventData<int> OnGUIStageRemoved;

		public static EventVoid OnStagingSeparationIndices;

		[MethodImpl(MethodImplOptions.NoInlining)]
		static StageManager()
		{
			throw null;
		}
	}

	public class Input
	{
		public static EventData<bool> OnPrecisionModeToggle;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Input()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Input()
		{
			throw null;
		}
	}

	public class CommNet
	{
		public static EventVoid OnNetworkInitialized;

		public static EventData<Vessel, bool> OnCommStatusChange;

		public static EventData<Vessel, bool> OnCommHomeStatusChange;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CommNet()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static CommNet()
		{
			throw null;
		}
	}

	public static EventData<EventReport> onJointBreak;

	public static EventData<EventReport> onCrash;

	public static EventData<EventReport> onCrashSplashdown;

	public static EventData<EventReport> onCollision;

	public static EventData<EventReport> onOverheat;

	public static EventData<EventReport> onOverPressure;

	public static EventData<EventReport> onOverG;

	public static EventData<EventReport> onStageSeparation;

	public static EventData<HostedFromToAction<ProtoCrewMember, Part>> onCrewTransferred;

	public static EventData<HostedFromToAction<Part, List<Part>>> onCrewTransferPartListCreated;

	public static EventData<CrewTransfer.CrewTransferData> onCrewTransferSelected;

	public static EventData<PartItemTransfer> onItemTransferStarted;

	public static EventData<ProtoCrewMember, Part, CrewHatchController> onAttemptTransfer;

	public static EventData<FromToAction<Part, Part>> onCrewOnEva;

	public static EventData<ProtoCrewMember, Part, Transform> onAttemptEva;

	public static EventData<EventReport> onCrewKilled;

	public static EventData<ProtoCrewMember> onKerbalAdded;

	public static EventData<ProtoCrewMember> onKerbalAddComplete;

	public static EventData<ProtoCrewMember> onKerbalRemoved;

	public static EventData<ProtoCrewMember, string, string> onKerbalNameChange;

	public static EventData<ProtoCrewMember, string, string> onKerbalNameChanged;

	public static EventData<ProtoCrewMember, ProtoCrewMember.KerbalType, ProtoCrewMember.KerbalType> onKerbalTypeChange;

	public static EventData<ProtoCrewMember, ProtoCrewMember.RosterStatus, ProtoCrewMember.RosterStatus> onKerbalStatusChange;

	public static EventData<ProtoCrewMember, ProtoCrewMember.KerbalType, ProtoCrewMember.KerbalType> onKerbalTypeChanged;

	public static EventData<ProtoCrewMember, ProtoCrewMember.RosterStatus, ProtoCrewMember.RosterStatus> onKerbalStatusChanged;

	public static EventData<ProtoCrewMember, bool, bool> onKerbalInactiveChange;

	public static EventData<ProtoCrewMember> onKerbalLevelUp;

	public static EventData<ProtoCrewMember> onKerbalPassedOutFromGeeForce;

	public static EventData<FromToAction<Part, Part>> onCrewBoardVessel;

	public static EventData<EventReport> onLaunch;

	public static EventData<EventReport> onUndock;

	public static EventData<EventReport> onSplashDamage;

	public static EventData<Vessel, Vessel, CometVessel, CometVessel> onCometVesselChanged;

	public static EventData<Vessel> onVesselPrecalcAssign;

	public static EventData<Vessel> onNewVesselCreated;

	public static EventData<Vessel> onAsteroidSpawned;

	public static EventData<Vessel> onCometSpawned;

	public static EventData<Vessel> OnDiscoverableObjectExpired;

	public static EventData<Vessel> onSentinelCometDetected;

	public static EventData<CometVessel> onCometVFXDimensionsModified;

	public static EventData<Vessel> onVesselWillDestroy;

	public static EventData<Vessel> onVesselCreate;

	public static EventData<Vessel> onVesselDestroy;

	public static EventData<Vessel> onVesselExplodeGroundCollision;

	public static EventData<Vessel> onVesselChange;

	public static EventData<Vessel, Vessel> onVesselSwitching;

	public static EventData<Vessel, Vessel> onVesselSwitchingToUnloaded;

	public static EventData<Transform, Transform> onVesselReferenceTransformSwitch;

	public static EventData<HostedFromToAction<Vessel, Vessel.Situations>> onVesselSituationChange;

	public static EventData<Vessel, bool> onVesselControlStateChange;

	public static EventData<HostedFromToAction<IDiscoverable, DiscoveryLevels>> onKnowledgeChanged;

	public static EventData<HostedFromToAction<Vessel, string>> onVesselRename;

	public static EventData<Vessel> onVesselGoOnRails;

	public static EventData<Vessel> onVesselGoOffRails;

	public static EventData<Vessel> onPhysicsEaseStart;

	public static EventData<Vessel> onPhysicsEaseStop;

	public static EventData<Vessel> onVesselLoaded;

	public static EventData<Vessel> onVesselUnloaded;

	public static EventData<Vessel> onVesselStandardModification;

	public static EventData<Vessel> onVesselWasModified;

	public static EventData<Vessel> onVesselPartCountChanged;

	public static EventData<Vessel> onVesselCrewWasModified;

	public static EventData<HostedFromToAction<Vessel, CelestialBody>> onVesselSOIChanged;

	public static EventData<Vessel> onVesselOrbitClosed;

	public static EventData<Vessel> onVesselOrbitEscaped;

	public static EventData<Vessel> OnVesselRecoveryRequested;

	public static EventData<Vessel> OnVesselOverrideGroupChanged;

	public static EventData<ProtoVessel, bool> onVesselRecovered;

	public static EventData<ProtoVessel> onVesselTerminated;

	public static EventData<FromToAction<ModuleDockingNode, ModuleDockingNode>> onSameVesselDock;

	public static EventData<FromToAction<ModuleDockingNode, ModuleDockingNode>> onSameVesselUndock;

	public static EventData<PartModule, string, double> OnResourceConverterOutput;

	public static EventData<FlightUIMode> OnFlightUIModeChanged;

	public static EventData<FromToAction<ProtoVessel, ConfigNode>> onProtoVesselSave;

	public static EventData<FromToAction<ProtoVessel, ConfigNode>> onProtoVesselLoad;

	public static EventData<FromToAction<ProtoPartSnapshot, ConfigNode>> onProtoPartSnapshotSave;

	public static EventData<FromToAction<ProtoPartSnapshot, ConfigNode>> onProtoPartSnapshotLoad;

	public static EventData<FromToAction<ProtoPartModuleSnapshot, ConfigNode>> onProtoPartModuleSnapshotSave;

	public static EventData<FromToAction<ProtoPartModuleSnapshot, ConfigNode>> onProtoPartModuleSnapshotLoad;

	public static EventData<Part> onPartCrossfeedStateChange;

	public static EventData<Part> onPartResourceListChange;

	public static EventData<Part> onPartPriorityChanged;

	public static EventData<HostedFromToAction<bool, Part>> onPartFuelLookupStateChange;

	public static EventData<HostedFromToAction<PartResource, bool>> onPartResourceFlowStateChange;

	public static EventData<HostedFromToAction<PartResource, PartResource.FlowMode>> onPartResourceFlowModeChange;

	public static EventData<PartResource> onPartResourceEmptyNonempty;

	public static EventData<PartResource> onPartResourceNonemptyEmpty;

	public static EventData<PartResource> onPartResourceEmptyFull;

	public static EventData<PartResource> onPartResourceFullEmpty;

	public static EventData<PartResource> onPartResourceFullNonempty;

	public static EventData<PartResource> onPartResourceNonemptyFull;

	public static EventData<uint, uint> onVesselPersistentIdChanged;

	public static EventData<uint, uint, uint> onPartPersistentIdChanged;

	public static EventData<Vessel, Vessel> onVesselsUndocking;

	internal static EventData<Vessel> onFlightGlobalsAddVessel;

	internal static EventData<Vessel> onFlightGlobalsRemoveVessel;

	public static EventData<int> onStageActivate;

	public static EventData<FromToAction<CelestialBody, CelestialBody>> onDominantBodyChange;

	public static EventData<Vessel> onVesselClearStaging;

	public static EventData<Vessel> onVesselResumeStaging;

	public static EventData<Part> onFairingsDeployed;

	public static EventData<Vessel, PatchedConicSolver> onManeuversLoaded;

	public static EventData<Vessel, PatchedConicSolver> onManeuverAdded;

	public static EventData<Vessel, PatchedConicSolver> onManeuverRemoved;

	public static EventData<Vector3d> onKrakensbaneEngage;

	public static EventData<Vector3d> onKrakensbaneDisengage;

	public static EventData<Vector3d, Vector3d> onFloatingOriginShift;

	public static EventData<HostTargetAction<CelestialBody, bool>> onRotatingFrameTransition;

	public static EventVoid onGamePause;

	public static EventVoid onGameUnpause;

	public static EventVoid onFlightReady;

	public static EventVoid onTimeWarpRateChanged;

	public static EventData<GameScenes> onGameSceneLoadRequested;

	public static EventData<FromToAction<GameScenes, GameScenes>> onGameSceneSwitchRequested;

	public static EventData<GameScenes> onLevelWasLoaded;

	public static EventData<GameScenes> onLevelWasLoadedGUIReady;

	public static EventData<GameScenes> onSceneConfirmExit;

	public static EventData<int, int> onScreenResolutionModified;

	public static EventVoid OnGameDatabaseLoaded;

	public static EventVoid OnUpgradesFilled;

	public static EventVoid OnUpgradesLinked;

	public static EventVoid OnPartLoaderLoaded;

	public static EventData<MapObject> onPlanetariumTargetChanged;

	public static EventVoid OnMapEntered;

	public static EventVoid OnMapExited;

	public static EventData<MapViewFiltering.VesselTypeFilter> OnMapViewFiltersModified;

	public static EventData<LaunchSite> LaunchSiteFound;

	public static EventData<Game> onGameStateSaved;

	public static EventData<Game> onGameStateCreated;

	public static EventData<ConfigNode> onGameStateSave;

	public static EventData<ConfigNode> onGameStateLoad;

	public static EventVoid onGameNewStart;

	public static EventVoid onGameAboutToQuicksave;

	public static EventData<ConfigNode> onGameStatePostLoad;

	public static EventData<FromToAction<ControlTypes, ControlTypes>> onInputLocksModified;

	public static EventVoid OnGameSettingsApplied;

	public static EventVoid OnGameSettingsWritten;

	public static EventVoid OnScenerySettingChanged;

	public static EventData<bool> OnAppFocus;

	public static EventData<bool> OnFlightGlobalsReady;

	public static EventVoid onLanguageSwitched;

	public static EventVoid OnExpansionSystemLoaded;

	public static EventData<FlightState> OnRevertToLaunchFlightState;

	public static EventData<FlightState> OnRevertToPrelaunchFlightState;

	public static EventData<string, string> OnDifficultySettingTabChanging;

	public static EventData<DifficultyOptionsMenu> OnDifficultySettingsShown;

	public static EventData<DifficultyOptionsMenu, bool> OnDifficultySettingsDismiss;

	public static EventData<Part> onPartPack;

	public static EventData<Part> onPartUnpack;

	public static EventData<ExplosionReaction> onPartExplode;

	public static EventData<Part> onPartDie;

	public static EventData<Part> onPartWillDie;

	public static EventData<Part> onPartExplodeGroundCollision;

	public static EventData<Part> onPartDestroyed;

	public static EventData<PartJoint, float> onPartJointBreak;

	public static EventData<PartJoint> onPartJointSet;

	public static EventData<Part> onPartUndock;

	public static EventData<Part> onPartUndockComplete;

	public static EventData<FromToAction<Part, Part>> onPartCouple;

	public static EventData<FromToAction<Part, Part>> onPartCoupleComplete;

	public static EventData<Part> onPartDeCouple;

	public static EventData<Part> onPartDeCoupleComplete;

	public static EventData<Vessel, Vessel> onPartDeCoupleNewVesselComplete;

	public static EventData<uint, uint> onVesselDocking;

	public static EventData<FromToAction<Part, Part>> onDockingComplete;

	public static EventData<HostTargetAction<Part, Part>> onPartAttach;

	public static EventData<HostTargetAction<Part, Part>> onPartRemove;

	public static EventData<UIPartActionWindow, Part> onPartActionUIShown;

	public static EventData<Part> onPartActionInitialized;

	public static EventData<Part> onPartActionUICreate;

	public static EventData<Part> onPartActionUIDismiss;

	public static EventData<bool> onPartActionNumericSlider;

	public static EventData<Part, RaycastHit> OnCollisionEnhancerHit;

	public static EventData<Part> onPartFailure;

	public static EventData<Part> onPartRepair;

	public static EventData<PartModule, AdjusterPartModuleBase> onPartModuleAdjusterAdded;

	public static EventData<PartModule, AdjusterPartModuleBase> onPartModuleAdjusterRemoved;

	public static EventData<ProtoPartSnapshot> onProtoPartFailure;

	public static EventData<ProtoPartSnapshot> onProtoPartRepair;

	public static EventData<ProtoPartModuleSnapshot> onProtoPartModuleAdjusterAdded;

	public static EventData<ProtoPartModuleSnapshot> onProtoPartModuleAdjusterRemoved;

	public static EventData<ProtoPartModuleSnapshot> onProtoPartModuleRepaired;

	public static EventData<Part> onPartVesselNamingChanged;

	public static EventData<AvailablePart> onVariantsAdded;

	public static EventData<Part, PartVariant> onVariantApplied;

	public static EventData<Part, ControlPoint> OnControlPointChanged;

	public static EventData<Part, PhysicMaterial> OnPhysicMaterialChanged;

	public static EventData<Part> onPartRepaired;

	public static EventData<Part, ModuleLight> onLightsOn;

	public static EventData<Part, ModuleLight> onLightsOff;

	public static EventData<BaseConverter, Part, double> OnEfficiencyChange;

	public static EventVoid OnResourceMapLoaded;

	public static EventData<ModuleAnimationGroup, bool> OnAnimationGroupStateChanged;

	public static EventData<ModuleAnimationGroup> OnAnimationGroupRetractComplete;

	public static EventData<Vessel, CelestialBody> OnOrbitalSurveyCompleted;

	public static EventData<string> onFlagSelect;

	public static EventData<string> onMissionFlagSelect;

	public static EventData<KerbalEVA, bool> onCommandSeatInteractionEnter;

	public static EventData<KerbalEVA, bool> onCommandSeatInteraction;

	public static EventData<KerbalEVA, Part> onPartLadderEnter;

	public static EventData<KerbalEVA, Part> onPartLadderExit;

	public static EventData<Vessel> onFlagPlant;

	public static EventData<FlagSite> afterFlagPlanted;

	public static EventData<Part> onWheelRepaired;

	public static EventData<Vessel> onActiveJointNeedUpdate;

	public static EventData<PQS> OnPQSStarting;

	public static EventData<PQSCity> OnPQSCityStarting;

	public static EventData<CelestialBody, string> OnPQSCityLoaded;

	public static EventData<CelestialBody, string> OnPQSCityOrientated;

	public static EventData<CelestialBody, string> OnPQSCityUnloaded;

	public static EventData<FromToAction<Waypoint, ConfigNode>> onCustomWaypointSave;

	public static EventData<FromToAction<Waypoint, ConfigNode>> onCustomWaypointLoad;

	public static EventData<CelestialBody, string> OnPOIRangeEntered;

	public static EventData<CelestialBody, string> OnPOIRangeExited;

	public static EventData<MultiModeEngine> onMultiModeEngineSwitchActive;

	public static EventData<ModuleEngines> onEngineActiveChange;

	public static EventVoid onDeltaVCalcsCompleted;

	public static EventData<ModuleEngines> onChangeEngineDVIncludeState;

	public static EventData<ModuleEngines> onEngineThrustPercentageChanged;

	public static EventData<DeltaVSituationOptions> onDeltaVAppAtmosphereChanged;

	public static EventVoid onDeltaVAppInfoItemsChanged;

	public static EventVoid onTooltipDestroyRequested;

	public static EventVoid onGUILock;

	public static EventVoid onGUIUnlock;

	public static EventData<VesselSpawnInfo> onGUILaunchScreenSpawn;

	public static EventVoid onGUILaunchScreenDespawn;

	public static EventVoid onManeuverNodeSelected;

	public static EventVoid onManeuverNodeDeselected;

	public static EventData<ShipTemplate> onGUILaunchScreenVesselSelected;

	public static EventVoid onGUIAstronautComplexSpawn;

	public static EventVoid onGUIAstronautComplexDespawn;

	public static EventVoid onGUIRnDComplexSpawn;

	public static EventVoid onGUIRnDComplexDespawn;

	public static EventVoid onGUIMissionControlSpawn;

	public static EventVoid onGUIMissionControlDespawn;

	public static EventVoid onGUIAdministrationFacilitySpawn;

	public static EventVoid onGUIAdministrationFacilityDespawn;

	public static EventData<KSCFacilityContextMenu> onFacilityContextMenuSpawn;

	public static EventData<KSCFacilityContextMenu> onFacilityContextMenuDespawn;

	public static EventDataModifier<bool, ITooltipController> onTooltipAboutToSpawn;

	public static EventData<ITooltipController, Tooltip> onTooltipSpawned;

	public static EventDataModifier<bool, Tooltip> onTooltipUpdate;

	public static EventDataModifier<bool, ITooltipController> onTooltipAboutToDespawn;

	public static EventData<Tooltip> onTooltipDespawned;

	public static EventData<MissionRecoveryDialog> onGUIRecoveryDialogSpawn;

	public static EventData<MissionRecoveryDialog> onGUIRecoveryDialogDespawn;

	public static EventVoid onGUIKSPediaSpawn;

	public static EventVoid onGUIKSPediaDespawn;

	public static EventVoid onHideUI;

	public static EventVoid onShowUI;

	public static EventVoid onUIScaleChange;

	public static EventData<MenuNavInput> onMenuNavGetInput;

	[Obsolete]
	public static EventVoid onGUIPrefabLauncherReady;

	public static EventVoid onGUIApplicationLauncherReady;

	public static EventData<GameScenes> onGUIApplicationLauncherUnreadifying;

	public static EventVoid onGUIApplicationLauncherDestroyed;

	public static EventVoid onGUIMessageSystemReady;

	public static EventVoid onGUIEditorToolbarReady;

	public static EventVoid onGUIEngineersReportReady;

	public static EventVoid onGUIEngineersReportDestroy;

	public static EventVoid onGUIDeltaVAppReady;

	public static EventVoid onGUIDeltaVAppDestroy;

	public static EventVoid onGUIManeuverToolReady;

	public static EventVoid onGUIManeuverToolDestroy;

	public static EventData<FlightGlobals.SpeedDisplayModes> onSetSpeedMode;

	public static EventVoid onGUIActionGroupFlightShowing;

	public static EventVoid onGUIActionGroupFlightShown;

	public static EventVoid onGUIActionGroupFlightClosed;

	public static EventVoid onGUIActionGroupShowing;

	public static EventVoid onGUIActionGroupShown;

	public static EventVoid onGUIActionGroupClosed;

	public static EventData<ShipConstruct> onEditorShipModified;

	public static EventData<HostedFromToAction<ShipConstruct, string>> onEditorVesselNamingChanged;

	public static EventData<EditorScreen> onEditorScreenChange;

	public static EventData<SymmetryMethod> onEditorSymmetryMethodChange;

	public static EventData<Space> onEditorSymmetryCoordsChange;

	public static EventData<int> onEditorSymmetryModeChange;

	public static EventData<bool> onEditorSnapModeChange;

	public static EventData<ConstructionMode> onEditorConstructionModeChange;

	public static EventData<ConstructionEventType, Part> onEditorPartEvent;

	public static EventVoid onEditorShowPartList;

	public static EventData<ShipConstruct> onEditorUndo;

	public static EventData<ShipConstruct> onEditorRedo;

	public static EventData<ShipConstruct> onEditorSetBackup;

	public static EventVoid onEditorRestart;

	public static EventData<ShipConstruct, CraftBrowserDialog.LoadType> onEditorLoad;

	public static EventVoid onEditorStarted;

	public static EventData<Part> onEditorPodPicked;

	public static EventData<Part> onEditorPodSelected;

	public static EventData<ShipConstruct> onAboutToSaveShip;

	public static EventData<Part> onEditorPartPicked;

	public static EventData<Part> onEditorPartPlaced;

	public static EventData<CompoundPart> onEditorCompoundPartLinked;

	public static EventData<CompoundPart> OnFlightCompoundPartLinked;

	public static EventData<CompoundPart> OnFlightCompoundPartDetached;

	public static EventVoid onEditorPodDeleted;

	public static EventData<Part> onEditorPartDeleted;

	public static EventVoid onEditorRestoreState;

	public static EventVoid onEditorNewShipDialogDismiss;

	public static EventData<Part, PartVariant> onEditorVariantApplied;

	public static EventData<AvailablePart, PartVariant> onEditorDefaultVariantChanged;

	public static EventData<VesselCrewManifest> onEditorShipCrewModified;

	public static EventData<ComboSelector, string, int> onSuitComboSelection;

	public static EventData<SuitLightColorPicker> onClickSuitLightButton;

	public static EventData<SuitButton> onClickHelmetNeckringButton;

	public static EventData<HostTargetAction<RDTech, RDTech.OperationResult>> OnTechnologyResearched;

	public static EventData<AvailablePart> OnPartPurchased;

	public static EventData<PartUpgradeHandler.Upgrade> OnPartUpgradePurchased;

	public static EventData<ShipConstruct> OnVesselRollout;

	public static EventData<ProgressNode> OnProgressReached;

	public static EventData<ProgressNode> OnProgressComplete;

	public static EventData<ProgressNode> OnProgressAchieved;

	public static EventData<FromToAction<ProgressNode, ConfigNode>> onProgressNodeSave;

	public static EventData<FromToAction<ProgressNode, ConfigNode>> onProgressNodeLoad;

	public static EventData<float, ScienceSubject, ProtoVessel, bool> OnScienceRecieved;

	public static EventData<ScienceData, Vessel, bool> OnTriggeredDataTransmission;

	public static EventData<ScienceData> OnExperimentDeployed;

	public static EventData<ScienceData> OnExperimentStored;

	public static EventData<ScienceData> OnROCExperimentStored;

	public static EventData<ScienceData> OnROCExperimentReset;

	public static EventData<Vessel> OnFlightLogRecorded;

	public static EventData<float, TransactionReasons> OnReputationChanged;

	public static EventData<float, TransactionReasons> OnScienceChanged;

	public static EventData<double, TransactionReasons> OnFundsChanged;

	public static EventData<ProtoCrewMember, int> OnCrewmemberHired;

	public static EventData<ProtoCrewMember, int> OnCrewmemberSacked;

	public static EventData<ProtoCrewMember, int> OnCrewmemberLeftForDead;

	public static EventData<FromToAction<ProtoCrewMember, ConfigNode>> onProtoCrewMemberSave;

	public static EventData<FromToAction<ProtoCrewMember, ConfigNode>> onProtoCrewMemberLoad;

	public static EventData<ProtoVessel, MissionRecoveryDialog, float> onVesselRecoveryProcessing;

	public static EventData<ProtoVessel, MissionRecoveryDialog, float> onVesselRecoveryProcessingComplete;

	public static EventData<string> onDeployGroundPart;

	public static EventData<ModuleGroundSciencePart> onGroundSciencePartDeployed;

	public static EventData<ModuleGroundSciencePart> onGroundSciencePartRemoved;

	public static EventData<ModuleGroundSciencePart> onGroundSciencePartChanged;

	public static EventData<ModuleGroundSciencePart> onGroundSciencePartEnabledStateChanged;

	public static EventData<ModuleGroundExpControl, bool, List<ModuleGroundSciencePart>> onGroundScienceControllerChanged;

	public static EventData<ModuleGroundExperiment> onGroundScienceExperimentScienceValueChanged;

	public static EventData<ModuleGroundExperiment> onGroundScienceExperimentScienceLimitChanged;

	public static EventData<ModuleGroundExpControl, List<ModuleGroundSciencePart>> onGroundScienceRegisterCluster;

	public static EventData<ModuleGroundExpControl, DeployedScienceCluster> onGroundScienceClusterRegistered;

	public static EventData<uint> onGroundScienceDeregisterCluster;

	public static EventData<ModuleGroundExpControl, DeployedScienceCluster> onGroundScienceClusterUpdated;

	public static EventData<DeployedScienceCluster> onGroundScienceClusterPowerStateChanged;

	public static EventData<DeployedScienceExperiment, DeployedSciencePart, DeployedScienceCluster, float> onGroundScienceGenerated;

	public static EventData<DeployedScienceExperiment, DeployedSciencePart, DeployedScienceCluster, float> onGroundScienceTransmitted;

	public static EventData<ModuleInventoryPart> onModuleInventoryChanged;

	public static EventData<ModuleInventoryPart, int> onModuleInventorySlotChanged;

	public static EventVoid OnDebugROCFinderToggled;

	public static EventVoid OnDebugROCScanPointsToggled;

	public static EventData<ModuleRoboticController> onRoboticControllerAxesChanged;

	public static EventData<ModuleRoboticController, ControlledAxis> onRoboticControllerAxesAdding;

	public static EventData<ModuleRoboticController, ControlledAxis> onRoboticControllerAxesRemoving;

	public static EventData<ModuleRoboticController> onRoboticControllerActionsChanged;

	public static EventData<ModuleRoboticController, ControlledAction> onRoboticControllerActionsAdding;

	public static EventData<ModuleRoboticController, ControlledAction> onRoboticControllerActionsRemoving;

	public static EventData<ModuleRoboticController> onRoboticControllerSequencePlayed;

	public static EventData<ModuleRoboticController> onRoboticControllerSequenceStopped;

	public static EventData<ModuleRoboticController, ModuleRoboticController.SequenceDirectionOptions> onRoboticControllerSequenceDirectionChanged;

	public static EventData<ModuleRoboticController, ModuleRoboticController.SequenceLoopOptions> onRoboticControllerSequenceLoopModeChanged;

	public static EventData<Part, bool> onRoboticPartLockChanging;

	public static EventData<Part, bool> onRoboticPartLockChanged;

	public static GameModifiers Modifiers;

	public static EventData<DestructibleBuilding> OnKSCStructureCollapsing;

	public static EventData<DestructibleBuilding> OnKSCStructureCollapsed;

	public static EventData<DestructibleBuilding> OnKSCStructureRepairing;

	public static EventData<DestructibleBuilding> OnKSCStructureRepaired;

	public static EventData<UpgradeableFacility, int> OnKSCFacilityUpgrading;

	public static EventData<UpgradeableFacility, int> OnKSCFacilityUpgraded;

	public static EventData<UpgradeableObject, int> OnUpgradeableObjLevelChange;

	public static EventData<AltimeterDisplayState> OnAltimeterDisplayModeToggle;

	public static EventData<CameraManager.CameraMode> OnCameraChange;

	public static EventVoid OnFlightCameraModeChange;

	public static EventData<FlightCamera.Modes> OnFlightCameraAngleChange;

	public static EventData<Kerbal> OnIVACameraKerbalChange;

	public static EventData<Vessel, float, Vector3> OnCameraDistanceAdjustedToFitVessel;

	public static EventData<MapObject> OnMapFocusChange;

	public static EventData<MapObject> OnTargetObjectChanged;

	public static EventVoid OnCollisionIgnoreUpdate;

	public static EventData<KerbalEVA, bool, bool> OnHelmetChanged;

	public static EventData<PhysicMaterial> onGlobalEvaPhysicMaterialChanged;

	public static EventData<ConstructionMode> OnEVAConstructionModeChanged;

	public static EventData<bool> OnEVAConstructionMode;

	public static EventData<bool> OnEVACargoMode;

	public static EventData<KerbalEVA> OnEVAConstructionWeldStart;

	public static EventData<KerbalEVA> OnEVAConstructionWeldFinish;

	public static EventData<Vessel, Part> OnEVAConstructionModePartAttached;

	public static EventData<Vessel, Part> OnEVAConstructionModePartDetached;

	public static EventData<Part> OnInventoryPartOnMouseChanged;

	public static EventVoid OnCombinedConstructionWeightLimitChanged;

	public static EventData<AlarmTypeBase> onAlarmAdded;

	public static EventData<AlarmTypeBase> onAlarmRemoving;

	public static EventData<uint> onAlarmRemoved;

	public static EventData<AlarmTypeBase> onAlarmTriggered;

	public static EventData<AlarmTypeBase> onAlarmActioned;

	public static EventData<Vessel> onAlarmAvailableVesselChanged;

	public static EventData<bool> onAlarmAppTimeDisplayChanged;

	public static EventData<KerbalEVA> OnVisorLowering;

	public static EventData<KerbalEVA> OnVisorLowered;

	public static EventData<KerbalEVA> OnVisorRaising;

	public static EventData<KerbalEVA> OnVisorRaised;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static GameEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T FindEvent<T>(string eventName) where T : BaseGameEvent
	{
		throw null;
	}
}
