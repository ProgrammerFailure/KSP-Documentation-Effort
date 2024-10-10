using System;
using System.Collections.Generic;
using Contracts;
using Expansions.Missions;
using Expansions.Missions.Adjusters;
using Expansions.Missions.Editor;
using Expansions.Serenity;
using Expansions.Serenity.DeployedScience.Runtime;
using FinePrint;
using KSPAchievements;
using ns11;
using ns2;
using UnityEngine;
using Upgradeables;

public class GameEvents : GameEventsBase
{
	public struct ExplosionReaction
	{
		public float distance;

		public float magnitude;

		public ExplosionReaction(float distance, float magnitude)
		{
			this.distance = distance;
			this.magnitude = magnitude;
		}
	}

	public struct HostTargetAction<T, U>
	{
		public T host;

		public U target;

		public HostTargetAction(T host, U target)
		{
			this.host = host;
			this.target = target;
		}
	}

	public struct FromToAction<T, U>
	{
		public T from;

		public U to;

		public FromToAction(T from, U to)
		{
			this.from = from;
			this.to = to;
		}
	}

	public struct HostedFromToAction<T, U>
	{
		public T host;

		public U from;

		public U to;

		public HostedFromToAction(T host, U from, U to)
		{
			this.host = host;
			this.from = from;
			this.to = to;
		}
	}

	public struct VesselSpawnInfo
	{
		public string craftSubfolder;

		public string profileName;

		public VesselSpawnDialog.CraftSelectedCallback OnFileSelected;

		public LaunchSiteFacility callingFacility;

		public VesselSpawnInfo(string craftSubfolder, string profileName, VesselSpawnDialog.CraftSelectedCallback OnFileSelected, LaunchSiteFacility callingFacility)
		{
			this.craftSubfolder = craftSubfolder;
			this.profileName = profileName;
			this.OnFileSelected = OnFileSelected;
			this.callingFacility = callingFacility;
		}
	}

	public static class VesselSituation
	{
		public static EventData<Vessel, CelestialBody> onLand = new EventData<Vessel, CelestialBody>("VesselSituation.OnLand");

		public static EventData<Vessel, CelestialBody> onOrbit = new EventData<Vessel, CelestialBody>("VesselSituation.OnOrbit");

		public static EventData<Vessel, CelestialBody> onFlyBy = new EventData<Vessel, CelestialBody>("VesselSituation.OnFlyBy");

		public static EventData<Vessel, CelestialBody> onReturnFromSurface = new EventData<Vessel, CelestialBody>("VesselSituation.OnReturnFromSurface");

		public static EventData<Vessel, CelestialBody> onReturnFromOrbit = new EventData<Vessel, CelestialBody>("VesselSituation.OnReturnFromOrbit");

		public static EventData<Vessel, CelestialBody> onEscape = new EventData<Vessel, CelestialBody>("VesselSituation.OnEscape");

		public static EventData<Vessel> onReachSpace = new EventData<Vessel>("VesselSituation.OnReachSpace");

		public static EventData<Vessel> onLaunch = new EventData<Vessel>("VesselSituation.OnLaunch");

		public static EventData<Vessel, string, ReturnFrom> onTargetedLanding = new EventData<Vessel, string, ReturnFrom>("VesselSituation.OnTargetedLanding");
	}

	public static class Contract
	{
		public static EventData<Contracts.Contract> onOffered = new EventData<Contracts.Contract>("Contract.Generated");

		public static EventData<Contracts.Contract> onAccepted = new EventData<Contracts.Contract>("Contract.Accepted");

		public static EventData<Contracts.Contract> onDeclined = new EventData<Contracts.Contract>("Contract.Declined");

		public static EventData<Contracts.Contract> onCompleted = new EventData<Contracts.Contract>("Contract.Completed");

		public static EventData<Contracts.Contract> onFailed = new EventData<Contracts.Contract>("Contract.Failed");

		public static EventData<Contracts.Contract> onCancelled = new EventData<Contracts.Contract>("Contract.Cancelled");

		public static EventData<Contracts.Contract> onFinished = new EventData<Contracts.Contract>("Contract.Finished");

		public static EventData<Contracts.Contract> onSeen = new EventData<Contracts.Contract>("Contract.Seen");

		public static EventData<Contracts.Contract> onRead = new EventData<Contracts.Contract>("Contract.Read");

		public static EventData<Contracts.Contract, ContractParameter> onParameterChange = new EventData<Contracts.Contract, ContractParameter>("Contract.ParameterChange");

		public static EventVoid onContractsLoaded = new EventVoid("onContractsLoaded");

		public static EventVoid onContractsListChanged = new EventVoid("onContractsListChanged");

		public static EventData<Vessel> onContractPreBuiltVesselSpawned = new EventData<Vessel>("onContractPRebuiltVesselSpawned");
	}

	public static class Mission
	{
		public static EventData<Expansions.Missions.Mission> onStarted = new EventData<Expansions.Missions.Mission>("Mission.Started");

		public static EventData<Expansions.Missions.Mission> onFinished = new EventData<Expansions.Missions.Mission>("Mission.Finished");

		public static EventData<Expansions.Missions.Mission> onCompleted = new EventData<Expansions.Missions.Mission>("Mission.Completed");

		public static EventData<Expansions.Missions.Mission> onFailed = new EventData<Expansions.Missions.Mission>("Mission.Failed");

		public static EventData<Expansions.Missions.Mission, FromToAction<MENode, MENode>> onActiveNodeChanging = new EventData<Expansions.Missions.Mission, FromToAction<MENode, MENode>>("Active Node Changed");

		public static EventData<Expansions.Missions.Mission, FromToAction<MENode, MENode>> onActiveNodeChanged = new EventData<Expansions.Missions.Mission, FromToAction<MENode, MENode>>("Active Node Changed");

		public static EventData<Expansions.Missions.Mission, Expansions.Missions.VesselSituation> onMissionCurrentVesselToBuildChanged = new EventData<Expansions.Missions.Mission, Expansions.Missions.VesselSituation>("onMissionCurrentVesselToBuildChanged");

		public static EventData<MENode> onNodeActivated = new EventData<MENode>("Mission.Node.Activated");

		public static EventData<MENode> onNodeDeactivated = new EventData<MENode>("Mission.Node.Deactivated");

		public static EventData<TestGroup> onTestGroupInitialized = new EventData<TestGroup>("Mission.TestGroup.Activated");

		public static EventData<TestGroup> onTestGroupCleared = new EventData<TestGroup>("Mission.TestGroup.Deactivated");

		public static EventData<TestModule> onTestInitialized = new EventData<TestModule>("Mission.Test.Activated");

		public static EventData<TestModule> onTestCleared = new EventData<TestModule>("Mission.Test.Deactivated");

		public static EventData<MENode, Vessel, Part, ProtoPartSnapshot> onNodeChangedVesselResources = new EventData<MENode, Vessel, Part, ProtoPartSnapshot>("Mission.onNodeChangedVesselResources");

		public static EventVoid onMissionsLoaded = new EventVoid("onMissionsLoaded");

		public static EventData<MissionBriefingDialog> onMissionBriefingCreated = new EventData<MissionBriefingDialog>("onMissionBriefingCreated");

		public static EventData<Expansions.Missions.Mission> onMissionBriefingChanged = new EventData<Expansions.Missions.Mission>("onMissionBriefingChanged");

		public static EventData<string> onMissionTagRemoved = new EventData<string>("onMissionTagRemoved");

		public static EventVoid onMissionImported = new EventVoid("onMissionImported");

		public static EventVoid onMissionLoaded = new EventVoid("onMissionLoaded");

		public static EventVoid onLocalizationLockOverriden = new EventVoid("onLocalizationLockOverriden");

		public static EventVoid onFailureListChanged = new EventVoid("onFailureListChanged");

		public static EventVoid onVesselSituationChanged = new EventVoid("onVesselSituationChanged");

		public static EventVoid onMissionsBuilderPQSLoaded = new EventVoid("onMissionsEditorPQSLoaded");

		public static EventData<MENode> onBuilderNodeAdded = new EventData<MENode>("MissionBuilder.Node.Added");

		public static EventData<MENode> onBuilderNodeDeleted = new EventData<MENode>("MissionBuilder.Node.Deleted");

		public static EventData<MENode> onBuilderNodeFocused = new EventData<MENode>("MissionBuilder.Node.Focused");

