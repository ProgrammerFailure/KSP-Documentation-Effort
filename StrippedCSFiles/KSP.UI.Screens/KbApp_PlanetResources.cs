using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KSP.UI.Screens;

public class KbApp_PlanetResources : KbApp
{
	public class ResourceDataItem
	{
		public string resourceName;

		public float fraction;

		public int id;

		public KbItem_resourceItem listItem;

		public PieChart.Slice pcs;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourceDataItem(int id, string resourceName, float fraction, KbItem_resourceItem listItem, PieChart.Slice pcs)
		{
			throw null;
		}
	}

	public KbItem_resourceHeader resourceHeaderPrefab;

	public KbItem_resourceFooter resourceFooterPrefab;

	public KbItem_resourceItem resourceItemPrefab;

	private KbItem_resourceHeader resourceHeader;

	private KbItem_resourceFooter resourceFooter;

	public PieChart pieChartPrefab;

	private PieChart pieChart;

	public GameObject textPrefab;

	private TextMeshProUGUI text;

	private CelestialBody currentBody;

	private List<ResourceDataItem> resourceList_surface;

	private List<ResourceDataItem> resourceList_ocean;

	private List<ResourceDataItem> resourceList_atmosphere;

	private List<PieChart.Slice> listPieChart;

	private bool OverlayConfigMode;

	private bool headerFooterInitialized;

	private List<ResourceDataItem> currentList;

	private bool sortByResource;

	private bool sortByAsc;

	private int resourcesCutoff;

	public int ResourcesCtrlCutoff
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KbApp_PlanetResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ActivateApp(MapObject target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitHeaderAndFooter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DisplayApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void HideApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnToggleSurface(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnToggleOcean(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnToggleAtmos(bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSort(int mode, bool asc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSort(bool mode, bool asc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onResourcesListItem_Click(PointerEventData eventData, UIRadioButton.State state, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onResourcesListItem_Enter(PointerEventData eventData, PointerEnterExitHandler handler)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onResourcesListItem_Exit(PointerEventData eventData, PointerEnterExitHandler handler)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateResourceList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddResourceDataItem(List<ResourceDataItem> list, PlanetaryResource r, float totalSlices)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisposeResourceList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetKBDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetResourceListAppearance(bool surfaceSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResourceListAndPiechart_select(ResourceDataItem rw)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSliceTap(PieChart.Slice slice)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSliceOver(PieChart.Slice slice)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSliceOver(PieChart.Slice slice)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSliceExit(PieChart.Slice slice)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSliceOverOff(PieChart.Slice slice)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInputResCtrlPlus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInputResCtrlMinus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnResourcesCtrlColorChange(UIStateButton btn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnResourcesCtrlStyleChange(UIStateButton btn)
	{
		throw null;
	}
}
