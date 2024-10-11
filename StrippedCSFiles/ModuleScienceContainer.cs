using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ModuleScienceContainer : PartModule, IScienceDataContainer
{
	[KSPField]
	public int capacity;

	private List<ScienceData> dataStore;

	private List<ScienceData> pagesWithDialogs;

	[KSPField]
	public string reviewActionName;

	[KSPField]
	public string storeActionName;

	[KSPField]
	public string collectActionName;

	[KSPField]
	public bool evaOnlyStorage;

	[KSPField]
	public bool canBeTransferredToInVessel;

	[KSPField]
	public bool canTransferInVessel;

	[KSPField]
	public float storageRange;

	[KSPField]
	public bool allowRepeatedSubjects;

	[KSPField]
	public bool dataIsRecoverable;

	[KSPField]
	public bool dataIsCollectable;

	[KSPField]
	public bool dataIsStorable;

	[KSPField]
	public bool CollectOnlyOwnData;

	[KSPField(guiActive = false, guiName = "#autoLOC_6001352")]
	public string status;

	[KSPField]
	public bool showStatus;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleScienceContainer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartActionUIOpened(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updateModuleUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetActiveVesselDataCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetStoredDataCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 1f)]
	public void StoreDataExternalEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 1f)]
	public void CollectDataExternalEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TransferToContainer(ModuleScienceContainer activeVesselContainer, string targetName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001312")]
	public void TransferDataEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001311")]
	public void CollectAllAction(KSPActionParam actParams)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001808")]
	public void CollectAllEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDataTransfer(PartItemTransfer.DismissAction dma, Part containerPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool StoreData(List<IScienceDataContainer> containers, bool dumpRepeats, bool excludeScienceContainers = false, bool scienceContainerPriority = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void sortContainerList(List<IScienceDataContainer> containers, bool scienceContainerPriority)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddData(ScienceData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasData(ScienceData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveData(ScienceData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScienceData[] GetData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReturnData(ScienceData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DumpData(ScienceData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001439")]
	public void ReviewDataEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReviewData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void reviewData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReviewDataItem(ScienceData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void discardData(ScienceData pageData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void keepData(ScienceData pageData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void sendDataToComms(ScienceData pageData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void sendDataToLab(ScienceData pageData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetScienceCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsRerunnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