		public static EventData<FromToAction<MENode, MENode>> onBuilderNodeConnection = new EventData<FromToAction<MENode, MENode>>("MissionBuilder.Node.Connection");

		public static EventData<FromToAction<MENode, MENode>> onBuilderNodeDisconnection = new EventData<FromToAction<MENode, MENode>>("MissionBuilder.Node.Disconnection");

		public static EventData<MENode> onBuilderNodeSelectionChanged = new EventData<MENode>("MissionBuilder.Node.SelectionChanged");

		public static EventVoid onBuilderMissionDifficultyChanged = new EventVoid("MissionBuilder.DifficultyChanged");
	}

	public class GameModifiers
	{
		public EventDataModifier<float, TransactionReasons> ReputationGain = new EventDataModifier<float, TransactionReasons>("Modify.ReputationGain");

		public EventDataModifier<float, TransactionReasons> ScienceGain = new EventDataModifier<float, TransactionReasons>("Modify.ScienceGain");

		public EventDataModifier<double, TransactionReasons> FundsGain = new EventDataModifier<double, TransactionReasons>("Modify.FundsGain");

		public EventData<CurrencyModifierQuery> OnCurrencyModifierQuery = new EventData<CurrencyModifierQuery>("OnCurrencyModifierQuery");

		public EventData<CurrencyModifierQuery> OnCurrencyModified = new EventData<CurrencyModifierQuery>("OnCurrencyModified");

		public EventData<ValueModifierQuery> onValueModifierQuery = new EventData<ValueModifierQuery>("onValueModifierQuery");
	}

	public static class StageManager
	{
		public static EventData<bool> SortIcons = new EventData<bool>("StageManager.SortIcons");

		public static EventData<Part> OnPartUpdateStageability = new EventData<Part>("OnPartUpdateStageability");

		public static EventVoid OnGUIStageSequenceModified = new EventVoid("OnGUIStageSequenceModified");

		public static EventData<int> OnGUIStageAdded = new EventData<int>("OnGUIStageAdded");

		public static EventData<int> OnGUIStageRemoved = new EventData<int>("OnGUIStageRemoved");

		public static EventVoid OnStagingSeparationIndices = new EventVoid("OnStagingSeparationIndices");
	}

	public class Input
	{
		public static EventData<bool> OnPrecisionModeToggle = new EventData<bool>("OnPrecisionModeToggle");
	}

	public class CommNet
	{
		public static EventVoid OnNetworkInitialized = new EventVoid("OnNetworkInitialised");

		public static EventData<Vessel, bool> OnCommStatusChange = new EventData<Vessel, bool>("OnVesselCommStatusChange");

		public static EventData<Vessel, bool> OnCommHomeStatusChange = new EventData<Vessel, bool>("OnVesselCommHomeStatusChange");
	}

	public static EventData<EventReport> onJointBreak = new EventData<EventReport>("onJointBreak");

	public static EventData<EventReport> onCrash = new EventData<EventReport>("onCrash");

	public static EventData<EventReport> onCrashSplashdown = new EventData<EventReport>("onCrashSplashdown");

	public static EventData<EventReport> onCollision = new EventData<EventReport>("onCollision");

	public static EventData<EventReport> onOverheat = new EventData<EventReport>("onOverheat");

	public static EventData<EventReport> onOverPressure = new EventData<EventReport>("onOverPressure");

	public static EventData<EventReport> onOverG = new EventData<EventReport>("onOverG");

	public static EventData<EventReport> onStageSeparation = new EventData<EventReport>("onStageSeparation");

	public static EventData<HostedFromToAction<ProtoCrewMember, Part>> onCrewTransferred = new EventData<HostedFromToAction<ProtoCrewMember, Part>>("onCrewTransferred");

	public static EventData<HostedFromToAction<Part, List<Part>>> onCrewTransferPartListCreated = new EventData<HostedFromToAction<Part, List<Part>>>("onCrewTransferPartListCreated");

	public static EventData<CrewTransfer.CrewTransferData> onCrewTransferSelected = new EventData<CrewTransfer.CrewTransferData>("onCrewTransferSelected");

	public static EventData<PartItemTransfer> onItemTransferStarted = new EventData<PartItemTransfer>("onItemTransferStarted");

	public static EventData<ProtoCrewMember, Part, CrewHatchController> onAttemptTransfer = new EventData<ProtoCrewMember, Part, CrewHatchController>("onAttemptTransfer");

	public static EventData<FromToAction<Part, Part>> onCrewOnEva = new EventData<FromToAction<Part, Part>>("onCrewOnEva");

	public static EventData<ProtoCrewMember, Part, Transform> onAttemptEva = new EventData<ProtoCrewMember, Part, Transform>("onAttemptEva");

	public static EventData<EventReport> onCrewKilled = new EventData<EventReport>("onCrewKilled");

	public static EventData<ProtoCrewMember> onKerbalAdded = new EventData<ProtoCrewMember>("onKerbalCreated");

	public static EventData<ProtoCrewMember> onKerbalAddComplete = new EventData<ProtoCrewMember>("onKerbalCreatedComplete");

	public static EventData<ProtoCrewMember> onKerbalRemoved = new EventData<ProtoCrewMember>("onKerbalRemoved");

	public static EventData<ProtoCrewMember, string, string> onKerbalNameChange = new EventData<ProtoCrewMember, string, string>("onKerbalNameChange");

	public static EventData<ProtoCrewMember, string, string> onKerbalNameChanged = new EventData<ProtoCrewMember, string, string>("onKerbalNameChanged");

	public static EventData<ProtoCrewMember, ProtoCrewMember.KerbalType, ProtoCrewMember.KerbalType> onKerbalTypeChange = new EventData<ProtoCrewMember, ProtoCrewMember.KerbalType, ProtoCrewMember.KerbalType>("onKerbalTypeChange");

	public static EventData<ProtoCrewMember, ProtoCrewMember.RosterStatus, ProtoCrewMember.RosterStatus> onKerbalStatusChange = new EventData<ProtoCrewMember, ProtoCrewMember.RosterStatus, ProtoCrewMember.RosterStatus>("onKerbalStatusChange");

	public static EventData<ProtoCrewMember, ProtoCrewMember.KerbalType, ProtoCrewMember.KerbalType> onKerbalTypeChanged = new EventData<ProtoCrewMember, ProtoCrewMember.KerbalType, ProtoCrewMember.KerbalType>("onKerbalTypeChanged");

	public static EventData<ProtoCrewMember, ProtoCrewMember.RosterStatus, ProtoCrewMember.RosterStatus> onKerbalStatusChanged = new EventData<ProtoCrewMember, ProtoCrewMember.RosterStatus, ProtoCrewMember.RosterStatus>("onKerbalStatusChanged");

	public static EventData<ProtoCrewMember, bool, bool> onKerbalInactiveChange = new EventData<ProtoCrewMember, bool, bool>("onKerbalInactiveChange");

	public static EventData<ProtoCrewMember> onKerbalLevelUp = new EventData<ProtoCrewMember>("onKerbalLevelUp");

	public static EventData<ProtoCrewMember> onKerbalPassedOutFromGeeForce = new EventData<ProtoCrewMember>("onKerbalPassedOutFromGeeForce");

	public static EventData<FromToAction<Part, Part>> onCrewBoardVessel = new EventData<FromToAction<Part, Part>>("onCrewBoardVessel");

	public static EventData<EventReport> onLaunch = new EventData<EventReport>("onLaunch");

	public static EventData<EventReport> onUndock = new EventData<EventReport>("onUndock");

	public static EventData<EventReport> onSplashDamage = new EventData<EventReport>("onSplashDamage");

	public static EventData<Vessel, Vessel, CometVessel, CometVessel> onCometVesselChanged = new EventData<Vessel, Vessel, CometVessel, CometVessel>("onCometVesselChanged");

	public static EventData<Vessel> onVesselPrecalcAssign = new EventData<Vessel>("onVesselPrecalcAssign");

	public static EventData<Vessel> onNewVesselCreated = new EventData<Vessel>("onNewVesselCreated");

	public static EventData<Vessel> onAsteroidSpawned = new EventData<Vessel>("onAsteroidSpawned");

	public static EventData<Vessel> onCometSpawned = new EventData<Vessel>("onCometSpawned");

	public static EventData<Vessel> OnDiscoverableObjectExpired = new EventData<Vessel>("OnDiscoverableObjectAboutToExpire");

