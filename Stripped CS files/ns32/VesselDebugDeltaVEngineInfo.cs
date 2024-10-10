using System.Collections.Generic;
using UnityEngine;

namespace ns32;

public class VesselDebugDeltaVEngineInfo
{
	public DeltaVEngineInfo engineInfo;

	public GameObject engineItem;

	public DictionaryValueList<DeltaVPropellantInfo, ScreenDeltaVPropellantInfo> propellantItems;

	public ScreenDeltaVEngineInfo screenEngineInfo;

	public GameObject propellantItemPrefab;

	public RectTransform contentParent;

	public bool setUpComplete;

	public VesselDebugDeltaVEngineInfo()
	{
		propellantItems = new DictionaryValueList<DeltaVPropellantInfo, ScreenDeltaVPropellantInfo>();
	}

	public ScreenDeltaVEngineInfo Create(GameObject engineItemPrefab, RectTransform contentParent, GameObject propellantItemPrefab, DeltaVEngineInfo engineInfo, List<DeltaVPropellantInfo> stagePropInfo, CalcType type, int stage)
	{
		this.engineInfo = engineInfo;
		this.contentParent = contentParent;
		this.propellantItemPrefab = propellantItemPrefab;
		engineItem = Object.Instantiate(engineItemPrefab);
		engineItem.transform.SetParent(contentParent);
		engineItem.transform.localScale = Vector3.one;
		screenEngineInfo = engineItem.GetComponent<ScreenDeltaVEngineInfo>();
		for (int i = 0; i < stagePropInfo.Count; i++)
		{
			GameObject gameObject = Object.Instantiate(propellantItemPrefab);
			gameObject.transform.SetParent(contentParent);
			gameObject.transform.localScale = Vector3.one;
			ScreenDeltaVPropellantInfo component = gameObject.GetComponent<ScreenDeltaVPropellantInfo>();
			propellantItems.Add(stagePropInfo[i], component);
			component.propellantInfo = stagePropInfo[i];
		}
		setUpComplete = true;
		return screenEngineInfo;
	}

	public bool Update(DeltaVEngineInfo engineInfo, List<DeltaVPropellantInfo> stagePropInfo, CalcType type, string calctypeDesc, int stage)
	{
		if (!setUpComplete)
		{
			return false;
		}
		if (screenEngineInfo != null && !screenEngineInfo.gameObject.activeInHierarchy)
		{
			return true;
		}
		if (screenEngineInfo != null)
		{
			screenEngineInfo.UpdateData(engineInfo, type, calctypeDesc, stage);
			for (int i = 0; i < stagePropInfo.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < propellantItems.KeysList.Count; j++)
				{
					if (propellantItems.KeysList[j].propellant.id != stagePropInfo[i].propellant.id)
					{
						if (!stagePropInfo.Contains(propellantItems.KeysList[j]))
						{
							return false;
						}
						continue;
					}
					propellantItems.ValuesList[j].UpdateData(stagePropInfo[i], type);
					flag = true;
					break;
				}
				if (!flag)
				{
					GameObject gameObject = Object.Instantiate(propellantItemPrefab);
					gameObject.transform.SetParent(contentParent);
					gameObject.transform.localScale = Vector3.one;
					ScreenDeltaVPropellantInfo component = gameObject.GetComponent<ScreenDeltaVPropellantInfo>();
					propellantItems.Add(stagePropInfo[i], component);
					component.propellantInfo = stagePropInfo[i];
				}
			}
			return true;
		}
		return false;
	}

	public void SetActive(bool active)
	{
		if (engineItem != null)
		{
			engineItem.SetActive(active);
		}
		for (int i = 0; i < propellantItems.ValuesList.Count; i++)
		{
			propellantItems.ValuesList[i].gameObject.SetActive(active);
		}
	}

	public void Destroy()
	{
		if (engineItem != null)
		{
			engineItem.DestroyGameObject();
			engineInfo = null;
		}
		int count = propellantItems.ValuesList.Count;
		while (count-- > 0)
		{
			propellantItems.ValuesList[count].gameObject.DestroyGameObject();
		}
		propellantItems.Clear();
	}

	public int SetPropellantUISiblings(int index)
	{
		for (int i = 0; i < propellantItems.ValuesList.Count; i++)
		{
			index++;
			propellantItems.ValuesList[i].transform.SetSiblingIndex(index);
		}
		return index;
	}
}
