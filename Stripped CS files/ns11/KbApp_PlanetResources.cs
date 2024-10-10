using System.Collections.Generic;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns11;

public class KbApp_PlanetResources : KbApp
{
	public class ResourceDataItem
	{
		public string resourceName;

		public float fraction;

		public int id;

		public KbItem_resourceItem listItem;

		public PieChart.Slice pcs;

		public ResourceDataItem(int id, string resourceName, float fraction, KbItem_resourceItem listItem, PieChart.Slice pcs)
		{
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(resourceName);
			if (definition != null)
			{
				this.resourceName = definition.displayName;
			}
			else
			{
				this.resourceName = resourceName;
			}
			this.fraction = fraction;
			this.id = id;
			this.listItem = listItem;
			this.pcs = pcs;
		}
	}

	public KbItem_resourceHeader resourceHeaderPrefab;

	public KbItem_resourceFooter resourceFooterPrefab;

	public KbItem_resourceItem resourceItemPrefab;

	public KbItem_resourceHeader resourceHeader;

	public KbItem_resourceFooter resourceFooter;

	public PieChart pieChartPrefab;

	public PieChart pieChart;

	public GameObject textPrefab;

	public TextMeshProUGUI text;

	public CelestialBody currentBody;

	public List<ResourceDataItem> resourceList_surface = new List<ResourceDataItem>();

	public List<ResourceDataItem> resourceList_ocean = new List<ResourceDataItem>();

	public List<ResourceDataItem> resourceList_atmosphere = new List<ResourceDataItem>();

	public List<PieChart.Slice> listPieChart = new List<PieChart.Slice>();

	public bool OverlayConfigMode;

	public bool headerFooterInitialized;

	public List<ResourceDataItem> currentList;

	public bool sortByResource = true;

	public bool sortByAsc = true;

	public int resourcesCutoff;

	public int ResourcesCtrlCutoff
	{
		get
		{
			return resourcesCutoff;
		}
		set
		{
			resourcesCutoff = Mathf.Clamp(value, 0, 100);
			resourceFooter.cutoffValue.text = resourcesCutoff + "%";
			if (OverlayConfigMode)
			{
				OverlayGenerator.Instance.Cutoff = ResourcesCtrlCutoff;
				if (OverlayGenerator.Instance.IsActive)
				{
					OverlayGenerator.Instance.GenerateOverlay(checkForLock: true);
				}
			}
		}
	}

	public override void ActivateApp(MapObject target)
	{
		currentBody = target.celestialBody;
		appFrame.appName.text = Localizer.Format("#autoLOC_7001301", currentBody.displayName).ToUpper();
		appFrame.scrollList.Clear(destroyElements: true);
		GameObject gameObject = Object.Instantiate(textPrefab);
		text = gameObject.GetComponent<TextMeshProUGUI>();
		text.transform.SetParent(base.transform);
		InitHeaderAndFooter();
		CreateResourceList();
		if (currentBody != null && currentBody.flightGlobalsIndex != OverlayGenerator.Instance.DisplayBody.flightGlobalsIndex)
		{
			currentBody.SetResourceMap(null);
		}
	}

	public void InitHeaderAndFooter()
	{
		if (!headerFooterInitialized)
		{
			resourceHeader = Object.Instantiate(resourceHeaderPrefab);
			resourceHeader.transform.SetParent(appFrame.anchorTop, worldPositionStays: false);
			resourceFooter = Object.Instantiate(resourceFooterPrefab);
			resourceFooter.transform.SetParent(appFrame.anchorBottom, worldPositionStays: false);
			pieChart = Object.Instantiate(pieChartPrefab);
			if (ApplicationLauncher.Instance.IsPositionedAtTop)
			{
				pieChart.transform.SetParent(appFrame.anchorPieChart, worldPositionStays: false);
			}
			else
			{
				pieChart.transform.SetParent(appFrame.anchorPieChartLeft, worldPositionStays: false);
			}
			resourceHeader.toggleSurface.onValueChanged.AddListener(OnToggleSurface);
			resourceHeader.toggleOcean.onValueChanged.AddListener(OnToggleOcean);
			resourceHeader.toggleAtmos.onValueChanged.AddListener(OnToggleAtmos);
			resourceHeader.listSorter.AddOnSortCallback(OnSort);
			resourceFooter.toggleColor.onValueChanged.AddListener(OnResourcesCtrlColorChange);
			resourceFooter.toggleStyle.onValueChanged.AddListener(OnResourcesCtrlStyleChange);
			resourceFooter.btnPlus.onClick.AddListener(MouseInputResCtrlPlus);
			resourceFooter.btnMinus.onClick.AddListener(MouseInputResCtrlMinus);
			headerFooterInitialized = true;
		}
	}