	public static EventData<Vessel> onSentinelCometDetected = new EventData<Vessel>("onSentinelCometDetected");

	public static EventData<CometVessel> onCometVFXDimensionsModified = new EventData<CometVessel>("onCometVFXDimensionsModified");

	public static EventData<Vessel> onVesselWillDestroy = new EventData<Vessel>("onVesselWillDestroy");

	public static EventData<Vessel> onVesselCreate = new EventData<Vessel>("onVesselCreate");

	public static EventData<Vessel> onVesselDestroy = new EventData<Vessel>("onVesselDestroy");

	public static EventData<Vessel> onVesselExplodeGroundCollision = new EventData<Vessel>("onVesselExplodeGroundCollision");

	public static EventData<Vessel> onVesselChange = new EventData<Vessel>("onVesselChange");

	public static EventData<Vessel, Vessel> onVesselSwitching = new EventData<Vessel, Vessel>("onVesselSwitching");

	public static EventData<Vessel, Vessel> onVesselSwitchingToUnloaded = new EventData<Vessel, Vessel>("onVesselSwitchingToUnloaded");

	public static EventData<Transform, Transform> onVesselReferenceTransformSwitch = new EventData<Transform, Transform>("onVesselReferenceTransformSwitch");

	public static EventData<HostedFromToAction<Vessel, Vessel.Situations>> onVesselSituationChange = new EventData<HostedFromToAction<Vessel, Vessel.Situations>>("onVesselSituationChange");

	public static EventData<Vessel, bool> onVesselControlStateChange = new EventData<Vessel, bool>("onVesselControlStateChange");

	public static EventData<HostedFromToAction<IDiscoverable, DiscoveryLevels>> onKnowledgeChanged = new EventData<HostedFromToAction<IDiscoverable, DiscoveryLevels>>("onKnowledgeChanged");

	public static EventData<HostedFromToAction<Vessel, string>> onVesselRename = new EventData<HostedFromToAction<Vessel, string>>("onVesselRenamed");

	public static EventData<Vessel> onVesselGoOnRails = new EventData<Vessel>("onVesselGoOnRails");

	public static EventData<Vessel> onVesselGoOffRails = new EventData<Vessel>("onVesselGoOffRails");

	public static EventData<Vessel> onPhysicsEaseStart = new EventData<Vessel>("onPhysicsEaseStart");

	public static EventData<Vessel> onPhysicsEaseStop = new EventData<Vessel>("onPhysicsEaseStop");

	public static EventData<Vessel> onVesselLoaded = new EventData<Vessel>("onVesselLoaded");

	public static EventData<Vessel> onVesselUnloaded = new EventData<Vessel>("onVesselUnloaded");

	public static EventData<Vessel> onVesselStandardModification = new EventData<Vessel>("onVesselStandardModification");

	public static EventData<Vessel> onVesselWasModified = new EventData<Vessel>("onVesselWasModified");

	public static EventData<Vessel> onVesselPartCountChanged = new EventData<Vessel>("onVesselPartCountChanged");

	public static EventData<Vessel> onVesselCrewWasModified = new EventData<Vessel>("onVesselCrewWasModified");

	public static EventData<HostedFromToAction<Vessel, CelestialBody>> onVesselSOIChanged = new EventData<HostedFromToAction<Vessel, CelestialBody>>("OnVesselSOIChanged");

	public static EventData<Vessel> onVesselOrbitClosed = new EventData<Vessel>("onVesselOrbitClosed");

	public static EventData<Vessel> onVesselOrbitEscaped = new EventData<Vessel>("onVesselOrbitEscaped");

	public static EventData<Vessel> OnVesselRecoveryRequested = new EventData<Vessel>("OnVesselRecoveryRequested");

	public static EventData<Vessel> OnVesselOverrideGroupChanged = new EventData<Vessel>("OnVesselOverrideGroupChanged");

	public static EventData<ProtoVessel, bool> onVesselRecovered = new EventData<ProtoVessel, bool>("onVesselRecovered");

	public static EventData<ProtoVessel> onVesselTerminated = new EventData<ProtoVessel>("onVesselTerminated");

	public static EventData<FromToAction<ModuleDockingNode, ModuleDockingNode>> onSameVesselDock = new EventData<FromToAction<ModuleDockingNode, ModuleDockingNode>>("onSameVesselDock");

	public static EventData<FromToAction<ModuleDockingNode, ModuleDockingNode>> onSameVesselUndock = new EventData<FromToAction<ModuleDockingNode, ModuleDockingNode>>("onSameVesselUndock");

	public static EventData<PartModule, string, double> OnResourceConverterOutput = new EventData<PartModule, string, double>("OnResourceConverterOutput");

	public static EventData<FlightUIMode> OnFlightUIModeChanged = new EventData<FlightUIMode>("OnFlightUIModeChanged");

	public static EventData<FromToAction<ProtoVessel, ConfigNode>> onProtoVesselSave = new EventData<FromToAction<ProtoVessel, ConfigNode>>("onProtoVesselSave");

	public static EventData<FromToAction<ProtoVessel, ConfigNode>> onProtoVesselLoad = new EventData<FromToAction<ProtoVessel, ConfigNode>>("onProtoVesselLoad");

	public static EventData<FromToAction<ProtoPartSnapshot, ConfigNode>> onProtoPartSnapshotSave = new EventData<FromToAction<ProtoPartSnapshot, ConfigNode>>("onProtoPartSnapshotSave");

	public static EventData<FromToAction<ProtoPartSnapshot, ConfigNode>> onProtoPartSnapshotLoad = new EventData<FromToAction<ProtoPartSnapshot, ConfigNode>>("onProtoPartSnapshotLoad");

	public static EventData<FromToAction<ProtoPartModuleSnapshot, ConfigNode>> onProtoPartModuleSnapshotSave = new EventData<FromToAction<ProtoPartModuleSnapshot, ConfigNode>>("onProtoPartModuleSnapshotSave");

	public static EventData<FromToAction<ProtoPartModuleSnapshot, ConfigNode>> onProtoPartModuleSnapshotLoad = new EventData<FromToAction<ProtoPartModuleSnapshot, ConfigNode>>("onProtoPartModuleSnapshotLoad");

	public static EventData<Part> onPartCrossfeedStateChange = new EventData<Part>("onPartCrossfeedStateChange");

	public static EventData<Part> onPartResourceListChange = new EventData<Part>("onPartResourceListChange");

	public static EventData<Part> onPartPriorityChanged = new EventData<Part>("onPartPriorityChanged");

	public static EventData<HostedFromToAction<bool, Part>> onPartFuelLookupStateChange = new EventData<HostedFromToAction<bool, Part>>("onPartFuelLookupStateChange");

	public static EventData<HostedFromToAction<PartResource, bool>> onPartResourceFlowStateChange = new EventData<HostedFromToAction<PartResource, bool>>("onPartResourceFlowStateChange");

	public static EventData<HostedFromToAction<PartResource, PartResource.FlowMode>> onPartResourceFlowModeChange = new EventData<HostedFromToAction<PartResource, PartResource.FlowMode>>("onPartResourceFlowModeChange");

	public static EventData<PartResource> onPartResourceEmptyNonempty = new EventData<PartResource>("onPartResourceEmptyNonempty");

	public static EventData<PartResource> onPartResourceNonemptyEmpty = new EventData<PartResource>("onPartResourceNonemptyEmpty");

	public static EventData<PartResource> onPartResourceEmptyFull = new EventData<PartResource>("onPartResourceEmptyFull");

	public static EventData<PartResource> onPartResourceFullEmpty = new EventData<PartResource>("onPartResourceFullEmpty");

	public static EventData<PartResource> onPartResourceFullNonempty = new EventData<PartResource>("onPartResourceFullNonempty");

	public static EventData<PartResource> onPartResourceNonemptyFull = new EventData<PartResource>("onPartResourceNonemptyFull");

	public static EventData<uint, uint> onVesselPersistentIdChanged = new EventData<uint, uint>("onVesselpersistentIdChanged");

	public static EventData<uint, uint, uint> onPartPersistentIdChanged = new EventData<uint, uint, uint>("onPartpersistentIdChanged");

	public static EventData<Vessel, Vessel> onVesselsUndocking = new EventData<Vessel, Vessel>("onVesselsUndocking");

