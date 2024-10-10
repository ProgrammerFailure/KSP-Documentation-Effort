using System;
using System.Collections.Generic;
using ns11;
using UnityEngine;

[Serializable]
public class DeltaVEngineStageSet
{
	[Serializable]
	public class DeltaVStageInfoEntry
	{
		public DeltaVStageInfo stageInfo_InstanceOne;

		public DeltaVStageInfo stageInfo_InstanceTwo;

		public VesselDeltaV vesselDeltaV;

		public DeltaVStageInfoEntry(VesselDeltaV vesselDeltaV)
		{
			this.vesselDeltaV = vesselDeltaV;
		}
	}

	public class StageSortingInstanceOne : IComparer<DeltaVStageInfoEntry>
	{
		public int Compare(DeltaVStageInfoEntry x, DeltaVStageInfoEntry y)
		{
			if (x.stageInfo_InstanceOne == null)
			{
				return 1;
			}
			if (y.stageInfo_InstanceOne == null)
			{
				return -1;
			}
			if (x.stageInfo_InstanceOne.stage < y.stageInfo_InstanceOne.stage)
			{
				return 1;
			}
			if (x.stageInfo_InstanceOne.stage > y.stageInfo_InstanceOne.stage)
			{
				return -1;
			}
			return 0;
		}
	}

	[SerializeField]
	public List<DeltaVStageInfoEntry> stageInfoEntries;

	[SerializeField]
	public List<DeltaVEngineInfo> engineInfo_InstanceOne;

	[SerializeField]
	public List<DeltaVEngineInfo> engineInfo_InstanceTwo;

	public int operatingIndex;

	public int workingIndex;

	public int flightSceneOperatingIndex;

	public VesselDeltaV vesselDeltaV;

	public List<int> payloadStages;

	public IComparer<DeltaVStageInfo> stageSorting = new RUIutils.FuncComparer<DeltaVStageInfo>((DeltaVStageInfo r1, DeltaVStageInfo r2) => RUIutils.SortAscDescPrimarySecondary(false, r1.stage.CompareTo(r2.stage)));

	public virtual List<DeltaVStageInfo> WorkingStageInfo
	{
		get
		{
			List<DeltaVStageInfo> list = new List<DeltaVStageInfo>();
			if (workingIndex == 0)
			{
				for (int i = 0; i < stageInfoEntries.Count; i++)
				{
					if (stageInfoEntries[i].stageInfo_InstanceOne != null)
					{
						list.Add(stageInfoEntries[i].stageInfo_InstanceOne);
					}
				}
				list.Sort(stageSorting);
				return list;
			}
			for (int j = 0; j < stageInfoEntries.Count; j++)
			{
				if (stageInfoEntries[j].stageInfo_InstanceTwo != null)
				{
					list.Add(stageInfoEntries[j].stageInfo_InstanceTwo);
				}
			}
			list.Sort(stageSorting);
			return list;
		}
	}

	public virtual List<DeltaVStageInfo> OperatingStageInfo
	{
		get
		{
			List<DeltaVStageInfo> list = new List<DeltaVStageInfo>();
			if (HighLogic.LoadedSceneIsFlight)
			{
				for (int i = 0; i < stageInfoEntries.Count; i++)
				{
					if (flightSceneOperatingIndex == 0)
					{
						if (stageInfoEntries[i].stageInfo_InstanceOne != null && stageInfoEntries[i].stageInfo_InstanceOne.stage < StageManager.CurrentStage)
						{
							list.Add(stageInfoEntries[i].stageInfo_InstanceOne);
						}
					}
					else if (stageInfoEntries[i].stageInfo_InstanceTwo != null && stageInfoEntries[i].stageInfo_InstanceTwo.stage < StageManager.CurrentStage)
					{
						list.Add(stageInfoEntries[i].stageInfo_InstanceTwo);
					}
					if (operatingIndex == 0)
					{
						if (stageInfoEntries[i].stageInfo_InstanceOne != null && stageInfoEntries[i].stageInfo_InstanceOne.stage == StageManager.CurrentStage)
						{
							list.Add(stageInfoEntries[i].stageInfo_InstanceOne);
						}
					}
					else if (stageInfoEntries[i].stageInfo_InstanceTwo != null && stageInfoEntries[i].stageInfo_InstanceTwo.stage == StageManager.CurrentStage)
					{
						list.Add(stageInfoEntries[i].stageInfo_InstanceTwo);
					}
				}
				list.Sort(stageSorting);
				return list;
			}
			if (operatingIndex == 0)
			{
				for (int j = 0; j < stageInfoEntries.Count; j++)
				{
					if (stageInfoEntries[j].stageInfo_InstanceOne != null)
					{
						list.Add(stageInfoEntries[j].stageInfo_InstanceOne);
					}
				}
				list.Sort(stageSorting);
				return list;
			}
			for (int k = 0; k < stageInfoEntries.Count; k++)
			{
				if (stageInfoEntries[k].stageInfo_InstanceTwo != null)
				{
					list.Add(stageInfoEntries[k].stageInfo_InstanceTwo);
				}
			}
			list.Sort(stageSorting);
			return list;
		}
	}