	public override void DisplayApp()
	{
	}

	public override void HideApp()
	{
	}

	public void OnToggleSurface(bool on)
	{
		if (on)
		{
			currentList = resourceList_surface;
			SetResourceListAppearance(surfaceSelected: true);
			OnSort(sortByResource, sortByAsc);
		}
	}

	public void OnToggleOcean(bool on)
	{
		if (on)
		{
			currentList = resourceList_ocean;
			SetResourceListAppearance(surfaceSelected: false);
			OnSort(sortByResource, sortByAsc);
		}
	}

	public void OnToggleAtmos(bool on)
	{
		if (on)
		{
			currentList = resourceList_atmosphere;
			SetResourceListAppearance(surfaceSelected: false);
			OnSort(sortByResource, sortByAsc);
		}
	}

	public void OnSort(int mode, bool asc)
	{
		sortByResource = mode == 0;
		OnSort(sortByResource, asc);
	}

	public void OnSort(bool mode, bool asc)
	{
		if (ResourceMap.Instance == null)
		{
			return;
		}
		sortByResource = mode;
		sortByAsc = asc;
		if (sortByResource)
		{
			currentList.Sort(new RUIutils.FuncComparer<ResourceDataItem>((ResourceDataItem r1, ResourceDataItem r2) => RUIutils.SortAscDescPrimarySecondary(sortByAsc, r1.resourceName.CompareTo(r2.resourceName), r1.fraction.CompareTo(r2.fraction))));
		}
		else
		{
			currentList.Sort(new RUIutils.FuncComparer<ResourceDataItem>((ResourceDataItem r1, ResourceDataItem r2) => RUIutils.SortAscDescPrimarySecondary(sortByAsc, r1.fraction.CompareTo(r2.fraction), r1.resourceName.CompareTo(r2.resourceName))));
		}
		appFrame.scrollList.Clear(destroyElements: false);
		listPieChart.Clear();
		int count = currentList.Count;
		for (int i = 0; i < count; i++)
		{
			ResourceDataItem resourceDataItem = currentList[i];
			if (resourceDataItem.fraction > 0f)
			{
				resourceDataItem.listItem.keyRich.text = resourceDataItem.resourceName;
				resourceDataItem.listItem.valueRich.text = KSPUtil.LocalizeNumber(resourceDataItem.fraction * 100f, "f1") + "%";
				resourceDataItem.listItem.radioButton.Data = resourceDataItem;
				appFrame.scrollList.AddItem(resourceDataItem.listItem.uiListItem);
				listPieChart.Add(resourceDataItem.pcs);
			}
		}
		if (!ResourceMap.Instance.IsPlanetScanned(currentBody.flightGlobalsIndex))
		{
			appFrame.scrollList.AddItem(text.GetComponent<UIListItem>());
			text.text = Localizer.Format("#autoLOC_462688");
		}
		else if (currentList.Count == 0)
		{
			appFrame.scrollList.AddItem(text.GetComponent<UIListItem>());
			text.text = Localizer.Format("#autoLOC_462695");
		}
		if (listPieChart.Count > 1)
		{
			pieChart.gameObject.SetActive(value: true);
			pieChart.SetSlices(listPieChart);
		}
		else
		{
			pieChart.gameObject.SetActive(value: false);
		}
	}

	public void onResourcesListItem_Click(PointerEventData eventData, UIRadioButton.State state, UIRadioButton.CallType callType)
	{
		UIRadioButton component = eventData.pointerPress.GetComponent<UIRadioButton>();
		ResourceListAndPiechart_select(component.Data as ResourceDataItem);
	}

	public void onResourcesListItem_Enter(PointerEventData eventData, PointerEnterExitHandler handler)
	{
		UIRadioButton component = handler.GetComponent<UIRadioButton>();
		SetSliceOver((component.Data as ResourceDataItem).pcs);
	}

	public void onResourcesListItem_Exit(PointerEventData eventData, PointerEnterExitHandler handler)
	{
		UIRadioButton component = handler.GetComponent<UIRadioButton>();
		SetSliceOverOff((component.Data as ResourceDataItem).pcs);
	}