	public static EventData<Vessel> onFlightGlobalsAddVessel = new EventData<Vessel>("onFlightGlobalsAddVessel");

	public static EventData<Vessel> onFlightGlobalsRemoveVessel = new EventData<Vessel>("onFlightGlobalsRemoveVessel");

	public static EventData<int> onStageActivate = new EventData<int>("onStageActivate");

	public static EventData<FromToAction<CelestialBody, CelestialBody>> onDominantBodyChange = new EventData<FromToAction<CelestialBody, CelestialBody>>("onDominantBodyChanged");

	public static EventData<Vessel> onVesselClearStaging = new EventData<Vessel>("onVesselClearStaging");

	public static EventData<Vessel> onVesselResumeStaging = new EventData<Vessel>("onVesselResumeStaging");

	public static EventData<Part> onFairingsDeployed = new EventData<Part>("onFairingsDeployed");

	public static EventData<Vessel, PatchedConicSolver> onManeuversLoaded = new EventData<Vessel, PatchedConicSolver>("onManeuversLoaded");

	public static EventData<Vessel, PatchedConicSolver> onManeuverAdded = new EventData<Vessel, PatchedConicSolver>("onManeuverAdded");

	public static EventData<Vessel, PatchedConicSolver> onManeuverRemoved = new EventData<Vessel, PatchedConicSolver>("onManeuverRemoved");

	public static EventData<Vector3d> onKrakensbaneEngage = new EventData<Vector3d>("onKrakensbaneEngage");

	public static EventData<Vector3d> onKrakensbaneDisengage = new EventData<Vector3d>("onKrakensbaneDisengage");

	public static EventData<Vector3d, Vector3d> onFloatingOriginShift = new EventData<Vector3d, Vector3d>("onFloatingOriginShift");

	public static EventData<HostTargetAction<CelestialBody, bool>> onRotatingFrameTransition = new EventData<HostTargetAction<CelestialBody, bool>>("onRotatingFrameTransition");

	public static EventVoid onGamePause = new EventVoid("onGamePause");

	public static EventVoid onGameUnpause = new EventVoid("onGameUnpause");

	public static EventVoid onFlightReady = new EventVoid("onFlightReady");

	public static EventVoid onTimeWarpRateChanged = new EventVoid("onTimeWarpRateChanged");

	public static EventData<GameScenes> onGameSceneLoadRequested = new EventData<GameScenes>("onGameSceneLoadRequested");

	public static EventData<FromToAction<GameScenes, GameScenes>> onGameSceneSwitchRequested = new EventData<FromToAction<GameScenes, GameScenes>>("onGameSceneSwitchRequested");

	public static EventData<GameScenes> onLevelWasLoaded = new EventData<GameScenes>("onNewGameLevelLoadRequestWasSanctionedAndActioned");

	public static EventData<GameScenes> onLevelWasLoadedGUIReady = new EventData<GameScenes>("onLevelWasLoadedGUIReady");

	public static EventData<GameScenes> onSceneConfirmExit = new EventData<GameScenes>("onLevelConfirmExit");

	public static EventData<int, int> onScreenResolutionModified = new EventData<int, int>("onScreenResolutionModified");

	public static EventVoid OnGameDatabaseLoaded = new EventVoid("OnGameDatabaseLoaded");

	public static EventVoid OnUpgradesFilled = new EventVoid("OnUpgradesFilled");

	public static EventVoid OnUpgradesLinked = new EventVoid("OnUpgradesLinked");

	public static EventVoid OnPartLoaderLoaded = new EventVoid("OnPartLoaderLoaded");

	public static EventData<MapObject> onPlanetariumTargetChanged = new EventData<MapObject>("onPlanetariumTargetChange");

	public static EventVoid OnMapEntered = new EventVoid("OnMapEntered");

	public static EventVoid OnMapExited = new EventVoid("OnMapExited");

	public static EventData<MapViewFiltering.VesselTypeFilter> OnMapViewFiltersModified = new EventData<MapViewFiltering.VesselTypeFilter>("OnMapViewFiltersModified");

	public static EventData<LaunchSite> LaunchSiteFound = new EventData<LaunchSite>("LaunchSiteFound");

	public static EventData<Game> onGameStateSaved = new EventData<Game>("onGameStateSaved");

	public static EventData<Game> onGameStateCreated = new EventData<Game>("onGameStateCreated");

	public static EventData<ConfigNode> onGameStateSave = new EventData<ConfigNode>("onGameStateSave");

	public static EventData<ConfigNode> onGameStateLoad = new EventData<ConfigNode>("onGameStateLoad");

	public static EventVoid onGameNewStart = new EventVoid("onGameNewStart");

	public static EventVoid onGameAboutToQuicksave = new EventVoid("onGameAboutToQuicksave");

	public static EventData<ConfigNode> onGameStatePostLoad = new EventData<ConfigNode>("onGameStatePostLoad");

	public static EventData<FromToAction<ControlTypes, ControlTypes>> onInputLocksModified = new EventData<FromToAction<ControlTypes, ControlTypes>>("onInputLocksModified");

	public static EventVoid OnGameSettingsApplied = new EventVoid("OnGameSettingsApplied");

	public static EventVoid OnGameSettingsWritten = new EventVoid("OnGameSettingsWritten");

	public static EventVoid OnScenerySettingChanged = new EventVoid("OnScenerySettingChanged");

	public static EventData<bool> OnAppFocus = new EventData<bool>("OnAppFocus");

	public static EventData<bool> OnFlightGlobalsReady = new EventData<bool>("OnFlightGlobalsReady");

	public static EventVoid onLanguageSwitched = new EventVoid("onLanguageSwitched");

	public static EventVoid OnExpansionSystemLoaded = new EventVoid("OnExpansionSystemLoaded");

	public static EventData<FlightState> OnRevertToLaunchFlightState = new EventData<FlightState>("OnFlightState");

	public static EventData<FlightState> OnRevertToPrelaunchFlightState = new EventData<FlightState>("OnFlightState");

	public static EventData<string, string> OnDifficultySettingTabChanging = new EventData<string, string>("OnDifficultySettingTabChanging");

	public static EventData<DifficultyOptionsMenu> OnDifficultySettingsShown = new EventData<DifficultyOptionsMenu>("OnDifficultySettingsShown");

	public static EventData<DifficultyOptionsMenu, bool> OnDifficultySettingsDismiss = new EventData<DifficultyOptionsMenu, bool>("OnDifficultySettingsDismiss");

	public static EventData<Part> onPartPack = new EventData<Part>("onPartPack");

	public static EventData<Part> onPartUnpack = new EventData<Part>("onPartUnpack");

	public static EventData<ExplosionReaction> onPartExplode = new EventData<ExplosionReaction>("onPartExplode");

	public static EventData<Part> onPartDie = new EventData<Part>("onPartDie");

	public static EventData<Part> onPartWillDie = new EventData<Part>("onPartWillDie");

	public static EventData<Part> onPartExplodeGroundCollision = new EventData<Part>("onPartExplodeGroundCollision");

	public static EventData<Part> onPartDestroyed = new EventData<Part>("onPartDestroyed");

	public static EventData<PartJoint, float> onPartJointBreak = new EventData<PartJoint, float>("onPartJointBreak");

	public static EventData<PartJoint> onPartJointSet = new EventData<PartJoint>("onPartJointSet");

	public static EventData<Part> onPartUndock = new EventData<Part>("onPartUndock");

	public static EventData<Part> onPartUndockComplete = new EventData<Part>("onPartUndockComplete");

	public static EventData<FromToAction<Part, Part>> onPartCouple = new EventData<FromToAction<Part, Part>>("onPartCouple");

	public static EventData<FromToAction<Part, Part>> onPartCoupleComplete = new EventData<FromToAction<Part, Part>>("onPartCoupleComplete");

	public static EventData<Part> onPartDeCouple = new EventData<Part>("onPartCouple");

	public static EventData<Part> onPartDeCoupleComplete = new EventData<Part>("onPartCoupleComplete");

	public static EventData<Vessel, Vessel> onPartDeCoupleNewVesselComplete = new EventData<Vessel, Vessel>("onPartDeCoupleNewVesselComplete");

	public static EventData<uint, uint> onVesselDocking = new EventData<uint, uint>("onVesselDocking");

	public static EventData<FromToAction<Part, Part>> onDockingComplete = new EventData<FromToAction<Part, Part>>("onDockingComplete");