	public virtual List<DeltaVEngineInfo> WorkingEngineInfo
	{
		get
		{
			if (engineInfo_InstanceOne == null)
			{
				engineInfo_InstanceOne = new List<DeltaVEngineInfo>();
			}
			if (engineInfo_InstanceTwo == null)
			{
				engineInfo_InstanceTwo = new List<DeltaVEngineInfo>();
			}
			if (vesselDeltaV.SimulationRunning)
			{
				if (workingIndex == 0)
				{
					return engineInfo_InstanceOne;
				}
				return engineInfo_InstanceTwo;
			}
			if (operatingIndex == 0)
			{
				return engineInfo_InstanceOne;
			}
			return engineInfo_InstanceTwo;
		}
	}

	public virtual List<DeltaVEngineInfo> OperatingEngineInfo
	{
		get
		{
			if (engineInfo_InstanceOne == null)
			{
				engineInfo_InstanceOne = new List<DeltaVEngineInfo>();
			}
			if (engineInfo_InstanceTwo == null)
			{
				engineInfo_InstanceTwo = new List<DeltaVEngineInfo>();
			}
			if (operatingIndex == 0)
			{
				return engineInfo_InstanceOne;
			}
			return engineInfo_InstanceTwo;
		}
	}

	public DeltaVEngineStageSet(VesselDeltaV vesselDeltaV)
	{
		stageInfoEntries = new List<DeltaVStageInfoEntry>();
		engineInfo_InstanceOne = new List<DeltaVEngineInfo>();
		engineInfo_InstanceTwo = new List<DeltaVEngineInfo>();
		payloadStages = new List<int>();
		workingIndex = 0;
		operatingIndex = 0;
		flightSceneOperatingIndex = 0;
		this.vesselDeltaV = vesselDeltaV;
	}

	public virtual void AddStageInfo(DeltaVStageInfo stageInfo)
	{
		SetPayload(stageInfo);
		DeltaVStageInfoEntry deltaVStageInfoEntry = new DeltaVStageInfoEntry(stageInfo.vesselDeltaV);
		stageInfoEntries.Add(deltaVStageInfoEntry);
		if (vesselDeltaV.SimulationRunning)
		{
			if (workingIndex == 0)
			{
				deltaVStageInfoEntry.stageInfo_InstanceOne = stageInfo;
			}
			else
			{
				deltaVStageInfoEntry.stageInfo_InstanceTwo = stageInfo;
			}
		}
		else if (operatingIndex == 0)
		{
			deltaVStageInfoEntry.stageInfo_InstanceOne = stageInfo;
		}
		else
		{
			deltaVStageInfoEntry.stageInfo_InstanceTwo = stageInfo;
		}
	}