	public void CreateResourceList()
	{
		DisposeResourceList();
		listPieChart.Clear();
		if (currentBody != null && ResourceMap.Instance != null)
		{
			List<PlanetaryResource> resourceItemList = ResourceMap.Instance.GetResourceItemList(HarvestTypes.Planetary, currentBody);
			float num = 0f;
			int count = resourceItemList.Count;
			for (int i = 0; i < count; i++)
			{
				num += resourceItemList[i].fraction;
			}
			for (int j = 0; j < count; j++)
			{
				AddResourceDataItem(resourceList_surface, resourceItemList[j], num);
			}
			List<PlanetaryResource> resourceItemList2 = ResourceMap.Instance.GetResourceItemList(HarvestTypes.Oceanic, currentBody);
			float num2 = 0f;
			int count2 = resourceItemList2.Count;
			for (int k = 0; k < count2; k++)
			{
				num2 += resourceItemList2[k].fraction;
			}
			for (int l = 0; l < count2; l++)
			{
				AddResourceDataItem(resourceList_ocean, resourceItemList2[l], num2);
			}
			List<PlanetaryResource> resourceItemList3 = ResourceMap.Instance.GetResourceItemList(HarvestTypes.Atmospheric, currentBody);
			float num3 = 0f;
			int count3 = resourceItemList3.Count;
			for (int m = 0; m < count3; m++)
			{
				num3 += resourceItemList3[m].fraction;
			}
			for (int n = 0; n < count3; n++)
			{
				AddResourceDataItem(resourceList_atmosphere, resourceItemList3[n], num3);
			}
			if (currentList == null)
			{
				currentList = resourceList_surface;
			}
			OnSort(sortByResource, sortByAsc);
			SetKBDisplay();
			OverlayConfigMode = true;
		}
	}

	public void AddResourceDataItem(List<ResourceDataItem> list, PlanetaryResource r, float totalSlices)
	{
		PieChart.Slice slice = new PieChart.Slice(r.resourceID, r.fraction / totalSlices, PartResourceLibrary.Instance.GetDefinition(r.resourceID).color);
		slice.AddOnTap(onSliceTap);
		slice.AddOnOver(onSliceOver);
		slice.AddOnExit(onSliceExit);
		KbItem_resourceItem kbItem_resourceItem = Object.Instantiate(resourceItemPrefab);
		kbItem_resourceItem.radioButton.onClick.AddListener(onResourcesListItem_Click);
		kbItem_resourceItem.hoverHandler.onPointerEnterObj.AddListener(onResourcesListItem_Enter);
		kbItem_resourceItem.hoverHandler.onPointerExitObj.AddListener(onResourcesListItem_Exit);
		ResourceDataItem resourceDataItem = new ResourceDataItem(r.resourceID, r.resourceName, r.fraction, kbItem_resourceItem, slice);
		list.Add(resourceDataItem);
		slice.Data = resourceDataItem;
		kbItem_resourceItem.radioButton.Data = resourceDataItem;
	}

	public void DisposeResourceList()
	{
		int count = resourceList_surface.Count;
		int count2 = resourceList_ocean.Count;
		int count3 = resourceList_atmosphere.Count;
		for (int i = 0; i < count; i++)
		{
			Object.Destroy(resourceList_surface[i].listItem.gameObject);
		}
		for (int j = 0; j < count2; j++)
		{
			Object.Destroy(resourceList_ocean[j].listItem.gameObject);
		}
		for (int k = 0; k < count3; k++)
		{
			Object.Destroy(resourceList_atmosphere[k].listItem.gameObject);
		}
		resourceList_surface.Clear();
		resourceList_ocean.Clear();
		resourceList_atmosphere.Clear();
	}

	public void SetKBDisplay()
	{
		switch (OverlayGenerator.Instance.OverlayStyle)
		{
		case 1:
			resourceFooter.toggleStyle.SetState("lines");
			OverlayGenerator.Instance.OverlayStyle = 1;
			break;
		case 2:
			resourceFooter.toggleStyle.SetState("dots");
			OverlayGenerator.Instance.OverlayStyle = 2;
			break;
		case 3:
			resourceFooter.toggleStyle.SetState("solid");
			OverlayGenerator.Instance.OverlayStyle = 3;
			break;
		}
		OverlaySetup.Instance.ResetMapConfig();
		ResourcesCtrlCutoff = OverlayGenerator.Instance.Cutoff;
		switch (OverlayGenerator.Instance.DisplayMode)
		{
		case MapDisplayTypes.Monochrome:
			resourceFooter.toggleColor.SetState("monochrome");
			break;
		case MapDisplayTypes.Inverse:
			resourceFooter.toggleColor.SetState("inverse");
			break;
		case MapDisplayTypes.HeatMapGreen:
			resourceFooter.toggleColor.SetState("hmgr");
			break;
		case MapDisplayTypes.HeatMapBlue:
			resourceFooter.toggleColor.SetState("hmbl");
			break;
		}
	}