	public static EventData<HostTargetAction<Part, Part>> onPartAttach = new EventData<HostTargetAction<Part, Part>>("onPartAttach");

	public static EventData<HostTargetAction<Part, Part>> onPartRemove = new EventData<HostTargetAction<Part, Part>>("onPartRemove");

	public static EventData<UIPartActionWindow, Part> onPartActionUIShown = new EventData<UIPartActionWindow, Part>("onPartActionUIShown");

	public static EventData<Part> onPartActionInitialized = new EventData<Part>("onPartActionInitialized");

	public static EventData<Part> onPartActionUICreate = new EventData<Part>("onPartActionUICreate");

	public static EventData<Part> onPartActionUIDismiss = new EventData<Part>("onPartActionUIDismiss");

	public static EventData<bool> onPartActionNumericSlider = new EventData<bool>("onPawNumericSlider");

	public static EventData<Part, RaycastHit> OnCollisionEnhancerHit = new EventData<Part, RaycastHit>("OnCollisionEnhancerHit");

	public static EventData<Part> onPartFailure = new EventData<Part>("onPartFailure");

	public static EventData<Part> onPartRepair = new EventData<Part>("onPartRepair");

	public static EventData<PartModule, AdjusterPartModuleBase> onPartModuleAdjusterAdded = new EventData<PartModule, AdjusterPartModuleBase>("onPartModuleAdjusterAdded");

	public static EventData<PartModule, AdjusterPartModuleBase> onPartModuleAdjusterRemoved = new EventData<PartModule, AdjusterPartModuleBase>("onPartModuleAdjusterRemoved");

	public static EventData<ProtoPartSnapshot> onProtoPartFailure = new EventData<ProtoPartSnapshot>("onProtoPartFailure");

	public static EventData<ProtoPartSnapshot> onProtoPartRepair = new EventData<ProtoPartSnapshot>("onProtoPartRepair");

	public static EventData<ProtoPartModuleSnapshot> onProtoPartModuleAdjusterAdded = new EventData<ProtoPartModuleSnapshot>("onProtoPartModuleAdjusterAdded");

	public static EventData<ProtoPartModuleSnapshot> onProtoPartModuleAdjusterRemoved = new EventData<ProtoPartModuleSnapshot>("onProtoPartModuleAdjusterRemoved");

	public static EventData<ProtoPartModuleSnapshot> onProtoPartModuleRepaired = new EventData<ProtoPartModuleSnapshot>("onProtoPartModuleRepaired");

	public static EventData<Part> onPartVesselNamingChanged = new EventData<Part>("onPartVesselNamingChanged");

	public static EventData<AvailablePart> onVariantsAdded = new EventData<AvailablePart>("onVariantsAdded");

	public static EventData<Part, PartVariant> onVariantApplied = new EventData<Part, PartVariant>("onVariantApplied");

	public static EventData<Part, ControlPoint> OnControlPointChanged = new EventData<Part, ControlPoint>("onControlPointChanged");

	public static EventData<Part, PhysicMaterial> OnPhysicMaterialChanged = new EventData<Part, PhysicMaterial>("OnPhysicMaterialChanged");

	public static EventData<Part> onPartRepaired = new EventData<Part>("onPartRepaired");

	public static EventData<Part, ModuleLight> onLightsOn = new EventData<Part, ModuleLight>("onLightsOn");

	public static EventData<Part, ModuleLight> onLightsOff = new EventData<Part, ModuleLight>("onLightsOff");

	public static EventData<BaseConverter, Part, double> OnEfficiencyChange = new EventData<BaseConverter, Part, double>("OnEfficiencyChange");

	public static EventVoid OnResourceMapLoaded = new EventVoid("OnResourceMapLoaded");

	public static EventData<ModuleAnimationGroup, bool> OnAnimationGroupStateChanged = new EventData<ModuleAnimationGroup, bool>("OnAnimationGroupStateChanged");

	public static EventData<ModuleAnimationGroup> OnAnimationGroupRetractComplete = new EventData<ModuleAnimationGroup>("OnAnimationGroupRetractComplete");

	public static EventData<Vessel, CelestialBody> OnOrbitalSurveyCompleted = new EventData<Vessel, CelestialBody>("OnOrbitalSurveyCompleted");

	public static EventData<string> onFlagSelect = new EventData<string>("onFlagSelect");

	public static EventData<string> onMissionFlagSelect = new EventData<string>("onMissionFlagSelect");

	public static EventData<KerbalEVA, bool> onCommandSeatInteractionEnter = new EventData<KerbalEVA, bool>("onCommandSeatInteractionEnter");

	public static EventData<KerbalEVA, bool> onCommandSeatInteraction = new EventData<KerbalEVA, bool>("onCommandSeatInteraction");

	public static EventData<KerbalEVA, Part> onPartLadderEnter = new EventData<KerbalEVA, Part>("onLadderEnter");

	public static EventData<KerbalEVA, Part> onPartLadderExit = new EventData<KerbalEVA, Part>("onLadderExit");

	public static EventData<Vessel> onFlagPlant = new EventData<Vessel>("onFlagPlant");

	public static EventData<FlagSite> afterFlagPlanted = new EventData<FlagSite>("afterFlagPlanted");

	public static EventData<Part> onWheelRepaired = new EventData<Part>("onWheelRepaired");

	public static EventData<Vessel> onActiveJointNeedUpdate = new EventData<Vessel>("onActiveJointNeedUpdate");

	public static EventData<GClass4> OnPQSStarting = new EventData<GClass4>("OnPQSStarting");

	public static EventData<PQSCity> OnPQSCityStarting = new EventData<PQSCity>("OnPQSCityStarting");

	public static EventData<CelestialBody, string> OnPQSCityLoaded = new EventData<CelestialBody, string>("OnPQSCityLoaded");

	public static EventData<CelestialBody, string> OnPQSCityOrientated = new EventData<CelestialBody, string>("OnPQSCityOrientated");

	public static EventData<CelestialBody, string> OnPQSCityUnloaded = new EventData<CelestialBody, string>("OnPQSCityUnloaded");

	public static EventData<FromToAction<Waypoint, ConfigNode>> onCustomWaypointSave = new EventData<FromToAction<Waypoint, ConfigNode>>("onCustomWaypointSave");

	public static EventData<FromToAction<Waypoint, ConfigNode>> onCustomWaypointLoad = new EventData<FromToAction<Waypoint, ConfigNode>>("onCustomWaypointLoad");

	public static EventData<CelestialBody, string> OnPOIRangeEntered = new EventData<CelestialBody, string>("OnPOIRangeEntered");

	public static EventData<CelestialBody, string> OnPOIRangeExited = new EventData<CelestialBody, string>("OnPOIRangeExited");

	public static EventData<MultiModeEngine> onMultiModeEngineSwitchActive = new EventData<MultiModeEngine>("onMultiModeEngineSwitchActive");

	public static EventData<ModuleEngines> onEngineActiveChange = new EventData<ModuleEngines>("onEngineActiveChange");

	public static EventVoid onDeltaVCalcsCompleted = new EventVoid("onDeltaVCalcsCompleted");

	public static EventData<ModuleEngines> onChangeEngineDVIncludeState = new EventData<ModuleEngines>("onChangeEngineDVIncludeState");

	public static EventData<ModuleEngines> onEngineThrustPercentageChanged = new EventData<ModuleEngines>("onEngineThrustPercentageChanged");

	public static EventData<DeltaVSituationOptions> onDeltaVAppAtmosphereChanged = new EventData<DeltaVSituationOptions>("onDeltaVAppAtmosphereChangedCompleted");

	public static EventVoid onDeltaVAppInfoItemsChanged = new EventVoid("onDeltaVAppInfoItemsChanged");

	public static EventVoid onTooltipDestroyRequested = new EventVoid("onTooltipDestroyRequested");

	public static EventVoid onGUILock = new EventVoid("onGUILock");

	public static EventVoid onGUIUnlock = new EventVoid("onGUIUnlock");

	public static EventData<VesselSpawnInfo> onGUILaunchScreenSpawn = new EventData<VesselSpawnInfo>("onGUILaunchScreenSpawn");

	public static EventVoid onGUILaunchScreenDespawn = new EventVoid("onGUILaunchScreenDespawn");

	public static EventVoid onManeuverNodeSelected = new EventVoid("onManeuverNodeSelected");