	public virtual void UpdateStageInfo(int stage)
	{
		if (!(StageManager.Instance != null))
		{
			return;
		}
		int num = -1;
		for (int i = 0; i < stageInfoEntries.Count; i++)
		{
			if (stageInfoEntries[i].stageInfo_InstanceOne == null || stageInfoEntries[i].stageInfo_InstanceOne.stage != stage)
			{
				if (stageInfoEntries[i].stageInfo_InstanceTwo != null && stageInfoEntries[i].stageInfo_InstanceTwo.stage == stage)
				{
					num = i;
					break;
				}
				continue;
			}
			num = i;
			break;
		}
		if (num != -1)
		{
			if (vesselDeltaV.SimulationRunning)
			{
				if (workingIndex == 0)
				{
					if (stageInfoEntries[num].stageInfo_InstanceOne != null)
					{
						stageInfoEntries[num].stageInfo_InstanceOne.Reset(stage, vesselDeltaV);
					}
					else if (vesselDeltaV.Vessel != null)
					{
						stageInfoEntries[num].stageInfo_InstanceOne = new DeltaVStageInfo(vesselDeltaV.Vessel, stage, vesselDeltaV);
					}
					else
					{
						stageInfoEntries[num].stageInfo_InstanceOne = new DeltaVStageInfo(vesselDeltaV.Ship, stage, vesselDeltaV);
					}
					SetPayload(stageInfoEntries[num].stageInfo_InstanceOne);
				}
				else
				{
					if (stageInfoEntries[num].stageInfo_InstanceTwo != null)
					{
						stageInfoEntries[num].stageInfo_InstanceTwo.Reset(stage, vesselDeltaV);
					}
					else if (vesselDeltaV.Vessel != null)
					{
						stageInfoEntries[num].stageInfo_InstanceTwo = new DeltaVStageInfo(vesselDeltaV.Vessel, stage, vesselDeltaV);
					}
					else
					{
						stageInfoEntries[num].stageInfo_InstanceTwo = new DeltaVStageInfo(vesselDeltaV.Ship, stage, vesselDeltaV);
					}
					SetPayload(stageInfoEntries[num].stageInfo_InstanceTwo);
				}
			}
			else if (operatingIndex == 0)
			{
				if (stageInfoEntries[num].stageInfo_InstanceOne != null)
				{
					stageInfoEntries[num].stageInfo_InstanceOne.Reset(stage, vesselDeltaV);
				}
				else if (vesselDeltaV.Vessel != null)
				{
					stageInfoEntries[num].stageInfo_InstanceOne = new DeltaVStageInfo(vesselDeltaV.Vessel, stage, vesselDeltaV);
				}
				else
				{
					stageInfoEntries[num].stageInfo_InstanceOne = new DeltaVStageInfo(vesselDeltaV.Ship, stage, vesselDeltaV);
				}
				SetPayload(stageInfoEntries[num].stageInfo_InstanceOne);
			}
			else
			{
				if (stageInfoEntries[num].stageInfo_InstanceTwo != null)
				{
					stageInfoEntries[num].stageInfo_InstanceTwo.Reset(stage, vesselDeltaV);
				}
				else if (vesselDeltaV.Vessel != null)
				{
					stageInfoEntries[num].stageInfo_InstanceTwo = new DeltaVStageInfo(vesselDeltaV.Vessel, stage, vesselDeltaV);
				}
				else
				{
					stageInfoEntries[num].stageInfo_InstanceTwo = new DeltaVStageInfo(vesselDeltaV.Ship, stage, vesselDeltaV);
				}
				SetPayload(stageInfoEntries[num].stageInfo_InstanceTwo);
			}
		}
		else if (vesselDeltaV.Vessel != null)
		{
			AddStageInfo(new DeltaVStageInfo(vesselDeltaV.Vessel, stage, vesselDeltaV));
		}
		else
		{
			AddStageInfo(new DeltaVStageInfo(vesselDeltaV.Ship, stage, vesselDeltaV));
		}
	}

	public virtual void UpdateStageInfo()
	{
		if (StageManager.Instance != null)
		{
			RemoveStaleStages();
			for (int num = StageManager.LastStage; num >= 0; num--)
			{
				UpdateStageInfo(num);
			}
			RemoveStagedStages();
		}
		SortStages();
		payloadStages.Clear();
	}

	public virtual void RemoveStaleStages()
	{
		if (!(StageManager.Instance != null))
		{
			return;
		}
		int lastStage = StageManager.LastStage;
		int count = stageInfoEntries.Count;
		while (count-- > 0)
		{
			if (stageInfoEntries[count].stageInfo_InstanceOne != null)
			{
				if (stageInfoEntries[count].stageInfo_InstanceOne.stage > lastStage)
				{
					stageInfoEntries.RemoveAt(count);
				}
			}
			else if (stageInfoEntries[count].stageInfo_InstanceTwo != null)
			{
				if (stageInfoEntries[count].stageInfo_InstanceTwo.stage > lastStage)
				{
					stageInfoEntries.RemoveAt(count);
				}
			}
			else
			{
				stageInfoEntries.RemoveAt(count);
			}
		}
	}

	public virtual void SetPayload(DeltaVStageInfo stageInfo)
	{
		if (stageInfo != null)
		{
			if (payloadStages.Contains(stageInfo.stage))
			{
				stageInfo.payloadStage = true;
			}
			else
			{
				stageInfo.payloadStage = false;
			}
		}
	}

