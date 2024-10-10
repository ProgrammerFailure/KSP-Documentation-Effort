using System.Collections.Generic;
using UnityEngine;

namespace ns32;

public class VesselDebugDeltaVInfo
{
	public DeltaVStageInfo stageInfo;

	public GameObject stageItem;

	public ScreenDeltaVPartsListInfo partsListInfo;

	public GameObject partsList;

	public DictionaryValueList<VesselDebugDeltaVEngineInfo, ScreenDeltaVEngineInfo> engineItems;

	public ScreenDeltaVStageInfo screenStageInfo;

	public VesselDebugDeltaVInfo(int stage)
	{
		engineItems = new DictionaryValueList<VesselDebugDeltaVEngineInfo, ScreenDeltaVEngineInfo>();
	}

	public bool Create(GameObject stageItemPrefab, RectTransform contentParent, GameObject engineItemPrefab, GameObject propellantItemPrefab, GameObject partsListPrefab, DeltaVStageInfo stageInfo, CalcType type)
	{
		this.stageInfo = stageInfo;
		stageItem = Object.Instantiate(stageItemPrefab);
		stageItem.transform.SetParent(contentParent);
		stageItem.transform.localScale = Vector3.one;
		screenStageInfo = stageItem.GetComponent<ScreenDeltaVStageInfo>();
		partsList = Object.Instantiate(partsListPrefab);
		partsList.transform.SetParent(contentParent);
		partsList.transform.localScale = Vector3.one;
		partsListInfo = partsList.GetComponent<ScreenDeltaVPartsListInfo>();
		partsListInfo.UpdateData(stageInfo.GetPartDisplayInfo());
		if (stageInfo.enginesActiveInStage == null)
		{
			return false;
		}
		for (int i = 0; i < stageInfo.enginesActiveInStage.Count; i++)
		{
			VesselDebugDeltaVEngineInfo vesselDebugDeltaVEngineInfo = new VesselDebugDeltaVEngineInfo();
			List<DeltaVPropellantInfo> stagePropInfo = new List<DeltaVPropellantInfo>();
			if (stageInfo.enginesActiveInStage[i].stageBurnTotals.ContainsKey(stageInfo.stage))
			{
				stagePropInfo = stageInfo.enginesActiveInStage[i].stageBurnTotals[stageInfo.stage].propellantInfo;
			}
			ScreenDeltaVEngineInfo val = vesselDebugDeltaVEngineInfo.Create(engineItemPrefab, contentParent, propellantItemPrefab, stageInfo.enginesActiveInStage[i], stagePropInfo, type, stageInfo.stage);
			engineItems.Add(vesselDebugDeltaVEngineInfo, val);
		}
		return true;
	}

	public bool Update(DeltaVStageInfo stageInfo, CalcType type, string calctypeDesc, bool showEngines, bool showParts, bool showAllStages)
	{
		if (stageInfo.enginesActiveInStage == null)
		{
			return false;
		}
		if (!showAllStages && stageInfo.enginesActiveInStage.Count <= 0)
		{
			SetActive(active: false, partsActive: false, enginesActive: false);
		}
		else
		{
			if (!(screenStageInfo != null))
			{
				return false;
			}
			screenStageInfo.UpdateData(stageInfo, type);
			if (!(partsListInfo != null))
			{
				return false;
			}
			if (showParts)
			{
				partsListInfo.UpdateData(stageInfo.GetPartDisplayInfo());
			}
			if (showEngines)
			{
				int count = engineItems.KeysList.Count;
				while (count-- > 0)
				{
					if (engineItems.KeysList[count].engineInfo == null || engineItems.KeysList[count].engineInfo.engine == null || !stageInfo.enginesActiveInStage.Contains(engineItems.KeysList[count].engineInfo))
					{
						engineItems.KeysList[count].Destroy();
						engineItems.Remove(engineItems.KeysList[count]);
					}
				}
				for (int i = 0; i < stageInfo.enginesActiveInStage.Count; i++)
				{
					List<DeltaVPropellantInfo> stagePropInfo = new List<DeltaVPropellantInfo>();
					if (stageInfo.enginesActiveInStage[i].stageBurnTotals.ContainsKey(stageInfo.stage))
					{
						stagePropInfo = stageInfo.enginesActiveInStage[i].stageBurnTotals[stageInfo.stage].propellantInfo;
					}
					bool flag = false;
					for (int j = 0; j < engineItems.KeysList.Count; j++)
					{
						if (engineItems.KeysList[j].engineInfo == stageInfo.enginesActiveInStage[i])
						{
							if (engineItems.KeysList[j].Update(stageInfo.enginesActiveInStage[i], stagePropInfo, type, calctypeDesc, stageInfo.stage))
							{
								flag = true;
								break;
							}
							return false;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			SetActive(active: true, showParts, showEngines);
		}
		return true;
	}

	public void SetActive(bool active, bool partsActive, bool enginesActive)
	{
		if (screenStageInfo != null)
		{
			screenStageInfo.gameObject.SetActive(active);
		}
		if (partsListInfo != null)
		{
			partsListInfo.gameObject.SetActive(partsActive);
		}
		for (int i = 0; i < engineItems.KeysList.Count; i++)
		{
			engineItems.KeysList[i].SetActive(enginesActive);
		}
	}

	public void Destroy()
	{
		if (stageItem != null)
		{
			stageItem.DestroyGameObject();
		}
		stageInfo = null;
		if (partsList != null)
		{
			partsList.DestroyGameObject();
		}
		int count = engineItems.Count;
		while (count-- > 0)
		{
			engineItems.KeyAt(count).Destroy();
		}
		engineItems.Clear();
	}

	public int SetSiblingsUIIndex(int index)
	{
		screenStageInfo.transform.SetSiblingIndex(index);
		if (partsListInfo != null)
		{
			index++;
			partsListInfo.transform.SetSiblingIndex(index);
		}
		for (int i = 0; i < engineItems.ValuesList.Count; i++)
		{
			index++;
			engineItems.ValuesList[i].transform.SetSiblingIndex(index);
		}
		for (int j = 0; j < engineItems.KeysList.Count; j++)
		{
			index = engineItems.KeysList[j].SetPropellantUISiblings(engineItems.ValuesList[j].transform.GetSiblingIndex());
		}
		return index;
	}
}