	public static EventVoid onManeuverNodeDeselected = new EventVoid("onManeuverNodeDeselected");

	public static EventData<ShipTemplate> onGUILaunchScreenVesselSelected = new EventData<ShipTemplate>("onGUILaunchScreenVesselSelected");

	public static EventVoid onGUIAstronautComplexSpawn = new EventVoid("onGUIAstronautComplexSpawn");

	public static EventVoid onGUIAstronautComplexDespawn = new EventVoid("onGUIAstronautComplexDespawn");

	public static EventVoid onGUIRnDComplexSpawn = new EventVoid("onGUIRnDComplexSpawn");

	public static EventVoid onGUIRnDComplexDespawn = new EventVoid("onGUIRnDComplexDespawn");

	public static EventVoid onGUIMissionControlSpawn = new EventVoid("onGUIMissionControlSpawn");

	public static EventVoid onGUIMissionControlDespawn = new EventVoid("onGUIMissionControlDespawn");

	public static EventVoid onGUIAdministrationFacilitySpawn = new EventVoid("onGUIAdministrationFacilitySpawn");

	public static EventVoid onGUIAdministrationFacilityDespawn = new EventVoid("onGUIAdministrationFacilityDespawn");

	public static EventData<KSCFacilityContextMenu> onFacilityContextMenuSpawn = new EventData<KSCFacilityContextMenu>("onFacilityContextMenuSpawn");

	public static EventData<KSCFacilityContextMenu> onFacilityContextMenuDespawn = new EventData<KSCFacilityContextMenu>("onFacilityContextMenuDespawn");

	public static EventDataModifier<bool, ITooltipController> onTooltipAboutToSpawn = new EventDataModifier<bool, ITooltipController>("onTooltipAboutToSpawn");

	public static EventData<ITooltipController, Tooltip> onTooltipSpawned = new EventData<ITooltipController, Tooltip>("onTooltipSpawned");

	public static EventDataModifier<bool, Tooltip> onTooltipUpdate = new EventDataModifier<bool, Tooltip>("onTooltipUpdate");

	public static EventDataModifier<bool, ITooltipController> onTooltipAboutToDespawn = new EventDataModifier<bool, ITooltipController>("onTooltipAboutToDespawn");

	public static EventData<Tooltip> onTooltipDespawned = new EventData<Tooltip>("onTooltipDespawned");

	public static EventData<MissionRecoveryDialog> onGUIRecoveryDialogSpawn = new EventData<MissionRecoveryDialog>("onGUIRecoveryDialogSpawn");

	public static EventData<MissionRecoveryDialog> onGUIRecoveryDialogDespawn = new EventData<MissionRecoveryDialog>("onGUIRecoveryDialogDespawn");

	public static EventVoid onGUIKSPediaSpawn = new EventVoid("onGUIKSPediaSpawn");

	public static EventVoid onGUIKSPediaDespawn = new EventVoid("onGUIKSPediaDespawn");

	public static EventVoid onHideUI = new EventVoid("OnHideUI");

	public static EventVoid onShowUI = new EventVoid("OnShowUI");

	public static EventVoid onUIScaleChange = new EventVoid("onUIScaleChange");

	public static EventData<MenuNavInput> onMenuNavGetInput = new EventData<MenuNavInput>("onMenuNavGetInput");

	[Obsolete]
	public static EventVoid onGUIPrefabLauncherReady = new EventVoid("onGUIPrefabLauncherReady");

	public static EventVoid onGUIApplicationLauncherReady = new EventVoid("onGUIApplicationLauncherReady");

	public static EventData<GameScenes> onGUIApplicationLauncherUnreadifying = new EventData<GameScenes>("onGUIApplicationLauncherUnreadifying");

	public static EventVoid onGUIApplicationLauncherDestroyed = new EventVoid("onGUIApplicationLauncherDestroyed");

	public static EventVoid onGUIMessageSystemReady = new EventVoid("onGUIMessageSystemReady");

	public static EventVoid onGUIEditorToolbarReady = new EventVoid("onGUIEditorToolbarReady");

	public static EventVoid onGUIEngineersReportReady = new EventVoid("onGUIEngineersReportReady");

	public static EventVoid onGUIEngineersReportDestroy = new EventVoid("onGUIEngineersReportDestroy");

	public static EventVoid onGUIDeltaVAppReady = new EventVoid("onGUIDeltaVAppReadyReady");

	public static EventVoid onGUIDeltaVAppDestroy = new EventVoid("onGUIDeltaVApp");

	public static EventVoid onGUIManeuverToolReady = new EventVoid("onGUIManeuverToolReady");

	public static EventVoid onGUIManeuverToolDestroy = new EventVoid("onGUIManeuverToolDestroy");

	public static EventData<FlightGlobals.SpeedDisplayModes> onSetSpeedMode = new EventData<FlightGlobals.SpeedDisplayModes>("onSetSpeedMode");

	public static EventVoid onGUIActionGroupFlightShowing = new EventVoid("onGUIActionGroupFlightShowing");

	public static EventVoid onGUIActionGroupFlightShown = new EventVoid("onGUIActionGroupFlightShown");

	public static EventVoid onGUIActionGroupFlightClosed = new EventVoid("onGUIActionGroupFlightClosed");

	public static EventVoid onGUIActionGroupShowing = new EventVoid("onGUIActionGroupShowing");

	public static EventVoid onGUIActionGroupShown = new EventVoid("onGUIActionGroupShown");

	public static EventVoid onGUIActionGroupClosed = new EventVoid("onGUIActionGroupClosed");

	public static EventData<ShipConstruct> onEditorShipModified = new EventData<ShipConstruct>("onEditorShipModified");

	public static EventData<HostedFromToAction<ShipConstruct, string>> onEditorVesselNamingChanged = new EventData<HostedFromToAction<ShipConstruct, string>>("onEditorVesselNamingChanged");

	public static EventData<EditorScreen> onEditorScreenChange = new EventData<EditorScreen>("onEditorScreenChange");

	public static EventData<SymmetryMethod> onEditorSymmetryMethodChange = new EventData<SymmetryMethod>("onEditorSymmetryMethodChange");

	public static EventData<Space> onEditorSymmetryCoordsChange = new EventData<Space>("onEditorSymmetryCoordsChange");

	public static EventData<int> onEditorSymmetryModeChange = new EventData<int>("onEditorSymmetryModeChange");

	public static EventData<bool> onEditorSnapModeChange = new EventData<bool>("onEditorSnapModeChange");

	public static EventData<ConstructionMode> onEditorConstructionModeChange = new EventData<ConstructionMode>("onEditorConstructionModeChange");

	public static EventData<ConstructionEventType, Part> onEditorPartEvent = new EventData<ConstructionEventType, Part>("onEditorPartEvent");

	public static EventVoid onEditorShowPartList = new EventVoid("onEditorShowPartList");

	public static EventData<ShipConstruct> onEditorUndo = new EventData<ShipConstruct>("onEditorUndo");

	public static EventData<ShipConstruct> onEditorRedo = new EventData<ShipConstruct>("onEditorRedo");

	public static EventData<ShipConstruct> onEditorSetBackup = new EventData<ShipConstruct>("onEditorSetBackup");

	public static EventVoid onEditorRestart = new EventVoid("onEditorRestart");

	public static EventData<ShipConstruct, CraftBrowserDialog.LoadType> onEditorLoad = new EventData<ShipConstruct, CraftBrowserDialog.LoadType>("onEditorLoad");

	public static EventVoid onEditorStarted = new EventVoid("onEditorStarted");

	public static EventData<Part> onEditorPodPicked = new EventData<Part>("onEditorPodPicked");

	public static EventData<Part> onEditorPodSelected = new EventData<Part>("onEditorPodSelected");

	public static EventData<ShipConstruct> onAboutToSaveShip = new EventData<ShipConstruct>("onAboutToSaveShip");

	public static EventData<Part> onEditorPartPicked = new EventData<Part>("onEditorPartPicked");

	public static EventData<Part> onEditorPartPlaced = new EventData<Part>("onEditorPartPlaced");

	public static EventData<CompoundPart> onEditorCompoundPartLinked = new EventData<CompoundPart>("onEditorCompoundPartLinked");

	public static EventData<CompoundPart> OnFlightCompoundPartLinked = new EventData<CompoundPart>("OnFlightCompoundPartLinked");