	public virtual void RemoveStagedStages(bool sortStages = false)
	{
		if (StageManager.Instance != null)
		{
			for (int num = stageInfoEntries.Count - 1; num >= 0; num--)
			{
				if (vesselDeltaV.SimulationRunning)
				{
					if (workingIndex == 0)
					{
						if (stageInfoEntries[num].stageInfo_InstanceOne != null && stageInfoEntries[num].stageInfo_InstanceOne.stage > StageManager.LastStage)
						{
							stageInfoEntries[num].stageInfo_InstanceOne = null;
						}
					}
					else if (stageInfoEntries[num].stageInfo_InstanceTwo != null && stageInfoEntries[num].stageInfo_InstanceTwo.stage > StageManager.LastStage)
					{
						stageInfoEntries[num].stageInfo_InstanceTwo = null;
					}
				}
				else if (operatingIndex == 0)
				{
					if (stageInfoEntries[num].stageInfo_InstanceOne != null && stageInfoEntries[num].stageInfo_InstanceOne.stage > StageManager.LastStage)
					{
						stageInfoEntries[num].stageInfo_InstanceOne = null;
					}
				}
				else if (stageInfoEntries[num].stageInfo_InstanceTwo != null && stageInfoEntries[num].stageInfo_InstanceTwo.stage > StageManager.LastStage)
				{
					stageInfoEntries[num].stageInfo_InstanceTwo = null;
				}
			}
		}
		if (sortStages)
		{
			SortStages();
		}
	}

	public virtual void SortStages()
	{
		StageSortingInstanceOne comparer = new StageSortingInstanceOne();
		stageInfoEntries.Sort(comparer);
	}

	public virtual void AddEngineWorkingSet(DeltaVEngineInfo engineInfo)
	{
		try
		{
			if (workingIndex == 0)
			{
				engineInfo_InstanceOne.Add(engineInfo);
			}
			else
			{
				engineInfo_InstanceTwo.Add(engineInfo);
			}
		}
		catch (Exception value)
		{
			Debug.LogError("DeltaVEngineStageSet]: Failed to add Engine " + engineInfo.engine.part.persistentId + " " + engineInfo.engine.part.partInfo.title + " to Working Set " + ((workingIndex == 0) ? "One" : "Two"));
			Console.WriteLine(value);
		}
	}

	public virtual void RemoveInvalidEnginesWorkingSet()
	{
		if (workingIndex == 0)
		{
			int count = engineInfo_InstanceOne.Count;
			while (count-- > 0)
			{
				if (vesselDeltaV.ActiveMode == VesselDeltaV.Mode.Vessel && vesselDeltaV.Vessel != null)
				{
					if (engineInfo_InstanceOne[count].engine == null || engineInfo_InstanceOne[count].engine.part == null || !vesselDeltaV.Vessel.parts.Contains(engineInfo_InstanceOne[count].engine.part))
					{
						engineInfo_InstanceOne.RemoveAt(count);
					}
				}
				else if (vesselDeltaV.ActiveMode == VesselDeltaV.Mode.Ship && vesselDeltaV.Ship != null && (engineInfo_InstanceOne[count].engine == null || engineInfo_InstanceOne[count].engine.part == null || !vesselDeltaV.Ship.parts.Contains(engineInfo_InstanceOne[count].engine.part)))
				{
					engineInfo_InstanceOne.RemoveAt(count);
				}
			}
			return;
		}
		int count2 = engineInfo_InstanceTwo.Count;
		while (count2-- > 0)
		{
			if (vesselDeltaV.ActiveMode == VesselDeltaV.Mode.Vessel && vesselDeltaV.Vessel != null)
			{
				if (engineInfo_InstanceTwo[count2].engine == null || engineInfo_InstanceTwo[count2].engine.part == null || !vesselDeltaV.Vessel.parts.Contains(engineInfo_InstanceTwo[count2].engine.part))
				{
					engineInfo_InstanceTwo.RemoveAt(count2);
				}
			}
			else if (vesselDeltaV.ActiveMode == VesselDeltaV.Mode.Ship && vesselDeltaV.Ship != null && (engineInfo_InstanceTwo[count2].engine == null || engineInfo_InstanceTwo[count2].engine.part == null || !vesselDeltaV.Ship.parts.Contains(engineInfo_InstanceTwo[count2].engine.part)))
			{
				engineInfo_InstanceTwo.RemoveAt(count2);
			}
		}
	}
}
