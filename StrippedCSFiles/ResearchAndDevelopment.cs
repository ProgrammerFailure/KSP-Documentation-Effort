using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using KSP.UI.Screens.SpaceCenter.MissionSummaryDialog;

[KSPScenario((ScenarioCreationOptions)3112, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER,
	GameScenes.EDITOR
})]
public class ResearchAndDevelopment : ScenarioModule
{
	public static ResearchAndDevelopment Instance;

	private static Dictionary<string, ScienceExperiment> experiments;

	private static Dictionary<string, MiniBiome> minibiomesbyScienceID;

	private static Dictionary<string, MiniBiome> minibiomesbyUnityTag;

	private Dictionary<string, ProtoTechNode> protoTechNodes;

	private static Dictionary<string, string> techTitles;

	private Dictionary<string, ScienceSubject> scienceSubjects;

	private EditorPartListFilter<AvailablePart> unresearchedTechFilter;

	private EditorPartListFilter<AvailablePart> unpurchasedPartFilter;

	private Dictionary<AvailablePart, int> experimentalPartsStock;

	private ScreenMessage message;

	private List<ScienceSubjectWidget> recoveredDataWidgets;

	private float science;

	private PSystemSetup.SpaceCenterFacility RnDFacility;

	public static Func<float, ScienceSubject, float> GetReferenceDataValueFunc;

	public static Func<float, ScienceSubject, float> GetSubjectValueFunc;

	public float Science
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ResearchAndDevelopment()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ResearchAndDevelopment()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddScience(float value, TransactionReasons reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetScience(float value, TransactionReasons reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CanAfford(float amount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void loadExperiments()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void loadMiniBiomes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ConfigNode getInitialTech()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddExperimentalPart(AvailablePart ap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsExperimentalPart(AvailablePart ap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveExperimentalPart(AvailablePart ap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeavingScene(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselRecoveryProcessing(ProtoVessel pv, MissionRecoveryDialog mrDialog, float recoveryScore)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCurrenciesModified(CurrencyModifierQuery query)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void reverseEngineerRecoveredVessel(ProtoVessel pv, MissionRecoveryDialog mrDialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<ScienceSubject> reverseEngineerPartsFrom(List<string> fromCBs, List<string> ignoreCBs, float subValue, string idVerb, string returnedFrom)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool partTechAvailable(AvailablePart ap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool partModelPurchased(AvailablePart ap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoTechNode GetTechState(string techID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTechState(string techID, ProtoTechNode techNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static RDTech.State GetTechnologyState(string techID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetTechnologyTitle(string techID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool PartTechAvailable(AvailablePart ap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool PartModelPurchased(AvailablePart ap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScienceExperiment GetExperiment(string experimentID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScienceSubject GetExperimentSubject(ScienceExperiment experiment, ExperimentSituations situation, CelestialBody body, string biome, string displaybiome)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScienceSubject GetExperimentSubject(ScienceExperiment experiment, ExperimentSituations situation, string sourceUId, string sourceTitle, CelestialBody body, string biome, string displaybiome)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScienceSubject GetSubjectByID(string subjectID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetReferenceDataValue(float dataAmount, ScienceSubject subject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetSubjectValue(float subjectScience, ScienceSubject subject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetScienceValue(float dataAmount, ScienceSubject subject, float xmitScalar = 1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetScienceValue(float dataAmount, float scienceValueRatio, ScienceSubject subject, float xmitScalar = 1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetNextScienceValue(float dataAmount, ScienceSubject subject, float xmitScalar = 1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ScienceSubject getScienceSubject(ScienceSubject subject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float SubmitScienceData(float dataAmount, ScienceSubject subject, float xmitScalar = 1f, ProtoVessel source = null, bool reverseEngineered = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float SubmitScienceData(float dataAmount, float scienceValueRatio, ScienceSubject subject, float xmitScalar = 1f, ProtoVessel source = null, bool reverseEngineered = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<ScienceSubject> GetSubjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetExperimentIDs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetSituationTagsDescriptions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetSituationTags()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetBiomeTagsLocalized(CelestialBody cb, bool includeMiniBiomes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetMiniBiomeTagsLocalized(CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetBiomeTags(CelestialBody cb, bool includeMiniBiomes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<string> GetMiniBiomeTags(CelestialBody cb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetMiniBiomedisplayNameByScienceID(string TagID, bool formatted)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetMiniBiomedisplayNameByUnityTag(string TagID, bool formatted)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetResults(string subjectID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string CheckForMissingParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PartAssignmentSummary()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheatAddScience(float sci)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnlockProtoTechNode(ProtoTechNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RefreshTechTreeUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheatTechnology()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string CountUniversalScience()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string ScienceTransmissionRewardString(float amount, TransactionReasons reason = TransactionReasons.ScienceTransmission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ResearchedValidContractObjectives(params string[] objectiveTypes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ResearchedValidContractObjectives(List<string> objectiveTypes, bool copy = true)
	{
		throw null;
	}
}