	public static EventData<CompoundPart> OnFlightCompoundPartDetached = new EventData<CompoundPart>("OnFlightCompoundPartDetached");

	public static EventVoid onEditorPodDeleted = new EventVoid("onEditorPodDeleted");

	public static EventData<Part> onEditorPartDeleted = new EventData<Part>("onEditorPartDeleted");

	public static EventVoid onEditorRestoreState = new EventVoid("onEditorRestoreState");

	public static EventVoid onEditorNewShipDialogDismiss = new EventVoid("onEditorNewShipDialogDismiss");

	public static EventData<Part, PartVariant> onEditorVariantApplied = new EventData<Part, PartVariant>("onEditorVariantApplied");

	public static EventData<AvailablePart, PartVariant> onEditorDefaultVariantChanged = new EventData<AvailablePart, PartVariant>("onEditorDefaultVariantChanged");

	public static EventData<VesselCrewManifest> onEditorShipCrewModified = new EventData<VesselCrewManifest>("onEditorShipCrewModified");

	public static EventData<ComboSelector, string, int> onSuitComboSelection = new EventData<ComboSelector, string, int>("onSuitComboSelection");

	public static EventData<SuitLightColorPicker> onClickSuitLightButton = new EventData<SuitLightColorPicker>("onClickSuitLightButton");

	public static EventData<SuitButton> onClickHelmetNeckringButton = new EventData<SuitButton>("onClickHelmetNeckringButton");

	public static EventData<HostTargetAction<RDTech, RDTech.OperationResult>> OnTechnologyResearched = new EventData<HostTargetAction<RDTech, RDTech.OperationResult>>("OnTechnologyResearched");

	public static EventData<AvailablePart> OnPartPurchased = new EventData<AvailablePart>("OnPartPurchased");

	public static EventData<PartUpgradeHandler.Upgrade> OnPartUpgradePurchased = new EventData<PartUpgradeHandler.Upgrade>("OnPartUpgradePurchased");

	public static EventData<ShipConstruct> OnVesselRollout = new EventData<ShipConstruct>("OnVesselRollout");

	public static EventData<ProgressNode> OnProgressReached = new EventData<ProgressNode>("OnProgressReached");

	public static EventData<ProgressNode> OnProgressComplete = new EventData<ProgressNode>("OnProgressComplete");

	public static EventData<ProgressNode> OnProgressAchieved = new EventData<ProgressNode>("OnProgressAchieved");

	public static EventData<FromToAction<ProgressNode, ConfigNode>> onProgressNodeSave = new EventData<FromToAction<ProgressNode, ConfigNode>>("onProgressNodeSave");

	public static EventData<FromToAction<ProgressNode, ConfigNode>> onProgressNodeLoad = new EventData<FromToAction<ProgressNode, ConfigNode>>("onProgressNodeLoad");

	public static EventData<float, ScienceSubject, ProtoVessel, bool> OnScienceRecieved = new EventData<float, ScienceSubject, ProtoVessel, bool>("OnScienceRecieved");

	public static EventData<ScienceData, Vessel, bool> OnTriggeredDataTransmission = new EventData<ScienceData, Vessel, bool>("OnTriggeredDataTransmission");

	public static EventData<ScienceData> OnExperimentDeployed = new EventData<ScienceData>("OnExperimentDeployed");

	public static EventData<ScienceData> OnExperimentStored = new EventData<ScienceData>("OnExperimentStored");

	public static EventData<ScienceData> OnROCExperimentStored = new EventData<ScienceData>("OnROCExperimentStored");

	public static EventData<ScienceData> OnROCExperimentReset = new EventData<ScienceData>("OnROCExperimentReset");

	public static EventData<Vessel> OnFlightLogRecorded = new EventData<Vessel>("OnFlightLogRecorded");

	public static EventData<float, TransactionReasons> OnReputationChanged = new EventData<float, TransactionReasons>("OnReputationChanged");

	public static EventData<float, TransactionReasons> OnScienceChanged = new EventData<float, TransactionReasons>("OnScienceChanged");

	public static EventData<double, TransactionReasons> OnFundsChanged = new EventData<double, TransactionReasons>("OnFundsChanged");

	public static EventData<ProtoCrewMember, int> OnCrewmemberHired = new EventData<ProtoCrewMember, int>("OnApplicantHired");

	public static EventData<ProtoCrewMember, int> OnCrewmemberSacked = new EventData<ProtoCrewMember, int>("OnApplicantHired");

	public static EventData<ProtoCrewMember, int> OnCrewmemberLeftForDead = new EventData<ProtoCrewMember, int>("OnCrewmemberLeftForDead");

	public static EventData<FromToAction<ProtoCrewMember, ConfigNode>> onProtoCrewMemberSave = new EventData<FromToAction<ProtoCrewMember, ConfigNode>>("onProtoCrewMemberSave");

	public static EventData<FromToAction<ProtoCrewMember, ConfigNode>> onProtoCrewMemberLoad = new EventData<FromToAction<ProtoCrewMember, ConfigNode>>("onProtoCrewMemberLoad");

	public static EventData<ProtoVessel, MissionRecoveryDialog, float> onVesselRecoveryProcessing = new EventData<ProtoVessel, MissionRecoveryDialog, float>("onVesselRecoveryProcessing");

	public static EventData<ProtoVessel, MissionRecoveryDialog, float> onVesselRecoveryProcessingComplete = new EventData<ProtoVessel, MissionRecoveryDialog, float>("onVesselRecoveryProcessingComplete");

	public static EventData<string> onDeployGroundPart = new EventData<string>("onDeployGroundPart");

	public static EventData<ModuleGroundSciencePart> onGroundSciencePartDeployed = new EventData<ModuleGroundSciencePart>("onGroundSciencePartDeployed");

	public static EventData<ModuleGroundSciencePart> onGroundSciencePartRemoved = new EventData<ModuleGroundSciencePart>("onGroundSciencePartRemoved ");

	public static EventData<ModuleGroundSciencePart> onGroundSciencePartChanged = new EventData<ModuleGroundSciencePart>("onGroundSciencePartChanged");

	public static EventData<ModuleGroundSciencePart> onGroundSciencePartEnabledStateChanged = new EventData<ModuleGroundSciencePart>("onGroundSciencePartEnabledStateChanged");

	public static EventData<ModuleGroundExpControl, bool, List<ModuleGroundSciencePart>> onGroundScienceControllerChanged = new EventData<ModuleGroundExpControl, bool, List<ModuleGroundSciencePart>>("onGroundScienceControllerChanged");

	public static EventData<ModuleGroundExperiment> onGroundScienceExperimentScienceValueChanged = new EventData<ModuleGroundExperiment>("onGroundScienceExperimentScienceValueChanged");

	public static EventData<ModuleGroundExperiment> onGroundScienceExperimentScienceLimitChanged = new EventData<ModuleGroundExperiment>("onGroundScienceExperimentScienceLimitChanged");

	public static EventData<ModuleGroundExpControl, List<ModuleGroundSciencePart>> onGroundScienceRegisterCluster = new EventData<ModuleGroundExpControl, List<ModuleGroundSciencePart>>("onGroundScienceRegisterCluster");

	public static EventData<ModuleGroundExpControl, DeployedScienceCluster> onGroundScienceClusterRegistered = new EventData<ModuleGroundExpControl, DeployedScienceCluster>("onGroundScienceClusterRegistered");

	public static EventData<uint> onGroundScienceDeregisterCluster = new EventData<uint>("onGroundScienceDeregisterCluster");

	public static EventData<ModuleGroundExpControl, DeployedScienceCluster> onGroundScienceClusterUpdated = new EventData<ModuleGroundExpControl, DeployedScienceCluster>("onGroundScienceClusterUpdated");

	public static EventData<DeployedScienceCluster> onGroundScienceClusterPowerStateChanged = new EventData<DeployedScienceCluster>("onGroundScienceClusterPowerStateChanged");

	public static EventData<DeployedScienceExperiment, DeployedSciencePart, DeployedScienceCluster, float> onGroundScienceGenerated = new EventData<DeployedScienceExperiment, DeployedSciencePart, DeployedScienceCluster, float>("onGroundScienceGenerated");

	public static EventData<DeployedScienceExperiment, DeployedSciencePart, DeployedScienceCluster, float> onGroundScienceTransmitted = new EventData<DeployedScienceExperiment, DeployedSciencePart, DeployedScienceCluster, float>("onGroundScienceTransmitted");

