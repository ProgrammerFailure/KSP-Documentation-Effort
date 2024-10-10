using System.Collections.Generic;
using CommNet;
using ns37;
using ns9;

public class ModuleScienceContainer : PartModule, IScienceDataContainer
{
	[KSPField]
	public int capacity;

	public List<ScienceData> dataStore = new List<ScienceData>();

	public List<ScienceData> pagesWithDialogs;

	[KSPField]
	public string reviewActionName = Localizer.Format("#autoLOC_6001313");

	[KSPField]
	public string storeActionName = Localizer.Format("#autoLOC_6001314");

	[KSPField]
	public string collectActionName = Localizer.Format("#autoLOC_238018");

	[KSPField]
	public bool evaOnlyStorage = true;

	[KSPField]
	public bool canBeTransferredToInVessel;

	[KSPField]
	public bool canTransferInVessel;

	[KSPField]
	public float storageRange = 1f;

	[KSPField]
	public bool allowRepeatedSubjects;

	[KSPField]
	public bool dataIsRecoverable = true;

	[KSPField]
	public bool dataIsCollectable = true;

	[KSPField]
	public bool dataIsStorable = true;

	[KSPField]
	public bool CollectOnlyOwnData;

	[KSPField(guiActive = false, guiName = "#autoLOC_6001352")]
	public string status = "";

	[KSPField]
	public bool showStatus;