	public void SetResourceListAppearance(bool surfaceSelected)
	{
		if (surfaceSelected)
		{
			resourceFooter.panelCutooff.gameObject.SetActive(value: true);
		}
		else
		{
			resourceFooter.panelCutooff.gameObject.SetActive(value: false);
		}
	}

	public void ResourceListAndPiechart_select(ResourceDataItem rw)
	{
		if (rw.listItem.radioButton.Value)
		{
			OverlayGenerator.Instance.IsActive = true;
			OverlayGenerator.Instance.DisplayResource = PartResourceLibrary.Instance.GetDefinition(rw.id);
			OverlayGenerator.Instance.GenerateOverlay(checkForLock: true);
		}
		else
		{
			OverlayGenerator.Instance.IsActive = false;
			OverlayGenerator.Instance.ClearDisplay();
		}
	}

	public void onSliceTap(PieChart.Slice slice)
	{
		if (listPieChart.Count >= 2)
		{
			UIRadioButton radioButton = (slice.Data as ResourceDataItem).listItem.radioButton;
			if (radioButton.Value)
			{
				radioButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATION, null);
			}
			else
			{
				radioButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATION, null);
			}
			ResourceListAndPiechart_select(slice.Data as ResourceDataItem);
		}
	}

	public void onSliceOver(PieChart.Slice slice)
	{
		SetSliceOver(slice);
		UIRadioButton radioButton = (slice.Data as ResourceDataItem).listItem.radioButton;
		ExecuteEvents.Execute(eventData: new PointerEventData(EventSystem.current), target: radioButton.gameObject, functor: ExecuteEvents.pointerEnterHandler);
	}

	public void SetSliceOver(PieChart.Slice slice)
	{
		if (listPieChart.Count >= 2)
		{
			slice.material.SetFloat("_Emission", 0.45f);
		}
	}

	public void onSliceExit(PieChart.Slice slice)
	{
		SetSliceOverOff(slice);
		UIRadioButton radioButton = (slice.Data as ResourceDataItem).listItem.radioButton;
		ExecuteEvents.Execute(eventData: new PointerEventData(EventSystem.current), target: radioButton.gameObject, functor: ExecuteEvents.pointerExitHandler);
	}

	public void SetSliceOverOff(PieChart.Slice slice)
	{
		if (listPieChart.Count >= 2)
		{
			slice.material.SetFloat("_Emission", 0.35f);
		}
	}

	public void MouseInputResCtrlPlus()
	{
		if (resourcesCutoff < 100)
		{
			ResourcesCtrlCutoff += 10;
		}
	}

	public void MouseInputResCtrlMinus()
	{
		if (resourcesCutoff > 0)
		{
			ResourcesCtrlCutoff -= 10;
		}
	}

	public void OnResourcesCtrlColorChange(UIStateButton btn)
	{
		if (OverlayConfigMode)
		{
			switch (btn.currentState)
			{
			case "hmbl":
				OverlayGenerator.Instance.DisplayMode = MapDisplayTypes.HeatMapBlue;
				break;
			case "hmgr":
				OverlayGenerator.Instance.DisplayMode = MapDisplayTypes.HeatMapGreen;
				break;
			case "inverse":
				OverlayGenerator.Instance.DisplayMode = MapDisplayTypes.Inverse;
				break;
			case "monochrome":
				OverlayGenerator.Instance.DisplayMode = MapDisplayTypes.Monochrome;
				break;
			}
			if (OverlayGenerator.Instance.IsActive)
			{
				OverlayGenerator.Instance.GenerateOverlay(checkForLock: true);
			}
		}
	}

	public void OnResourcesCtrlStyleChange(UIStateButton btn)
	{
		if (OverlayConfigMode)
		{
			switch (btn.currentState)
			{
			case "dots":
				OverlayGenerator.Instance.OverlayStyle = 2;
				break;
			case "lines":
				OverlayGenerator.Instance.OverlayStyle = 1;
				break;
			case "solid":
				OverlayGenerator.Instance.OverlayStyle = 3;
				break;
			}
			OverlaySetup.Instance.ResetMapConfig();
			if (OverlayGenerator.Instance.IsActive)
			{
				OverlayGenerator.Instance.GenerateOverlay(checkForLock: true);
			}
		}
	}
}
