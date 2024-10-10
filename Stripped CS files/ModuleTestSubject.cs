using System.Collections;
using System.Collections.Generic;
using Contracts;
using Contracts.Parameters;
using Contracts.Templates;
using ns9;

public class ModuleTestSubject : PartModule
{
	[KSPField]
	public uint situationMask = uint.MaxValue;

	[KSPField]
	public bool useProgressForBodies = true;

	[KSPField]
	public bool usePrestigeForSit = true;

	[KSPField]
	public bool useStaging;

	[KSPField]
	public bool useEvent;

	[KSPField]
	public string TestNotes = "";

	public bool isTestSubject;

	public bool isExperimentalPart;

	public List<PartTestConstraint> constraints;

	public static EventData<ModuleTestSubject> onTestRun = new EventData<ModuleTestSubject>("onPartTestRun");

	public static EventData<ModuleTestSubject> onTestSujectDestroyed = new EventData<ModuleTestSubject>("onTestSujectDestroyed");

	public static string cacheAutoLOC_231004;

	public static string cacheAutoLOC_231008;

	public static string cacheAutoLOC_6002280;

	public string GetTestNotes()
	{
		if (!string.IsNullOrEmpty(TestNotes))
		{
			return TestNotes;
		}
		if (useStaging)
		{
			return cacheAutoLOC_231004;
		}
		if (useEvent)
		{
			return cacheAutoLOC_231008;
		}
		return null;
	}

	public override void OnAwake()
	{
		if (constraints == null)
		{
			constraints = new List<PartTestConstraint>();
		}
	}

	public void OnDestroy()
	{
		if (isTestSubject)
		{
			GameEvents.onPartDie.Remove(onPartDie);
			GameEvents.Contract.onParameterChange.Remove(OnContractStateChange);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		if (constraints == null)
		{
			constraints = new List<PartTestConstraint>();
		}
		if (node.HasNode("CONSTRAINT"))
		{
			constraints.Clear();
		}
		ConfigNode[] nodes = node.GetNodes("CONSTRAINT");
		int i = 0;
		for (int num = nodes.Length; i < num; i++)
		{
			PartTestConstraint partTestConstraint = new PartTestConstraint(nodes[i]);
			if (partTestConstraint.valid)
			{
				constraints.Add(partTestConstraint);
			}
		}
	}

	public override void OnStart(StartState state)
	{
		base.Events["RunTestEvent"].active = false;
		StartCoroutine(delayedStart(5));
	}

	public IEnumerator delayedStart(int frameDelay)
	{
		for (int i = 0; i < frameDelay; i++)
		{
			yield return null;
		}
		isExperimentalPart = !ResearchAndDevelopment.PartModelPurchased(base.part.partInfo);
		if (!HighLogic.LoadedSceneIsFlight || ContractSystem.Instance == null)
		{
			yield break;
		}
		Contracts.Templates.PartTest[] currentActiveContracts = ContractSystem.Instance.GetCurrentActiveContracts<Contracts.Templates.PartTest>();
		int num = currentActiveContracts.Length;
		Contracts.Templates.PartTest partTest;
		do
		{
			if (num-- > 0)
			{
				partTest = currentActiveContracts[num];
				continue;
			}
			yield break;
		}
		while (partTest.PartName != base.part.partInfo.title);
		isTestSubject = true;
		if (useEvent && !partTest.Hauled)
		{
			base.Events["RunTestEvent"].active = true;
		}
		GameEvents.onPartDie.Add(onPartDie);
		GameEvents.Contract.onParameterChange.Add(OnContractStateChange);
	}

	public void onPartDie(Part p)
	{
		if (p == base.part)
		{
			onTestSujectDestroyed.Fire(this);
		}
	}

	public void RunTest()
	{
		onTestRun.Fire(this);
	}

	public override void OnActive()
	{
		if (useStaging && base.part.stagingOn)
		{
			RunTest();
		}
	}

	[KSPEvent(active = false, guiActive = true, guiName = "#autoLOC_6001499")]
	public void RunTestEvent()
	{
		RunTest();
		if (base.part.hasStagingIcon && base.part.State == PartStates.IDLE)
		{
			base.part.force_activate();
		}
	}

	public void OnContractStateChange(Contract c, ContractParameter p)
	{
		if (c != null && p != null && p.State == ParameterState.Complete && c is Contracts.Templates.PartTest partTest && partTest.PartName == base.part.partInfo.title && p is Contracts.Parameters.PartTest)
		{
			base.Events["RunTestEvent"].active = false;
		}
	}

	public bool SituationAvailable(Vessel.Situations sit)
	{
		return ((uint)sit & situationMask) != 0;
	}

	public override string GetModuleDisplayName()
	{
		if (cacheAutoLOC_6002280 != null)
		{
			return Localizer.Format("#autoLOC_6002280");
		}
		return cacheAutoLOC_6002280;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_231004 = Localizer.Format("#autoLOC_231004");
		cacheAutoLOC_231008 = Localizer.Format("#autoLOC_231008");
		cacheAutoLOC_6002280 = Localizer.Format("#autoLOC_6002280");
	}

	public override bool IsStageable()
	{
		return false;
	}
}