	public override void OnLoad(ConfigNode node)
	{
		dataStore.Clear();
		ConfigNode[] nodes = node.GetNodes("ScienceData");
		int i = 0;
		for (int num = nodes.Length; i < num; i++)
		{
			ConfigNode node2 = nodes[i];
			dataStore.Add(new ScienceData(node2));
		}
		nodes = node.GetNodes("CommsData");
		int j = 0;
		for (int num2 = nodes.Length; j < num2; j++)
		{
			ConfigNode node2 = nodes[j];
			dataStore.Add(new ScienceData(node2));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		int i = 0;
		for (int count = dataStore.Count; i < count; i++)
		{
			dataStore[i].Save(node.AddNode(dataIsRecoverable ? "ScienceData" : "CommsData"));
		}
	}

	public override void OnStart(StartState state)
	{
		pagesWithDialogs = new List<ScienceData>();
		updateModuleUI();
		GameEvents.onPartActionUICreate.Add(onPartActionUIOpened);
	}

	public void OnDestroy()
	{
		GameEvents.onPartActionUICreate.Remove(onPartActionUIOpened);
	}

	public void onPartActionUIOpened(Part p)
	{
		if (p == base.part)
		{
			updateModuleUI();
		}
	}

	public void updateModuleUI()
	{
		int count = dataStore.Count;
		base.Events["ReviewDataEvent"].active = count > 0 && pagesWithDialogs.Count != count;
		base.Events["ReviewDataEvent"].guiName = reviewActionName + " (" + count + ")";
		base.Fields["status"].guiActive = (showStatus && count == 0) || capacity > 0;
		if (capacity > 0)
		{
			status = Localizer.Format("#autoLOC_237286", count, capacity);
		}
		else
		{
			status = Localizer.Format("#autoLOC_237288", count);
		}
		int activeVesselDataCount = GetActiveVesselDataCount();
		int storedDataCount = GetStoredDataCount();
		base.Events["StoreDataExternalEvent"].guiName = storeActionName + " (" + activeVesselDataCount + ")";
		base.Events["StoreDataExternalEvent"].active = activeVesselDataCount > 0 && dataIsStorable;
		base.Events["StoreDataExternalEvent"].externalToEVAOnly = evaOnlyStorage;
		base.Events["StoreDataExternalEvent"].unfocusedRange = storageRange;
		base.Events["CollectDataExternalEvent"].guiName = collectActionName + " (" + storedDataCount + ")";
		bool flag = storedDataCount > 0 && dataIsCollectable;
		base.Events["CollectDataExternalEvent"].active = flag;
		base.Events["TransferDataEvent"].active = flag && canTransferInVessel;
		base.Events["CollectAllEvent"].active = canTransferInVessel && activeVesselDataCount > 0 && activeVesselDataCount > dataStore.Count;
		base.Events["CollectDataExternalEvent"].externalToEVAOnly = evaOnlyStorage;
		base.Events["CollectDataExternalEvent"].unfocusedRange = storageRange;
	}

	public int GetActiveVesselDataCount()
	{
		if (FlightGlobals.ActiveVessel != null)
		{
			int num = 0;
			Vessel activeVessel = FlightGlobals.ActiveVessel;
			int count = activeVessel.parts.Count;
			while (count-- > 0)
			{
				Part part = activeVessel.parts[count];
				int count2 = part.Modules.Count;
				while (count2-- > 0)
				{
					if (part.Modules[count2] is IScienceDataContainer scienceDataContainer)
					{
						num += scienceDataContainer.GetScienceCount();
					}
				}
			}
			return num;
		}
		return 0;
	}

	public int GetStoredDataCount()
	{
		if (CollectOnlyOwnData)
		{
			return dataStore.Count;
		}
		int num = 0;
		int count = base.part.Modules.Count;
		while (count-- > 0)
		{
			if (base.part.Modules[count] is IScienceDataContainer scienceDataContainer)
			{
				num += scienceDataContainer.GetScienceCount();
			}
		}
		return num;
	}

	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 1f)]
	public void StoreDataExternalEvent()
	{
		List<IScienceDataContainer> list = FlightGlobals.ActiveVessel.FindPartModulesImplementing<IScienceDataContainer>();
		bool flag = false;
		int count = list.Count;
		while (count-- > 0)
		{
			if (list[count].GetScienceCount() > 0)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (StoreData(list, dumpRepeats: false))
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237370", base.part.partInfo.title), 5f, ScreenMessageStyle.UPPER_LEFT);
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237374", base.part.partInfo.title), 5f, ScreenMessageStyle.UPPER_LEFT);
			}
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237379", base.part.partInfo.title), 3f, ScreenMessageStyle.UPPER_CENTER);
		}
		updateModuleUI();
	}

	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 1f)]
	public void CollectDataExternalEvent()
	{
		TransferToContainer(FlightGlobals.ActiveVessel.FindPartModuleImplementing<ModuleScienceContainer>(), FlightGlobals.ActiveVessel.GetDisplayName());
		updateModuleUI();
	}

	public void TransferToContainer(ModuleScienceContainer activeVesselContainer, string targetName)
	{
		List<IScienceDataContainer> list = ((!CollectOnlyOwnData) ? base.part.FindModulesImplementing<IScienceDataContainer>() : new List<IScienceDataContainer> { this });
		if (activeVesselContainer != null)
		{
			bool flag = false;
			int count = list.Count;
			while (count-- > 0)
			{
				if (list[count].GetScienceCount() > 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				if (activeVesselContainer.StoreData(list, dumpRepeats: false))
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237418", activeVesselContainer.part.partInfo.title), 5f, ScreenMessageStyle.UPPER_LEFT);
				}
				else
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237422", activeVesselContainer.part.partInfo.title), 5f, ScreenMessageStyle.UPPER_LEFT);
				}
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237427", activeVesselContainer.part.partInfo.title), 3f, ScreenMessageStyle.UPPER_CENTER);
			}
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237432", base.part.partInfo.title, targetName), 5f, ScreenMessageStyle.UPPER_CENTER);
		}
	}

	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001312")]
	public void TransferDataEvent()
	{
		if (PartItemTransfer.Instance != null)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237449"), 3f, ScreenMessageStyle.UPPER_CENTER);
		}
		else
		{
			ExperimentTransfer.Create(base.part, this, OnDataTransfer);
		}
	}

	[KSPAction("#autoLOC_6001311")]
	public void CollectAllAction(KSPActionParam actParams)
	{
		updateModuleUI();
		if (base.Events["CollectAllEvent"].active)
		{
			CollectAllEvent();
		}
	}

	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001808")]
	public void CollectAllEvent()
	{
		List<IScienceDataContainer> list = new List<IScienceDataContainer>();
		int count = base.vessel.Parts.Count;
		while (count-- > 0)
		{
			Part part = base.vessel.Parts[count];
			int count2 = part.Modules.Count;
			while (count2-- > 0)
			{
				if (part.Modules[count2] is IScienceDataContainer scienceDataContainer && scienceDataContainer != this && scienceDataContainer.GetScienceCount() > 0)
				{
					list.Add(scienceDataContainer);
				}
			}
		}
		if (list.Count > 0)
		{
			if (StoreData(list, dumpRepeats: false))
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237485", base.part.partInfo.title), 5f, ScreenMessageStyle.UPPER_LEFT);
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237489", base.part.partInfo.title), 5f, ScreenMessageStyle.UPPER_LEFT);
			}
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237494", base.part.partInfo.title), 3f, ScreenMessageStyle.UPPER_CENTER);
		}
		updateModuleUI();
	}

	public void OnDataTransfer(PartItemTransfer.DismissAction dma, Part containerPart)
	{
		if (dma == PartItemTransfer.DismissAction.ItemMoved && !(containerPart == null))
		{
			TransferToContainer(containerPart.FindModuleImplementing<ModuleScienceContainer>(), containerPart.partInfo.title);
			updateModuleUI();
		}
	}

	public bool StoreData(List<IScienceDataContainer> containers, bool dumpRepeats, bool excludeScienceContainers = false, bool scienceContainerPriority = false)
	{
		if (excludeScienceContainers)
		{
			List<IScienceDataContainer> list = new List<IScienceDataContainer>();
			int count = containers.Count;
			while (count-- > 0)
			{
				if (containers[count] as ModuleScienceContainer != null)
				{
					list.Add(containers[count]);
				}
			}
			int count2 = list.Count;
			while (count2-- > 0)
			{
				containers.Remove(list[count2]);
			}
		}
		sortContainerList(containers, scienceContainerPriority);
		int num = 0;
		int count3 = containers.Count;
		for (int i = 0; i < count3; i++)
		{
			IScienceDataContainer scienceDataContainer = containers[i];
			int scienceCount = scienceDataContainer.GetScienceCount();
			if (scienceCount == 0)
			{
				continue;
			}
			if (scienceCount + dataStore.Count > capacity && (float)capacity != 0f)
			{
				num++;
				continue;
			}
			ScienceData[] data = scienceDataContainer.GetData();
			int j = 0;
			for (int num2 = data.Length; j < num2; j++)
			{
				if (data[j] == null)
				{
					continue;
				}
				bool flag;
				if (flag = !allowRepeatedSubjects)
				{
					flag = false;
					int count4 = dataStore.Count;
					while (count4-- > 0)
					{
						if (dataStore[count4].subjectID == data[j].subjectID)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001022", base.part.partInfo.title, data[j].title), 5f, ScreenMessageStyle.UPPER_LEFT);
					if (dumpRepeats)
					{
						scienceDataContainer.DumpData(data[j]);
					}
					else
					{
						num++;
					}
				}
				else
				{
					scienceDataContainer.DumpData(data[j]);
					AddData(data[j]);
				}
			}
		}
		return num == 0;
	}

	public void sortContainerList(List<IScienceDataContainer> containers, bool scienceContainerPriority)
	{
		int count = containers.Count;
		while (count-- > 0)
		{
			int num = int.MaxValue;
			int index = 0;
			for (int num2 = count; num2 >= 0; num2--)
			{
				int scienceCount = containers[num2].GetScienceCount();
				if (scienceCount < num)
				{
					num = scienceCount;
					index = num2;
				}
			}
			IScienceDataContainer value = containers[count];
			containers[count] = containers[index];
			containers[index] = value;
		}
		if (scienceContainerPriority)
		{
			return;
		}
		List<IScienceDataContainer> list = new List<IScienceDataContainer>();
		List<IScienceDataContainer> list2 = new List<IScienceDataContainer>();
		int count2 = containers.Count;
		while (count2-- > 0)
		{
			if (containers[count2] as ModuleScienceContainer != null)
			{
				list2.Add(containers[count2]);
			}
			else
			{
				list.Add(containers[count2]);
			}
		}
		int num3 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			containers[num3] = list[i];
			num3++;
		}
		for (int j = 0; j < list2.Count; j++)
		{
			containers[num3] = list2[j];
			num3++;
		}
	}

	public bool AddData(ScienceData data)
	{
		if (dataStore.Count >= capacity && capacity != 0)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237617", base.part.partInfo.title, data.title, dataStore.Count, capacity), 5f, ScreenMessageStyle.UPPER_LEFT);
			return false;
		}
		data.container = base.part.flightID;
		dataStore.Add(data);
		updateModuleUI();
		if (capacity != 0)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237607", base.part.partInfo.title, data.title, dataStore.Count, capacity), 5f, ScreenMessageStyle.UPPER_LEFT);
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237611", base.part.partInfo.title, data.title), 5f, ScreenMessageStyle.UPPER_LEFT);
		}
		return true;
	}

	public bool HasData(ScienceData data)
	{
		int count = dataStore.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!(dataStore[count].subjectID == data.subjectID));
		return true;
	}

	public void RemoveData(ScienceData data)
	{
		dataStore.Remove(data);
		updateModuleUI();
	}

	public ScienceData[] GetData()
	{
		return dataStore.ToArray();
	}

	public void ReturnData(ScienceData data)
	{
		int count = dataStore.Count;
		if (count >= capacity && capacity != 0)
		{
			return;
		}
		if (!allowRepeatedSubjects)
		{
			int index = count;
			while (index-- > 0)
			{
				if (dataStore[index].subjectID == data.subjectID)
				{
					return;
				}
			}
		}
		dataStore.Add(data);
		updateModuleUI();
	}

	public void DumpData(ScienceData data)
	{
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237662", base.part.partInfo.title, data.title), 5f, ScreenMessageStyle.UPPER_LEFT);
		dataStore.Remove(data);
		updateModuleUI();
	}

	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001439")]
	public void ReviewDataEvent()
	{
		reviewData();
		updateModuleUI();
	}

	public void ReviewData()
	{
		reviewData();
		updateModuleUI();
	}

	public void reviewData()
	{
		int count = dataStore.Count;
		if (count > 0)
		{
			for (int i = 0; i < count; i++)
			{
				ReviewDataItem(dataStore[i]);
			}
		}
	}

	public void ReviewDataItem(ScienceData data)
	{
		if (!pagesWithDialogs.Contains(data))
		{
			pagesWithDialogs.Add(data);
			ExperimentsResultDialog.DisplayResult(new ExperimentResultDialogPage(base.part, data, data.baseTransmitValue, data.transmitBonus, showTransmitWarning: false, "", showResetOption: false, new ScienceLabSearch(base.vessel, data), discardData, keepData, sendDataToComms, sendDataToLab));
		}
	}

	public void discardData(ScienceData pageData)
	{
		pagesWithDialogs.Remove(pageData);
		RemoveData(pageData);
		updateModuleUI();
	}

	public void keepData(ScienceData pageData)
	{
		pagesWithDialogs.Remove(pageData);
		updateModuleUI();
	}

	public void sendDataToComms(ScienceData pageData)
	{
		pagesWithDialogs.Remove(pageData);
		IScienceDataTransmitter bestTransmitter = ScienceUtil.GetBestTransmitter(base.vessel);
		if (bestTransmitter != null)
		{
			List<ScienceData> list = new List<ScienceData>();
			list.Add(pageData);
			bestTransmitter.TransmitData(list);
			RemoveData(pageData);
		}
		else if (CommNetScenario.CommNetEnabled)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237738"), 3f, ScreenMessageStyle.UPPER_CENTER);
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_237740"), 3f, ScreenMessageStyle.UPPER_CENTER);
		}
		updateModuleUI();
	}

	public void sendDataToLab(ScienceData pageData)
	{
		pagesWithDialogs.Remove(pageData);
		ScienceLabSearch scienceLabSearch = new ScienceLabSearch(base.vessel, pageData);
		if (scienceLabSearch.NextLabForDataFound)
		{
			StartCoroutine(scienceLabSearch.NextLabForData.ProcessData(pageData));
			RemoveData(pageData);
		}
		else
		{
			scienceLabSearch.PostErrorToScreen();
			updateModuleUI();
		}
	}

	public int GetScienceCount()
	{
		return dataStore.Count;
	}

	public bool IsRerunnable()
	{
		return true;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003058");
	}
}