	public static EventData<ModuleInventoryPart> onModuleInventoryChanged = new EventData<ModuleInventoryPart>("onModuleInventoryChanged");

	public static EventData<ModuleInventoryPart, int> onModuleInventorySlotChanged = new EventData<ModuleInventoryPart, int>("onModuleInventorySlotChanged");

	public static EventVoid OnDebugROCFinderToggled = new EventVoid("OnDebugROCFinderToggled");

	public static EventVoid OnDebugROCScanPointsToggled = new EventVoid("OnDebugROCScanPointsToggled");

	public static EventData<ModuleRoboticController> onRoboticControllerAxesChanged = new EventData<ModuleRoboticController>("onRoboticControllerAxesChanged");

	public static EventData<ModuleRoboticController, ControlledAxis> onRoboticControllerAxesAdding = new EventData<ModuleRoboticController, ControlledAxis>("onRoboticControllerAxesAdding");

	public static EventData<ModuleRoboticController, ControlledAxis> onRoboticControllerAxesRemoving = new EventData<ModuleRoboticController, ControlledAxis>("onRoboticControllerAxesRemoving");

	public static EventData<ModuleRoboticController> onRoboticControllerActionsChanged = new EventData<ModuleRoboticController>("onRoboticControllerActionsChanged");

	public static EventData<ModuleRoboticController, ControlledAction> onRoboticControllerActionsAdding = new EventData<ModuleRoboticController, ControlledAction>("onRoboticControllerActionsAdding");

	public static EventData<ModuleRoboticController, ControlledAction> onRoboticControllerActionsRemoving = new EventData<ModuleRoboticController, ControlledAction>("onRoboticControllerActionsRemoving");

	public static EventData<ModuleRoboticController> onRoboticControllerSequencePlayed = new EventData<ModuleRoboticController>("onRoboticControllerSequencePlayed");

	public static EventData<ModuleRoboticController> onRoboticControllerSequenceStopped = new EventData<ModuleRoboticController>("onRoboticControllerSequenceStopped");

	public static EventData<ModuleRoboticController, ModuleRoboticController.SequenceDirectionOptions> onRoboticControllerSequenceDirectionChanged = new EventData<ModuleRoboticController, ModuleRoboticController.SequenceDirectionOptions>("onRoboticControllerSequenceDirectionChanged");

	public static EventData<ModuleRoboticController, ModuleRoboticController.SequenceLoopOptions> onRoboticControllerSequenceLoopModeChanged = new EventData<ModuleRoboticController, ModuleRoboticController.SequenceLoopOptions>("onRoboticControllerSequenceLoopModeChanged");

	public static EventData<Part, bool> onRoboticPartLockChanging = new EventData<Part, bool>("onRoboticPartLockChanging");

	public static EventData<Part, bool> onRoboticPartLockChanged = new EventData<Part, bool>("onRoboticPartLockChanged");

	public static GameModifiers Modifiers = new GameModifiers();

	public static EventData<DestructibleBuilding> OnKSCStructureCollapsing = new EventData<DestructibleBuilding>("OnKSCStructureCollapsing");

	public static EventData<DestructibleBuilding> OnKSCStructureCollapsed = new EventData<DestructibleBuilding>("OnKSCStructureCollapsed");

	public static EventData<DestructibleBuilding> OnKSCStructureRepairing = new EventData<DestructibleBuilding>("OnKSCStructureRepairing");

	public static EventData<DestructibleBuilding> OnKSCStructureRepaired = new EventData<DestructibleBuilding>("OnKSCStructureRepaired");

	public static EventData<UpgradeableFacility, int> OnKSCFacilityUpgrading = new EventData<UpgradeableFacility, int>("OnKSCFacilityUpgrading");

	public static EventData<UpgradeableFacility, int> OnKSCFacilityUpgraded = new EventData<UpgradeableFacility, int>("OnKSCFacilityUpgraded");

	public static EventData<UpgradeableObject, int> OnUpgradeableObjLevelChange = new EventData<UpgradeableObject, int>("OnUpgradeableObjLevelChange");

	public static EventData<AltimeterDisplayState> OnAltimeterDisplayModeToggle = new EventData<AltimeterDisplayState>("OnAltimeterDisplayModeToggle");

	public static EventData<CameraManager.CameraMode> OnCameraChange = new EventData<CameraManager.CameraMode>("OnCameraChange");

	public static EventVoid OnFlightCameraModeChange = new EventVoid("OnFlightCameraModeChange");

	public static EventData<FlightCamera.Modes> OnFlightCameraAngleChange = new EventData<FlightCamera.Modes>("OnFlightCameraAngleChange");

	public static EventData<Kerbal> OnIVACameraKerbalChange = new EventData<Kerbal>("OnIVACameraAngleChange");

	public static EventData<Vessel, float, Vector3> OnCameraDistanceAdjustedToFitVessel = new EventData<Vessel, float, Vector3>("OnCameraDistanceToVesselAdjustedToFit");

	public static EventData<MapObject> OnMapFocusChange = new EventData<MapObject>("OnMapFocusChange");

	public static EventData<MapObject> OnTargetObjectChanged = new EventData<MapObject>("OnTargetObjectChanged");

	public static EventVoid OnCollisionIgnoreUpdate = new EventVoid("OnCollisionIgnoreUpdate");

	public static EventData<KerbalEVA, bool, bool> OnHelmetChanged = new EventData<KerbalEVA, bool, bool>("OnHelmetChanged");

	public static EventData<PhysicMaterial> onGlobalEvaPhysicMaterialChanged = new EventData<PhysicMaterial>("OnGlobalEvaPhysicMaterialChanged");

	public static EventData<ConstructionMode> OnEVAConstructionModeChanged = new EventData<ConstructionMode>("onEVAConstructionModeChanged");

	public static EventData<bool> OnEVAConstructionMode = new EventData<bool>("OnEVAConstructionMode");

	public static EventData<bool> OnEVACargoMode = new EventData<bool>("OnEVACargoMode");

	public static EventData<KerbalEVA> OnEVAConstructionWeldStart = new EventData<KerbalEVA>("OnEVAConstructionWeldStart");

	public static EventData<KerbalEVA> OnEVAConstructionWeldFinish = new EventData<KerbalEVA>("OnEVAConstructionWeldFinish");

	public static EventData<Vessel, Part> OnEVAConstructionModePartAttached = new EventData<Vessel, Part>("OnEVAConstructionModePartAttached");

	public static EventData<Vessel, Part> OnEVAConstructionModePartDetached = new EventData<Vessel, Part>("OnEVAConstructionModePartAttached");

	public static EventData<Part> OnInventoryPartOnMouseChanged = new EventData<Part>("OnInventoryPartOnMouseChanged");

	public static EventVoid OnCombinedConstructionWeightLimitChanged = new EventVoid("OnCombinedConstructionWeightLimitChanged");

	public static EventData<AlarmTypeBase> onAlarmAdded = new EventData<AlarmTypeBase>("onAlarmAdded");

	public static EventData<AlarmTypeBase> onAlarmRemoving = new EventData<AlarmTypeBase>("onAlarmRemoving");

	public static EventData<uint> onAlarmRemoved = new EventData<uint>("onAlarmRemoved");

	public static EventData<AlarmTypeBase> onAlarmTriggered = new EventData<AlarmTypeBase>("onAlarmTriggered");

	public static EventData<AlarmTypeBase> onAlarmActioned = new EventData<AlarmTypeBase>("onAlarmActioned");

	public static EventData<Vessel> onAlarmAvailableVesselChanged = new EventData<Vessel>("onAlarmAvailableVesselChanged");

	public static EventData<bool> onAlarmAppTimeDisplayChanged = new EventData<bool>("onAlarmAppTimeDisplayChanged");

	public static EventData<KerbalEVA> OnVisorLowering = new EventData<KerbalEVA>("OnVisorLowering");

	public static EventData<KerbalEVA> OnVisorLowered = new EventData<KerbalEVA>("OnVisorLowered");

	public static EventData<KerbalEVA> OnVisorRaising = new EventData<KerbalEVA>("OnVisorRaising");

	public static EventData<KerbalEVA> OnVisorRaised = new EventData<KerbalEVA>("OnVisorRaised");

	public static T FindEvent<T>(string eventName) where T : BaseGameEvent
	{
		return BaseGameEvent.FindEvent<T>(eventName);
	}
}
