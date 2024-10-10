using System;
using System.Collections.Generic;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class RDArchivesController : MonoBehaviour
{
	public struct Filter
	{
		public string key;

		public string value;

		public Filter(string key, string value)
		{
			this.key = key;
			this.value = value;
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

		public ReportData(string id, string description, string situation, string biome, float data, float value, float science)
		{
			this.id = id;
			this.description = description;
			this.situation = situation;
			this.biome = biome;
			this.data = data;
			this.value = value;
			this.science = science;
		}
	}

	public UIList<ReportData> reportList;

	public RectTransform reportListAnchor;

	public UIListSorter reportListSorter;

	public RectTransform reportListCache;

	public RDReportListItemContainer reportItemContainer;

	public TextMeshProUGUI reportsFoundLabel;

	public RDDropDownListContainer dropdownListContainer;

	public RDPlanetListItemContainer planetListItemPrefab;

	public UIList planetList;

	public int reportListSizeMax = 25;

	public TextMeshProUGUI instructorText;

	public RawImage instructorRawImage;

	public KerbalInstructor instructorPrefab;

	public RenderTexture instructorTexture;

	public int instructorPortraitSize = 128;

	public KerbalInstructor instructor;

	public RDArchivesAvatarController avatarController;

	public RDPlanetListItemContainer current_planetSelection;

	public RDDropDownList dropDownExperiments;

	public RDDropDownList dropDownSituations;

	public RDDropDownList dropDownBiomes;

	public Dictionary<string, List<Filter>> searchFilter_planets = new Dictionary<string, List<Filter>>();

	public List<Filter> searchFilter_experiments = new List<Filter>();

	public List<Filter> searchFilter_situations = new List<Filter>();

	public List<ScienceSubject> subjects;

	public int reportListSize;

	public int reportListSizeActual;

	public List<ReportData> reportListData = new List<ReportData>();

	public List<ReportData> reportListDataFiltered = new List<ReportData>();

	public List<RDReportListItemContainer> reportListContainers = new List<RDReportListItemContainer>();

	public const string colorHiddenTag = "<color=#aaaaaa>";

	public void OnDestroy()
	{
		DestroyInstructor();
		DestroyReportLists();
	}

	public void Start()
	{
		reportList = new UIList<ReportData>(reportListAnchor);
		reportListSorter.AddOnSortCallback(OnReportListSort);
		OnReportListSort(reportListSorter.StartSortingIndex, reportListSorter.startSortingAsc);
		dropDownExperiments = dropdownListContainer.dropDownListPrefab.CreateInstanceFromPrefab("Experiments", Localizer.Format("#autoLOC_468312"), DropdownListCallback, DropdownListItemCallback, 51);
		dropDownSituations = dropdownListContainer.dropDownListPrefab.CreateInstanceFromPrefab("Situations", Localizer.Format("#autoLOC_468313"), DropdownListCallback, DropdownListItemCallback, 52);
		dropDownBiomes = dropdownListContainer.dropDownListPrefab.CreateInstanceFromPrefab("Biomes", Localizer.Format("#autoLOC_468314"), DropdownListCallback, DropdownListItemCallback, 53);
		dropdownListContainer.Initialize(dropDownExperiments, dropDownSituations, dropDownBiomes);
		List<string> experimentIDs = RDEnvironmentAdapter.GetExperimentIDs();
		int i = 0;
		for (int count = experimentIDs.Count; i < count; i++)
		{
			searchFilter_experiments.Add(new Filter(experimentIDs[i] + "@", RDEnvironmentAdapter.GetExperiment(experimentIDs[i]).experimentTitle));
		}
		experimentIDs = RDEnvironmentAdapter.GetSituationTags();
		List<string> situationTagsDescriptions = RDEnvironmentAdapter.GetSituationTagsDescriptions();
		int j = 0;
		for (int count2 = experimentIDs.Count; j < count2; j++)
		{
			searchFilter_situations.Add(new Filter(experimentIDs[j], situationTagsDescriptions[j]));
		}
		subjects = RDEnvironmentAdapter.GetSubjects();
		AddPlanets();
		CacheReportLists();
		SetupInstructor();
		FilterReports();
		RefreshALLSubFilterLists(null);
		UpdateReports();
	}

	public void DropdownListCallback(RDDropDownList list, bool opening)
	{
		if (opening)
		{
			RefreshSubFilterList(list, current_planetSelection);
			return;
		}
		FilterReports();
		UpdateReports();
		UpdateInstructor();
		if (list.name == "Experiments")
		{
			FilterSituations();
			FilterBiomes();
		}
		else if (list.name == "Situations")
		{
			FilterExperiments();
			FilterBiomes();
		}
		else if (list.name == "Biomes")
		{
			FilterExperiments();
			FilterSituations();
		}
	}

	public void DropdownListItemCallback(RDDropDownList list, bool selected)
	{
		if (list.name == "Experiments")
		{
			FilterReports();
			if (!selected)
			{
				RefreshSubFilterListAndFilter(list, current_planetSelection, searchFilter_experiments);
			}
			else
			{
				UpdateSelectedList(list, searchFilter_experiments, RUIutils.And<ReportData>(GetPlanetFilterFunction(), dropDownSituations.GetFilter(), dropDownBiomes.GetBiomeFilter()));
			}
			FilterSituations();
			FilterBiomes();
		}
		else if (list.name == "Situations")
		{
			FilterReports();
			if (!selected)
			{
				RefreshSubFilterListAndFilter(list, current_planetSelection, searchFilter_situations);
			}
			else
			{
				UpdateSelectedList(list, searchFilter_situations, RUIutils.And<ReportData>(GetPlanetFilterFunction(), dropDownExperiments.GetFilter(), dropDownBiomes.GetBiomeFilter()));
			}
			FilterExperiments();
			FilterBiomes();
		}
		else if (list.name == "Biomes")
		{
			FilterReports();
			if (!selected)
			{
				RefreshSubFilterListAndFilter(list, current_planetSelection, GetPlanetFilterlist(), biomeFilter: true);
			}
			else
			{
				UpdateSelectedList(list, GetPlanetFilterlist(), RUIutils.And<ReportData>(GetPlanetFilterFunction(), dropDownExperiments.GetFilter(), dropDownSituations.GetFilter()), biomeFilter: true);
			}
			FilterExperiments();
			FilterSituations();
		}
		UpdateReports();
		UpdateInstructor();
	}

	public void FilterExperiments()
	{
		FilterList(dropDownExperiments, searchFilter_experiments, RUIutils.And<ReportData>(GetPlanetFilterFunction(), dropDownSituations.GetFilter(), dropDownBiomes.GetBiomeFilter()));
	}

	public void FilterSituations()
	{
		FilterList(dropDownSituations, searchFilter_situations, RUIutils.And<ReportData>(GetPlanetFilterFunction(), dropDownExperiments.GetFilter(), dropDownBiomes.GetBiomeFilter()));
	}

	public void FilterBiomes()
	{
		FilterList(dropDownBiomes, GetPlanetFilterlist(), RUIutils.And<ReportData>(GetPlanetFilterFunction(), dropDownExperiments.GetFilter(), dropDownSituations.GetFilter()), filterBiome: true);
	}

	public void FilterList(RDDropDownList list, List<Filter> filter, Func<ReportData, bool> expression, bool filterBiome = false)
	{
		if (list.sortingBy == string.Empty)
		{
			RefreshSubFilterListAndFilter(list, current_planetSelection, filter, filterBiome);
		}
		else
		{
			RefreshSubFilterListAndFilterKEEPSelection(list, current_planetSelection, filter, FilterList(reportListData, expression), filterBiome);
		}
	}

	public static List<ReportData> FilterList(List<ReportData> list, Func<ReportData, bool> filter)
	{
		List<ReportData> list2 = new List<ReportData>();
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			ReportData reportData = list[i];
			if (filter(reportData))
			{
				list2.Add(reportData);
			}
		}
		return list2;
	}

	public void ClearAllSubFilterSortingFlags()
	{
		for (int i = 0; i < dropdownListContainer.lists.Length; i++)
		{
			dropdownListContainer.lists[i].sortingBy = "";
		}
	}

	public void ClearALLSubFilterLists()
	{
		for (int i = 0; i < dropdownListContainer.lists.Length; i++)
		{
			ClearSubFilterList(dropdownListContainer.lists[i]);
		}
	}

	public void ClearSubFilterList(RDDropDownList list)
	{
		if (list.open)
		{
			list.ClearList();
		}
	}

	public void RefreshALLSubFilterLists(RDPlanetListItemContainer planetSelection)
	{
		if (planetSelection != null)
		{
			for (int i = 0; i < dropdownListContainer.lists.Length; i++)
			{
				RefreshSubFilterList(dropdownListContainer.lists[i], planetSelection);
			}
		}
		else
		{
			RefreshSubFilterList(dropDownExperiments, null);
			RefreshSubFilterList(dropDownSituations, null);
			ClearSubFilterList(dropDownBiomes);
		}
	}

	public void RefreshSubFilterList(RDDropDownList list, RDPlanetListItemContainer planetSelection)
	{
		if (list.open)
		{
			if (list.name == "Experiments")
			{
				RefreshSubFilterListAndFilter(list, planetSelection, searchFilter_experiments);
			}
			else if (list.name == "Situations")
			{
				RefreshSubFilterListAndFilter(list, planetSelection, searchFilter_situations);
			}
			else if (list.name == "Biomes")
			{
				RefreshSubFilterListAndFilter(list, planetSelection, GetPlanetFilterlist(), biomeFilter: true);
			}
		}
	}

	public void UpdateSelectedList(RDDropDownList list, List<Filter> items, Func<ReportData, bool> expression, bool biomeFilter = false)
	{
		List<ReportData> list2 = FilterList(reportListData, expression);
		int num = 0;
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			int num2 = 0;
			int j = 0;
			for (int count2 = list2.Count; j < count2; j++)
			{
				if (biomeFilter)
				{
					if (list2[j].biome == items[i].key)
					{
						num2++;
					}
				}
				else if (list2[j].id.Contains(items[i].key))
				{
					num2++;
				}
			}
			if (num2 > 0)
			{
				list.UpdateItem(num++, Localizer.Format("#autoLOC_6001922", num2));
			}
		}
		int num3 = 0;
		int k = 0;
		for (int count3 = list2.Count; k < count3; k++)
		{
			if (biomeFilter)
			{
				if (list2[k].biome == list.sortingBy)
				{
					num3++;
				}
			}
			else if (list2[k].id.Contains(list.sortingBy))
			{
				num3++;
			}
		}
		list.UpdateSelectedItem(Localizer.Format("#autoLOC_6001922", num3) + " <color=#aaaaaa>" + (list2.Count - num3) + " " + Localizer.Format("#autoLOC_7003408") + "</color>");
	}

	public void RefreshSubFilterListAndFilter(RDDropDownList list, RDPlanetListItemContainer planetSelection, List<Filter> items, bool biomeFilter = false)
	{
		list.ClearList();
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			int num = 0;
			int j = 0;
			for (int count2 = reportListDataFiltered.Count; j < count2; j++)
			{
				if (biomeFilter)
				{
					if (reportListDataFiltered[j].biome == items[i].key)
					{
						num++;
					}
				}
				else if (reportListDataFiltered[j].id.Contains(items[i].key))
				{
					num++;
				}
			}
			if (num > 0)
			{
				list.AddItem(items[i].key, items[i].value, Localizer.Format("#autoLOC_6001922", num));
			}
		}
	}

	public void RefreshSubFilterListAndFilterKEEPSelection(RDDropDownList list, RDPlanetListItemContainer planetSelection, List<Filter> items, List<ReportData> filteredList, bool biomeFilter = false)
	{
		list.ClearList(removeFiltering: false);
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			int num = 0;
			int j = 0;
			for (int count2 = filteredList.Count; j < count2; j++)
			{
				if (biomeFilter)
				{
					if (filteredList[j].biome == items[i].key)
					{
						num++;
					}
				}
				else if (filteredList[j].id.Contains(items[i].key))
				{
					num++;
				}
			}
			if (num > 0)
			{
				if (list.sortingBy == items[i].key)
				{
					list.AddSelectedItem(items[i].key, items[i].value, Localizer.Format("#autoLOC_6001922", num) + " <color=#aaaaaa>" + (filteredList.Count - num) + " " + Localizer.Format("#autoLOC_7003408") + "</color>");
				}
				else
				{
					list.AddItem(items[i].key, items[i].value, Localizer.Format("#autoLOC_6001922", num));
				}
			}
		}
	}

	public void CacheReportLists()
	{
		int i = 0;
		for (int count = subjects.Count; i < count; i++)
		{
			string id = subjects[i].id;
			string title = subjects[i].title;
			string BodyName = "";
			string Biome = "";
			string Situation = "";
			ScienceUtil.GetExperimentFieldsFromScienceID(id, out BodyName, out Situation, out Biome);
			float num = subjects[i].scientificValue * subjects[i].subjectValue / subjects[i].dataScale;
			float value = subjects[i].science / subjects[i].scienceCap;
			float num2 = subjects[i].science;
			if (HighLogic.LoadedSceneIsGame)
			{
				num *= HighLogic.CurrentGame.Parameters.Career.ScienceGainMultiplier;
				num2 *= HighLogic.CurrentGame.Parameters.Career.ScienceGainMultiplier;
			}
			reportListData.Add(new ReportData(id, title, Situation, Biome, num, value, num2));
			if (i < reportListSizeMax)
			{
				RDReportListItemContainer rDReportListItemContainer = UnityEngine.Object.Instantiate(reportItemContainer);
				rDReportListItemContainer.SetDescriptionLabel(title);
				reportListContainers.Add(rDReportListItemContainer);
			}
		}
	}

	public void DestroyReportLists()
	{
		int i = 0;
		for (int count = reportListContainers.Count; i < count; i++)
		{
			UnityEngine.Object.Destroy(reportListContainers[i]);
		}
	}

	public void FilterReports()
	{
		reportListDataFiltered = FilterList(reportListData, RUIutils.And<ReportData>(GetPlanetFilterFunction(), dropDownExperiments.GetFilter(), dropDownSituations.GetFilter(), dropDownBiomes.GetBiomeFilter()));
	}

	public void FilterReportsFunc(Func<ReportData, bool> expression)
	{
		reportListDataFiltered = FilterList(reportListData, expression);
	}

	public void UpdateReports()
	{
		reportList.ClearUI(reportListCache);
		reportListSize = subjects.Count;
		OnReportListSort(reportListSorter.StartSortingIndex, reportListSorter.startSortingAsc);
	}

	public void OnReportListSort(int button, bool asc)
	{
		switch (button)
		{
		case 0:
			reportListDataFiltered.Sort(new RUIutils.FuncComparer<ReportData>((ReportData r1, ReportData r2) => RUIutils.SortAscDescPrimarySecondary(asc, r1.description.CompareTo(r2.description), r1.value.CompareTo(r2.value))));
			break;
		case 1:
			reportListDataFiltered.Sort(new RUIutils.FuncComparer<ReportData>((ReportData r1, ReportData r2) => RUIutils.SortAscDescPrimarySecondary(asc, r1.data.CompareTo(r2.data), r1.description.CompareTo(r2.description))));
			break;
		case 2:
			reportListDataFiltered.Sort(new RUIutils.FuncComparer<ReportData>((ReportData r1, ReportData r2) => RUIutils.SortAscDescPrimarySecondary(asc, r1.value.CompareTo(r2.value), r1.description.CompareTo(r2.description))));
			break;
		case 3:
			reportListDataFiltered.Sort(new RUIutils.FuncComparer<ReportData>((ReportData r1, ReportData r2) => RUIutils.SortAscDescPrimarySecondary(asc, r1.science.CompareTo(r2.science), r1.description.CompareTo(r2.description))));
			break;
		}
		reportList.ClearUI(reportListCache);
		UpdateReportListFromCache();
	}

	public void UpdateReportListFromCache()
	{
		reportListSizeActual = Mathf.Clamp(reportListDataFiltered.Count, 0, reportListSizeMax);
		for (int i = 0; i < reportListSizeActual; i++)
		{
			UpdateReportListItem(i, reportListDataFiltered[i].id, reportListDataFiltered[i].description, reportListDataFiltered[i].situation, reportListDataFiltered[i].biome, reportListDataFiltered[i].data, reportListDataFiltered[i].value, reportListDataFiltered[i].science);
		}
		if (reportListDataFiltered.Count > reportListSizeMax)
		{
			reportsFoundLabel.text = Localizer.Format("#autoLOC_468660", reportListSizeActual, reportListDataFiltered.Count, reportListSize);
		}
		else
		{
			reportsFoundLabel.text = Localizer.Format("#autoLOC_468662", reportListDataFiltered.Count, reportListSize);
		}
	}

	public void UpdateReportListItem(int index, string id, string description, string situation, string biome, float data, float value, float science)
	{
		RDReportListItemContainer rDReportListItemContainer = reportListContainers[index];
		rDReportListItemContainer.id = id;
		rDReportListItemContainer.SetDescriptionLabel(description);
		rDReportListItemContainer.SetDataLabel(data);
		rDReportListItemContainer.SetValueSlider(value);
		rDReportListItemContainer.SetScienceLabel(science);
		reportList.AddItem(new UIListData<ReportData>(new ReportData(id, description, situation, biome, data, value, science), rDReportListItemContainer.GetComponent<UIListItem>()));
	}

	public Func<ReportData, bool> GetPlanetFilterFunction()
	{
		if (current_planetSelection != null)
		{
			return (ReportData a) => a.id.Contains("@" + current_planetSelection.name);
		}
		return (ReportData a) => true;
	}

	public List<Filter> GetPlanetFilterlist()
	{
		if (current_planetSelection != null)
		{
			return searchFilter_planets[current_planetSelection.name];
		}
		return new List<Filter>();
	}

	public void AddPlanets()
	{
		planetList.Clear(destroyElements: true);
		if (!(PSystemManager.Instance == null) && !(PSystemManager.Instance.systemPrefab == null))
		{
			if (PSystemManager.Instance.systemPrefab.rootBody == null)
			{
				Debug.LogError("PSystemManager: systemPrefab root body is null!");
			}
			else
			{
				AddPlanetRecursively(null, PSystemManager.Instance.systemPrefab.rootBody, 0f, 20f, 0);
			}
		}
		else
		{
			Debug.LogError("PSystemManager: systemPrefab is null!");
		}
	}

	public RDPlanetListItemContainer AddPlanetRecursively(PSystemBody parent, PSystemBody body, float offset_pos, float offset_pos_inc, int hierarchy_level)
	{
		if (body.scaledVersion == null)
		{
			Debug.LogError("PSystemBody (" + body.name + "): scaledVersion is null!");
			return null;
		}
		List<Filter> list = new List<Filter>();
		int i = 0;
		for (int num = body.celestialBody.MiniBiomes.Length - 1; i < num; i++)
		{
			int count = list.Count;
			bool flag = false;
			for (int j = 0; j < count; j++)
			{
				if (list[j].key == body.celestialBody.MiniBiomes[i].TagKeyID)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				list.Add(new Filter(body.celestialBody.MiniBiomes[i].TagKeyID, Localizer.Format(body.celestialBody.MiniBiomes[i].GetDisplayName.LocalizeRemoveGender())));
			}
		}
		List<string> biomeTags = RDEnvironmentAdapter.GetBiomeTags(body.celestialBody, includeMiniBiomes: false);
		List<string> biomeTagsLocalized = RDEnvironmentAdapter.GetBiomeTagsLocalized(body.celestialBody, includeMiniBiomes: false);
		int k = 0;
		for (int count2 = biomeTags.Count; k < count2; k++)
		{
			int count3 = list.Count;
			bool flag2 = false;
			for (int l = 0; l < count3; l++)
			{
				if (list[l].key == biomeTags[k])
				{
					flag2 = true;
				}
			}
			if (!flag2 && biomeTagsLocalized.Count >= k)
			{
				list.Add(new Filter(biomeTags[k], biomeTagsLocalized[k].LocalizeRemoveGender()));
			}
		}
		searchFilter_planets.Add(body.celestialBody.bodyName, list);
		GameObject gameObject = UnityEngine.Object.Instantiate(body.scaledVersion);
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<ScaledSpaceFader>());
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<MaterialSetDirection>());
		UnityEngine.Object.DestroyImmediate(GameObject.Find(gameObject.name + "/Atmosphere"));
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<SphereCollider>());
		float container_height = 80f;
		float num2 = 1f;
		if (hierarchy_level > 1)
		{
			offset_pos += offset_pos_inc;
		}
		if (hierarchy_level < 2)
		{
			num2 = 0.8f;
		}
		else
		{
			num2 = 0.5f;
			container_height = 60f;
		}
		RDPlanetListItemContainer rDPlanetListItemContainer = UnityEngine.Object.Instantiate(planetListItemPrefab);
		rDPlanetListItemContainer.Setup(body.celestialBody.bodyName, body.celestialBody.displayName, gameObject, parent != null, offset_pos, num2, container_height, hierarchy_level, planetList);
		rDPlanetListItemContainer.SetSelectionCallback(Cascade);
		planetList.AddItem(rDPlanetListItemContainer.GetComponent<UIListItem>());
		hierarchy_level++;
		int count4 = body.children.Count;
		for (int m = 0; m < count4; m++)
		{
			rDPlanetListItemContainer.AddChild(AddPlanetRecursively(body, body.children[m], offset_pos, offset_pos_inc, hierarchy_level));
		}
		if (hierarchy_level > 2)
		{
			rDPlanetListItemContainer.Hide();
		}
		return rDPlanetListItemContainer;
	}

	public void Cascade(RDPlanetListItemContainer container, bool selected)
	{
		if (selected)
		{
			if (current_planetSelection != null && (container.hierarchy_level != 0 || current_planetSelection.hierarchy_level != 0))
			{
				if (container.hierarchy_level == 0 && current_planetSelection.hierarchy_level != 0)
				{
					current_planetSelection.GetParentInHierarchy(1).HideChildren();
				}
				else if (container.hierarchy_level == current_planetSelection.hierarchy_level)
				{
					current_planetSelection.HideChildren();
				}
				else if (container.hierarchy_level < current_planetSelection.hierarchy_level)
				{
					current_planetSelection.GetParentInHierarchy(container.hierarchy_level).HideChildren();
				}
			}
			current_planetSelection = container;
			if (container.cascading)
			{
				container.ShowChildren();
			}
			ClearAllSubFilterSortingFlags();
			FilterReports();
			RefreshALLSubFilterLists(container);
		}
		else
		{
			if (container.cascading)
			{
				container.HideChildren();
			}
			current_planetSelection = null;
			ClearALLSubFilterLists();
			FilterReports();
			RefreshALLSubFilterLists(null);
		}
		UpdateReports();
		UpdateInstructor();
	}

	public void SetInstructorText(string text)
	{
		instructorText.text = Localizer.Format("#autoLOC_468826", text);
	}

	public void SetupInstructor()
	{
		instructor = UnityEngine.Object.Instantiate(instructorPrefab);
		instructor.instructorCamera.targetTexture = instructorTexture;
		instructor.instructorCamera.ResetAspect();
		instructorRawImage.texture = instructorTexture;
		avatarController = instructor.gameObject.GetComponent<RDArchivesAvatarController>();
	}

	public void DestroyInstructor()
	{
		if (instructor != null)
		{
			UnityEngine.Object.Destroy(instructor.gameObject);
		}
	}

	public void UpdateInstructor(bool setText = true)
	{
		float score = 0f;
		float num = float.MaxValue;
		float num2 = 0f;
		int count = reportListDataFiltered.Count;
		if (count > 0)
		{
			float num3 = 0f;
			for (int i = 0; i < count; i++)
			{
				ReportData reportData = reportListDataFiltered[i];
				num3 += reportData.value;
				if (num > reportData.value)
				{
					num = reportData.value;
				}
				if (num2 < reportData.value)
				{
					num2 = reportData.value;
				}
			}
			score = num3 / (float)count;
		}
		string text = avatarController.SetAvatarState(score, num, num2, count);
		if (setText)
		{
			SetInstructorText(text);
		}
	}

	public float RandomFloat()
	{
		float num = UnityEngine.Random.Range(0, 100);
		if (num != 0f)
		{
			num /= 100f;
		}
		return num;
	}
}
