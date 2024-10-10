using System;
using System.Collections;
using ns2;
using UnityEngine;
using UnityEngine.UI;

namespace ns18;

public class BuildingPicker : MonoBehaviour
{
	[Serializable]
	public class FacilityUIInfo
	{
		public string name;

		public ButtonSpritesMgr.ButtonSprites spriteSet;

		[NonSerialized]
		public SpaceCenterBuilding scBuilding;

		public BuildingPickerItem buildingPickerItem;
	}

	public BuildingPickerItem itemPrefab;

	public RectTransform uiParent;

	public LayoutGroup layoutGroup;

	public FacilityUIInfo[] faciltyInfos;

	public bool isReady;

	public IEnumerator Start()
	{
		isReady = false;
		HideUI();
		yield return new WaitForSeconds(1f);
		if (ConstructBuildingList())
		{
			ConstructUI();
			isReady = true;
		}
	}

	public void HideUI()
	{
		if (!uiParent.gameObject.activeSelf)
		{
			return;
		}
		int i = 0;
		for (int num = faciltyInfos.Length; i < num; i++)
		{
			FacilityUIInfo facilityUIInfo = faciltyInfos[i];
			if (facilityUIInfo.buildingPickerItem != null)
			{
				facilityUIInfo.buildingPickerItem.DestroyPointer();
			}
		}
		uiParent.gameObject.SetActive(value: false);
	}

	public void ShowUI()
	{
		if (!uiParent.gameObject.activeSelf)
		{
			uiParent.gameObject.SetActive(value: true);
		}
	}

	public void Update()
	{
		if (isReady && !InputLockManager.IsLocked(ControlTypes.KSC_FACILITIES | ControlTypes.KSC_UI))
		{
			if (isReady)
			{
				ShowUI();
			}
		}
		else
		{
			HideUI();
		}
	}

	public bool ConstructBuildingList()
	{
		SpaceCenterBuilding[] array = UnityEngine.Object.FindObjectsOfType<SpaceCenterBuilding>();
		int num = array.Length;
		while (num-- > 0)
		{
			int num2 = faciltyInfos.Length;
			while (num2-- > 0)
			{
				if (!(array[num].facilityName == faciltyInfos[num2].name))
				{
					if (num2 == 0)
					{
						Debug.LogError("[BuildingPicker]: Facility Name Mismatch: No entry exists for " + array[num].facilityName + " in BuildingPicker list", base.gameObject);
					}
					continue;
				}
				faciltyInfos[num2].scBuilding = array[num];
				break;
			}
		}
		int num3 = faciltyInfos.Length;
		do
		{
			if (num3-- <= 0)
			{
				return false;
			}
		}
		while (!(faciltyInfos[num3].scBuilding != null));
		return true;
	}

	public void ConstructUI()
	{
		int i = 0;
		for (int num = faciltyInfos.Length; i < num; i++)
		{
			FacilityUIInfo facilityUIInfo = faciltyInfos[i];
			SpaceCenterBuilding scBuilding = facilityUIInfo.scBuilding;
			if (!(scBuilding == null) && scBuilding.IsOpen())
			{
				BuildingPickerItem buildingPickerItem = UnityEngine.Object.Instantiate(itemPrefab);
				buildingPickerItem.Setup(scBuilding, facilityUIInfo.spriteSet);
				buildingPickerItem.transform.SetParent(layoutGroup.transform, worldPositionStays: false);
				facilityUIInfo.buildingPickerItem = buildingPickerItem;
			}
		}
	}
}
