using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class RDArchivesController : MonoBehaviour
{
	public struct Filter
	{
		public string key;

		public string value;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Filter(string key, string value)
		{
			throw null;
		}
	}

	public struct ReportData
	{
		public string id;

		public string description;

		public string situation;

		public string biome;

		public float data;

		public float value;

		public float science;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ReportData(string id, string description, string situation, string biome, float data, float value, float science)
		{
			throw null;
		}
	}

	private UIList<ReportData> reportList;

	public RectTransform reportListAnchor;

	public UIListSorter reportListSorter;

	public RectTransform reportListCache;

	public RDReportListItemContainer reportItemContainer;

	public TextMeshProUGUI reportsFoundLabel;

	public RDDropDownListContainer dropdownListContainer;

	public RDPlanetListItemContainer planetListItemPrefab;

	public UIList planetList;

	public int reportListSizeMax;

	public TextMeshProUGUI instructorText;

	public RawImage instructorRawImage;

	public KerbalInstructor instructorPrefab;

	public RenderTexture instructorTexture;

	public int instructorPortraitSize;

	private KerbalInstructor instructor;

	private RDArchivesAvatarController avatarController;

	private RDPlanetListItemContainer current_planetSelection;

	private RDDropDownList dropDownExperiments;

	private RDDropDownList dropDownSituations;

	private RDDropDownList dropDownBiomes;

	private Dictionary<string, List<Filter>> searchFilter_planets;

	private List<Filter> searchFilter_experiments;

	private List<Filter> searchFilter_situations;

	private List<ScienceSubject> subjects;

	private int reportListSize;

	private int reportListSizeActual;

	private List<ReportData> reportListData;

	private List<ReportData> reportListDataFiltered;

	private List<RDReportListItemContainer> reportListContainers;

	private const string colorHiddenTag = "<color=#aaaaaa>";

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDArchivesController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DropdownListCallback(RDDropDownList list, bool opening)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DropdownListItemCallback(RDDropDownList list, bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FilterExperiments()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FilterSituations()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FilterBiomes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FilterList(RDDropDownList list, List<Filter> filter, Func<ReportData, bool> expression, bool filterBiome = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<ReportData> FilterList(List<ReportData> list, Func<ReportData, bool> filter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearAllSubFilterSortingFlags()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearALLSubFilterLists()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearSubFilterList(RDDropDownList list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshALLSubFilterLists(RDPlanetListItemContainer planetSelection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshSubFilterList(RDDropDownList list, RDPlanetListItemContainer planetSelection)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSelectedList(RDDropDownList list, List<Filter> items, Func<ReportData, bool> expression, bool biomeFilter = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshSubFilterListAndFilter(RDDropDownList list, RDPlanetListItemContainer planetSelection, List<Filter> items, bool biomeFilter = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshSubFilterListAndFilterKEEPSelection(RDDropDownList list, RDPlanetListItemContainer planetSelection, List<Filter> items, List<ReportData> filteredList, bool biomeFilter = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CacheReportLists()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyReportLists()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FilterReports()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FilterReportsFunc(Func<ReportData, bool> expression)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateReports()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnReportListSort(int button, bool asc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateReportListFromCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateReportListItem(int index, string id, string description, string situation, string biome, float data, float value, float science)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Func<ReportData, bool> GetPlanetFilterFunction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<Filter> GetPlanetFilterlist()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddPlanets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private RDPlanetListItemContainer AddPlanetRecursively(PSystemBody parent, PSystemBody body, float offset_pos, float offset_pos_inc, int hierarchy_level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Cascade(RDPlanetListItemContainer container, bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetInstructorText(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupInstructor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyInstructor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateInstructor(bool setText = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float RandomFloat()
	{
		throw null;
	}
}
